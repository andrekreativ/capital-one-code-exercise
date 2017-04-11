using System;
using System.Linq;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Identifies pairs of credit card payment transactions with opposite amounts and created with the specified window of time.
    /// </summary>
    public class CreditCardPaymentsIdentifierByOppositeAmounts : ICreditCardPaymentsIdentifier
    {
        /// <summary>
        /// The default time window to identify matching payment transactions.
        /// </summary>
        public static TimeSpan DefaultMaxDifference = TimeSpan.FromHours(24);

        /// <summary>
        /// The time window to identify matching payment transactions.
        /// </summary>
        public TimeSpan TimeDifferenceToIdentify { get; protected set; }

        public CreditCardPaymentsIdentifierByOppositeAmounts()
        {
            TimeDifferenceToIdentify = DefaultMaxDifference;
        }

        public CreditCardPaymentsIdentifierByOppositeAmounts(TimeSpan timeDifference)
        {
            TimeDifferenceToIdentify = timeDifference;
        }

        /// <summary>
        /// Determines if two transactions were created in the same time window.
        /// </summary>
        /// <param name="firstTransaction">The first transaction.</param>
        /// <param name="secondTransaction">The second transaction.</param>
        /// <returns>true if they are in the same window, false otherwise.</returns>
        public bool AreInTheSameTimeWindow(BankTransaction firstTransaction, BankTransaction secondTransaction)
        {
            TimeSpan timeDifference = firstTransaction.TransactionTime - secondTransaction.TransactionTime;

            return timeDifference.Duration() <= TimeDifferenceToIdentify;
        }

        /// <summary>
        /// Attemnpts to identify credit card transactions.
        /// </summary>
        /// <param name="allTransactions">The list of all transactions to analyze.</param>
        /// <param name="creditCardPayments">Return a list with pair of transactions identified as credit card payments.</param>
        /// <param name="otherTransactions">Returns the list of all other transactions that were not identified as credit card payments.</param>
        /// <returns>true if it was able to identify at least one pair of credit card payment transactions, false if not.</returns>
        public bool TryGetCreditCardPayments(
            IEnumerable<BankTransaction> allTransactions, out IList<Tuple<BankTransaction, BankTransaction>> creditCardPayments, out IList<BankTransaction> otherTransactions)
        {
            otherTransactions = null;
            creditCardPayments = null;

            if (allTransactions == null)
            {
                return false;
            }

            IDictionary<long, Queue<BankTransaction>> AmountsHash = new Dictionary<long,Queue<BankTransaction>>();
            creditCardPayments = new List<Tuple<BankTransaction, BankTransaction>>();
            otherTransactions = new List<BankTransaction>();

            // Order by time asc., older transactions first.
            foreach (var transaction in allTransactions.OrderBy(t=> t.TransactionTime))
            {
                long opposingAmount = transaction.Amount * -1;

                if (AmountsHash.ContainsKey(opposingAmount))
                {
                    // Check if we have opposing amounts in the same time window.
                    if (!TryFindMatchingTransaction(creditCardPayments, otherTransactions, AmountsHash, transaction))
                    {
                        // We couldn't find a matching transaction at the moment.
                        // Add it to its queue to search for another match later.

                        AddToQueue(AmountsHash, transaction);
                    }
                }
                else
                {
                    // We don't have transactions with opposing amounts.
                    AddToQueue(AmountsHash, transaction);
                }
            }

            if (creditCardPayments.Count>0)
            {
                // We found credit card payments.
                // Flush the rest of the transactions in the queues
                // that didn't have a matching payment.
                foreach(var queue in AmountsHash.Values)
                {
                    while(queue.Count > 0)
                    {
                        // Add to the list of other transaction.
                        BankTransaction transaction = queue.Dequeue();
                        otherTransactions.Add(transaction);
                    }
                }

                return true;
            }
            else
            {
                // We didn't find any credit card payments.
                creditCardPayments = null;
                otherTransactions = null;

                return false;
            }
        }

        private static void AddToQueue(IDictionary<long, Queue<BankTransaction>> AmountsHash, BankTransaction transaction)
        {
            if (!AmountsHash.TryGetValue(transaction.Amount, out Queue<BankTransaction> queue))
            {
                // Create a new queue for this amount.
                queue = new Queue<BankTransaction>();
                AmountsHash[transaction.Amount] = queue;
            }

            queue.Enqueue(transaction);
        }

        private bool TryFindMatchingTransaction(
            IList<Tuple<BankTransaction, BankTransaction>> creditCardPayments, IList<BankTransaction> otherTransactions, IDictionary<long, Queue<BankTransaction>> AmountsHash, BankTransaction secondTransaction)
        {
            long opposingAmount = secondTransaction.Amount * -1;
            Queue<BankTransaction> queue = AmountsHash[opposingAmount];

            while (queue.Count > 0)
            {
                // Get the oldest transaction in the queue.
                BankTransaction firstTransaction = queue.Dequeue();

                if (AreInTheSameTimeWindow(firstTransaction, secondTransaction))
                {
                    // They are matching payments.
                    Tuple<BankTransaction, BankTransaction> tuple = new Tuple<BankTransaction, BankTransaction>(
                        item1: firstTransaction,
                        item2: secondTransaction);
                    creditCardPayments.Add(tuple);

                    return true;
                }
                else
                {
                    // The first transaction is too old.
                    // That means we should discart it because the next transaction we
                    // select from the outer loop will be newer and it will not be in
                    // the same time window either.
                    // The next transaction coming from the queue will be newer, that one could match.
                    otherTransactions.Add(firstTransaction);
                }
            }

            // None of the transactions with opposing amounts are in the same time window.
            return false;
        }
    }
}
