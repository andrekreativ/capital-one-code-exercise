using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Interface for the plug-ins to calculate the average month.
    /// </summary>
    /// <remarks>
    /// TODO: Add other average mechanisms.
    /// </remarks>
    public interface IAverageMonthCalculator
    {
        /// <summary>
        /// Attempts to calculate the avarage month from a list of monthly summaries with total amounts.
        /// </summary>
        /// <param name="monthSummaries">The list of monthly summaries with total amounts.</param>
        /// <param name="averageMonth">Returns the average month.</param>
        /// <returns>true if it was able to calculate the average month, false otherwise.</returns>
        bool TryGetAverageMonth(IEnumerable<MonthlySummary> monthSummaries, out MonthlySummary averageMonth);
    }
}