using System.Collections.Generic;
using System.Threading.Tasks;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Connectors
{
    /// <summary>
    /// Interface to obtain transactions from a source.
    /// </summary>
    public interface ITransactionsConnector
    {
        /// <summary>
        /// Async method to get all the transactions.
        /// </summary>
        /// <returns>The list of all transactions.</returns>
        Task<IEnumerable<BankTransaction>> GetAllTransactionsAsync();

        /// <summary>
        /// Gets the projected transactions for a month.
        /// Attempts to predict what transactions haven't occurred yet, but probably will occur for
        /// the user during the given month.
        /// </summary>
        /// <param name="year">The year to project</param>
        /// <param name="month">The number of the month to project.</param>
        /// <returns>A list of transactions.</returns>
        Task<IEnumerable<BankTransaction>> GetProjectedTransactionsForMonthAsync(int year, int month);
    }
}
