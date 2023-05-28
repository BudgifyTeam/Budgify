using System.Globalization;

namespace BudgifyApi
{
    internal class Utils
    {
        /// <summary>
        /// Converts a string representation of a date to a DateTime object.
        /// </summary>
        /// <param name="date">The string representation of the date.</param>
        /// <returns>A DateTime object representing the converted date.</returns>
        public static DateTime convertDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
    }
}
