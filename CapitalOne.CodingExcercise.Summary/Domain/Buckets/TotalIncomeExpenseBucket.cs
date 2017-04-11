using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Bucket that calculates the total expenses and total income for the list of transactions that were added to it.
    /// </summary>
    /// <remarks>
    /// This implementation only saves the total amounts and discards the list of transactions,
    /// because the list is not needed in the scenarios where this type of bucket is used.
    /// </remarks>
    public class TotalIncomeExpenseBucket : ISummaryBucket<Transaction>
    {
        public long Expenses { get; protected set; }

        public long Income { get; protected set; }

        public TotalIncomeExpenseBucket()
        {
            Expenses = 0;
            Income = 0;
        }

        public void Add(Transaction transaction)
        {
            if (transaction != null)
            {
                if (transaction.Amount > 0)
                    Income += transaction.Amount;
                else if (transaction.Amount < 0)
                    Expenses += transaction.Amount;
            }
        }
    }
}
