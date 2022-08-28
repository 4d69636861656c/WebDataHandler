namespace BrowserDataFetcher
{
    using System;

    /// <summary>
    /// The <see cref="GeneralTools"/> helper class.
    /// </summary>
    public static class GeneralTools
    {
        #region Public Methods

        /// <summary>
        /// Are two arrays equal.
        /// </summary>
        /// <param name="a">
        /// Left side.
        /// </param>
        /// <param name="b">
        /// Right side.
        /// </param>
        /// <returns>
        /// <c>True</c> if arrays equal, <c>false</c> otherwise.
        /// </returns>
        public static bool AreEqual(byte[] a, byte[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return HaveSameContents(a, b);
        }

        /// <summary>
        /// A constant time equals comparison - does not terminate early if test fails.
        /// </summary>
        /// <param name="a">
        /// First array.
        /// </param>
        /// <param name="b">
        /// Second array.
        /// </param>
        /// <returns>
        /// <c>True</c> if arrays equal, <c>false</c> otherwise.
        /// </returns>
        public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
        {
            if (null == a || null == b)
            {
                return false;
            }

            if (a == b)
            {
                return true;
            }

            int len = System.Math.Min(a.Length, b.Length);
            int nonEqual = a.Length ^ b.Length;

            for (int i = 0; i < len; ++i)
            {
                nonEqual |= (a[i] ^ b[i]);
            }

            for (int i = len; i < b.Length; ++i)
            {
                nonEqual |= (b[i] ^ ~b[i]);
            }

            return 0 == nonEqual;
        }

        /// <summary>
        /// Clones a byte array.
        /// </summary>
        /// <param name="data">
        /// The byte data to clone.
        /// </param>
        /// <returns>
        /// A brand new byte array.
        /// </returns>
        public static byte[] Clone(byte[] data)
        {
            return data == null ? null : (byte[])data.Clone();
        }

        /// <summary>
        /// Clones a long array.
        /// </summary>
        /// <param name="data">
        /// The long array to clone.
        /// </param>
        /// <returns>
        /// A brand new long array.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong[] Clone(ulong[] data)
        {
            return data == null ? null : (ulong[])data.Clone();
        }

        /// <summary>
        /// Fills the byte array.
        /// </summary>
        /// <param name="buf">
        /// The byte array.
        /// </param>
        /// <param name="b">
        /// The new byte to be written.
        /// </param>
        public static void Fill(byte[] buf, byte b)
        {
            int i = buf.Length;
            while (i > 0)
            {
                buf[--i] = b;
            }
        }

        /// <summary>
        /// Makes a copy of a range of bytes from the passed in data array.
        /// The range can extend beyond the end of the input array, in which case the return array will be padded with zeroes.
        /// </summary>
        /// <param name="data">
        /// The array from which the data is to be copied.
        /// </param>
        /// <param name="from">
        /// The start index at which the copying should take place.
        /// </param>
        /// <param name="to">
        /// The final index of the range (exclusive).
        /// </param>
        /// <returns>
        /// A brand new byte array containing the range given.
        /// </returns>
        public static byte[] CopyOfRange(byte[] data, int from, int to)
        {
            int newLength = GetLength(from, to);
            byte[] tmp = new byte[newLength];
            Array.Copy(data, from, tmp, 0, System.Math.Min(newLength, data.Length - from));
            return tmp;
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Bit permute.
        /// </summary>
        internal static ulong BitPermuteStep(ulong x, ulong m, int s)
        {
            ulong t = (x ^ (x >> s)) & m;
            return (t ^ (t << s)) ^ x;
        }

        /// <summary>
        /// Bit permute simple.
        /// </summary>
        internal static ulong BitPermuteStepSimple(ulong x, ulong m, int s)
        {
            return ((x & m) << s) | ((x >> s) & m);
        }

        /// <summary>
        /// Checks the data length.
        /// </summary>
        /// <param name="buffer">
        /// The buffer where data is stored.
        /// </param>
        /// <param name="offset">
        /// The start offset in the data.
        /// </param>
        /// <param name="length">
        /// The number of bytes to write.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <exception cref="Exception">
        /// The <see cref="Exception"/> to be thrown if the check goes wrong.
        /// </exception>
        internal static void CheckDataLength(byte[] buffer, int offset, int length, string message)
        {
            if (offset > (buffer.Length - length))
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Checks the output length.
        /// </summary>
        /// <param name="buffer">
        /// The buffer where data is stored.
        /// </param>
        /// <param name="offset">
        /// The start offset in the data.
        /// </param>
        /// <param name="length">
        /// The number of bytes to write.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <exception cref="Exception">
        /// The <see cref="Exception"/> to be thrown if the check goes wrong.
        /// </exception>
        internal static void CheckOutputLength(byte[] buffer, int offset, int length, string message)
        {
            if (offset > (buffer.Length - length))
            {
                throw new Exception(message);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Checks if arrays have the same contents.
        /// </summary>
        /// <param name="a">
        /// First array.
        /// </param>
        /// <param name="b">
        /// Second array.
        /// </param>
        /// <returns>
        /// <c>True</c> if arrays have the same contents, <c>false</c> otherwise.
        /// </returns>
        private static bool HaveSameContents(byte[] a, byte[] b)
        {
            int i = a.Length;

            if (i != b.Length)
            {
                return false;
            }

            while (i != 0)
            {
                --i;

                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="from">
        /// The start index.
        /// </param>
        /// <param name="to">
        /// The final index.
        /// </param>
        /// <returns>
        /// The length.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        private static int GetLength(int from, int to)
        {
            int newLength = to - from;
            if (newLength < 0)
            {
                throw new ArgumentException(from + " > " + to);
            }

            return newLength;
        }

        #endregion Private Methods
    }
}