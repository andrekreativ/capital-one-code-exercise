namespace CapitalOne.CodingExcercise.Summary.Model
{
    /// <summary>
    /// Used to save a monthly summary.
    /// </summary>
    public class MonthlySummary
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public long Spent { get; set; }

        public long Income { get; set; }

        public MonthlySummary()
        {
            Year = 0;
            Month = 0;
            Spent = 0;
            Income = 0;
        }
    }
}
