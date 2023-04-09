namespace CWMAssistApp.Extention
{
    public static class TimeHelper
    {
        public static DateTime GetWeekFirstDay()
        {
            var dateTimeNow = DateTime.Now;
            switch ((int)dateTimeNow.DayOfWeek)
            {
                case 0:
                    return dateTimeNow.AddDays(-6);
                default:
                    return dateTimeNow.AddDays(1 - (int)dateTimeNow.DayOfWeek);
            }
        }

        public static DateTime GetWeekLastDay()
        {
            var dateTimeNow = DateTime.Now;
            switch ((int)dateTimeNow.DayOfWeek)
            {
                case 0:
                    return dateTimeNow;
                default:
                    return dateTimeNow.AddDays(7 - (int)dateTimeNow.DayOfWeek);
            }
        }
    }
}
