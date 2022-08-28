namespace BrowserDataFetcher.Model.Gecko
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
            /// The rev host.
            /// </summary>
            rev_host,

            /// <summary>
            /// The visit count.
            /// </summary>
            visit_count,

            /// <summary>
            /// The hidden.
            /// </summary>
            hidden,

            /// <summary>
            /// The typed.
            /// </summary>
            typed,

            /// <summary>
            /// The frecency.
            /// </summary>
            frecency,

            /// <summary>
            /// The last visit date.
            /// </summary>
            last_visit_date,

            /// <summary>
            /// The GUID.
            /// </summary>
            guid,

            /// <summary>
            /// The foreign count.
            /// </summary>
            foreign_count,

            /// <summary>
            /// The URL hash.
            /// </summary>
            url_hash,

            /// <summary>
            /// The description.
            /// </summary>
            description,

            /// <summary>
            /// The preview image URL.
            /// </summary>
            preview_image_url,

            /// <summary>
            /// The origin ID.
            /// </summary>
            origin_id,

            /// <summary>
            /// The site name.
            /// </summary>
            site_name,
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
        /// Gets or sets the rev host.
        /// </summary>
        public string RevHost { get; set; }

        /// <summary>
        /// Gets or sets the visit count.
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is typed or not.
        /// </summary>
        public bool IsTyped { get; set; }

        /// <summary>
        /// Gets or sets the frecency.
        /// </summary>
        public int Frecency { get; set; }

        /// <summary>
        /// Gets or sets the last visit date.
        /// </summary>
        public DateTimeOffset LastVisitDate { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the foreign count.
        /// </summary>
        public int ForeignCount { get; set; }

        /// <summary>
        /// Gets or sets the URL hash.
        /// </summary>
        public long UrlHash { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the preview image URL.
        /// </summary>
        public string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the origin ID.
        /// </summary>
        public int Originid { get; set; }

        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        public string SiteName { get; set; }

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