namespace BrowserDataFetcher.Model.Chromium
{
    using System;

    /// <summary>
    /// The <see cref="Login"/> class.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// The <see cref="Header"/>.
        /// </summary>
        public enum Header
        {
            /// <summary>
            /// The origin URL.
            /// </summary>
            origin_url,

            /// <summary>
            /// The action URL.
            /// </summary>
            action_url,

            /// <summary>
            /// The username element.
            /// </summary>
            username_element,

            /// <summary>
            /// The username value.
            /// </summary>
            username_value,

            /// <summary>
            /// The password element.
            /// </summary>
            password_element,

            /// <summary>
            /// The password value.
            /// </summary>
            password_value,

            /// <summary>
            /// The submit element.
            /// </summary>
            submit_element,

            /// <summary>
            /// The signon realm.
            /// </summary>
            signon_realm,

            /// <summary>
            /// The date created.
            /// </summary>
            date_created,

            /// <summary>
            /// Blacklisted by user.
            /// </summary>
            blacklisted_by_user,

            /// <summary>
            /// The scheme.
            /// </summary>
            scheme,

            /// <summary>
            /// The password type.
            /// </summary>
            password_type,

            /// <summary>
            /// The times used.
            /// </summary>
            times_used,

            /// <summary>
            /// The form data.
            /// </summary>
            form_data,

            /// <summary>
            /// The display name.
            /// </summary>
            display_name,

            /// <summary>
            /// The icon URL.
            /// </summary>
            icon_url,

            /// <summary>
            /// The federation URL.
            /// </summary>
            federation_url,

            /// <summary>
            /// Skip zero click.
            /// </summary>
            skip_zero_click,

            /// <summary>
            /// The generation upload status.
            /// </summary>
            generation_upload_status,

            /// <summary>
            /// The possible username pairs.
            /// </summary>
            possible_username_pairs,

            /// <summary>
            /// The ID.
            /// </summary>
            id,

            /// <summary>
            /// The date last used.
            /// </summary>
            date_last_used,

            /// <summary>
            /// The moving blocked for.
            /// </summary>
            moving_blocked_for,

            /// <summary>
            /// The date password modified.
            /// </summary>
            date_password_modified
        }

        /// <summary>
        /// Gets or sets the origin URL.
        /// </summary>
        public string OriginUrl { get; set; }

        /// <summary>
        /// Gets or sets the action URL.
        /// </summary>
        public string ActionUrl { get; set; }

        /// <summary>
        /// Gets or sets the username element.
        /// </summary>
        public string UsernameElement { get; set; }

        /// <summary>
        /// Gets or sets the username value.
        /// </summary>
        public string UsernameValue { get; set; }

        /// <summary>
        /// Gets or sets the password element.
        /// </summary>
        public string PasswordElement { get; set; }

        /// <summary>
        /// Gets or sets the password value.
        /// </summary>
        public string PasswordValue { get; set; }

        /// <summary>
        /// Gets or sets the decrypted password value.
        /// </summary>
        public string DecryptedPasswordValue { get; set; }

        /// <summary>
        /// Gets or sets the submit element.
        /// </summary>
        public string SubmitElement { get; set; }

        /// <summary>
        /// Gets or sets the signon realm.
        /// </summary>
        public string SignonRealm { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is blacklisted by the user.
        /// </summary>
        public bool IsBlacklistedByUser { get; set; }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        public int Scheme { get; set; }

        /// <summary>
        /// Gets or sets the password type.
        /// </summary>
        public int PasswordType { get; set; }

        /// <summary>
        /// Gets or sets the times used.
        /// </summary>
        public int TimesUsed { get; set; }

        /// <summary>
        /// Gets or sets the form data.
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the icon URL.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the federation URL.
        /// </summary>
        public string FederationUrl { get; set; }

        /// <summary>
        /// Gets or sets the skip zero click.
        /// </summary>
        public int SkipZeroClick { get; set; }

        /// <summary>
        /// Gets or sets the generation upload status.
        /// </summary>
        public int GenerationUploadStatus { get; set; }

        /// <summary>
        /// Gets or sets the possible username pairs.
        /// </summary>
        public string PossibleUsernamePairs { get; set; }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date last used.
        /// </summary>
        public DateTimeOffset DateLastUsed { get; set; }

        /// <summary>
        /// Gets or sets the moving blocked for.
        /// </summary>
        public string MovingBlockedFor { get; set; }

        /// <summary>
        /// Gets or sets the date password modified.
        /// </summary>
        public DateTimeOffset DatePasswordModified { get; set; }

        /// <summary>
        /// To string method override.
        /// </summary>
        /// <returns>
        /// A formatted string.
        /// </returns>
        public override string ToString()
        {
            return $"OriginUrl = '{OriginUrl}' | UsernameValue = '{UsernameValue}' | DecryptedPasswordValue = '{DecryptedPasswordValue}'";
        }
    }
}