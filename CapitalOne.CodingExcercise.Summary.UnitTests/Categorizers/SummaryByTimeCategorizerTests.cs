using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.Categorizers
{
    public class SummaryByTimeCategorizerTests
    {
        /// <summary>
        /// The last day to use in a month. Using 28 to simplify date creation.
        /// </summary>
        const int lastDayNumber = 28;

        /// <summary>
        /// Test cases with known amounts to verify accuracy of the monthly summary.
        /// </summary>
        /// <param name="years">Array with the years to include in the summary.</param>
        /// <param name="months">Array with the months to include in the summary.</param>
        [Theory]
        // All months of the same year.
        [InlineData(new int[] { 2016, 2016, 2016, 2016, 2016, 2016 }, new int[] { 5, 6, 7, 8, 9, 10 })]
        // Use 3 months from one year and 3 of the next year.
        [InlineData(new int[] { 2016, 2016, 2016, 2017, 2017, 2017 }, new int[] { 10, 11, 12, 1, 2, 3 })]
        // Use the same month for different years.
        [InlineData(new int[] { 2012, 2013, 2014, 2015, 2016, 2017 }, new int[] { 3, 3, 3, 3, 3, 3 })]
        public void MonthlySummaryStaticAmounts(int[] years, int[] months)
        {
            const int totalMonths = 6;
            Assert.Equal(totalMonths, years.Length);
            Assert.Equal(totalMonths, months.Length);

            // Arrange: Fixed amounts.
            long[][] allAmounts = new long[][]
            {
                new long[] { 1000, 100, -200, -100 },
                new long[] { 2000, 200, -300, -150 },
                new long[] { 3000, 300, -400, -200 },
                new long[] { 0, -100, -220 },
                new long[] { 1000, 200 },
                new long[] { 0, 0, 0, 0}
            };

            long[] ExpectedIncomePerMonth = new long[]
            {
                1100, 2200, 3300, 0, 1200, 0
            };

            long[] ExpectedExpensesPerMonth = new long[]
            {
                -300, -450, -600, -320, 0, 0
            };

            // Create the transaction with the fixed amounts.
            IEnumerable<Transaction> allTransactions = new List<Transaction>();
            for (int i = 0; i < totalMonths; i++)
            {
                long[] amounts = allAmounts[i];
                IEnumerable<Transaction> transactions = CreateTransactionsInMonth(amounts, years[i], months[i]);
                allTransactions = allTransactions.Concat(transactions);
            }

            // Randomize the order of the transactions in the list.
            Random random = new Random();
            allTransactions = allTransactions.OrderBy(x => random.Next()).ToList();

            // Act
            SummaryByTimeCategorizer categorizer = new SummaryByTimeCategorizer();
            var summaryBuckets = categorizer.CategorizeByYearAndMonth(allTransactions as IList<Transaction>);

            IList<YearMonthKey> monthKeys = new List<YearMonthKey>();

            for (int i = 0; i < totalMonths; i++)
            {
                YearMonthKey monthKey = new YearMonthKey(years[i], months[i]);
                monthKeys.Add(monthKey);

                // Verify the summary contains the month.
                Assert.True(summaryBuckets.ContainsKey(monthKey), monthKey.ToString());
                var monthSummary = summaryBuckets[monthKey];

                // Verify the summary contains the expected amount for the month.
                Assert.Equal(ExpectedIncomePerMonth[i], monthSummary.Income);
                Assert.Equal(ExpectedExpensesPerMonth[i], monthSummary.Expenses);
            }

            // Verify the summary doesn't contain another month that was not in the list.
            foreach(var monthKey in summaryBuckets.Keys)
            {
                Assert.True(monthKeys.Contains(monthKey), monthKey.ToString());
            }
        }

        /// <summary>
        /// Creates random transactions for a year and verifies the summary per month.
        /// </summary>
        /// <param name="monthsToExclude">
        /// Months to exclude in the year. Provide an empty array to create data for all months.
        /// </param>
        [Theory]
        // Create data in all months of the year.
        [InlineData(new int[] {})]
        // Do not create data in the following months.
        [InlineData(new int[]  { 4, 7, 8 })]
        public void MonthlySummaryWithRandomAmounts(int[] monthsToExclude)
        {
            const int year = 2016;
            int minNumberOfTransaction = 1;
            int maxNumberOfTransaction = 100;
            int minAmount = 0;
            int maxamount = 1000000;

            // Arrange: Create income and expenses transactions.
            IDictionary<YearMonthKey, long> ExpectedIncomePerMonth = new Dictionary<YearMonthKey, long>();
            IDictionary<YearMonthKey, long> ExpectedExpensesPerMonth = new Dictionary<YearMonthKey, long>();

            IEnumerable<Transaction> allIncome = CreateRandomTransactions(
                year,
                minNumberOfTransaction,
                maxNumberOfTransaction,
                minAmount,
                maxamount,
                positiveAmounts : true,
                monthsToExclude : monthsToExclude,
                TotalAmounts: ExpectedIncomePerMonth);

            IEnumerable<Transaction> allExpenses = CreateRandomTransactions(
                year,
                minNumberOfTransaction,
                maxNumberOfTransaction,
                minAmount,
                maxamount,
                positiveAmounts: false,
                monthsToExclude: monthsToExclude,
                TotalAmounts: ExpectedExpensesPerMonth);

            IEnumerable<Transaction> allTransactions = allIncome.Concat(allExpenses);

            // Randomize the order of the transactions in the list.
            Random random = new Random();
            allTransactions = allTransactions.OrderBy(x => random.Next()).ToList();

            // Act
            SummaryByTimeCategorizer categorizer = new SummaryByTimeCategorizer();
            var summaryBuckets = categorizer.CategorizeByYearAndMonth(allTransactions as IList<Transaction>);
            // Assert
            Assert.NotNull(summaryBuckets);
            foreach(var kvp in ExpectedIncomePerMonth)
            {
                var monthKey = kvp.Key;
                long expectedIncome = kvp.Value;
                long expectedExpenses = ExpectedExpensesPerMonth[monthKey];

                // Verify the month is included in the summary.
                Assert.True(summaryBuckets.ContainsKey(monthKey));
                var monthSummary = summaryBuckets[monthKey];
                Assert.NotNull(monthSummary);

                // Verify it has the expected amounts.
                Assert.Equal(expectedIncome, monthSummary.Income);
                Assert.Equal(expectedExpenses, monthSummary.Expenses);
            }

            // Verify that the summary doesn't include months that were excluded.
            foreach(int month in monthsToExclude)
            {
                YearMonthKey monthKey = new YearMonthKey(year, month);
                Assert.False(summaryBuckets.ContainsKey(monthKey));
            }
        }

        public IEnumerable<Transaction> CreateRandomTransactions(
            int year,
            int minNumberOfTransaction,
            int maxNumberOfTransaction,
            int minAmount,
            int maxamount,
            bool positiveAmounts,
            int[] monthsToExclude,
            IDictionary<YearMonthKey, long> TotalAmounts)
        {
            const int firstMonth = 1;
            const int lastMonth = 12;

            Random random = new Random();
            IEnumerable<Transaction> allTransactions = new List<Transaction>();

            for (int month = firstMonth; month <= lastMonth; month++)
            {
                if (!monthsToExclude.Contains(month))
                {
                    YearMonthKey monthKey = new YearMonthKey(year, month);

                    // If the month is not excluded, then generate random transaction.
                    int numberOfTransaction = random.Next(minNumberOfTransaction, maxNumberOfTransaction);

                    // Generate expenses.
                    IEnumerable<Transaction> transaction = CreateTransactionsInMonth(
                        amounts: CreateRandomAmounts(numberOfTransaction, minAmount, maxamount, positiveAmounts),
                        year: year,
                        month: month);

                    // Save the totals to verify the summary.
                    long total = transaction.Select(t => t.Amount).Sum();
                    TotalAmounts[monthKey] = total;

                    // Concatenate to the list of all transactions.
                    allTransactions = allTransactions.Concat(transaction);
                }
            }

            return allTransactions;
        }

        /// <summary>
        /// Creates an array with random amounts.
        /// </summary>
        /// <param name="numberOfTransactions">The number of transactions to use.</param>
        /// <param name="minAmount">The min amount.</param>
        /// <param name="maxAmount">The max amount.</param>
        /// <param name="positiveAmounts">true to generate positive amounts, false for negative amounts.</param>
        /// <returns>An array with randmom amounts.</returns>
        public long[] CreateRandomAmounts(int numberOfTransactions, int minAmount, int maxAmount, bool positiveAmounts)
        {
            Random random = new Random();
            long[] amounts = new long[numberOfTransactions];
            int multiplier = positiveAmounts ? 1 : -1;
            for (int i = 0; i< numberOfTransactions; i++)
            {
                amounts[i] = random.Next(minAmount, maxAmount) * multiplier;
            }
            return amounts;
        }

        /// <summary>
        /// Creates a list of test transactions for a month.
        /// </summary>
        /// <param name="amounts">An array with the amounts to use.</param>
        /// <param name="year">The year to create the transactions.</param>
        /// <param name="month">The month to create the transactions.</param>
        /// <returns>A list of transactions.</returns>
        public IList<Transaction> CreateTransactionsInMonth(long[] amounts, int year, int month)
        {
            IList<Transaction> list = new List<Transaction>();

            int totalTransaction = amounts.Length;
            for (int i = 0; i < totalTransaction; i++)
            {
                int day = 1 + i % lastDayNumber;
                Transaction transaction = new Transaction()
                {
                    Amount = amounts[i],
                    TransactionTime = new DateTime(year, month, day)
                };
                list.Add(transaction);
            }

            return list;
        }
    }
}
