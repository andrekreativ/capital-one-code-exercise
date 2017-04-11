using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Generic interface to define buckets of transactions.
    /// </summary>
    /// <typeparam name="T">The type of transactions.</typeparam>
    /// <remarks>
    /// It doesn't define a collection to save the transactions in order to allow implementions of this interface
    /// to calcuate metrics and total numbers without saving the list.
    /// </remarks>
    public interface ISummaryBucket<T> where T : Transaction
    {
        /// <summary>
        /// Adds a transaction to the bucket.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        void Add(T transaction);
    }
}
