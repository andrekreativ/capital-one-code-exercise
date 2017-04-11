using System;
using System.Linq;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Identified the list of years and months in a list of transactions.
    /// </summary>
    public class MonthsRangeIdentifier : IMonthsRangeIdentifier
    {
        /// <summary>
        /// Gets the list of years and months.
        /// </summary>
        /// <param name="transactions">The list of transactions.</param>
        /// <returns>The list of year and months keys.</returns>
        public IEnumerable<YearMonthKey> GetMonthsInTransaction(IEnumerable<BankTransaction> transactions)
        {
            return GetMonthsInTransaction(transactions, greaterOrEqualThanDate: null);
        }

        /// <summary>
        /// Gets the list of years and months.
        /// </summary>
        /// <param name="transactions">The list of transactions.</param>
        /// <param name="greaterOrEqualThanDate">Optional filter to select only years and months greater or equal to the specified date.</param>
        /// <returns>The list of year and months keys.</returns>
        public IEnumerable<YearMonthKey> GetMonthsInTransaction(IEnumerable<BankTransaction> transactions, DateTime? greaterOrEqualThanDate)
        {
            ISet<YearMonthKey> distinctMonths = new HashSet<YearMonthKey>();

            if (transactions == null)
            {
                return new List<YearMonthKey>();
            }

            // Get the list of all distint months.
            foreach(var transaction in transactions)
            {
                if (greaterOrEqualThanDate != null && greaterOrEqualThanDate.HasValue)
                {
                    if (transaction.TransactionTime < greaterOrEqualThanDate.Value)
                    {
                        // Skip this transaction.
                        continue;
                    }
                }

                YearMonthKey monthKey = new YearMonthKey(
                    transaction.TransactionTime.Year, transaction.TransactionTime.Month);

                distinctMonths.Add(monthKey);
            }

            return distinctMonths
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month);
        }
    }
}
