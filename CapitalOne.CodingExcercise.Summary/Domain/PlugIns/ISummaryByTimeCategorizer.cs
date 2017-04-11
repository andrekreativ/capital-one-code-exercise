using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Interface for the plug-in to create summaries by years and months.
    /// </summary>
    public interface ISummaryByTimeCategorizer
    {
        /// <summary>
        /// Gets a list of summaries by year.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>The list of summaries by year.</returns>
        IEnumerable<MonthlySummary> GetSummaryyYear(IEnumerable<Transaction> list);

        /// <summary>
        /// Gets a list of summaries by year and month.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>The list of summaries by year and month.</returns>
        IEnumerable<MonthlySummary> GetSummaryByYearAndMonth(IEnumerable<Transaction> list);
    }
}