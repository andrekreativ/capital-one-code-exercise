using System;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain.Buckets
{
    /// <summary>
    /// Bucket with a list of transactions.
    /// </summary>
    /// <remarks>
    /// Although this bucket only wrapps a list of transactions, we could include fields to calculate other metrics
    /// such as total amounts, averages, etc. while adding transactions to the bucket.
    /// </remarks>
    /// <typeparam name="T">The type of transaction.</typeparam>
    public class TransactionListBucket<T> : ISummaryBucket<T>
        where T : Transaction
    {
        public IList<T> Transactions { get; protected set;  }

        public TransactionListBucket()
        {
            Transactions = new List<T>();
        }

        public void Add(T transaction)
        {
            Transactions.Add(transaction);
        }

        public static Func<TransactionListBucket<T>> BucketConstructor = () => new TransactionListBucket<T>();
    }
}
