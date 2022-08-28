namespace BrowserDataFetcher.Engine
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The <see cref="FirefoxDataHandler"/> class.
    /// </summary>
    public class FirefoxDataHandler : GeckoDataHandler
    {
        #region Public Properties

        /// <inheritdoc />
        public override string ProfilesPath
        {
            get
            {
                return GetProfilesPath();
            }
        }

        /// <inheritdoc />
        public override string CookiesPath
        {
            get
            {
                return "\\cookies.sqlite";
            }
        }

        /// <inheritdoc />
        public override string LoginsPath
        {
            get
            {
                return "\\logins.json";
            }
        }

        /// <inheritdoc />
        public override string PlacesPath
        {
            get
            {
                return "\\places.sqlite";
            }
        }

        /// <inheritdoc />
        public override string[] Profiles
        {
            get
            {
                return GetProfiles();
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets the <c>Profiles</c> directory path.
        /// </summary>
        /// <returns>The <c>Profiles</c> directory or <see cref="string.Empty"/> if not found.</returns>
        private string GetProfilesPath()
        {
            string firefoxProfile = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Mozilla\\Firefox\\Profiles";

            if (Directory.Exists(firefoxProfile))
            {
                return firefoxProfile;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the <c>Profiles</c> file paths array.
        /// </summary>
        /// <returns>The <c>Profiles</c> file array or an empty array if not found.</returns>
        private string[] GetProfiles()
        {
            return Directory.GetDirectories(this.ProfilesPath)
                .Where(str => File.Exists(str + this.CookiesPath) && File.Exists(str + "\\logins.json")).ToArray();
        }

        #endregion Private Methods
    }
}