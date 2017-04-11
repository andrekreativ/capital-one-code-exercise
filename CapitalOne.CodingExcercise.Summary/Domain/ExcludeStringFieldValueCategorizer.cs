using System;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Domain.Buckets;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Excludes transactions that match a specified string value in a field.
    /// </summary>
    public class ExcludeStringFieldValueCategorizer
        : TransactionCategorizer<BankTransaction, bool, TransactionListBucket<BankTransaction>>, IExcludeByFieldValueCategorizer<string>
    {
        /// <summary>
        /// Key used to include transactions.
        /// </summary>
        public const bool IncludeKey = true;

        /// <summary>
        /// Key used to exclude transactions.
        /// </summary>
        public const bool ExcludeKey = false;

        /// <summary>
        /// The set of excluded values.
        /// </summary>
        public ISet<string> ExcludedValues { get; protected set; }

        public Func<BankTransaction, string> GetFieldValue { get; protected set; }

        public ExcludeStringFieldValueCategorizer()
            : this(new HashSet<string>(), (BankTransaction t) => (default(string)))
        {
        }

        public ExcludeStringFieldValueCategorizer(ISet<string> excludedValues, Func<BankTransaction, string> getFieldValue)
            : base(createBucket: TransactionListBucket<BankTransaction>.BucketConstructor)
        {
            ExcludedValues = new HashSet<string>();

            AddExcludedValues(excludedValues);

            GetFieldValue = getFieldValue;
            GetBucketKey = GetBucketKeyFromFieldValue;
        }

        private void AddExcludedValues(ISet<string> excludedValues)
        {
            if (excludedValues == null)
                throw new ArgumentNullException("excludedValues");

            ExcludedValues.Clear();

            foreach (var merchant in excludedValues)
            {
                ExcludedValues.Add(merchant.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Gets the key to determine if a transaction must be included or excluded based on
        /// the value a field.
        /// </summary>
        /// <param name="transaction">The bank transaction.</param>
        /// <returns>
        /// The include key if the transaction must be included in the final list,
        /// or the exclude key if the transaction matches an excluded value and must not be included in the final list.
        /// </returns>
        public bool GetBucketKeyFromFieldValue(BankTransaction transaction)
        {
            if (transaction == null)
            {
                // An empty transaction does not match the excluded criteria because it has no values.
                return IncludeKey;
            }

            string value = GetFieldValue(transaction).ToLowerInvariant();

            return !ExcludedValues.Contains(value);
        }

        /// <summary>
        /// Removes the transactions that match the specified values.
        /// </summary>
        /// <param name="transactions">The list of transactions to evaluate.</param>
        /// <param name="excludedValues">The list of excluded values.</param>
        /// <param name="getFieldValue">The function to specify and get the value of the field.</param>
        /// <returns>The list without the values the match the specified list of excluded values.</returns>
        public IEnumerable<BankTransaction> RemoveExcludedTransactions(
            IEnumerable<BankTransaction> transactions, ISet<string> excludedValues, Func<BankTransaction, string> getFieldValue)
        {
            AddExcludedValues(excludedValues);

            GetFieldValue = getFieldValue;

            var buckets = Categorize(transactions);

            if (!buckets.ContainsKey(IncludeKey))
            {
                return new List<BankTransaction>();
            }

            var bucketIncluded = buckets[ExcludeStringFieldValueCategorizer.IncludeKey];
            if (bucketIncluded == null)
            {
                return new List<BankTransaction>();
            }

            return bucketIncluded.Transactions;
        }
    }
}
