namespace BrowserDataFetcher
{
    /// <summary>
    /// An IAeadCipher based on an IBlockCipher.
    /// </summary>
    public interface IAeadBlockCipher : IAeadCipher
    {
        /// <summary>
        /// Gets the block size.
        /// </summary>
        /// <returns>
        /// The block size for this cipher, in bytes.
        /// </returns>
        int GetBlockSize();

        /// <summary>
        /// Gets the block cipher underlying this algorithm.
        /// </summary>
        /// <returns>
        /// Returns the block cipher underlying this algorithm.
        /// </returns>
        IBlockCipher GetUnderlyingCipher();
    }
}