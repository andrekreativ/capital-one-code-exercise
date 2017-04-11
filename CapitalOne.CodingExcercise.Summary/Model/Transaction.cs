using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace CapitalOne.CodingExcercise.Summary.Model
{
    /// <summary>
    /// Base class for all type of transactions.
    /// </summary>
    [DataContract(Name = "transaction")]
    public class Transaction
    {
        /// <summary>
        /// The transaction Id.
        /// </summary>
        [DataMember(Name = "transaction-id")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Used to hold the original transaction time when deserializing.
        /// </summary>
        [DataMember(Name = "transaction-time")]
        private string JsonTransactionTime { get; set; }

        private DateTime? _transactionTime { get; set; }

        /// <summary>
        /// The time of the transaction.
        /// </summary>
        [IgnoreDataMember]
        public DateTime TransactionTime
        {
            get
            {
                if (!_transactionTime.HasValue && !string.IsNullOrEmpty(JsonTransactionTime))
                {
                    // 1970-01-01T00:00:00.000
                    // yyyy-MM-ddTHH:mm:ss.fffZ
                    if (DateTime.TryParseExact(JsonTransactionTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        _transactionTime = date;
                    }
                    else
                    {
                        _transactionTime = default(DateTime);
                    }
                }
                return _transactionTime.Value;
            }
            set
            {
                _transactionTime = value;
            }
        }

        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        /// <remarks>
        /// Negative amount = debit, positive amount = credit.Centocents. 20000 centocents = $2.
        /// </remarks>
        [DataMember(Name = "amount")]
        public long Amount { get; set; }
    }
}
