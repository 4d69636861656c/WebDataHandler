namespace BrowserDataFetcher.Engine
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="IChromiumEngine"/> interface.
    /// </summary>
    public interface IChromiumEngine : IBrowserEngineState
    {
        /// <summary>
        /// Gets the cookie path.
        /// </summary>
        string CookiePath { get; set; }

        /// <summary>
        /// Gets the local state path.
        /// </summary>
        string LocalStatePath { get; set; }

        /// <summary>
        /// Gets the login data path.
        /// </summary>
        string LoginDataPath { get; set; }

        /// <summary>
        /// Gets the history path.
        /// </summary>
        string HistoryPath { get; set; }

        /// <summary>
        /// Gets the bookmark path.
        /// </summary>
        string BookmarkPath { get; set; }

        /// <summary>
        /// Gets the web data path.
        /// </summary>
        string WebDataPath { get; set; }

        /// <summary>
        /// Gets all cookies.
        /// </summary>
        /// <returns>
        /// The list of <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Cookie> GetCookies();

        /// <summary>
        /// Get cookies by cookie header.
        /// </summary>
        /// <param name="by">
        /// Cookie header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value);

        /// <summary>
        /// Get cookies by cookie header.
        /// </summary>
        /// <param name="by">
        /// Cookie header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value, byte[] key);

        /// <summary>
        /// Get cookies by cookie header.
        /// </summary>
        /// <param name="by">
        /// Cookie header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="key">
        /// The <see cref="KeyParameter"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value, KeyParameter key);

        /// <summary>
        /// Gets all the login data.
        /// </summary>
        /// <returns>
        /// A list of <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Login> GetLogins();

        /// <summary>
        /// Gets login data by header.
        /// </summary>
        /// <param name="by">
        /// The login header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value);

        /// <summary>
        /// Gets login data by header.
        /// </summary>
        /// <param name="by">
        /// The login header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value, byte[] key);

        /// <summary>
        /// Gets login data by header.
        /// </summary>
        /// <param name="by">
        /// The login header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value, KeyParameter key);

        /// <summary>
        /// Gets browser history.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Chromium.Site"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Site> GetHistory();

        /// <summary>
        /// Gets history by header.
        /// </summary>
        /// <param name="by">
        /// The login header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Site"/> data.
        /// </returns>
        IEnumerable<Model.Chromium.Site> GetHistoryBy(Model.Chromium.Site.Header by, object value);
    }
}