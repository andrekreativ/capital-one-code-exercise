using System;
using System.Collections.Generic;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Generic categorizer of transactions. Used to summarize and create lists of transactions that match certain criteria.
    /// </summary>
    /// <typeparam name="Transaction">The type of transactions.</typeparam>
    /// <typeparam name="BucketKey">The type used to create buckets (categories).</typeparam>
    /// <typeparam name="Bucket">The type of the bucket to create.</typeparam>
    public class TransactionCategorizer<Transaction,BucketKey,Bucket>
        where Bucket : ISummaryBucket<Transaction>
        where Transaction : Model.Transaction
    {
        /// <summary>
        /// Function used to create a dictionary with a customized implementation.
        /// </summary>
        public Func<IDictionary<BucketKey, Bucket>> CreateDictionary { get; set; }

        /// <summary>
        /// Function to get the bucket key (hash key) from a transaction.
        /// </summary>
        public Func<Transaction, BucketKey> GetBucketKey { get; set; }

        /// <summary>
        /// Function used to create a new bucket.
        /// </summary>
        public Func<Bucket> CreateBucket { get; set; }

        public TransactionCategorizer()
        {
            CreateDictionary = () => new Dictionary<BucketKey, Bucket>();
            GetBucketKey = (Transaction t) => default(BucketKey);
            CreateBucket = () => default(Bucket);
        }

        public TransactionCategorizer(Func<Bucket> createBucket)
            : this()
        {
            CreateBucket = createBucket;
        }

        public TransactionCategorizer(
            Func<Bucket> createBucket,
            Func<Transaction, BucketKey> getBucketKey)
            : this()
        {
            GetBucketKey = getBucketKey;
            CreateBucket = createBucket;
        }

        /// <summary>
        /// Categorizes the transactions into buckets that follow the rules specified by the getBucketKey.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <param name="getBucketKey">The function to create the buckets (categories).</param>
        /// <returns>A dictionary with the bucket keys and the buckets of transactions.</returns>
        public virtual IDictionary<BucketKey, Bucket> Categorize(IEnumerable<Transaction> list, Func<Transaction, BucketKey> getBucketKey)
        {
            var buckets = CreateDictionary();

            if (list == null)
                return buckets;

            foreach (var element in list)
            {
                BucketKey key = getBucketKey(element);
                if (key != null)
                {
                    if (!buckets.TryGetValue(key, out Bucket bucket))
                    {
                        // Create a new bucket.
                        bucket = CreateBucket();
                        buckets[key] = bucket;
                    }
                    bucket.Add(element);
                }
            }

            return buckets;
        }

        /// <summary>
        /// Categorizes the transactions into buckets that follow the rules specified by the getBucketKey provided in the constructor.
        /// </summary>
        /// <param name="list">The list of transactions.</param>
        /// <returns>A dictionary with the bucket keys and the buckets of transactions.</returns>
        public virtual IDictionary<BucketKey, Bucket> Categorize(IEnumerable<Transaction> list)
        {
            return Categorize(list, GetBucketKey);
        }
    }
}
