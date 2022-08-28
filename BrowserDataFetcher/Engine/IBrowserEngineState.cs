namespace BrowserDataFetcher.Engine
{
    /// <summary>
    /// The <see cref="IBrowserEngineState"/> interface.
    /// </summary>
    public interface IBrowserEngineState
    {
        /// <summary>
        /// Gets a boolean value indicating whether the database with the stored cookies was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the cookie database file exists, <c>false</c> otherwise.
        /// </returns>
        bool CookiesExist();

        /// <summary>
        /// Gets a value indicating whether the database with the stored logins was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser logins database file exists, <c>false</c> otherwise.
        /// </returns>
        bool LoginsExist();

        /// <summary>
        /// Gets a value indicating whether the database with the stored browser history was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser history database file exists, <c>false</c> otherwise.
        /// </returns>
        bool HistoryExists();

        /// <summary>
        /// Gets a value indicating whether the database with the stored bookmarks was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser bookmarks database file exists, <c>false</c> otherwise.
        /// </returns>
        bool BookmarksExist();

        /// <summary>
        /// Gets a value indicating whether the database file web data was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser web data database file exists, <c>false</c> otherwise.
        /// </returns>
        bool WebDataExist();

        /// <summary>
        /// Gets a value indicating whether the downloads database file was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the downloads database file exists, <c>false</c> otherwise.
        /// </returns>
        bool DownloadsExist();

        /// <summary>
        /// Returns a value indicating whether the file that stores the decryption key was found.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the decryption key exists, <c>false</c> otherwise.
        /// </returns>
        bool KeyExists();
    }
}