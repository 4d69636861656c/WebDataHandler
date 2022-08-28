namespace BrowserDataFetcher.Model.Gecko
{
    using System;

    /// <summary>
    /// The <see cref="Cookie"/> class.
    /// </summary>
    public class Cookie
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
            /// The origin attributes.
            /// </summary>
            originAttributes,

            /// <summary>
            /// The name.
            /// </summary>
            name,

            /// <summary>
            /// The value.
            /// </summary>
            value,

            /// <summary>
            /// The host.
            /// </summary>
            host,

            /// <summary>
            /// The path.
            /// </summary>
            path,

            /// <summary>
            /// The expiry.
            /// </summary>
            expiry,

            /// <summary>
            /// The last accessed.
            /// </summary>
            lastAccessed,

            /// <summary>
            /// The creation time.
            /// </summary>
            creationTime,

            /// <summary>
            /// The is secure.
            /// </summary>
            isSecure,

            /// <summary>
            /// Is HTTP-only.
            /// </summary>
            isHttpOnly,

            /// <summary>
            /// The in-browser element.
            /// </summary>
            inBrowserElement,

            /// <summary>
            /// The same site.
            /// </summary>
            sameSite,

            /// <summary>
            /// The raw same site.
            /// </summary>
            rawSameSite,

            /// <summary>
            /// The scheme map.
            /// </summary>
            schemeMap,
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the origin attributes.
        /// </summary>
        public string OriginAttributes { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the expiry.
        /// </summary>
        public DateTimeOffset Expiry { get; set; }

        /// <summary>
        /// Gets or sets the last accessed.
        /// </summary>
        public DateTimeOffset LastAccessed { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is secure or not.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is HTTP-only or not.
        /// </summary>
        public bool IsHttpOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an in-browser element or not.
        /// </summary>
        public bool InBrowserElement { get; set; }

        /// <summary>
        /// Gets or sets the same site.
        /// </summary>
        public short SameSite { get; set; }

        /// <summary>
        /// Gets or sets the raw same site.
        /// </summary>
        public short RawSameSite { get; set; }

        /// <summary>
        /// Gets or sets the scheme map.
        /// </summary>
        public short SchemeMap { get; set; }

        /// <summary>
        /// To string method override.
        /// </summary>
        /// <returns>
        /// A formatted string.
        /// </returns>
        public override string ToString()
        {
            return $"Host = '{Host}' | Name = '{Name}' | Value = '{Value}'";
        }
    }
}