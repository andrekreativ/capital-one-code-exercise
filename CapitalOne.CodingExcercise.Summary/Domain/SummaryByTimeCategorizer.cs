using System.Linq;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Creates summaries of transactions grouping by periods of time.
    /// </summary>
    public class SummaryByTimeCategorizer
        : TransactionCategorizer<Transaction, ITimeKey, TotalIncomeExpenseBucket>, ISummaryByTimeCategorizer
    {
        public SummaryByTimeCategorizer()
            : base(createBucket: () => new TotalIncomeExpenseBucket(),
                   getBucketKey: GetGroupByYearAndMonthKey)
        {
        }

        /// <summary>
        /// Creates a summary by year and months.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>A dictionary with a bucket for each year and month combination.</returns>
        public IDictionary<ITimeKey, TotalIncomeExpenseBucket> CategorizeByYearAndMonth(IEnumerable<Transaction> list)
        {
            return Categorize(list, GetGroupByYearAndMonthKey);
        }

        /// <summary>
        /// Creates a summary by year.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>A dictionary with a bucket for each year.</returns>
        public IDictionary<ITimeKey, TotalIncomeExpenseBucket> CategorizeByYear(IEnumerable<Transaction> list)
        {
            return Categorize(list, GetGroupByYearAndMonthKey);
        }

        /// <summary>
        /// Gets the key to group by year and month.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The year/month key.</returns>
        public static YearMonthKey GetGroupByYearAndMonthKey(Transaction transaction)
        {
            if (transaction == null)
                return null;

            return new YearMonthKey(
                year: transaction.TransactionTime.Year,
                month: transaction.TransactionTime.Month);
        }

        /// <summary>
        /// Gets the key to group by year.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The key for the year.</returns>
        public static YearKey GetGroupByYearKey(Transaction transaction)
        {
            if (transaction == null)
                return null;

            return new YearKey(
                year: transaction.TransactionTime.Year);
        }

        /// <summary>
        /// Gets a list of summaries by year and month.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>The list of summaries by year and month.</returns>
        public IEnumerable<MonthlySummary> GetSummaryByYearAndMonth(IEnumerable<Transaction> list)
        {
            var dictionary = CategorizeByYearAndMonth(list);

            IEnumerable<MonthlySummary> summaryList = new List<MonthlySummary>();

            if (dictionary.Count == 0)
                return summaryList;

            summaryList = dictionary
                .Select(kvp => new
                {
                    YearMonthKey = kvp.Key as YearMonthKey,
                    Summary = kvp.Value
                })
                .Select(pair => new MonthlySummary()
                {
                    Year = pair.YearMonthKey.Year,
                    Month = pair.YearMonthKey.Month,
                    Income = pair.Summary.Income,
                    Spent = pair.Summary.Expenses
                });

            return summaryList
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month);
        }

        /// <summary>
        /// Gets a list of summaries by year.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>The list of summaries by year.</returns>
        public IEnumerable<MonthlySummary> GetSummaryyYear(IEnumerable<Transaction> list)
        {
            var dictionary = CategorizeByYear(list);

            IEnumerable<MonthlySummary> summaryList = new List<MonthlySummary>();

            if (dictionary.Count == 0)
                return summaryList;

            summaryList = dictionary
                .Select(kvp => new
                {
                    YearKey = kvp.Key as YearKey,
                    Summary = kvp.Value
                })
                .Select(pair => new MonthlySummary()
                {
                    Year = pair.YearKey.Year,
                    Income = pair.Summary.Income,
                    Spent = pair.Summary.Expenses
                });

            return summaryList.OrderByDescending(s => s.Year);
        }
    }
}
