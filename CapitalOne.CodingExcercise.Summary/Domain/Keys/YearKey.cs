using System;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Key used to create buckets by years.
    /// </summary>
    public class YearKey : Tuple<int>, ITimeKey
    {
        public int Year
        {
            get { return Item1; }
        }

        public YearKey(int year) : base(year)
        {
            ValidateYearValue(year);
        }

        internal static void ValidateYearValue(int year)
        {
            if (year <= 0)
                throw new ArgumentOutOfRangeException(
                    paramName: "year",
                    actualValue: year,
                    message: "The year value must be greater than 0.");
        }
    }
}
