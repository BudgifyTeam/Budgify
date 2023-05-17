using System.Globalization;

namespace BudgifyApi
{
    internal class Utils
    {
        public static DateTime convertDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
    }
}
