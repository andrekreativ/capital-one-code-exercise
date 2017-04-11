using System.Runtime.Serialization;

namespace CapitalOne.CodingExcercise.Summary.Model
{
    /// <summary>
    /// A bank transaction.
    /// </summary>
    [DataContract(Name = "transaction")]
    public class BankTransaction : Transaction
    {
        /// <summary>
        /// The bank account the transaction is associated with.
        /// </summary>
        [DataMember(Name = "account-id")]
        public string BankAccount { get; set; }

        /// <summary>
        /// Un-prettified merchant string.
        /// </summary>
        /// <remarks>Should never be displayed to the user.</remarks>
        [DataMember(Name = "raw-merchant")]
        public string RawMerchant { get; set; }

        /// <summary>
        /// Prettified merchant string. Should always be displayed to the user, even if they aren't using the app or their phone is off.
        /// </summary>
        [DataMember(Name = "merchant")]
        public string Merchant { get;set; }

        /// <summary>
        /// Determines if a transaction is currently pending.
        /// </summary>
        /// <remarks>
        /// Transactions show up as pending shortly after you swipe your card, and days later they are replaced by similar cleared (i.e. non-pending)
        /// transactions with different transaction IDs.
        /// </remarks>
        [DataMember(Name = "is-pending")]
        public bool IsPending { get; set; }

        /// <summary>
        /// When transactions clear, their IDs change for bad reasons. This is the ID the transaction had when it was pending,
        /// if any (not all transactions pend, and not all pending transactions are successfully matched with their cleared versions).
        /// </summary>
        [DataMember(Name = "previous-transaction-id")]
        public string PreviousTransactionId { get; set; }

        /// <summary>
        /// A vaguely human-readable description of the category of transaction this is (from the aggregator, generally).
        /// </summary>
        [DataMember(Name = "categorization")]
        public string Categorization { get; set; }

        /// <summary>
        /// Memo with a description used only for testing.
        /// </summary>
        /// <remarks>This is not displayed to the user.</remarks>
        [DataMember(Name = "memo-only-for-testing")]
        public string MemoOnlyForTesting { get; set; }

        /// <summary>
        /// The name of the payee, used only for testing.
        /// </summary>
        /// <remarks>This is not displayed to the user.</remarks>
        [DataMember(Name = "payee-name-only-for-testing")]
        public string PayeeNameOnlyForTesting { get; set; }

        /// <summary>
        /// Millisecond timestamp of when we think the transaction cleared.May be in the future.
        /// </summary>
        /// <remarks>
        /// If the transaction is pending, has undefined behavior.
        /// </remarks>
        [DataMember(Name = "clear-date")]
        public long ClearDate { get; set; }
    }
}
