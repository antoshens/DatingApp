namespace DatingApp.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetFullAge(this DateTime dateTime)
        {
            var today = DateTime.Today;
            var fullAge = today.Year - dateTime.Year;

            if (dateTime.Date > today.AddYears(-fullAge)) fullAge--;

            return fullAge;
        }
    }
}
