namespace BrowserDataFetcher
{
    using System;

    /// <summary>
    /// Implements the Galois/Counter mode (GCM).
    /// </summary>
    public class GcmBlockCipher : IAeadBlockCipher
    {
        #region Private Fields

        /// <summary>
        /// The block size.
        /// </summary>
        private const int BlockSize = 16;

        /// <summary>
        /// The cipher.
        /// </summary>
        private readonly IBlockCipher _cipher;

        /// <summary>
        /// The multiplier.
        /// </summary>
        private readonly IGcmMultiplier _multiplier;

        /// <summary>
        /// The exponentiator.
        /// </summary>
        private IGcmExponentiator _exponentiator;

        // These fields are set by Init and not modified by processing.

        /// <summary>
        /// For encryption.
        /// </summary>
        private bool _forEncryption;

        /// <summary>
        /// The initialized.
        /// </summary>
        private bool _initialised;

        /// <summary>
        /// The MAC size.
        /// </summary>
        private int _macSize;

        /// <summary>
        /// The last key.
        /// </summary>
        private byte[] _lastKey;

        /// <summary>
        /// The nonce.
        /// </summary>
        private byte[] _nonce;

        /// <summary>
        /// The initial associated text.
        /// </summary>
        private byte[] _initialAssociatedText;

        /// <summary>
        /// The H table.
        /// </summary>
        private byte[] H;

        /// <summary>
        /// The J0 table.
        /// </summary>
        private byte[] J0;

        // These fields are modified during processing.

        /// <summary>
        /// The buffer block.
        /// </summary>
        private byte[] _bufBlock;

        /// <summary>
        /// The MAC block.
        /// </summary>
        private byte[] _macBlock;

        /// <summary>
        /// The substitution tables.
        /// </summary>
        private byte[] S, S_at, S_atPre;

        /// <summary>
        /// The counter.
        /// </summary>
        private byte[] _counter;

        /// <summary>
        /// The blocks remaining.
        /// </summary>
        private uint _blocksRemaining;

        /// <summary>
        /// The buffer offset.
        /// </summary>
        private int _bufOff;

        /// <summary>
        /// The total length.
        /// </summary>
        private ulong _totalLength;

        /// <summary>
        /// The at block.
        /// </summary>
        private byte[] _atBlock;

        /// <summary>
        /// The at block position.
        /// </summary>
        private int _atBlockPos;

        /// <summary>
        /// The at length.
        /// </summary>
        private ulong _atLength;

        /// <summary>
        /// The at length.
        /// </summary>
        private ulong _atLengthPre;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The algorithm name.
        /// </summary>
        public virtual string AlgorithmName
        {
            get
            {
                return _cipher.AlgorithmName + "/GCM";
            }
        }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmBlockCipher"/> class.
        /// </summary>
        /// <param name="c">
        /// The block cipher interface.
        /// </param>
        public GcmBlockCipher(IBlockCipher c) : this(c, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmBlockCipher"/> class.
        /// </summary>
        /// <param name="c">
        /// The block cipher interface.
        /// </param>
        /// <param name="m">
        /// The Galois/Counter Mode multiplier.
        /// </param>
        public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
        {
            if (c.GetBlockSize() != BlockSize)
            {
                throw new ArgumentException("cipher required with a block size of " + BlockSize + ".");
            }

            if (m == null)
            {
                m = new Tables4kGcmMultiplier();
            }

            this._cipher = c;
            this._multiplier = m;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the underlying cipher.
        /// </summary>
        /// <returns>
        /// The <see cref="IBlockCipher"/>.
        /// </returns>
        public IBlockCipher GetUnderlyingCipher()
        {
            return _cipher;
        }

        /// <summary>
        /// Gets the block size.
        /// </summary>
        /// <returns>
        /// The block size.
        /// </returns>
        public virtual int GetBlockSize()
        {
            return BlockSize;
        }

        /// <summary>
        /// Initialization method.
        /// MAC sizes from 32 bits to 128 bits (must be a multiple of 8) are supported.
        /// The default is 128 bits.
        /// </summary>
        /// <param name="forEncryption">
        /// For encryption.
        /// </param>
        /// <param name="parameters">
        /// The cipher parameters.
        /// </param>
        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            this._forEncryption = forEncryption;
            this._macBlock = null;
            this._initialised = true;

            KeyParameter keyParam;
            byte[] newNonce = null;

            if (parameters is AeadParameters)
            {
                AeadParameters param = (AeadParameters)parameters;

                newNonce = param.GetNonce();
                _initialAssociatedText = param.GetAssociatedText();

                int macSizeBits = param.MacSize;
                if (macSizeBits < 32 || macSizeBits > 128 || macSizeBits % 8 != 0)
                {
                    throw new ArgumentException("Invalid value for MAC size: " + macSizeBits);
                }

                _macSize = macSizeBits / 8;
                keyParam = param.Key;
            }
            else if (parameters is ParametersWithIv)
            {
                ParametersWithIv param = (ParametersWithIv)parameters;

                newNonce = param.GetIV();
                _initialAssociatedText = null;
                _macSize = 16;
                keyParam = (KeyParameter)param.Parameters;
            }
            else
            {
                throw new ArgumentException("invalid parameters passed to GCM");
            }

            int bufLength = forEncryption ? BlockSize : (BlockSize + _macSize);
            this._bufBlock = new byte[bufLength];

            if (newNonce == null || newNonce.Length < 1)
            {
                throw new ArgumentException("IV must be at least 1 byte");
            }

            if (forEncryption)
            {
                if (_nonce != null && GeneralTools.AreEqual(_nonce, newNonce))
                {
                    if (keyParam == null)
                    {
                        throw new ArgumentException("cannot reuse nonce for GCM encryption");
                    }

                    if (_lastKey != null && GeneralTools.AreEqual(_lastKey, keyParam.GetKey()))
                    {
                        throw new ArgumentException("cannot reuse nonce for GCM encryption");
                    }
                }
            }

            _nonce = newNonce;
            if (keyParam != null)
            {
                _lastKey = keyParam.GetKey();
            }

            if (keyParam != null)
            {
                _cipher.Init(true, keyParam);

                this.H = new byte[BlockSize];
                _cipher.ProcessBlock(H, 0, H, 0);

                _multiplier.Init(H);
                _exponentiator = null;
            }
            else if (this.H == null)
            {
                throw new ArgumentException("Key must be specified in initial init");
            }

            this.J0 = new byte[BlockSize];

            if (_nonce.Length == 12)
            {
                Array.Copy(_nonce, 0, J0, 0, _nonce.Length);
                this.J0[BlockSize - 1] = 0x01;
            }
            else
            {
                gHASH(J0, _nonce, _nonce.Length);
                byte[] X = new byte[BlockSize];
                Pack.UInt64_To_BE((ulong)_nonce.Length * 8UL, X, 8);
                gHASHBlock(J0, X);
            }

            this.S = new byte[BlockSize];
            this.S_at = new byte[BlockSize];
            this.S_atPre = new byte[BlockSize];
            this._atBlock = new byte[BlockSize];
            this._atBlockPos = 0;
            this._atLength = 0;
            this._atLengthPre = 0;
            this._counter = GeneralTools.Clone(J0);
            this._blocksRemaining = uint.MaxValue - 1;
            this._bufOff = 0;
            this._totalLength = 0;

            if (_initialAssociatedText != null)
            {
                ProcessAadBytes(_initialAssociatedText, 0, _initialAssociatedText.Length);
            }
        }

        /// <summary>
        /// Gets the message authentication code.
        /// </summary>
        /// <returns>
        /// A <see cref="T:byte[]"/> representing the message authentication code.
        /// </returns>
        public virtual byte[] GetMac()
        {
            return _macBlock == null
                ? new byte[_macSize]
                : GeneralTools.Clone(_macBlock);
        }

        /// <summary>
        /// Gets the output size.
        /// </summary>
        /// <param name="len">
        /// The length.
        /// </param>
        /// <returns>
        /// The output size.
        /// </returns>
        public virtual int GetOutputSize(int len)
        {
            int totalData = len + _bufOff;

            if (_forEncryption)
            {
                return totalData + _macSize;
            }

            return totalData < _macSize ? 0 : totalData - _macSize;
        }

        /// <summary>
        /// Gets the update output size.
        /// </summary>
        /// <param name="len">
        /// The length.
        /// </param>
        /// <returns>
        /// The update output size.
        /// </returns>
        public virtual int GetUpdateOutputSize(int len)
        {
            int totalData = len + _bufOff;
            if (!_forEncryption)
            {
                if (totalData < _macSize)
                {
                    return 0;
                }
                totalData -= _macSize;
            }
            return totalData - totalData % BlockSize;
        }

        /// <summary>
        /// Processes additional authenticated bytes.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public virtual void ProcessAadByte(byte input)
        {
            CheckStatus();

            _atBlock[_atBlockPos] = input;
            if (++_atBlockPos == BlockSize)
            {
                gHASHBlock(S_at, _atBlock);
                _atBlockPos = 0;
                _atLength += BlockSize;
            }
        }

        /// <summary>
        /// Processes the additional authenticated data.
        /// </summary>
        /// <param name="inBytes">
        /// The <see cref="T:byte[]"/> containing the input data.
        /// </param>
        /// <param name="inOff">
        /// Offset into the input array the data starts at.
        /// </param>
        /// <param name="len">
        /// The length.
        /// </param>
        public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
        {
            CheckStatus();

            for (int i = 0; i < len; ++i)
            {
                _atBlock[_atBlockPos] = inBytes[inOff + i];
                if (++_atBlockPos == BlockSize)
                {
                    gHASHBlock(S_at, _atBlock);
                    _atBlockPos = 0;
                    _atLength += BlockSize;
                }
            }
        }

        /// <summary>
        /// Processes the bytes.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="outOff">
        /// Offset into the output array the output will start at.
        /// </param>
        /// <returns>
        /// Returns the processed bytes.
        /// </returns>
        public virtual int ProcessByte(byte input, byte[] output, int outOff)
        {
            CheckStatus();

            _bufBlock[_bufOff] = input;
            if (++_bufOff == _bufBlock.Length)
            {
                ProcessBlock(_bufBlock, 0, output, outOff);
                if (_forEncryption)
                {
                    _bufOff = 0;
                }
                else
                {
                    Array.Copy(_bufBlock, BlockSize, _bufBlock, 0, _macSize);
                    _bufOff = _macSize;
                }
                return BlockSize;
            }
            return 0;
        }

        /// <summary>
        /// Processes the bytes.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="len">
        /// The length.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="inOff">
        /// Offset into the input array the data starts at.
        /// </param>
        /// <param name="outOff">
        /// Offset into the output array the output will start at.
        /// </param>
        /// <returns>
        /// Returns the processed bytes.
        /// </returns>
        public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
        {
            CheckStatus();

            GeneralTools.CheckDataLength(input, inOff, len, "input buffer too short");

            int resultLen = 0;

            if (_forEncryption)
            {
                if (_bufOff != 0)
                {
                    while (len > 0)
                    {
                        --len;
                        _bufBlock[_bufOff] = input[inOff++];
                        if (++_bufOff == BlockSize)
                        {
                            ProcessBlock(_bufBlock, 0, output, outOff);
                            _bufOff = 0;
                            resultLen += BlockSize;
                            break;
                        }
                    }
                }

                while (len >= BlockSize)
                {
                    ProcessBlock(input, inOff, output, outOff + resultLen);
                    inOff += BlockSize;
                    len -= BlockSize;
                    resultLen += BlockSize;
                }

                if (len > 0)
                {
                    Array.Copy(input, inOff, _bufBlock, 0, len);
                    _bufOff = len;
                }
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    _bufBlock[_bufOff] = input[inOff + i];
                    if (++_bufOff == _bufBlock.Length)
                    {
                        ProcessBlock(_bufBlock, 0, output, outOff + resultLen);
                        Array.Copy(_bufBlock, BlockSize, _bufBlock, 0, _macSize);
                        _bufOff = _macSize;
                        resultLen += BlockSize;
                    }
                }
            }

            return resultLen;
        }

        /// <summary>
        /// Finishes a multiple-part encryption or decryption operation, depending on how this cipher was initialized.
        /// Input data that may have been buffered during a previous update operation is processed, with padding (if requested) being applied.
        /// If an AEAD mode such as GCM/CCM is being used, the authentication tag is appended in the case of encryption,
        /// or verified in the case of decryption. The result is stored in a new buffer. Upon finishing, this method resets this cipher object
        /// to the state it was in when previously initialized via a call to init. That is, the object is reset and available to encrypt or
        /// decrypt (depending on the operation mode that was specified in the call to init) more data.
        /// <remarks>
        /// If any exception is thrown, this cipher object may need to be reset before it can be used again.
        /// Some AAD was sent after the cipher started. We determine the difference b/w the hash value we actually used when the cipher
        /// started and the final hash value calculated. Then we carry this difference forward by multiplying by H^c, where c is the
        /// number of (full or partial) cipher-text blocks produced, and adjust the current hash.
        /// </remarks>
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="outOff">
        /// Offset into the output array the output will start at.
        /// </param>
        /// <returns>
        /// The number of bytes stored in output.
        /// </returns>
        /// <exception cref="Exception">
        /// The <see cref="Exception"/>. When thrown, this cipher object may need to be reset before it can be used again.
        /// </exception>
        public int DoFinal(byte[] output, int outOff)
        {
            CheckStatus();

            if (_totalLength == 0)
            {
                InitCipher();
            }

            int extra = _bufOff;

            if (_forEncryption)
            {
                GeneralTools.CheckOutputLength(output, outOff, extra + _macSize, "Output buffer too short");
            }
            else
            {
                if (extra < _macSize)
                {
                    throw new Exception("data too short");
                }

                extra -= _macSize;

                GeneralTools.CheckOutputLength(output, outOff, extra, "Output buffer too short");
            }

            if (extra > 0)
            {
                ProcessPartial(_bufBlock, 0, extra, output, outOff);
            }

            _atLength += (uint)_atBlockPos;

            if (_atLength > _atLengthPre)
            {
                if (_atBlockPos > 0)
                {
                    gHASHPartial(S_at, _atBlock, 0, _atBlockPos);
                }

                if (_atLengthPre > 0)
                {
                    GcmUtilities.Xor(S_at, S_atPre);
                }

                long c = (long)(((_totalLength * 8) + 127) >> 7);

                byte[] H_c = new byte[16];
                if (_exponentiator == null)
                {
                    _exponentiator = new BasicGcmExponentiator();
                    _exponentiator.Init(H);
                }
                _exponentiator.ExponentiateX(c, H_c);

                GcmUtilities.Multiply(S_at, H_c);

                GcmUtilities.Xor(S, S_at);
            }

            byte[] X = new byte[BlockSize];
            Pack.UInt64_To_BE(_atLength * 8UL, X, 0);
            Pack.UInt64_To_BE(_totalLength * 8UL, X, 8);

            gHASHBlock(S, X);

            byte[] tag = new byte[BlockSize];
            _cipher.ProcessBlock(J0, 0, tag, 0);
            GcmUtilities.Xor(tag, S);

            int resultLen = extra;

            this._macBlock = new byte[_macSize];
            Array.Copy(tag, 0, _macBlock, 0, _macSize);

            if (_forEncryption)
            {
                Array.Copy(_macBlock, 0, output, outOff + _bufOff, _macSize);
                resultLen += _macSize;
            }
            else
            {
                byte[] msgMac = new byte[_macSize];
                Array.Copy(_bufBlock, extra, msgMac, 0, _macSize);
                if (!GeneralTools.ConstantTimeAreEqual(this._macBlock, msgMac))
                {
                    throw new Exception("MAC check in GCM failed");
                }
            }

            Reset(false);

            return resultLen;
        }

        /// <summary>
        /// The object is reset and available to encrypt or decrypt (depending on the operation mode that was specified in the call to init) more data.
        /// </summary>
        public virtual void Reset()
        {
            Reset(true);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initializes the cipher.
        /// </summary>
        private void InitCipher()
        {
            if (_atLength > 0)
            {
                Array.Copy(S_at, 0, S_atPre, 0, BlockSize);
                _atLengthPre = _atLength;
            }

            if (_atBlockPos > 0)
            {
                gHASHPartial(S_atPre, _atBlock, 0, _atBlockPos);
                _atLengthPre += (uint)_atBlockPos;
            }

            if (_atLengthPre > 0)
            {
                Array.Copy(S_atPre, 0, S, 0, BlockSize);
            }
        }

        /// <summary>
        /// The object is reset and available to encrypt or decrypt (depending on the operation mode that was specified in the call to init) more data.
        /// </summary>
        /// <param name="clearMac">
        /// Flag for clearing the message authentication code.
        /// </param>
        private void Reset(bool clearMac)
        {
            _cipher.Reset();

            S = new byte[BlockSize];
            S_at = new byte[BlockSize];
            S_atPre = new byte[BlockSize];
            _atBlock = new byte[BlockSize];
            _atBlockPos = 0;
            _atLength = 0;
            _atLengthPre = 0;
            _counter = GeneralTools.Clone(J0);
            _blocksRemaining = uint.MaxValue - 1;
            _bufOff = 0;
            _totalLength = 0;

            if (_bufBlock != null)
            {
                GeneralTools.Fill(_bufBlock, 0);
            }

            if (clearMac)
            {
                _macBlock = null;
            }

            if (_forEncryption)
            {
                _initialised = false;
            }
            else
            {
                if (_initialAssociatedText != null)
                {
                    ProcessAadBytes(_initialAssociatedText, 0, _initialAssociatedText.Length);
                }
            }
        }

        /// <summary>
        /// Processes a block.
        /// </summary>
        /// <param name="buf">
        /// The buffer.
        /// </param>
        /// <param name="bufOff">
        /// The offset for the buffer.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="outOff">
        /// Offset into the output array the output will start at.
        /// </param>
        private void ProcessBlock(byte[] buf, int bufOff, byte[] output, int outOff)
        {
            GeneralTools.CheckOutputLength(output, outOff, BlockSize, "Output buffer too short");

            if (_totalLength == 0)
            {
                InitCipher();
            }

            byte[] ctrBlock = new byte[BlockSize];
            GetNextCtrBlock(ctrBlock);

            if (_forEncryption)
            {
                GcmUtilities.Xor(ctrBlock, buf, bufOff);
                gHASHBlock(S, ctrBlock);
                Array.Copy(ctrBlock, 0, output, outOff, BlockSize);
            }
            else
            {
                gHASHBlock(S, buf, bufOff);
                GcmUtilities.Xor(ctrBlock, 0, buf, bufOff, output, outOff);
            }

            _totalLength += BlockSize;
        }

        /// <summary>
        /// Processes a block.
        /// </summary>
        /// <param name="buf">
        /// The buffer.
        /// </param>
        /// <param name="len">
        /// The length.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="outOff">
        /// Offset into the output array the output will start at.
        /// </param>
        /// <param name="off">
        /// The offset.
        /// </param>
        private void ProcessPartial(byte[] buf, int off, int len, byte[] output, int outOff)
        {
            byte[] ctrBlock = new byte[BlockSize];
            GetNextCtrBlock(ctrBlock);

            if (_forEncryption)
            {
                GcmUtilities.Xor(buf, off, ctrBlock, 0, len);
                gHASHPartial(S, buf, off, len);
            }
            else
            {
                gHASHPartial(S, buf, off, len);
                GcmUtilities.Xor(buf, off, ctrBlock, 0, len);
            }

            Array.Copy(buf, off, output, outOff, len);
            _totalLength += (uint)len;
        }

        /// <summary>
        /// Produce a message authentication code.
        /// </summary>
        /// <param name="Y">
        /// The J0 table.
        /// </param>
        /// <param name="b">
        /// The buffer into which the data is read.
        /// </param>
        /// <param name="len">
        /// The length.
        /// </param>
        private void gHASH(byte[] Y, byte[] b, int len)
        {
            for (int pos = 0; pos < len; pos += BlockSize)
            {
                int num = System.Math.Min(len - pos, BlockSize);
                gHASHPartial(Y, b, pos, num);
            }
        }

        /// <summary>
        /// Produces the gHASH block.
        /// </summary>
        /// <param name="Y">
        /// The J0 table.
        /// </param>
        /// <param name="b">
        /// The buffer.
        /// </param>
        private void gHASHBlock(byte[] Y, byte[] b)
        {
            GcmUtilities.Xor(Y, b);
            _multiplier.MultiplyH(Y);
        }

        /// <summary>
        /// Produces the gHASH block.
        /// </summary>
        /// <param name="Y">
        /// The J0 table.
        /// </param>
        /// <param name="b">
        /// The buffer.
        /// </param>
        /// <param name="off">
        /// The offset.
        /// </param>
        private void gHASHBlock(byte[] Y, byte[] b, int off)
        {
            GcmUtilities.Xor(Y, b, off);
            _multiplier.MultiplyH(Y);
        }

        /// <summary>
        /// Produces the gHASH partial block.
        /// </summary>
        /// <param name="Y">
        /// The J0 table.
        /// </param>
        /// <param name="b">
        /// The buffer.
        /// </param>
        /// <param name="off">
        /// The offset.
        /// </param>
        /// <param name="len">
        /// The length.
        /// </param>
        private void gHASHPartial(byte[] Y, byte[] b, int off, int len)
        {
            GcmUtilities.Xor(Y, b, off, len);
            _multiplier.MultiplyH(Y);
        }

        /// <summary>
        /// Gets the next control block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="InvalidOperationException"/>.
        /// </exception>
        private void GetNextCtrBlock(byte[] block)
        {
            if (_blocksRemaining == 0)
                throw new InvalidOperationException("Attempt to process too many blocks");

            _blocksRemaining--;

            uint c = 1;
            c += _counter[15]; _counter[15] = (byte)c; c >>= 8;
            c += _counter[14]; _counter[14] = (byte)c; c >>= 8;
            c += _counter[13]; _counter[13] = (byte)c; c >>= 8;
            c += _counter[12]; _counter[12] = (byte)c;

            _cipher.ProcessBlock(_counter, 0, block, 0);
        }

        /// <summary>
        /// Checks the status.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="InvalidOperationException"/> thrown when GCM cipher is not initialized or it cannot be reused.
        /// </exception>
        private void CheckStatus()
        {
            if (!_initialised)
            {
                if (_forEncryption)
                {
                    throw new InvalidOperationException("GCM cipher cannot be reused for encryption");
                }
                throw new InvalidOperationException("GCM cipher needs to be initialised");
            }
        }

        #endregion Private Methods
    }
}