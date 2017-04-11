using System;
using CapitalOne.CodingExcercise.Summary.Domain.Buckets;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Groups transactions by absolute amounts.
    /// </summary>
    /// <remarks>
    /// Currently only used in unit tests as a demo of other ways to group transactions.
    /// </remarks>
    public class GroupByAbsoluteAmountCategorizer :
        TransactionCategorizer<BankTransaction, long, TransactionListBucket<BankTransaction>>
    {
        public GroupByAbsoluteAmountCategorizer()
            : base(createBucket: TransactionListBucket<BankTransaction>.BucketConstructor)
        {
            GetBucketKey = GetaAbsoluteAmountKey;
        }

        public Func<BankTransaction, long> GetaAbsoluteAmountKey = (BankTransaction transaction) =>
        {
            if (transaction == null)
                return 0;
            return Math.Abs(transaction.Amount);
        };
    }
}
