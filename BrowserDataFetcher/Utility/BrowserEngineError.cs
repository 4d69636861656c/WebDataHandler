namespace BrowserDataFetcher
{
    /// <summary>
    /// The <see cref="BrowserEngineError"/>.
    /// </summary>
    public enum BrowserEngineError
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        UnknownError,

        /// <summary>
        /// Cookies not found.
        /// </summary>
        CookiesNotFound,

        /// <summary>
        /// Logins not found.
        /// </summary>
        LoginsNotFound,

        /// <summary>
        /// History not found.
        /// </summary>
        HistoryNotFound,

        /// <summary>
        /// Bookmarks not found.
        /// </summary>
        BookmarksNotFound,

        /// <summary>
        /// Web data not found.
        /// </summary>
        WebDataNotFound,

        /// <summary>
        /// Downloads not found.
        /// </summary>
        DownloadsNotFound,

        /// <summary>
        /// Local state not found.
        /// </summary>
        LocalStateNotFound,

        /// <summary>
        /// MozGlue not found.
        /// </summary>
        MozGlueNotFound,

        /// <summary>
        /// NSS3 not found.
        /// </summary>
        Nss3NotFound,

        /// <summary>
        /// Profile not found.
        /// </summary>
        ProfileNotFound,

        /// <summary>
        /// Address not found.
        /// </summary>
        AddressNotFound,

        /// <summary>
        /// Function not found.
        /// </summary>
        FunctionNotFound,

        /// <summary>
        /// Could not set profile.
        /// </summary>
        CouldNotSetProfile,

        /// <summary>
        /// Process is not 64-bit.
        /// </summary>
        ProcessIsNot64Bit,

        /// <summary>
        /// No arguments specified.
        /// </summary>
        NoArgumentsSpecified,
    }
}