namespace BrowserDataFetcher
{
    using BrowserDataFetcher.Engine;
    using System;
    using System.Linq;

    /// <summary>
    /// The <see cref="BrowserDataHandler"/>.
    /// </summary>
    public static class BrowserDataHandler
    {
        /// <summary>
        /// Tries to get a cookie.
        /// </summary>
        /// <param name="browserEngine">
        /// The browser engine.
        /// </param>
        /// <param name="cookieName">
        /// The cookie name.
        /// </param>
        /// <param name="chromiumCookie">
        /// The Chromium cookie.
        /// </param>
        /// <param name="geckoCookie">
        /// The Gecko cookie.
        /// </param>
        /// <returns>
        /// <c>True</c> if managed to retrieve a cookie, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetCookie(IBrowserEngineState browserEngine, string cookieName, out Model.Chromium.Cookie chromiumCookie, out Model.Gecko.Cookie geckoCookie)
        {
            chromiumCookie = null;
            geckoCookie = null;

            switch (browserEngine)
            {
                case ChromeDataHandler chrome:
                    if (chrome.CookiesExist())
                    {
                        chromiumCookie = chrome.GetCookies().FirstOrDefault(c => c.Name.Contains(cookieName));

                        return !(chromiumCookie is null);
                    }

                    break;

                case OperaDataHandler opera:
                    if (opera.CookiesExist())
                    {
                        chromiumCookie = opera.GetCookies().FirstOrDefault(c => c.Name.Contains(cookieName));

                        return !(chromiumCookie is null);
                    }

                    break;

                case EdgeDataHandler edge:
                    if (edge.CookiesExist())
                    {
                        chromiumCookie = edge.GetCookies().FirstOrDefault(c => c.Name.Contains(cookieName));

                        return !(chromiumCookie is null);
                    }

                    break;

                case FirefoxDataHandler firefox:
                    if (firefox.CookiesExist())
                    {
                        geckoCookie = firefox.GetCookies().FirstOrDefault(c => c.Name.Contains(cookieName));

                        return !(geckoCookie is null);
                    }

                    break;
            }

            return false;
        }

        /// <summary>
        /// Tries to get login data.
        /// </summary>
        /// <param name="browserEngine">
        /// The browser engine.
        /// </param>
        /// <param name="hostName">
        /// The host name.
        /// </param>
        /// <param name="chromiumLoginData">
        /// The Chromium login data.
        /// </param>
        /// <param name="geckoLoginData">
        /// The Gecko login data.
        /// </param>
        /// <returns>
        /// <c>True</c> if managed to retrieve requested login info, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetLoginData(IBrowserEngineState browserEngine, string hostName, out Model.Chromium.Login chromiumLoginData, out Model.Gecko.Login geckoLoginData)
        {
            chromiumLoginData = null;
            geckoLoginData = null;

            switch (browserEngine)
            {
                case ChromeDataHandler chrome:
                    if (chrome.LoginsExist())
                    {
                        chromiumLoginData = chrome.GetLogins().FirstOrDefault(l => l.OriginUrl.Contains(hostName));

                        return !(chromiumLoginData is null);
                    }

                    break;

                case OperaDataHandler opera:
                    if (opera.LoginsExist())
                    {
                        chromiumLoginData = opera.GetLogins().FirstOrDefault(l => l.OriginUrl.Contains(hostName));

                        return !(chromiumLoginData is null);
                    }

                    break;

                case EdgeDataHandler edge:
                    if (edge.LoginsExist())
                    {
                        chromiumLoginData = edge.GetLogins().FirstOrDefault(l => l.OriginUrl.Contains(hostName));

                        return !(chromiumLoginData is null);
                    }

                    break;

                case FirefoxDataHandler firefox:
                    if (firefox.LoginsExist())
                    {
                        geckoLoginData = firefox.GetLogins().FirstOrDefault(l => l.Hostname.Contains(hostName));

                        return !(geckoLoginData is null);
                    }

                    break;
            }

            return false;
        }

        /// <summary>
        /// Tries to get browser history.
        /// </summary>
        /// <param name="browserEngine">
        /// The browser engine.
        /// </param>
        /// <param name="title">
        /// The title to search for.
        /// </param>
        /// <param name="chromiumHistory">
        /// The Chromium site.
        /// </param>
        /// <param name="geckoHistory">
        /// The Gecko site.
        /// </param>
        /// <returns>
        /// <c>True</c> if managed to retrieve history, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetHistory(IBrowserEngineState browserEngine, string title, out Model.Chromium.Site chromiumHistory, out Model.Gecko.Site geckoHistory)
        {
            chromiumHistory = null;
            geckoHistory = null;

            switch (browserEngine)
            {
                case ChromeDataHandler chrome:
                    if (chrome.HistoryExists())
                    {
                        chromiumHistory = chrome.GetHistory().FirstOrDefault(s => s.Title.Contains(title));

                        return !(chromiumHistory is null);
                    }

                    break;

                case OperaDataHandler opera:
                    if (opera.HistoryExists())
                    {
                        chromiumHistory = opera.GetHistory().FirstOrDefault(s => s.Title.Contains(title));

                        return !(chromiumHistory is null);
                    }

                    break;

                case EdgeDataHandler edge:
                    if (edge.HistoryExists())
                    {
                        chromiumHistory = edge.GetHistory().FirstOrDefault(s => s.Title.Contains(title));

                        return !(chromiumHistory is null);
                    }

                    break;

                case FirefoxDataHandler firefox:
                    if (firefox.HistoryExists())
                    {
                        geckoHistory = firefox.GetHistory().FirstOrDefault(s => s.Title.Contains(title));

                        return !(geckoHistory is null);
                    }

                    break;
            }

            return false;
        }

        /// <summary>
        /// Tries to get a bookmark.
        /// </summary>
        /// <param name="browserEngine">
        /// The browser engine.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="chromiumBookmark">
        /// The Chromium site.
        /// </param>
        /// <param name="geckoBookmark">
        /// The Gecko site.
        /// </param>
        /// <returns>
        /// <c>True</c> if managed to retrieve the bookmarked site, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetBookmark(IBrowserEngineState browserEngine, string title, out Model.Chromium.Site chromiumBookmark, out Model.Gecko.Site geckoBookmark)
        {
            throw new NotImplementedException("This functionality is not yet implemented!");
        }

        /// <summary>
        /// Tries to get card data.
        /// </summary>
        /// <param name="browserEngine">
        /// The browser engine.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="chromiumCardData">
        /// The Chromium card data.
        /// </param>
        /// <param name="geckoCardData">
        /// The Gecko card data.
        /// </param>
        /// <returns>
        /// <c>True</c> if managed to retrieve the requested card data, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetCardData(IBrowserEngineState browserEngine, string id, out Model.Chromium.Site chromiumCardData, out Model.Gecko.Site geckoCardData)
        {
            throw new NotImplementedException("This functionality is not yet implemented!");
        }
    }
}