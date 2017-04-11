using System.Collections.Generic;
using System.Linq;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Calculates the average month as the average total income and average total expenses in all months.
    /// </summary>
    public class AverageMonthCalculator : IAverageMonthCalculator
    {
        /// <summary>
        /// Attempts to calculate the avarage month from a list of monthly summaries with total amounts.
        /// Calculates the average month as the average total income and average total expenses in all months.
        /// </summary>
        /// <param name="monthSummaries">The list of monthly summaries with total amounts.</param>
        /// <param name="averageMonth">Returns the average month.</param>
        /// <returns>true if it was able to calculate the average month, false otherwise.</returns>
        public bool TryGetAverageMonth(IEnumerable<MonthlySummary> monthSummaries, out MonthlySummary averageMonth)
        {
            averageMonth = null;

            if (monthSummaries == null)
            {
                return false;
            }

            averageMonth = new MonthlySummary()
            {
                Spent = (long)monthSummaries.Select(m => m.Spent).Average(),
                Income = (long)monthSummaries.Select(m => m.Income).Average()
            };

            return true;
        }
    }
}
