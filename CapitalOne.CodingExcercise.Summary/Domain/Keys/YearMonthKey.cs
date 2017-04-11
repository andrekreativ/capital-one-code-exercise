using System;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Key used to create buckets by year and months.
    /// </summary>
    public class YearMonthKey : Tuple<int, int>, ITimeKey
    {
        public int Year
        {
            get { return Item1; }
        }

        public int Month
        {
            get { return Item2; }
        }

        public YearMonthKey(int year, int month)
            : base(year, month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(
                    paramName: "month",
                    actualValue:  month,
                    message: "The month value must be between 1 and 12.");

            YearKey.ValidateYearValue(year);
        }
    }
}
