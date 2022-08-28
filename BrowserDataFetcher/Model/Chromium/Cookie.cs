namespace BrowserDataFetcher.Model.Chromium
{
    using System;

    /// <summary>
    /// The <see cref="Cookie"/>.
    /// </summary>
    public class Cookie
    {
        /// <summary>
        /// The <see cref="Header"/>.
        /// </summary>
        public enum Header
        {
            /// <summary>
            /// The creation UTC.
            /// </summary>
            creation_utc,

            /// <summary>
            /// The top frame site key.
            /// </summary>
            top_frame_site_key,

            /// <summary>
            /// The host key.
            /// </summary>
            host_key,

            /// <summary>
            /// The name.
            /// </summary>
            name,

            /// <summary>
            /// The value.
            /// </summary>
            value,

            /// <summary>
            /// The encrypted value.
            /// </summary>
            encrypted_value,

            /// <summary>
            /// The path.
            /// </summary>
            path,

            /// <summary>
            /// The expires UTC.
            /// </summary>
            expires_utc,

            /// <summary>
            /// Is secure.
            /// </summary>
            is_secure,

            /// <summary>
            /// Is HTTP-only.
            /// </summary>
            is_httponly,

            /// <summary>
            /// The last access utc.
            /// </summary>
            last_access_utc,

            /// <summary>
            /// Has expires.
            /// </summary>
            has_expires,

            /// <summary>
            /// Is persistent.
            /// </summary>
            is_persistent,

            /// <summary>
            /// The priority.
            /// </summary>
            priority,

            /// <summary>
            /// Same site.
            /// </summary>
            samesite,

            /// <summary>
            /// The source scheme.
            /// </summary>
            source_scheme,

            /// <summary>
            /// The source port.
            /// </summary>
            source_port,

            /// <summary>
            /// Is same party.
            /// </summary>
            is_same_party,
        }

        /// <summary>
        /// Gets or sets the creation UTC.
        /// </summary>
        public DateTimeOffset CreationUTC { get; set; }

        /// <summary>
        /// Gets or sets the top frame site key.
        /// </summary>
        public string TopFrameSiteKey { get; set; }

        /// <summary>
        /// Gets or sets the host key.
        /// </summary>
        public string HostKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the encrypted value.
        /// </summary>
        public string EncryptedValue { get; set; }

        /// <summary>
        /// Gets or sets the decrypted value.
        /// </summary>
        public string DecryptedValue { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the expires UTC.
        /// </summary>
        public DateTimeOffset ExpiresUTC { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is secured or not.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is HTTP-only or not.
        /// </summary>
        public bool IsHttpOnly { get; set; }

        /// <summary>
        /// Gets or sets the last access UTC.
        /// </summary>
        public DateTimeOffset LastAccessUTC { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this expires or not.
        /// </summary>
        public bool HasExpires { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is persistent or not.
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public short Priority { get; set; }

        /// <summary>
        /// Gets or sets the samesite value.
        /// </summary>
        public short Samesite { get; set; }

        /// <summary>
        /// Gets or sets the source scheme.
        /// </summary>
        public short SourceScheme { get; set; }

        /// <summary>
        /// Gets or sets the source port.
        /// </summary>
        public int SourcePort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether party is same.
        /// </summary>
        public bool IsSameParty { get; set; }

        /// <summary>
        /// To string override method.
        /// </summary>
        /// <returns>
        /// A formatted string.
        /// </returns>
        public override string ToString()
        {
            return $"HostKey = '{HostKey}' | Name = '{Name}' | DecryptedValue = '{DecryptedValue}'";
        }
    }
}