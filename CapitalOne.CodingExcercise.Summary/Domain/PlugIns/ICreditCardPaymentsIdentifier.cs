using System;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Interface for the plug-in to identify credit card payments.
    /// </summary>
    public interface ICreditCardPaymentsIdentifier
    {
        /// <summary>
        /// Attemnpts to identify credit card transactions.
        /// </summary>
        /// <param name="allTransactions">The list of all transactions to analyze.</param>
        /// <param name="creditCardPayments">Return a list with pair of transactions identified as credit card payments.</param>
        /// <param name="otherTransactions">Returns the list of all other transactions that were not identified as credit card payments.</param>
        /// <returns>true if it was able to identify at least one pair of credit card payment transactions, false if not.</returns>
        bool TryGetCreditCardPayments(
            IEnumerable<BankTransaction> allTransactions, out IList<Tuple<BankTransaction, BankTransaction>> creditCardPayments, out IList<BankTransaction> otherTransactions);
    }
}