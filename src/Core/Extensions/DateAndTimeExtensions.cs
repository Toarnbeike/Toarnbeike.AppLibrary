namespace Toarnbeike.AppLibrary.Extensions;

public static class DateAndTimeExtensions
{
    extension(DateOnly)
    {
        /// <summary>
        /// Gets the current date in UTC.
        /// </summary>
        public static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
    }

    extension(DateOnly date)
    {
        /// <summary>
        /// Calculates the age in whole years as of a specified date.
        /// </summary>
        /// <param name="asOfDate">The date used to calculate the age. If <see langword="null"/>, the current date is used.</param>
        /// <returns>The age in whole years.</returns>
        public int GetAge(DateOnly? asOfDate = null)
        {
            asOfDate ??= DateOnly.Today;
            var age = asOfDate.Value.Year - date.Year;
            if (asOfDate.Value.Month < date.Month ||
                (asOfDate.Value.Month == date.Month && asOfDate.Value.Day < date.Day))
            {
                age--;
            }

            return age;
        }
    }

    extension(DateTime value)
    {
        /// <summary>
        /// Converts the stored date and time value to a file-name-safe timestamp string in `yyyy-MM-dd_HH-mm-ss` format.
        /// </summary>
        /// <returns>A string representation that is both valid and sortable for use in file names.</returns>
        public string ToFileNameFormat() =>
            value.ToString("yyyy-MM-dd_HH-mm-ss");
    }

    extension(DateTimeOffset value)
    {
        /// <summary>
        /// Converts the stored date and time value to a file-name-safe timestamp string in `yyyy-MM-dd_HH-mm-ss` format.
        /// </summary>
        /// <returns>A string representation that is both valid and sortable for use in file names.</returns>
        public string ToFileNameFormat() =>
            value.ToString("yyyy-MM-dd_HH-mm-ss");
    }
}
