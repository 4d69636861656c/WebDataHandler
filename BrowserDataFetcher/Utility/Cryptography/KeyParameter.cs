namespace BrowserDataFetcher
{
    using System;

    /// <summary>
    /// The <see cref="KeyParameter"/> type.
    /// </summary>
    public class KeyParameter : ICipherParameters
    {
        /// <summary>
        /// The key.
        /// </summary>
        private readonly byte[] key;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyParameter"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        public KeyParameter(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.key = (byte[])key.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyParameter"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="keyOff">
        /// The key offset.
        /// </param>
        /// <param name="keyLen">
        /// The key length.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <see cref="ArgumentOutOfRangeException"/>.
        /// </exception>
        public KeyParameter(byte[] key, int keyOff, int keyLen)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (keyOff < 0 || keyOff > key.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(keyOff));
            }

            if (keyLen < 0 || keyLen > (key.Length - keyOff))
            {
                throw new ArgumentOutOfRangeException(nameof(keyLen));
            }

            this.key = new byte[keyLen];
            Array.Copy(key, keyOff, this.key, 0, keyLen);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns>
        /// Returns the key as a <see cref="T:byte[]"/>.
        /// </returns>
        public byte[] GetKey()
        {
            return (byte[])key.Clone();
        }
    }
}