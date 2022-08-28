namespace BrowserDataFetcher
{
    /// <summary>
    /// Authenticated Encryption with Associated Data (AEAD) Parameters.
    /// </summary>
    public class AeadParameters : ICipherParameters
    {
        /// <summary>
        /// The associated text.
        /// </summary>
        private readonly byte[] _plainText;

        /// <summary>
        /// The nonce (initialization vector).
        /// </summary>
        private readonly byte[] _nonce;

        /// <summary>
        /// The key.
        /// </summary>
        private readonly KeyParameter _key;

        /// <summary>
        /// Message authentication code.
        /// </summary>
        private readonly int _macSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="AeadParameters"/> class.
        /// </summary>
        /// <param name="key">
        /// The key to be used by underlying cipher.
        /// </param>
        /// <param name="macSize">
        /// Cipher block chaining message authentication code size in bits.
        /// </param>
        /// <param name="nonce">
        /// Unique nonce to be used.
        /// </param>
        public AeadParameters(KeyParameter key, int macSize, byte[] nonce) : this(key, macSize, nonce, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AeadParameters"/> class.
        /// </summary>
        /// <param name="key">
        /// The key to be used by underlying cipher.
        /// </param>
        /// <param name="macSize">
        /// Cipher block chaining message authentication code size in bits.
        /// </param>
        /// <param name="nonce">
        /// Unique nonce to be used.
        /// </param>
        /// <param name="plainText">
        /// The associated text, if it exists.
        /// </param>
        public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] plainText)
        {
            this._key = key;
            this._nonce = nonce;
            this._macSize = macSize;
            this._plainText = plainText;
        }

        /// <summary>
        /// The key.
        /// </summary>
        public virtual KeyParameter Key
        {
            get
            {
                return _key;
            }
        }

        /// <summary>
        /// The MAC size.
        /// </summary>
        public virtual int MacSize
        {
            get
            {
                return _macSize;
            }
        }

        /// <summary>
        /// Gets the associated text.
        /// </summary>
        /// <returns>
        ///	An array of bits.
        /// </returns>
        public virtual byte[] GetAssociatedText()
        {
            return _plainText;
        }

        /// <summary>
        /// Gets the nonce (initialization vector).
        /// </summary>
        /// <returns>
        ///	An array of bits.
        /// </returns>
        public virtual byte[] GetNonce()
        {
            return _nonce;
        }
    }
}