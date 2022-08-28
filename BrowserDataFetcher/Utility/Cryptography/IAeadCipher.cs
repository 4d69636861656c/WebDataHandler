namespace BrowserDataFetcher
{
    /// <summary>
    /// A cipher mode that includes authenticated encryption with a streaming mode and optional associated data.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface may operate in a packet mode (where all input data is buffered and processed during the
    /// call to DoFinal, or in a streaming mode (where output data is incrementally produced with each call to ProcessByte or
    /// ProcessBytes. This is important to consider during decryption: in a streaming mode, unauthenticated plaintext data may be
    /// output prior to the call to DoFinal that results in an authentication failure. The higher level protocol utilizing this
    /// cipher must ensure the plaintext data is handled appropriately until the end of data is reached and the entire cipher text
    /// is authenticated.
    /// </remarks>
    /// <see cref="AeadParameters"/>
    public interface IAeadCipher
    {
        /// <summary>
        /// The name of the algorithm this cipher implements.
        /// </summary>
        string AlgorithmName { get; }

        /// <summary>
        /// Initialise the cipher.
        /// </summary>
        /// <remarks>
        /// Parameter can either be an <c>AeadParameters</c> or a <c>ParametersWithIV</c> object.
        /// </remarks>
        /// <param name="forEncryption">
        /// Initialize for encryption if <c>true</c>, for decryption if <c>false</c>.
        /// </param>
        /// <param name="parameters">
        /// The key or other data required by the cipher.
        /// </param>
        void Init(bool forEncryption, ICipherParameters parameters);

        /// <summary>
        /// Add a single byte to the associated data check.
        /// </summary>
        /// <remarks>
        /// If the implementation supports it, this will be an online operation and will not retain the associated data.
        /// </remarks>
        /// <param name="input">
        /// The byte to be processed.
        /// </param>
        void ProcessAadByte(byte input);

        /// <summary>
        /// Add a sequence of bytes to the associated data check.
        /// </summary>
        /// <remarks>
        /// If the implementation supports it, this will be an online operation and will not retain the associated data.
        /// </remarks>
        /// <param name="inBytes">
        /// The input <see cref="T:byte[]"/>.
        /// </param>
        /// <param name="inOff">
        /// The offset into the input array where the data to be processed starts.
        /// </param>
        /// <param name="len">
        /// The number of bytes to be processed.
        /// </param>
        void ProcessAadBytes(byte[] inBytes, int inOff, int len);

        /// <summary>
        /// Encrypts/decrypts a single byte.
        /// </summary>
        /// <remarks>
        ///	<c>DataLengthException</c> will be thrown if the output buffer is too small.
        /// </remarks>
        /// <param name="input">
        ///	The byte to be processed.
        /// </param>
        /// <param name="outBytes">
        /// The output buffer the processed byte goes into.
        /// </param>
        /// <param name="outOff">
        /// the offset into the output byte array the processed data starts at.
        /// </param>
        /// <returns>
        ///	Returns the number of bytes written to out.
        /// </returns>
        int ProcessByte(byte input, byte[] outBytes, int outOff);

        /// <summary>
        /// Processes a block of bytes from in putting the result into out.
        /// </summary>
        /// <remarks>
        ///	<c>DataLengthException</c> will be thrown if the output buffer is too small.
        /// </remarks>
        /// <param name="inBytes">
        /// The input byte array.
        /// </param>
        /// <param name="inOff">
        /// The offset into the in array where the data to be processed starts.
        /// </param>
        /// <param name="len">
        /// The number of bytes to be processed.
        /// </param>
        /// <param name="outBytes">
        /// The output buffer the processed bytes go into.
        /// </param>
        /// <param name="outOff">
        /// The offset into the output byte array the processed data starts at.
        /// </param>
        /// <returns>
        /// The number of bytes written to out.
        /// </returns>
        int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);

        /// <summary>
        /// Finish the operation either appending or verifying the MAC at the end of the data.
        /// </summary>
        /// <remarks>
        /// Throws <c>InvalidOperationException</c> if the cipher is in an inappropriate state.
        /// Throws <c>InvalidCipherTextException</c> if the MAC fails to match.
        /// </remarks>
        /// <param name="outBytes">
        /// The space for any resulting output data.
        /// </param>
        /// <param name="outOff">
        /// The offset into out to start copying the data at.
        /// </param>
        /// <returns>
        /// Returns the number of bytes written into out.
        /// </returns>
        int DoFinal(byte[] outBytes, int outOff);

        /// <summary>
        /// Gets the value of the MAC associated with the last stream processed.
        /// </summary>
        /// <returns>
        /// Return MAC for plaintext data.
        /// </returns>
        byte[] GetMac();

        /// <summary>
        /// Gets the size of the output buffer required for a <c>ProcessBytes</c> an input of len bytes.
        /// </summary>
        /// <param name="len">
        /// The length of the input.
        /// </param>
        /// <returns>
        /// Returns the space required to accommodate a call to <c>ProcessBytes</c> with len bytes of input.
        /// </returns>
        int GetUpdateOutputSize(int len);

        /// <summary>
        /// Gets the size of the output buffer required for a <c>ProcessBytes</c> plus a <c>DoFinal</c> with an input of len bytes.
        /// </summary>
        /// <param name="len">
        /// The length of the input.
        /// </param>
        /// <returns>
        /// Returns the space required to accommodate a call to <c>ProcessBytes</c> and <c>DoFinal</c> with len bytes of input.
        /// </returns>
        int GetOutputSize(int len);

        /// <summary>
        /// Reset the cipher to the same state as it was after the last init (if there was one).
        /// </summary>
        void Reset();
    }
}