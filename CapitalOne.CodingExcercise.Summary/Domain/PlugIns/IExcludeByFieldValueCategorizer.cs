using System;
using System.Collections.Generic;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.Domain
{
    /// <summary>
    /// Interface for plug-ins used to match and exclude transactions with certain values.
    /// </summary>
    /// <typeparam name="FieldType">The data type of the field to match.</typeparam>
    public interface IExcludeByFieldValueCategorizer<FieldType>
    {
        /// <summary>
        /// The function to specify and get the value of the field.
        /// </summary>
        Func<BankTransaction, FieldType> GetFieldValue { get; }

        /// <summary>
        /// Removes the transactions that match the specified values.
        /// </summary>
        /// <param name="transactions">The list of transactions to evaluate.</param>
        /// <param name="excludedValues">The list of excluded values.</param>
        /// <param name="getFieldValue">The function to specify and get the value of the field.</param>
        /// <returns>The list without the values the match the specified list of excluded values.</returns>
        IEnumerable<BankTransaction> RemoveExcludedTransactions(
            IEnumerable<BankTransaction> transactions, ISet<FieldType> excludedValues, Func<BankTransaction,FieldType> getFieldValue);
    }
}