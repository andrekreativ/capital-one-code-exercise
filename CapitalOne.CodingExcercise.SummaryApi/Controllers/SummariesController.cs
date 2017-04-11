using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CapitalOne.CodingExcercise.Summary.Connectors;
using CapitalOne.CodingExcercise.Summary.Model;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.SummaryApi.ViewModels;

namespace CapitalOne.CodingExcercise.SummaryApi.Controllers
{
    [Route("api/[controller]")]
    public class SummariesController : Controller
    {
        // Plug-ins.
        private readonly ITransactionsConnector _transactionsConnector;
        private readonly IMonthsRangeIdentifier _monthsRangeIdentifier;
        private readonly ISummaryByTimeCategorizer _summaryByTimeCategorizer;
        private readonly IAverageMonthCalculator _averageMonthCalculator;
        private readonly IExcludeByFieldValueCategorizer<string> _excludeByFieldValueCategorizer;
        private readonly ICreditCardPaymentsIdentifier _creditCardPaymentsIdentifier;

        // Demo list of merchants to exclude
        const string KrispyKreme = "Krispy Kreme Donuts";
        const string Dunkin = "DUNKIN #336784";
        private ISet<string> ExcludedMerchants = new HashSet<string>()
        {
            { KrispyKreme },
            { Dunkin }
        };

        public SummariesController(
            ITransactionsConnector transactionsConnector,
            IMonthsRangeIdentifier monthsRangeIdentifier,
            ISummaryByTimeCategorizer summaryByTimeCategorizer,
            IAverageMonthCalculator averageMonthCalculator,
            IExcludeByFieldValueCategorizer<string> excludeByFieldValueCategorizer,
            ICreditCardPaymentsIdentifier creditCardPaymentsIdentifier)
        {
            _transactionsConnector = transactionsConnector;
            _monthsRangeIdentifier = monthsRangeIdentifier;
            _summaryByTimeCategorizer = summaryByTimeCategorizer;
            _averageMonthCalculator = averageMonthCalculator;
            _excludeByFieldValueCategorizer = excludeByFieldValueCategorizer;
            _creditCardPaymentsIdentifier = creditCardPaymentsIdentifier;
        }

        // GET api/summaries
        [HttpGet]
        public async Task<IEnumerable<ViewModels.Summary>>
            Get(bool ignoreDonuts = false, bool crystalBall = false, bool ignoreCcPayments = false)
        {
            ViewModels.Summary summary = new ViewModels.Summary()
            {
                IgnoreDonuts = ignoreDonuts,
                CrystalBall = crystalBall,
                IgnoreCcPayments = ignoreCcPayments
            };

            // Get all transactions.
            IEnumerable<Summary.Model.BankTransaction> allTransactions = await _transactionsConnector.GetAllTransactionsAsync();

            if (allTransactions != null)
            {
                if (crystalBall)
                {
                    // Option: Include projected transactions.

                    // Get the list of months to project starting with the current month (utc time).
                    DateTime currentDate = DateTime.UtcNow;
                    DateTime firstDayOfTheMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                    var monthsList = _monthsRangeIdentifier.GetMonthsInTransaction(
                        allTransactions, greaterOrEqualThanDate: firstDayOfTheMonth);

                    // Add Projected transaction
                    foreach(var month in monthsList)
                    {
                        var projectedTransactions = await _transactionsConnector.GetProjectedTransactionsForMonthAsync(
                            month.Year, month.Month);

                        if (projectedTransactions != null)
                        {
                            // Concatenate to the full list
                            allTransactions = allTransactions.Concat(projectedTransactions);
                        }
                    }
                }

                if (ignoreDonuts)
                {
                    // Option: Ignore merchants that sell donuts.

                    allTransactions = _excludeByFieldValueCategorizer.RemoveExcludedTransactions(
                        allTransactions,
                        excludedValues: ExcludedMerchants,
                        getFieldValue: (Summary.Model.BankTransaction t) => (t.RawMerchant));
                }

                if (ignoreCcPayments)
                {
                    if (_creditCardPaymentsIdentifier.TryGetCreditCardPayments(
                        allTransactions,
                        out IList<Tuple<Summary.Model.BankTransaction, Summary.Model.BankTransaction>> creditCardPayments,
                        out IList<Summary.Model.BankTransaction> otherTransactions))
                    {
                        // We were able to identify credit card payments.
                        // Copy the new list without credit card payments.
                        allTransactions = otherTransactions;

                        // Copy the list of credit card payments.
                        summary.ExcludedCreditCardPayments = ViewModels.BankTransaction.CreateFromSummarySourceBankTransactionTupleList(creditCardPayments);
                    }
                }

                // Summarize by year/month.
                var summaryByYearAndMonth = _summaryByTimeCategorizer.GetSummaryByYearAndMonth(allTransactions);
                summary.Months = MonthlySummaryView.CreateListFromSummarySource(summaryByYearAndMonth);

                // Calculate average month
                if (_averageMonthCalculator.TryGetAverageMonth(summaryByYearAndMonth, out MonthlySummary averageMonth))
                {
                    summary.AverageMonth = MonthlySummaryView.CreateFromMonthlySummary(averageMonth, description: "Average");
                }
            }

            return new ViewModels.Summary[] { summary };
        }
    }
}
