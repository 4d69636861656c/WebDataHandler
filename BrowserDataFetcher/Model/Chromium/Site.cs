namespace BrowserDataFetcher.Model.Chromium
{
    using System;

    /// <summary>
    /// The <see cref="Site"/>.
    /// </summary>
    public class Site
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
            /// The URL.
            /// </summary>
            url,

            /// <summary>
            /// The title.
            /// </summary>
            title,

            /// <summary>
            /// The visit count.
            /// </summary>
            visit_count,

            /// <summary>
            /// The typed count.
            /// </summary>
            typed_count,

            /// <summary>
            /// The last visit time.
            /// </summary>
            last_visit_time,

            /// <summary>
            /// The hidden.
            /// </summary>
            hidden,
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the visit count.
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// Gets or sets the typed count.
        /// </summary>
        public int TypedCount { get; set; }

        /// <summary>
        /// Gets or sets the last visit time.
        /// </summary>
        public DateTimeOffset LastVisitTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this element is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// To string method override.
        /// </summary>
        /// <returns>
        /// A formatted string.
        /// </returns>
        public override string ToString()
        {
            return $"Title = '{Title}' | Url = '{Url}'";
        }
    }
}