namespace BrowserDataFetcher
{
    /// <summary>
    /// The <see cref="BasicGcmExponentiator"/> class.
    /// </summary>
    public class BasicGcmExponentiator : IGcmExponentiator
    {
        /// <summary>
        /// The <see cref="ulong"/> array.
        /// </summary>
        private ulong[] x;

        /// <summary>
        /// Initializes the object fields.
        /// </summary>
        /// <param name="x">
        /// The <see cref="byte"/> array.
        /// </param>
        public void Init(byte[] x)
        {
            this.x = GcmUtilities.AsUlongs(x);
        }

        /// <summary>
        /// Raises the byte array to a power.
        /// </summary>
        /// <param name="pow">
        /// The power.
        /// </param>
        /// <param name="output">
        /// The <see cref="byte"/> array.
        /// </param>
        public void ExponentiateX(long pow, byte[] output)
        {
            ulong[] y = GcmUtilities.OneAsUlongs();

            if (pow > 0)
            {
                ulong[] powX = GeneralTools.Clone(x);
                do
                {
                    if ((pow & 1L) != 0)
                    {
                        GcmUtilities.Multiply(y, powX);
                    }
                    GcmUtilities.Square(powX, powX);
                    pow >>= 1;
                }
                while (pow > 0);
            }

            GcmUtilities.AsBytes(y, output);
        }
    }
}