using System;
using System.Collections.Generic;

namespace CapitalOne.CodingExcercise.SummaryApi.ViewModels
{
    /// <summary>
    /// Data contract with the summary that is returned to the user.
    /// </summary>
    public class Summary
    {
        /// <summary>
        /// Displays if the ignore donuts option was used.
        /// </summary>
        public bool IgnoreDonuts { get; set; }

        /// <summary>
        /// Displays if the crystal ball (display projected transactions) option was used.
        /// </summary>
        public bool CrystalBall { get; set; }

        /// <summary>
        /// Displays if the ignore credit card payments options was used.
        /// </summary>
        public bool IgnoreCcPayments { get; set; }

        /// <summary>
        /// Used to show the average month.
        /// </summary>
        public MonthlySummaryView AverageMonth { get; set; }

        /// <summary>
        /// The list of months with their total income and expenses.
        /// </summary>
        public IList<MonthlySummaryView> Months { get; set; }

        /// <summary>
        /// Contains the list of excluded credit card payments when the ignore credit card payments options is used.
        /// </summary>
        public IList<Tuple<BankTransaction,BankTransaction>> ExcludedCreditCardPayments { get; set; }
    }
}
