using System;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Interface for the plug-in to identify ranges of years and months in a list of transactions.
    /// </summary>
    public interface IMonthsRangeIdentifier
    {
        /// <summary>
        /// Gets the list of years and months.
        /// </summary>
        /// <param name="transactions">The list of transactions.</param>
        /// <returns>The list of year and months keys.</returns>
        IEnumerable<YearMonthKey> GetMonthsInTransaction(IEnumerable<BankTransaction> transactions);

        /// <summary>
        /// Gets the list of years and months.
        /// </summary>
        /// <param name="transactions">The list of transactions.</param>
        /// <param name="greaterOrEqualThanDate">Optional filter to select only years and months greater or equal to the specified date.</param>
        /// <returns>The list of year and months keys.</returns>
        IEnumerable<YearMonthKey> GetMonthsInTransaction(IEnumerable<BankTransaction> transactions, DateTime? greaterOrEqualThanDate);
    }
}