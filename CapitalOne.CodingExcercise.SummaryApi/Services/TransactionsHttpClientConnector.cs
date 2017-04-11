using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using CapitalOne.CodingExcercise.Summary.Connectors;
using CapitalOne.CodingExcercise.Summary.Model;
using CapitalOne.CodingExcercise.SummaryApi.SourceModels;

namespace CapitalOne.CodingExcercise.SummaryApi.Services
{
    /// <summary>
    /// Connects to http endpoints to get transactions.
    /// </summary>
    /// <remarks>
    /// Full enpoints URI are hardcoded just for demo purposes.
    /// </remarks>
    public class TransactionsHttpClientConnector : ITransactionsConnector
    {
        // Note: adding these parameters as constansts just for demo purposes.
        // In a real service these paramaters would be stored and used securely.
        private const long TestUid = 1110590645;
        private const string TestToken = "4E175269CBE8A3E66ADB9207294D424E";
        private const string TestApiToken = "AppTokenForInterview";

        private CommonArgs CommonArguments = new CommonArgs()
        {
            UId = TestUid,
            Token = TestToken,
            ApiToken = TestApiToken,
            JsonStrictMode = false,
            JsonVerboseResponse = false
        };

        /// <summary>
        /// Gets all the transactions.
        /// </summary>
        /// <remarks>Without projected transactions.</remarks>
        /// <returns>A list of transactions.</returns>
        public async Task<IEnumerable<BankTransaction>> GetAllTransactionsAsync()
        {
            var rootArgs = new
            {
                args = CommonArguments
            };

            return await GetTransactionsAsync(
                requestUri: "https://2016.api.levelmoney.com/api/v2/core/get-all-transactions",
                rootArgs: rootArgs);
        }

        // TODO: Evaluate errors and include error handling.
        private static async Task<IEnumerable<BankTransaction>> GetTransactionsAsync(string requestUri, object rootArgs)
        {
            using (var client = new HttpClient())
            {
                // Set headers.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Call the endpoint
                var response = await client.PostAsync(
                    requestUri,
                    new StringContent(JsonConvert.SerializeObject(rootArgs), Encoding.UTF8, "application/json"));

                var streamTask = await response.Content.ReadAsStreamAsync();

                // Deserialize the response.
                var serializer = new DataContractJsonSerializer(typeof(GetAllTransactionsResponse));
                var getTransactionsResponse = serializer.ReadObject(streamTask) as GetAllTransactionsResponse;

                // Use null propagation: if repositories is null it returns null, otherwise returns the list of transactions.
                return getTransactionsResponse?.Transactions;
            }
        }

        /// <summary>
        /// Gets the projected transactions for a given year and month.
        /// </summary>
        /// <param name="year">The year number.</param>
        /// <param name="month">The month number.</param>
        /// <returns>The list of projected transactions.</returns>
        public async Task<IEnumerable<BankTransaction>> GetProjectedTransactionsForMonthAsync(int year, int month)
        {
            var rootArgs = new
            {
                args = CommonArguments,
                year = year,
                month = month
            };

            return await GetTransactionsAsync(
                requestUri: "https://2016.api.levelmoney.com/api/v2/core/projected-transactions-for-month",
                rootArgs: rootArgs);
        }
    }
}
