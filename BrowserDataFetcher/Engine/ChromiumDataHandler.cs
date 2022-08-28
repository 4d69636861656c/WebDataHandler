namespace BrowserDataFetcher.Engine
{
    using BrowserDataFetcher.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;

    /// <summary>
    /// The <see cref="ChromiumDataHandler"/> class. This is the base type for all Chromium-based user data handlers.
    /// </summary>
    public abstract class ChromiumDataHandler : IChromiumEngine
    {
        #region Private Fields

        /// <summary>
        /// The SQL command for retrieving cookies.
        /// </summary>
        private const string CookieSqlCommand = "SELECT creation_utc,top_frame_site_key,host_key,name,value,encrypted_value,path,expires_utc,is_secure,is_httponly,last_access_utc,has_expires,is_persistent,priority,samesite,source_scheme,source_port,is_same_party FROM cookies";

        /// <summary>
        /// The SQL command for retrieving login data.
        /// </summary>
        private const string LoginSqlCommand = "SELECT origin_url,action_url,username_element,username_value,password_element,password_value,submit_element,signon_realm,date_created,blacklisted_by_user,scheme,password_type,times_used,form_data,display_name,icon_url,federation_url,skip_zero_click,generation_upload_status,possible_username_pairs,id,date_last_used,moving_blocked_for,date_password_modified FROM logins";

        /// <summary>
        /// The SQL command for retrieving history.
        /// </summary>
        private const string HistorySqlCommand = "SELECT id,url,title,visit_count,typed_count,last_visit_time,hidden FROM urls";

        /// <summary>
        /// The SQL command for retrieving download data.
        /// </summary>
        private const string DownloadSqlCommand = "SELECT id,guid,current_path,target_path,start_time,received_bytes,total_bytes,state,danger_type,interrupt_reason,hash,end_time,opened,last_access_time,transient,referrer,site_url,tab_url,tab_referrer_url,http_method,by_ext_id,by_ext_name,etag,last_modified,mime_type,original_mime_type,embedder_download_data FROM downloads";

        /// <summary>
        /// The SQL command for retrieving auto fills.
        /// </summary>
        private const string AutoFillsCommandText = "SELECT name,value FROM autofill";

        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private readonly JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the cookie path.
        /// </summary>
        public virtual string CookiePath { get; set; }

        /// <summary>
        /// Gets the local state path.
        /// </summary>
        public virtual string LocalStatePath { get; set; }

        /// <summary>
        /// Gets the login data path.
        /// </summary>
        public virtual string LoginDataPath { get; set; }

        /// <summary>
        /// Gets the history path.
        /// </summary>
        public virtual string HistoryPath { get; set; }

        /// <summary>
        /// Gets the bookmark path.
        /// </summary>
        public virtual string BookmarkPath { get; set; }

        /// <summary>
        /// Gets the web data path.
        /// </summary>
        public virtual string WebDataPath { get; set; }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        /// The <see cref="ChromiumDataHandler"/> constructor.
        /// </summary>
        protected ChromiumDataHandler()
        {
            this._jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Gets a boolean value indicating whether the database with the stored cookies was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the cookie database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool CookiesExist()
        {
            return File.Exists(this.CookiePath);
        }

        /// <summary>
        /// Gets a value indicating whether the database with the stored logins was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser logins database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool LoginsExist()
        {
            return File.Exists(this.LoginDataPath);
        }

        /// <summary>
        /// Gets a value indicating whether the database with the stored browser history was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser history database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool HistoryExists()
        {
            return File.Exists(this.HistoryPath);
        }

        /// <summary>
        /// Gets a value indicating whether the database with the stored bookmarks was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser bookmarks database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool BookmarksExist()
        {
            return File.Exists(this.BookmarkPath);
        }

        /// <summary>
        /// Gets a value indicating whether the database file web data was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the browser web data database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool WebDataExist()
        {
            return File.Exists(this.WebDataPath);
        }

        /// <summary>
        /// Gets a value indicating whether the downloads database file was found or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the downloads database file exists, <c>false</c> otherwise.
        /// </returns>
        public bool DownloadsExist()
        {
            return File.Exists(this.HistoryPath);
        }

        /// <summary>
        /// Returns a value indicating whether the file that stores the decryption key was found.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the decryption key exists, <c>false</c> otherwise.
        /// </returns>
        public bool KeyExists()
        {
            return File.Exists(this.LocalStatePath);
        }

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
        public IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value)
        {
            return this.GetCookiesBy(by, value, this.GetKey());
        }

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
        public IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value, byte[] key)
        {
            return this.GetCookiesBy(by, value, new KeyParameter(key));
        }

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
        public IEnumerable<Model.Chromium.Cookie> GetCookiesBy(Model.Chromium.Cookie.Header by, object value, KeyParameter key)
        {
            List<Model.Chromium.Cookie> cookies = new List<Model.Chromium.Cookie>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!this.CookiesExist())
            {
                throw new BrowserEngineException(BrowserEngineError.CookiesNotFound, $"The Cookie database could not be found: {this.CookiePath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.CookiePath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"{CookieSqlCommand} WHERE {by} = '{value}'";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cookies.Add(new Model.Chromium.Cookie()
                        {
                            CreationUTC = reader.GetInt64(0).WebKitTimeStampToDateTime(),
                            TopFrameSiteKey = reader.GetString(1),
                            HostKey = reader.GetString(2),
                            Name = reader.GetString(3),
                            Value = reader.GetString(4),
                            EncryptedValue = reader.GetString(5),
                            DecryptedValue = ChromiumDecryptor.DecryptValue((byte[])reader[5], key),
                            Path = reader.GetString(6),
                            ExpiresUTC = reader.GetInt64(7).WebKitTimeStampToDateTime(),
                            IsSecure = reader.GetBoolean(8),
                            IsHttpOnly = reader.GetBoolean(9),
                            LastAccessUTC = reader.GetInt64(10).WebKitTimeStampToDateTime(),
                            HasExpires = reader.GetBoolean(11),
                            IsPersistent = reader.GetBoolean(12),
                            Priority = reader.GetInt16(13),
                            Samesite = reader.GetInt16(14),
                            SourceScheme = reader.GetInt16(15),
                            SourcePort = reader.GetInt32(16),
                            IsSameParty = reader.GetBoolean(17),
                        });
                    }
                }
                conn.Close();
            }

            File.Delete(tempFile);

            return cookies;
        }

        /// <summary>
        /// Gets all cookies.
        /// </summary>
        /// <returns>
        /// The list of <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Cookie> GetCookies()
        {
            return this.GetCookies(this.GetKey());
        }

        /// <summary>
        /// Get cookies by using the key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Cookie> GetCookies(byte[] key)
        {
            return this.GetCookies(new KeyParameter(key));
        }

        /// <summary>
        /// Gets cookies by key parameter.
        /// </summary>
        /// <param name="key">
        /// The <see cref="KeyParameter"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Cookie"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Cookie> GetCookies(KeyParameter key)
        {
            List<Model.Chromium.Cookie> cookies = new List<Model.Chromium.Cookie>();

            if (!this.CookiesExist())
            {
                throw new BrowserEngineException(BrowserEngineError.CookiesNotFound, $"The Cookie database could not be found: {this.CookiePath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.CookiePath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = CookieSqlCommand;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cookies.Add(new Model.Chromium.Cookie()
                        {
                            CreationUTC = reader.GetInt64(0).WebKitTimeStampToDateTime(),
                            TopFrameSiteKey = reader.GetString(1),
                            HostKey = reader.GetString(2),
                            Name = reader.GetString(3),
                            Value = reader.GetString(4),
                            EncryptedValue = reader.GetString(5),
                            DecryptedValue = ChromiumDecryptor.DecryptValue((byte[])reader[5], key),
                            Path = reader.GetString(6),
                            ExpiresUTC = reader.GetInt64(7).WebKitTimeStampToDateTime(),
                            IsSecure = reader.GetBoolean(8),
                            IsHttpOnly = reader.GetBoolean(9),
                            LastAccessUTC = reader.GetInt64(10).WebKitTimeStampToDateTime(),
                            HasExpires = reader.GetBoolean(11),
                            IsPersistent = reader.GetBoolean(12),
                            Priority = reader.GetInt16(13),
                            Samesite = reader.GetInt16(14),
                            SourceScheme = reader.GetInt16(15),
                            SourcePort = reader.GetInt32(16),
                            IsSameParty = reader.GetBoolean(17),
                        });
                    }
                }
                conn.Close();
            }

            File.Delete(tempFile);

            return cookies;
        }

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
        public IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value)
        {
            return this.GetLoginsBy(by, value, this.GetKey());
        }

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
        public IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value, byte[] key)
        {
            return this.GetLoginsBy(by, value, new KeyParameter(key));
        }

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
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Model.Chromium.Login> GetLoginsBy(Model.Chromium.Login.Header by, object value, KeyParameter key)
        {
            List<Model.Chromium.Login> password = new List<Model.Chromium.Login>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!this.LoginsExist())
            {
                throw new BrowserEngineException(BrowserEngineError.LoginsNotFound, $"The Login database could not be found: {this.LoginDataPath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.LoginDataPath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"{LoginSqlCommand} WHERE {by} = '{value}'";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        password.Add(new Model.Chromium.Login()
                        {
                            OriginUrl = reader.GetString(0),
                            ActionUrl = reader.GetString(1),
                            UsernameElement = reader.GetString(2),
                            UsernameValue = reader.GetString(3),
                            PasswordElement = reader.GetString(4),
                            PasswordValue = reader.GetString(5),
                            DecryptedPasswordValue = ChromiumDecryptor.DecryptValue((byte[])reader[5], key),
                            SubmitElement = reader.GetString(6),
                            SignonRealm = reader.GetString(7),
                            DateCreated = reader.GetInt64(8).WebKitTimeStampToDateTime(),
                            IsBlacklistedByUser = reader.GetBoolean(9),
                            Scheme = reader.GetInt32(10),
                            PasswordType = reader.GetInt32(11),
                            TimesUsed = reader.GetInt32(12),
                            FormData = ChromiumDecryptor.DecryptValue((byte[])reader[13], key),
                            DisplayName = reader.GetString(14),
                            IconUrl = reader.GetString(15),
                            FederationUrl = reader.GetString(16),
                            SkipZeroClick = reader.GetInt32(17),
                            GenerationUploadStatus = reader.GetInt32(18),
                            PossibleUsernamePairs = ChromiumDecryptor.DecryptValue((byte[])reader[19], key),
                            Id = reader.GetInt32(20),
                            DateLastUsed = reader.GetInt64(21).WebKitTimeStampToDateTime(),
                            MovingBlockedFor = ChromiumDecryptor.DecryptValue((byte[])reader[22], key),
                            DatePasswordModified = reader.GetInt64(23).WebKitTimeStampToDateTime(),
                        });
                    }
                }
                conn.Close();
            }

            File.Delete(tempFile);

            return password;
        }

        /// <summary>
        /// Gets all the login data.
        /// </summary>
        /// <returns>
        /// A list of <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Login> GetLogins()
        {
            return this.GetLogins(this.GetKey());
        }

        /// <summary>
        /// Gets login data by key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Login> GetLogins(byte[] key)
        {
            return this.GetLogins(new KeyParameter(key));
        }

        /// <summary>
        /// Gets login data by key.
        /// </summary>
        /// <param name="key">
        /// The <see cref="KeyParameter"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Chromium.Login"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Login> GetLogins(KeyParameter key)
        {
            List<Model.Chromium.Login> password = new List<Model.Chromium.Login>();

            if (!this.LoginsExist())
            {
                throw new BrowserEngineException(BrowserEngineError.LoginsNotFound, $"The Login database could not be found: {this.LoginDataPath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.LoginDataPath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = LoginSqlCommand;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        password.Add(new Model.Chromium.Login()
                        {
                            OriginUrl = reader.GetString(0),
                            ActionUrl = reader.GetString(1),
                            UsernameElement = reader.GetString(2),
                            UsernameValue = reader.GetString(3),
                            PasswordElement = reader.GetString(4),
                            PasswordValue = reader.GetString(5),
                            DecryptedPasswordValue = ChromiumDecryptor.DecryptValue((byte[])reader[5], key),
                            SubmitElement = reader.GetString(6),
                            SignonRealm = reader.GetString(7),
                            DateCreated = reader.GetInt64(8).WebKitTimeStampToDateTime(),
                            IsBlacklistedByUser = reader.GetBoolean(9),
                            Scheme = reader.GetInt32(10),
                            PasswordType = reader.GetInt32(11),
                            TimesUsed = reader.GetInt32(12),
                            FormData = ChromiumDecryptor.DecryptValue((byte[])reader[13], key),
                            DisplayName = reader.GetString(14),
                            IconUrl = reader.GetString(15),
                            FederationUrl = reader.GetString(16),
                            SkipZeroClick = reader.GetInt32(17),
                            GenerationUploadStatus = reader.GetInt32(18),
                            PossibleUsernamePairs = ChromiumDecryptor.DecryptValue((byte[])reader[19], key),
                            Id = reader.GetInt32(20),
                            DateLastUsed = reader.GetInt64(21).WebKitTimeStampToDateTime(),
                            MovingBlockedFor = ChromiumDecryptor.DecryptValue((byte[])reader[22], key),
                            DatePasswordModified = reader.GetInt64(23).WebKitTimeStampToDateTime(),
                        });
                    }
                }
                conn.Close();
            }
            File.Delete(tempFile);

            return password;
        }

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
        /// <exception cref="ArgumentNullException">
        /// The <see cref="ArgumentNullException"/>.
        /// </exception>
        /// <exception cref="BrowserEngineException">
        /// The <see cref="BrowserEngineException"/>.
        /// </exception>
        public IEnumerable<Model.Chromium.Site> GetHistoryBy(Model.Chromium.Site.Header by, object value)
        {
            List<Model.Chromium.Site> history = new List<Model.Chromium.Site>();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!this.HistoryExists())
            {
                throw new BrowserEngineException(BrowserEngineError.HistoryNotFound, $"The History database could not be found: {this.HistoryPath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.HistoryPath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"{HistorySqlCommand} WHERE {by} = '{value}'";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new Model.Chromium.Site()
                        {
                            Id = reader.GetInt32(0),
                            Url = reader.GetString(1),
                            Title = reader.GetString(2),
                            VisitCount = reader.GetInt32(3),
                            TypedCount = reader.GetInt32(4),
                            LastVisitTime = reader.GetInt64(5).WebKitTimeStampToDateTime(),
                            IsHidden = reader.GetBoolean(6),
                        });
                    }
                }
                conn.Close();
            }

            File.Delete(tempFile);

            return history;
        }

        /// <summary>
        /// Gets browser history.
        /// </summary>
        /// <returns>
        /// The <see cref="Model.Chromium.Site"/> data.
        /// </returns>
        public IEnumerable<Model.Chromium.Site> GetHistory()
        {
            List<Model.Chromium.Site> history = new List<Model.Chromium.Site>();

            if (!this.LoginsExist())
            {
                throw new BrowserEngineException(BrowserEngineError.HistoryNotFound, $"The History database could not be found: {this.HistoryPath}");
            }

            string tempFile = this.CopyToTemporaryFile(this.HistoryPath);

            using (var conn = new SQLiteConnection($"Data Source={tempFile};pooling=false"))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = HistorySqlCommand;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new Model.Chromium.Site()
                        {
                            // Store retrieved information:
                            Id = reader.GetInt32(0),
                            Url = reader.GetString(1),
                            Title = reader.GetString(2),
                            VisitCount = reader.GetInt32(3),
                            TypedCount = reader.GetInt32(4),
                            LastVisitTime = reader.GetInt64(5).WebKitTimeStampToDateTime(),
                            IsHidden = reader.GetBoolean(6),
                        });
                    }
                }
                conn.Close();
            }

            File.Delete(tempFile);

            return history;
        }

        /// <summary>
        /// Returns the Chromium primary key to decrypt database values.
        /// </summary>
        public byte[] GetKey()
        {
            if (!this.KeyExists())
            {
                throw new BrowserEngineException(BrowserEngineError.LocalStateNotFound, "The Key for decryption (Local State) could not be found: " + this.LocalStatePath);
            }

            return DataProtectionApi.Decrypt(Convert.FromBase64String((Regex.Match(
                File.ReadAllText(this.LocalStatePath),
                "\"os_crypt\"\\s*:\\s*\\{\\s*.*?(?=\"encrypted_key)\"encrypted_key\"\\s*:\\s*\"(?<encKey>.*?)\"\\s*\\}").Groups["encKey"]).Value).Skip(5).ToArray());
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Copies a file to a temporary location.
        /// </summary>
        /// <param name="path">
        /// Path to the file that should be copied to a temporary location.
        /// </param>
        /// <returns>
        /// The path to the temp file.
        /// </returns>
        private string CopyToTemporaryFile(string path)
        {
            string tempFilePath = this.GetTemporaryFilePath();

            if (File.Exists(tempFilePath))
            {
                return this.CopyToTemporaryFile(path);
            }

            File.Copy(path, tempFilePath);

            return tempFilePath;
        }

        /// <summary>
        /// Create a path to a temporary file.
        /// </summary>
        /// <returns>
        /// The path to the temporary file.
        /// </returns>
        private string GetTemporaryFilePath()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        #endregion Private Methods
    }
}