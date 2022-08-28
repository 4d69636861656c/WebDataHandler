namespace BrowserDataFetcher
{
    /// <summary>
    /// The <see cref="IGcmExponentiator"/> interface.
    /// </summary>
    public interface IGcmExponentiator
    {
        /// <summary>
        /// Initializes the object fields.
        /// </summary>
        /// <param name="x">
        /// The <see cref="byte"/> array.
        /// </param>
        void Init(byte[] x);

        /// <summary>
        /// Raises the byte array to a power.
        /// </summary>
        /// <param name="pow">
        /// The power.
        /// </param>
        /// <param name="output">
        /// The <see cref="byte"/> array.
        /// </param>
        void ExponentiateX(long pow, byte[] output);
    }
}