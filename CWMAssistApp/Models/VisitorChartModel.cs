namespace CWMAssistApp.Models
{
    public class VisitorChartModel
    {
        public int[] ThisWeekVisitorDayList { get; set; }
        public int[] LastWeekVisitorDayList { get; set; }
        public int ThisWeekVisitorCount { get; set; }
        public int BetweenWeeksRate { get; set; }
        public decimal WeekRate { get; set; }
        public bool ArrowUp { get; set; }
    }
}
