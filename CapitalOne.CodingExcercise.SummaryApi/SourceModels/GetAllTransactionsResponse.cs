using System.Collections.Generic;
using System.Runtime.Serialization;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.SummaryApi.SourceModels
{
    /// <summary>
    /// A response with transactions.
    /// </summary>
    [DataContract(Name = "getalltransactionsresponse")]
    public class GetAllTransactionsResponse
    {
        /// <summary>
        /// An error message if there was any.
        /// </summary>
        [DataMember(Name ="error")]
        public string Error { get; set; }

        /// <summary>
        /// The list of transactions.
        /// </summary>
        [DataMember(Name = "transactions")]
        public IList<BankTransaction> Transactions { get; set; }
    }
}
