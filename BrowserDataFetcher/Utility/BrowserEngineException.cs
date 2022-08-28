namespace BrowserDataFetcher
{
    using System;

    /// <summary>
    /// The <see cref="BrowserEngineException"/> type.
    /// </summary>
    public class BrowserEngineException : Exception
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        public BrowserEngineError Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserEngineException"/> class.
        /// </summary>
        /// <param name="e">
        /// The <see cref="BrowserEngineError"/> object.
        /// </param>
        public BrowserEngineException(BrowserEngineError e) : base()
        {
            Error = e;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserEngineException"/> class.
        /// </summary>
        /// <param name="e">
        /// The <see cref="BrowserEngineError"/> object.
        /// </param>
        /// <param name="msg">
        /// The exception message.
        /// </param>
        public BrowserEngineException(BrowserEngineError e, string msg) : base(msg)
        {
            Error = e;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserEngineException"/> class.
        /// </summary>
        /// <param name="e">
        /// The <see cref="BrowserEngineError"/> object.
        /// </param>
        /// <param name="msg">
        /// The exception message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception thrown.
        /// </param>
        public BrowserEngineException(BrowserEngineError e, string msg, Exception innerException) : base(msg, innerException)
        {
            Error = e;
        }
    }
}