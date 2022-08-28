namespace BrowserDataFetcher.Helpers
{
    using System;

    /// <summary>
    /// The <see cref="Extensions"/> static class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Reverses an <see cref="ulong"/>.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The new <see cref="ulong"/> value.
        /// </returns>
        public static ulong Reverse(this ulong source)
        {
            source = GeneralTools.BitPermuteStepSimple(source, 0x5555555555555555UL, 1);
            source = GeneralTools.BitPermuteStepSimple(source, 0x3333333333333333UL, 2);
            source = GeneralTools.BitPermuteStepSimple(source, 0x0F0F0F0F0F0F0F0FUL, 4);
            return ReverseBytes(source);
        }

        /// <summary>
        /// Reverses the bits for a <see cref="ulong"/>.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The new <see cref="ulong"/> value.
        /// </returns>
        public static ulong ReverseBytes(this ulong source)
        {
            return RotateLeft(source & 0xFF000000FF000000UL, 8) |
                   RotateLeft(source & 0x00FF000000FF0000UL, 24) |
                   RotateLeft(source & 0x0000FF000000FF00UL, 40) |
                   RotateLeft(source & 0x000000FF000000FFUL, 56);
        }

        /// <summary>
        /// Rotates to the left an <see cref="ulong"/> value.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="distance">
        /// The distance.
        /// </param>
        /// <returns>
        /// The new <see cref="ulong"/> value.
        /// </returns>
        public static ulong RotateLeft(this ulong source, int distance)
        {
            return (source << distance) ^ (source >> -distance);
        }

        /// <summary>
        /// Converts a <see cref="long"/> (WebKitTimeStamp) to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="microseconds">
        /// The value in microseconds.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset WebKitTimeStampToDateTime(this long microseconds)
        {
            DateTime dateTime = new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            if (microseconds != 0 && microseconds.ToString().Length < 18)
            {
                microseconds /= 1000000;
                dateTime = dateTime.AddSeconds(microseconds).ToLocalTime();
            }

            return dateTime;
        }

        /// <summary>
        /// Converts a <see cref="long"/> (WebKitEpoch) to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="webKitEpoch">
        /// The web kit epoch value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset WebKitEpochToDateTime(this long webKitEpoch)
        {
            const long epochDifferenceMicroseconds = 11644473600000000; // difference in microseconds between 1601 and 1970
            var epoch = (webKitEpoch - epochDifferenceMicroseconds) / 1000000; // adjust to seconds since 1st Jan 1970
            return DateTimeOffset.FromUnixTimeSeconds(epoch);
        }

        /// <summary>
        /// Converts a <see cref="long"/> (<c>UnixTime</c> in seconds) to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="seconds">
        /// The value in seconds.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset UnixTimeInSecondsToDate(this long seconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(seconds);
        }

        /// <summary>
        /// Converts a <see cref="long"/> (<c>UnixTime</c> in milliseconds) to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="milliseconds">
        /// The value in milliseconds.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset UnixTimeInMillisecondsToDate(this long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        }

        /// <summary>
        /// Converts a <see cref="long"/> (<c>UnixTime</c> in microseconds) to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="microseconds">
        /// The value in microseconds.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/>.
        /// </returns>
        public static DateTimeOffset UnixTimeInMicrosecondsToDate(this long microseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(microseconds / 1000);
        }
    }
}