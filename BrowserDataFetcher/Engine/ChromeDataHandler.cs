namespace BrowserDataFetcher.Engine
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The <see cref="ChromeDataHandler"/> type.
    /// </summary>
    public class ChromeDataHandler : ChromiumDataHandler
    {
        #region Public Properties

        /// <inheritdoc />
        public override string CookiePath
        {
            get
            {
                return GetCookiesPath();
            }
        }

        /// <inheritdoc />
        public override string LocalStatePath
        {
            get
            {
                return GetLocalStatePath();
            }
        }

        /// <inheritdoc />
        public override string LoginDataPath
        {
            get
            {
                return GetLoginDataPath();
            }
        }

        /// <inheritdoc />
        public override string HistoryPath
        {
            get
            {
                return GetHistoryPath();
            }
        }

        /// <inheritdoc />
        public override string BookmarkPath
        {
            get
            {
                return GetBookmarkPath();
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets the <c>Cookies</c> file path.
        /// </summary>
        /// <returns>The <c>Cookies</c> file or <see cref="string.Empty"/> if not found.</returns>
        private string GetCookiesPath()
        {
            string chromeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Google\\Chrome\\User Data";
            string cookiesPath = $"{chromeUserFolder}\\Default\\Network\\Cookies";

            if (File.Exists(cookiesPath))
            {
                return cookiesPath;
            }
            else if (Directory.Exists(chromeUserFolder))
            {
                string[] files = Directory.GetFiles(chromeUserFolder, "Cookies", SearchOption.AllDirectories);

                foreach (var file in files.Where(File.Exists))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the <c>Local State</c> file path.
        /// </summary>
        /// <returns>The <c>Local State</c> file or <see cref="string.Empty"/> if not found.</returns>
        private string GetLocalStatePath()
        {
            string chromeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Google\\Chrome\\User Data";
            string localStatePath = $"{chromeUserFolder}\\Local State";

            if (File.Exists(localStatePath))
            {
                return localStatePath;
            }
            else if (Directory.Exists(chromeUserFolder))
            {
                string[] files = Directory.GetFiles(chromeUserFolder, "Local State", SearchOption.AllDirectories);

                foreach (var file in files.Where(File.Exists))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the <c>Login Data</c> file path.
        /// </summary>
        /// <returns>The <c>Login Data</c> file or <see cref="string.Empty"/> if not found.</returns>
        private string GetLoginDataPath()
        {
            string chromeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Google\\Chrome\\User Data";
            string loginDataPath = $"{chromeUserFolder}\\Default\\Login Data";

            if (File.Exists(loginDataPath))
            {
                return loginDataPath;
            }
            else if (Directory.Exists(chromeUserFolder))
            {
                string[] files = Directory.GetFiles(chromeUserFolder, "Login Data", SearchOption.AllDirectories);

                foreach (var file in files.Where(File.Exists))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the <c>History</c> file path.
        /// </summary>
        /// <returns>The <c>History</c> file or <see cref="string.Empty"/> if not found.</returns>
        private string GetHistoryPath()
        {
            string chromeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Google\\Chrome\\User Data";
            string historyPath = $"{chromeUserFolder}\\Default\\History";

            if (File.Exists(historyPath))
            {
                return historyPath;
            }
            else if (Directory.Exists(chromeUserFolder))
            {
                string[] files = Directory.GetFiles(chromeUserFolder, "History", SearchOption.AllDirectories);

                foreach (var file in files.Where(File.Exists))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the <c>Bookmarks</c> file path.
        /// </summary>
        /// <returns>The <c>Bookmarks</c> file or <see cref="string.Empty"/> if not found.</returns>
        private string GetBookmarkPath()
        {
            string chromeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Google\\Chrome\\User Data";
            string bookmarkPath = $"{chromeUserFolder}\\Default\\Bookmarks";

            if (File.Exists(bookmarkPath))
            {
                return bookmarkPath;
            }
            else if (Directory.Exists(chromeUserFolder))
            {
                string[] files = Directory.GetFiles(chromeUserFolder, "Bookmarks", SearchOption.AllDirectories);

                foreach (var file in files.Where(File.Exists))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        #endregion Private Methods
    }
}