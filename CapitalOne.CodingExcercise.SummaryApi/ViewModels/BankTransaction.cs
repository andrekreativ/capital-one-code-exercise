using System;
using System.Collections.Generic;
using SourceBankTransaction = CapitalOne.CodingExcercise.Summary.Model.BankTransaction;

namespace CapitalOne.CodingExcercise.SummaryApi.ViewModels
{
    /// <summary>
    /// Data contract to display transactions to the user.
    /// </summary>
    public class BankTransaction
    {
        /// <summary>
        /// The transaction Id.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// The time of the transaction.
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The formatted amount to display.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// The bank account the transaction is associated with.
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Prettified merchant string. Should always be displayed to the user, even if they aren't using the app or their phone is off.
        /// </summary>
        public string Merchant { get; set; }

        /// <summary>
        /// Determines if a transaction is currently pending.
        /// </summary>
        /// <remarks>
        /// Transactions show up as pending shortly after you swipe your card, and days later they are replaced by similar cleared (i.e. non-pending)
        /// transactions with different transaction IDs.
        /// </remarks>
        public bool IsPending { get; set; }

        /// <summary>
        /// A vaguely human-readable description of the category of transaction this is (from the aggregator, generally).
        /// </summary>
        public string Categorization { get; set; }

        public static BankTransaction CreateFromSourceBankTransaction(SourceBankTransaction summaryBankTransaction)
        {
            if (summaryBankTransaction == null)
                return null;
            string formattedAmount = MonthlySummaryView.FormatAmountWithNegativeSign(
                amount: MonthlySummaryView.CentoCentsToDollars(summaryBankTransaction.Amount));
            BankTransaction bankTransaction = new BankTransaction()
            {
                TransactionId = summaryBankTransaction.TransactionId,
                TransactionTime = summaryBankTransaction.TransactionTime,
                Amount = formattedAmount,
                BankAccount = summaryBankTransaction.BankAccount,
                Merchant = summaryBankTransaction.Merchant,
                Categorization = summaryBankTransaction.Categorization,
                IsPending = summaryBankTransaction.IsPending
            };

            return bankTransaction;
        }

        public static IList<Tuple<BankTransaction, BankTransaction>> CreateFromSummarySourceBankTransactionTupleList(IList<Tuple<SourceBankTransaction, SourceBankTransaction>> sourceList)
        {
            IList<Tuple<BankTransaction, BankTransaction>> list = new List<Tuple<BankTransaction, BankTransaction>>();

            if (sourceList == null)
            {
                return list;
            }

            foreach(var sourceTuple in sourceList)
            {
                Tuple<BankTransaction, BankTransaction> tuple = new Tuple<BankTransaction, BankTransaction>(
                    item1: CreateFromSourceBankTransaction(sourceTuple.Item1),
                    item2: CreateFromSourceBankTransaction(sourceTuple.Item2));
                list.Add(tuple);
            }

            return list;
        }
    }
}
