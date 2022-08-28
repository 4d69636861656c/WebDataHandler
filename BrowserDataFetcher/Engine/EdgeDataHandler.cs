namespace BrowserDataFetcher.Engine
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The <see cref="EdgeDataHandler"/> type.
    /// </summary>
    public class EdgeDataHandler : ChromiumDataHandler
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
            string edgeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Microsoft\\Edge\\User Data";
            string cookiesPath = $"{edgeUserFolder}\\Default\\Network\\Cookies";

            if (File.Exists(cookiesPath))
            {
                return cookiesPath;
            }
            else if (Directory.Exists(edgeUserFolder))
            {
                string[] files = Directory.GetFiles(edgeUserFolder, "Cookies", SearchOption.AllDirectories);

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
            string edgeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Microsoft\\Edge\\User Data";
            string localStatePath = $"{edgeUserFolder}\\Local State";

            if (File.Exists(localStatePath))
            {
                return localStatePath;
            }
            else if (Directory.Exists(edgeUserFolder))
            {
                string[] files = Directory.GetFiles(edgeUserFolder, "Local State", SearchOption.AllDirectories);

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
            string edgeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Microsoft\\Edge\\User Data";
            string loginDataPath = $"{edgeUserFolder}\\Default\\Login Data";

            if (File.Exists(loginDataPath))
            {
                return loginDataPath;
            }
            else if (Directory.Exists(edgeUserFolder))
            {
                string[] files = Directory.GetFiles(edgeUserFolder, "Login Data", SearchOption.AllDirectories);

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
            string edgeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Microsoft\\Edge\\User Data";
            string historyPath = $"{edgeUserFolder}\\Default\\History";

            if (File.Exists(historyPath))
            {
                return historyPath;
            }
            else if (Directory.Exists(edgeUserFolder))
            {
                string[] files = Directory.GetFiles(edgeUserFolder, "History", SearchOption.AllDirectories);

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
            string edgeUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Microsoft\\Edge\\User Data";
            string bookmarkPath = $"{edgeUserFolder}\\Default\\Bookmarks";

            if (File.Exists(bookmarkPath))
            {
                return bookmarkPath;
            }
            else if (Directory.Exists(edgeUserFolder))
            {
                string[] files = Directory.GetFiles(edgeUserFolder, "Bookmarks", SearchOption.AllDirectories);

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