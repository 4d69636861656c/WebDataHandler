namespace BrowserDataFetcher.Engine
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The <see cref="OperaDataHandler"/> type.
    /// </summary>
    public class OperaDataHandler : ChromiumDataHandler
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
            string operaUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Roaming\\Opera Software\\Opera Stable";
            string cookiesPath = $"{operaUserFolder}\\Cookies";

            if (File.Exists(cookiesPath))
            {
                return cookiesPath;
            }
            else if (Directory.Exists(operaUserFolder))
            {
                string[] files = Directory.GetFiles(operaUserFolder, "Cookies", SearchOption.AllDirectories);

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
            string operaUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Roaming\\Opera Software\\Opera Stable";
            string localStatePath = $"{operaUserFolder}\\Local State";

            if (File.Exists(localStatePath))
            {
                return localStatePath;
            }
            else if (Directory.Exists(operaUserFolder))
            {
                string[] files = Directory.GetFiles(operaUserFolder, "Local State", SearchOption.AllDirectories);

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
            string operaUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Roaming\\Opera Software\\Opera Stable";
            string loginDataPath = $"{operaUserFolder}\\Login Data";

            if (File.Exists(loginDataPath))
            {
                return loginDataPath;
            }
            else if (Directory.Exists(operaUserFolder))
            {
                string[] files = Directory.GetFiles(operaUserFolder, "Login Data", SearchOption.AllDirectories);

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
            string operaUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Roaming\\Opera Software\\Opera Stable";
            string historyPath = $"{operaUserFolder}\\History";

            if (File.Exists(historyPath))
            {
                return historyPath;
            }
            else if (Directory.Exists(operaUserFolder))
            {
                string[] files = Directory.GetFiles(operaUserFolder, "History", SearchOption.AllDirectories);

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
            string operaUserFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Roaming\\Opera Software\\Opera Stable";
            string bookmarkPath = $"{operaUserFolder}\\Bookmarks";

            if (File.Exists(bookmarkPath))
            {
                return bookmarkPath;
            }
            else if (Directory.Exists(operaUserFolder))
            {
                string[] files = Directory.GetFiles(operaUserFolder, "Bookmarks", SearchOption.AllDirectories);

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