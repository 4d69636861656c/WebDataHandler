namespace BrowserDataFetcher
{
    /// <summary>
    /// Base interface for a symmetric key block cipher.
    /// </summary>
    public interface IBlockCipher
    {
        /// <summary>
        /// The name of the algorithm this cipher implements.
        /// </summary>
        string AlgorithmName { get; }

        /// <summary>
        /// Initializes the cipher.
        /// </summary>
        /// <param name="forEncryption">
        /// Initialise for encryption if <c>true</c>, for decryption if <c>false</c>.
        /// </param>
        /// <param name="parameters">
        /// The key or other data required by the cipher.
        /// </param>
        void Init(bool forEncryption, ICipherParameters parameters);

        /// <summary>
        /// Gets the block size.
        /// </summary>
        /// <returns>
        /// The block size for this cipher, in bytes.
        /// </returns>
        int GetBlockSize();

        /// <summary>
        /// Indicates whether this cipher can handle partial blocks.
        /// </summary>
        bool IsPartialBlockOkay { get; }

        /// <summary>
        /// Processes a block.
        /// </summary>
        /// <remarks>
        /// <c>DataLengthException</c> will be thrown if input block is wrong size, or outBuf too small.
        /// </remarks>
        /// <param name="inBuf">
        /// The input buffer.
        /// </param>
        /// <param name="inOff">
        /// The offset into <paramref>inBuf</paramref> that the input block begins.
        /// </param>
        /// <param name="outBuf">
        /// The output buffer.
        /// </param>
        /// <param name="outOff">
        /// The offset into <paramref>outBuf</paramref> to write the output block.
        /// </param>
        /// <returns>
        /// The number of bytes processed and produced.
        /// </returns>
        int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);

        /// <summary>
        /// Reset the cipher to the same state as it was after the last init (if there was one).
        /// </summary>
        void Reset();
    }
}