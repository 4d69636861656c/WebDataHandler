namespace BrowserDataFetcher.Engine
{
    using BrowserDataFetcher.Helpers;
    using BrowserDataFetcher.Model.Gecko;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;

    /// <summary>
    /// The <see cref="GeckoDataHandler"/> class. This is the base type for all Gecko-based user data handlers.
    /// </summary>
    public abstract class GeckoDataHandler : IGeckoEngine
    {
        #region Private Fields

        /// <summary>
        /// The SQL command for retrieving cookies.
        /// </summary>
        private const string CookieSqlCommand = "SELECT id,originAttributes,name,value,host,path,expiry,lastAccessed,creationTime,isSecure,isHttpOnly,inBrowserElement,sameSite,rawSameSite,schemeMap FROM moz_cookies";

        /// <summary>
        /// The SQL command for retrieving the browser user history.
        /// </summary>
        private const string HistorySqlCommand = "SELECT id,url,title,rev_host,visit_count,hidden,typed,frecency,last_visit_date,guid,foreign_count,url_hash,description,preview_image_url,origin_id,site_name FROM moz_places";

        /// <summary>
        /// The SQL command for retrieving browser bookmarks.
        /// </summary>
        private const string BookmarksSqlCommand = "SELECT id,type,fk,parent,position,title,keyword_id,folder_type,dateAdded,lastModified,guid,syncStatus,syncChangeCounter FROM moz_bookmarks";

        /// <summary>
        /// The SQL command for retrieving browser download data.
        /// </summary>
        private const string DownloadSqlCommand = "SELECT id,place_id,anno_attribute_id,content,flags,expiration,type,dateAdded,lastModified FROM moz_annos";

        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private readonly JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The <c>Profiles</c> directory path.
        /// </summary>
        public virtual string ProfilesPath { get; set; }

        /// <summary>
        /// The <c>Cookies</c> database file path.
        /// </summary>
        public virtual string CookiesPath { get; set; }

        /// <summary>
        /// The <c>Login</c> JSON file path.
        /// </summary>
        public virtual string LoginsPath { get; set; }

        /// <summary>
        /// The <c>Places</c> database file path.
        /// </summary>
        public virtual string PlacesPath { get; set; }

        /// <summary>
        /// The <c>Profiles</c> file paths array.
        /// </summary>
        public virtual string[] Profiles { get; set; }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        /// The <see cref="GeckoDataHandler"/> constructor.
        /// </summary>
        protected GeckoDataHandler()
        {
            _jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Gets a value indicating whether the cookies database file for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the cookies database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool CookiesExist()
        {
            return File.Exists(this.ProfilesPath + this.CookiesPath);
        }

        /// <summary>
        /// Gets a value indicating whether the logins JSON file for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the logins JSON file exists, <c>false</c> otherwise.
        /// </returns>
        public bool LoginsExist()
        {
            return File.Exists(this.ProfilesPath + this.LoginsPath);
        }

        /// <summary>
        /// Gets a value indicating whether the history database file for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the history database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool HistoryExists()
        {
            return File.Exists(this.ProfilesPath + this.PlacesPath);
        }

        /// <summary>
        /// Gets a value indicating whether the bookmarks database file for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the bookmarks database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool BookmarksExist()
        {
            return File.Exists(this.ProfilesPath + this.PlacesPath);
        }

        /// <summary>
        /// Gets a value indicating whether the download data for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the download data exists, <c>false</c> otherwise.
        /// </returns>
        public bool DownloadsExist()
        {
            return File.Exists(this.ProfilesPath + this.PlacesPath);
        }

        /// <summary>
        /// Gets a value indicating whether the web data for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the web data exists, <c>false</c> otherwise.
        /// </returns>
        public bool WebDataExist()
        {
            return File.Exists(this.ProfilesPath + this.PlacesPath);
        }

        /// <summary>
        /// Gets a value indicating whether the primary key for a specific profile exists.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the primary key exists, <c>false</c> otherwise.
        /// </returns>
        public bool KeyExists()
        {
            return File.Exists(this.ProfilesPath + this.PlacesPath);
        }

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
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Cookie> GetCookiesBy(Cookie.Header by, object value)
        {
            List<Cookie> cookies = new List<Cookie>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (string profile in this.Profiles)
            {
                if (!LoginsExist())
                {
                    throw new BrowserEngineException(BrowserEngineError.CookiesNotFound, $"The Cookie database could not be found: {CookieSqlCommand}");
                }

                string tempFile = CopyToTemporaryFile(profile + this.CookiesPath);

                using (var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={tempFile};pooling=false"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"{CookieSqlCommand} WHERE {by} = '{value}'";

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cookies.Add(new Cookie()
                            {
                                Id = reader.GetInt32(0),
                                OriginAttributes = reader.GetString(1),
                                Name = reader.GetString(2),
                                Value = reader.GetString(3),
                                Host = reader.GetString(4),
                                Path = reader.GetString(5),
                                Expiry = reader.GetInt64(6).UnixTimeInSecondsToDate(),
                                LastAccessed = reader.GetInt64(7).UnixTimeInMicrosecondsToDate(),
                                CreationTime = reader.GetInt64(8).UnixTimeInMicrosecondsToDate(),
                                IsSecure = reader.GetBoolean(9),
                                IsHttpOnly = reader.GetBoolean(10),
                                InBrowserElement = reader.GetBoolean(11),
                                SameSite = reader.GetInt16(12),
                                RawSameSite = reader.GetInt16(13),
                                SchemeMap = reader.GetInt16(14),
                            });
                        }
                    }
                    conn.Dispose();
                }

                File.Delete(tempFile);
            }

            return cookies;
        }

        /// <summary>
        /// Gets all cookies for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Cookie"/> data.
        /// </returns>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Cookie> GetCookies()
        {
            List<Cookie> cookies = new List<Cookie>();

            foreach (string profile in this.Profiles)
            {
                if (!CookiesExist())
                {
                    throw new BrowserEngineException(BrowserEngineError.CookiesNotFound, $"The Cookie database could not be found: {CookieSqlCommand}");
                }

                string tempFile = CopyToTemporaryFile(profile + this.CookiesPath);

                using (var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={tempFile};pooling=false"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = CookieSqlCommand;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cookies.Add(new Cookie()
                            {
                                Id = reader.GetInt32(0),
                                OriginAttributes = reader.GetString(1),
                                Name = reader.GetString(2),
                                Value = reader.GetString(3),
                                Host = reader.GetString(4),
                                Path = reader.GetString(5),
                                Expiry = (reader.GetInt64(6) * 1000).UnixTimeInSecondsToDate(),
                                LastAccessed = reader.GetInt64(7).UnixTimeInMicrosecondsToDate(),
                                CreationTime = reader.GetInt64(8).UnixTimeInMicrosecondsToDate(),
                                IsSecure = reader.GetBoolean(9),
                                IsHttpOnly = reader.GetBoolean(10),
                                InBrowserElement = reader.GetBoolean(11),
                                SameSite = reader.GetInt16(12),
                                RawSameSite = reader.GetInt16(13),
                                SchemeMap = reader.GetInt16(14),
                            });
                        }
                    }
                    conn.Dispose();
                }

                File.Delete(tempFile);
            }

            return cookies;
        }

        /// <summary>
        /// Gets all login data for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Login"/> data.
        /// </returns>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Login> GetLogins()
        {
            List<Login> loginDataList = new List<Login>();

            string programFiles = Environment.GetEnvironmentVariable("ProgramW6432");
            if (string.IsNullOrEmpty(programFiles))
            {
                programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }

            GeckoDecryptor.LoadNSS(programFiles + @"\Mozilla Firefox"); // Load NSS
            foreach (string profile in this.Profiles)
            {
                if (!LoginsExist())
                {
                    throw new BrowserEngineException(BrowserEngineError.LoginsNotFound, $"The Login File could not be found: {this.LoginsPath}");
                }

                if (!GeckoDecryptor.SetProfile(profile))
                {
                    throw new BrowserEngineException(BrowserEngineError.CouldNotSetProfile, $"Profile could not be set: {profile}");
                }

                dynamic json = _jsonSerializer.Deserialize(File.ReadAllText(profile + this.LoginsPath), typeof(object));
                List<Login> logins = ConvertDynamicObjectsToLogins(json.logins);

                loginDataList.AddRange(logins);
            }

            GeckoDecryptor.UnLoadNSS();

            return loginDataList;
        }

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
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Login> GetLoginsBy(Login.Header by, object value)
        {
            List<Login> loginDataList = new List<Login>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            string programFiles = Environment.GetEnvironmentVariable("ProgramW6432");
            if (string.IsNullOrEmpty(programFiles))
            {
                programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }

            GeckoDecryptor.LoadNSS(programFiles + @"\Mozilla Firefox");
            foreach (string profile in this.Profiles)
            {
                if (!LoginsExist())
                {
                    throw new BrowserEngineException(BrowserEngineError.LoginsNotFound, $"The Login File could not be found: {this.LoginsPath}");
                }

                if (!GeckoDecryptor.SetProfile(profile))
                {
                    throw new BrowserEngineException(BrowserEngineError.CouldNotSetProfile, $"Profile could not be set: {profile}");
                }

                dynamic json = _jsonSerializer.Deserialize(File.ReadAllText(profile + this.LoginsPath), typeof(object));
                foreach (Login l in ConvertDynamicObjectsToLogins(json.logins))
                {
                    switch (by)
                    {
                        case Login.Header.id:
                            if (l.Id == (int)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.hostname:
                            if (l.Hostname == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.httpRealm:
                            if (l.HttpRealm == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.formSubmitURL:
                            if (l.FormSubmitURL == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.usernameField:
                            if (l.UsernameField == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.passwordField:
                            if (l.PasswordField == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.encryptedUsername:
                            if (l.EncryptedUsername == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.encryptedPassword:
                            if (l.EncryptedPassword == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.guid:
                            if (l.Guid == (string)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.encType:
                            if (l.EncType == (short)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.timeCreated:
                            if (l.TimeCreated == ((long)value).UnixTimeInMillisecondsToDate())
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.timeLastUsed:
                            if (l.TimeLastUsed == ((long)value).UnixTimeInMillisecondsToDate())
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.timePasswordChanged:
                            if (l.TimePasswordChanged == ((long)value).UnixTimeInMillisecondsToDate())
                            {
                                loginDataList.Add(l);
                            }
                            break;

                        case Login.Header.timesUsed:
                            if (l.TimesUsed == (int)value)
                            {
                                loginDataList.Add(l);
                            }
                            break;
                    }
                }

                GeckoDecryptor.UnLoadNSS();
            }

            return loginDataList;
        }

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
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Site> GetHistoryBy(Site.Header by, object value)
        {
            List<Site> history = new List<Site>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (string profile in this.Profiles)
            {
                if (!HistoryExists())
                {
                    throw new BrowserEngineException(BrowserEngineError.HistoryNotFound, $"The History database could not be found: {profile + this.PlacesPath}");
                }

                string tempFile = CopyToTemporaryFile(profile + this.PlacesPath);

                using (var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={tempFile};pooling=false"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"{HistorySqlCommand} WHERE {by} = '{value}'";

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new Site
                            {
                                Id = reader.GetInt32(0),
                                Url = reader.GetString(1),
                                Title = reader[2].Equals(DBNull.Value) ? null : reader.GetString(2),
                                RevHost = reader.GetString(3),
                                VisitCount = reader.GetInt32(4),
                                IsHidden = reader.GetBoolean(5),
                                IsTyped = reader.GetBoolean(6),
                                Frecency = reader.GetInt32(7),
                                LastVisitDate = reader[8].Equals(DBNull.Value) ? DateTimeOffset.MinValue : reader.GetInt64(8).UnixTimeInMicrosecondsToDate(),
                                Guid = reader.GetString(9),
                                ForeignCount = reader.GetInt32(10),
                                UrlHash = reader.GetInt64(11),
                                Description = reader[12].Equals(DBNull.Value) ? null : reader.GetString(12),
                                PreviewImageUrl = reader[13].Equals(DBNull.Value) ? null : reader.GetString(13),
                                Originid = reader.GetInt32(14),
                                SiteName = reader[15].Equals(DBNull.Value) ? null : reader.GetString(15),
                            });
                        }
                    }
                    conn.Dispose();
                }

                File.Delete(tempFile);
            }

            return history;
        }

        /// <summary>
        /// Gets all site history for Gecko-based browsers.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Gecko.Site"/> data.
        /// </returns>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Site> GetHistory()
        {
            List<Site> history = new List<Site>();

            foreach (string profile in this.Profiles)
            {
                if (!HistoryExists())
                {
                    throw new BrowserEngineException(BrowserEngineError.HistoryNotFound, $"The History database could not be found: {profile + this.PlacesPath}");
                }

                string tempFile = CopyToTemporaryFile(profile + this.PlacesPath);

                using (var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={tempFile};pooling=false"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = HistorySqlCommand;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new Site()
                            {
                                Id = reader.GetInt32(0),
                                Url = reader.GetString(1),
                                Title = reader[2].Equals(DBNull.Value) ? null : reader.GetString(2),
                                RevHost = reader.GetString(3),
                                VisitCount = reader.GetInt32(4),
                                IsHidden = reader.GetBoolean(5),
                                IsTyped = reader.GetBoolean(6),
                                Frecency = reader.GetInt32(7),
                                LastVisitDate = reader[8].Equals(DBNull.Value) ? DateTimeOffset.MinValue : reader.GetInt64(8).UnixTimeInMicrosecondsToDate(),
                                Guid = reader.GetString(9),
                                ForeignCount = reader.GetInt32(10),
                                UrlHash = reader.GetInt64(11),
                                Description = reader[12].Equals(DBNull.Value) ? null : reader.GetString(12),
                                PreviewImageUrl = reader[13].Equals(DBNull.Value) ? null : reader.GetString(13),
                                Originid = reader.GetInt32(14),
                                SiteName = reader[15].Equals(DBNull.Value) ? null : reader.GetString(15),
                            });
                        }
                    }
                    conn.Dispose();
                }

                File.Delete(tempFile);
            }

            return history;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Copies a file to a temporary location.
        /// </summary>
        /// <param name="path">
        /// Path to the file that will be copied to a temporary location.
        /// </param>
        /// <returns>
        /// The path to the temporary file location.
        /// </returns>
        private string CopyToTemporaryFile(string path)
        {
            string tempFilePath = GetTemporaryFilePath();

            if (File.Exists(tempFilePath))
            {
                return CopyToTemporaryFile(path);
            }

            File.Copy(path, tempFilePath);

            return tempFilePath;
        }

        /// <summary>
        /// Creates a path to a temporary file.
        /// </summary>
        /// <returns>
        /// The path to the temporary file.
        /// </returns>
        private string GetTemporaryFilePath()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Converts passed dynamic object list to a list of <see cref="Model.Gecko.Login"/> data.
        /// </summary>
        /// <param name="logins">
        /// The dynamic JSON object.
        /// </param>
        /// <returns>
        /// A list of <see cref="Model.Gecko.Login"/> data.
        /// </returns>
        private static List<Login> ConvertDynamicObjectsToLogins(List<object> logins)
        {
            List<Login> loginDataList = new List<Login>();

            foreach (dynamic obj in logins)
            {
                loginDataList.Add(new Login
                {
                    Id = obj.id,
                    Hostname = (string)obj.hostname,
                    HttpRealm = (string)obj.httpRealm,
                    FormSubmitURL = (string)obj.formSubmitURL,
                    UsernameField = (string)obj.usernameField,
                    PasswordField = (string)obj.passwordField,
                    EncryptedUsername = (string)obj.encryptedUsername,
                    DecryptedUsername = GeckoDecryptor.DecryptValue((string)obj.encryptedUsername),
                    EncryptedPassword = (string)obj.encryptedPassword,
                    DecryptedPassword = GeckoDecryptor.DecryptValue((string)obj.encryptedPassword),
                    Guid = (string)obj.guid,
                    EncType = (short)obj.encType,
                    TimeCreated = ((long)obj.timeCreated).UnixTimeInMillisecondsToDate(),
                    TimeLastUsed = ((long)obj.timeLastUsed).UnixTimeInMillisecondsToDate(),
                    TimePasswordChanged = ((long)obj.timePasswordChanged).UnixTimeInMillisecondsToDate(),
                    TimesUsed = (uint)obj.timesUsed,
                });
            }

            return loginDataList;
        }

        #endregion Private Methods
    }
}