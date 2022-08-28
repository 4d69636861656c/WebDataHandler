namespace BrowserDataFetcher
{
    /// <summary>
    /// The <see cref="IGcmMultiplier"/> interface.
    /// </summary>
    public interface IGcmMultiplier
    {
        /// <summary>
        /// Initialization method.
        /// MAC sizes from 32 bits to 128 bits (must be a multiple of 8) are supported.
        /// The default is 128 bits.
        /// </summary>
        void Init(byte[] H);

        /// <summary>
        /// Multiplies the byte array by a factor.
        /// </summary>
        void MultiplyH(byte[] x);
    }
}