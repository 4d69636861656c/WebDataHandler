namespace BrowserDataFetcher
{
    using System;

    /// <summary>
    /// The <see cref="ParametersWithIv"/> class.
    /// </summary>
    public class ParametersWithIv : ICipherParameters
    {
        /// <summary>
        /// The <see cref="ICipherParameters"/>.
        /// </summary>
        private readonly ICipherParameters _parameters;

        /// <summary>
        /// The initialization vector.
        /// </summary>
        private readonly byte[] _iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersWithIv"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="iv">
        /// The initialization vector.
        /// </param>
        public ParametersWithIv(ICipherParameters parameters, byte[] iv) : this(parameters, iv, 0, iv.Length) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersWithIv"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="iv">
        /// The initialization vector.
        /// </param>
        /// <param name="ivOff">
        /// The initialization vector offset.
        /// </param>
        /// <param name="ivLen">
        /// The initialization vector length.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        public ParametersWithIv(ICipherParameters parameters, byte[] iv, int ivOff, int ivLen)
        {
            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            this._parameters = parameters;
            this._iv = GeneralTools.CopyOfRange(iv, ivOff, ivOff + ivLen);
        }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        /// <returns>
        /// Returns the initialization vector.
        /// </returns>
        public byte[] GetIV()
        {
            return (byte[])_iv.Clone();
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public ICipherParameters Parameters
        {
            get
            {
                return _parameters;
            }
        }
    }
}