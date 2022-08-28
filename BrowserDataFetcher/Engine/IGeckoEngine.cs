namespace BrowserDataFetcher.Engine
{
    using BrowserDataFetcher.Model.Gecko;
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="IGeckoEngine"/> interface.
    /// </summary>
    public interface IGeckoEngine : IBrowserEngineState
    {
        /// <summary>
        /// The <c>Profiles</c> directory path.
        /// </summary>
        string ProfilesPath { get; set; }

        /// <summary>
        /// The <c>Cookies</c> database file path.
        /// </summary>
        string CookiesPath { get; set; }

        /// <summary>
        /// The <c>Login</c> JSON file path.
        /// </summary>
        string LoginsPath { get; set; }

        /// <summary>
        /// The <c>Places</c> database file path.
        /// </summary>
        string PlacesPath { get; set; }

        /// <summary>
        /// The <c>Profiles</c> file paths array.
        /// </summary>
        string[] Profiles { get; set; }

        /// <summary>
        /// Gets all cookies for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Cookie"/> data.
        /// </returns>
        IEnumerable<Cookie> GetCookies();

        /// <summary>
        /// Gets cookies by cookie header.
        /// </summary>
        /// <param name="by">
        /// Cookie header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Gecko.Cookie"/> data.
        /// </returns>
        IEnumerable<Cookie> GetCookiesBy(Cookie.Header by, object value);

        /// <summary>
        /// Gets all login data for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Login"/> data.
        /// </returns>
        IEnumerable<Login> GetLogins();

        /// <summary>
        /// Gets logins by login header.
        /// </summary>
        /// <param name="by">
        /// Login header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Gecko.Login"/> data.
        /// </returns>
        IEnumerable<Login> GetLoginsBy(Login.Header by, object value);

        /// <summary>
        /// Gets all site history for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Site"/> data.
        /// </returns>
        IEnumerable<Site> GetHistory();

        /// <summary>
        /// Gets history by site header.
        /// </summary>
        /// <param name="by">
        /// Site header.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Gecko.Site"/> data.
        /// </returns>
        IEnumerable<Site> GetHistoryBy(Site.Header by, object value);
    }
}