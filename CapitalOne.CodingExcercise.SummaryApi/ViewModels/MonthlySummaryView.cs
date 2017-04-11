using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.SummaryApi.ViewModels
{
    /// <summary>
    /// Data contract used to show the summary for a month that is displayed to the user.
    /// </summary>
    /// <remarks>
    /// TODO: Refactor code to take out the formating methods and logic.
    /// </remarks>
    [DataContract(Name = "monthlysummary")]
    public class MonthlySummaryView
    {
        public const float CentoCentsToDollarEquivalent = 10000;

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "spent")]
        public string Spent { get; protected set; }

        [DataMember(Name = "income")]
        public string Income { get; protected set; }

        [IgnoreDataMember]
        public Func<float, string> AmountFormatter { get; set; }

        private static CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

        public MonthlySummaryView()
        {
            // Default amount formatter.
            AmountFormatter = FormatAmountWithNegativeSign;
            Description = string.Empty;
            Spent = AmountFormatter(0);
            Income = AmountFormatter(0);
        }

        public MonthlySummaryView(string description, float income, float spent)
            : this()
        {
            Description = description;
            Spent = AmountFormatter(spent);
            Income = AmountFormatter(income);
        }

        /// <summary>
        /// Creates a new monthly summary view.
        /// </summary>
        /// <param name="sourceMonthlySummary">The source of the monthly summary.</param>
        /// <param name="description">Optiona description for the month.</param>
        /// <returns>A monthly summary to display to the user.</returns>
        public static MonthlySummaryView CreateFromMonthlySummary(MonthlySummary sourceMonthlySummary, string description = null)
        {
            if (sourceMonthlySummary == null)
                return null;

            string summaryDescription = string.IsNullOrEmpty(description) ?
                    string.Format("{0}-{1}", sourceMonthlySummary.Year, sourceMonthlySummary.Month) :
                    description;

            MonthlySummaryView monthlySummary = new MonthlySummaryView(
                summaryDescription,
                income: CentoCentsToDollars(sourceMonthlySummary.Income),
                spent: CentoCentsToDollars(sourceMonthlySummary.Spent));

            return monthlySummary;
        }

        /// <summary>
        /// Creates a new list of monthly summaries to display.
        /// </summary>
        /// <param name="sourceMonthlySummary">The list with the summary sources.</param>
        /// <returns>A list of monthly summaries to display.</returns>
        public static IList<MonthlySummaryView> CreateListFromSummarySource(IEnumerable<MonthlySummary> sourceMonthlySummary)
        {
            IEnumerable<MonthlySummaryView> list = new List<MonthlySummaryView>();

            if (sourceMonthlySummary == null)
                return list as IList<MonthlySummaryView>;

            list = sourceMonthlySummary.Select(s => CreateFromMonthlySummary(s));

            return list.ToList();
        }

        /// <summary>
        /// Formats amounts with the dollar sign, the negative sign and two decimals.
        /// </summary>
        /// <param name="amount">The amount to format.</param>
        /// <returns>A string with the formatted amount.</returns>
        public static string FormatAmountWithNegativeSign(float amount)
        {
            culture.NumberFormat.CurrencyNegativePattern = 1;

            return string.Format(culture, "{0:c}", amount);
        }

        /// <summary>
        /// Converts centoCents to regular amounts (in dollars).
        /// </summary>
        /// <param name="centoCents">The centocents amount.</param>
        /// <returns>The amount in dollars.</returns>
        public static float CentoCentsToDollars(long centoCents)
        {
            return centoCents / CentoCentsToDollarEquivalent;
        }
    }
}
