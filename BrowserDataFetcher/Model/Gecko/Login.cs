namespace BrowserDataFetcher.Model.Gecko
{
    using System;

    /// <summary>
    /// The <see cref="Login"/>.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// The <see cref="Header"/>.
        /// </summary>
        public enum Header
        {
            /// <summary>
            /// The ID.
            /// </summary>
            id,

            /// <summary>
            /// The hostname.
            /// </summary>
            hostname,

            /// <summary>
            /// The HTTP realm.
            /// </summary>
            httpRealm,

            /// <summary>
            /// The form submit URL.
            /// </summary>
            formSubmitURL,

            /// <summary>
            /// The username field.
            /// </summary>
            usernameField,

            /// <summary>
            /// The password field.
            /// </summary>
            passwordField,

            /// <summary>
            /// The encrypted username.
            /// </summary>
            encryptedUsername,

            /// <summary>
            /// The encrypted password.
            /// </summary>
            encryptedPassword,

            /// <summary>
            /// The GUID.
            /// </summary>
            guid,

            /// <summary>
            /// The encryption type.
            /// </summary>
            encType,

            /// <summary>
            /// The time created.
            /// </summary>
            timeCreated,

            /// <summary>
            /// The time last used.
            /// </summary>
            timeLastUsed,

            /// <summary>
            /// The time password changed.
            /// </summary>
            timePasswordChanged,

            /// <summary>
            /// The times used.
            /// </summary>
            timesUsed,
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the HTTP realm.
        /// </summary>
        public string HttpRealm { get; set; }

        /// <summary>
        /// Gets or sets the form submit URL.
        /// </summary>
        public string FormSubmitURL { get; set; }

        /// <summary>
        /// Gets or sets the username field.
        /// </summary>
        public string UsernameField { get; set; }

        /// <summary>
        /// Gets or sets the password field.
        /// </summary>
        public string PasswordField { get; set; }

        /// <summary>
        /// Gets or sets the encrypted username.
        /// </summary>
        public string EncryptedUsername { get; set; }

        /// <summary>
        /// Gets or sets the decrypted username.
        /// </summary>
        public string DecryptedUsername { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password.
        /// </summary>
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the decrypted password.
        /// </summary>
        public string DecryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the encryption type.
        /// </summary>
        public short EncType { get; set; }

        /// <summary>
        /// Gets or sets the time created.
        /// </summary>
        public DateTimeOffset TimeCreated { get; set; }

        /// <summary>
        /// Gets or sets the time last used.
        /// </summary>
        public DateTimeOffset TimeLastUsed { get; set; }

        /// <summary>
        /// Gets or sets the time the password changed.
        /// </summary>
        public DateTimeOffset TimePasswordChanged { get; set; }

        /// <summary>
        /// Gets or sets the times used.
        /// </summary>
        public uint TimesUsed { get; set; }

        /// <summary>
        /// To string method override.
        /// </summary>
        /// <returns>
        /// A formatted string.
        /// </returns>
        public override string ToString()
        {
            return $"Hostname = '{Hostname}' | DecryptedUsername = '{DecryptedUsername}' | DecryptedPassword = '{DecryptedPassword}'";
        }
    }
}