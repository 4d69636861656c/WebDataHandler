namespace BrowserDataFetcher
{
    /// <summary>
    /// The <see cref="Tables4kGcmMultiplier"/> class.
    /// </summary>
    public class Tables4kGcmMultiplier : IGcmMultiplier
    {
        private byte[] H;
        private ulong[] T;

        /// <summary>
        /// Initialization method.
        /// MAC sizes from 32 bits to 128 bits (must be a multiple of 8) are supported.
        /// The default is 128 bits.
        /// </summary>
        public void Init(byte[] H)
        {
            if (T == null)
            {
                T = new ulong[512];
            }
            else if (GeneralTools.AreEqual(this.H, H))
            {
                return;
            }

            this.H = GeneralTools.Clone(H);

            // T[0] = 0

            // T[1] = H.p^7
            GcmUtilities.AsUlongs(this.H, T, 2);
            GcmUtilities.MultiplyP7(T, 2, T, 2);

            for (int n = 2; n < 256; n += 2)
            {
                // T[2.n] = T[n].p^-1
                GcmUtilities.DivideP(T, n, T, n << 1);

                // T[2.n + 1] = T[2.n] + T[1]
                GcmUtilities.Xor(T, n << 1, T, 2, T, (n + 1) << 1);
            }
        }

        /// <summary>
        /// Multiplies the byte array by a factor.
        /// </summary>
        public void MultiplyH(byte[] x)
        {
            int pos = x[15] << 1;
            ulong z0 = T[pos + 0], z1 = T[pos + 1];

            for (int i = 14; i >= 0; --i)
            {
                pos = x[i] << 1;

                ulong c = z1 << 56;
                z1 = T[pos + 1] ^ ((z1 >> 8) | (z0 << 56));
                z0 = T[pos + 0] ^ (z0 >> 8) ^ c ^ (c >> 1) ^ (c >> 2) ^ (c >> 7);
            }

            Pack.UInt64_To_BE(z0, x, 0);
            Pack.UInt64_To_BE(z1, x, 8);
        }
    }
}