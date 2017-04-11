using System.Collections.Generic;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.AverageCalculators
{
    public class AverageMonthCalculatorTests
    {
        /// <summary>
        /// Verify the calculation of the average month, calculated as the average of the total income and total expenses of each month.
        /// </summary>
        [Theory]
        [InlineData(new long[] { 225, 888, 162, 2484, 2792},     1310, new long[] { -867, -2148, -1677, -5669, -9708},  -4013)]
        [InlineData(new long[] { 1968, 8546, 3241, 6505, 4639 }, 4979, new long[] { -1544, -8368, -4967, -4699, -8026}, -5520)]
        [InlineData(new long[] { 7472, 5196, 3256, 862, 2774 },  3912, new long[] { -3910, -2698, -7701, -2295, -3847}, -4090)]
        [InlineData(new long[] { 2987, 3996, 5774, 6801, 8356},  5582, new long[] { -2733, -6390, -9250, -7446, -4716}, -6107)]
        [InlineData(new long[] { 38, 8134, 6, 3741, 1521 } ,     2688, new long[] { -6835, -9876, -1935, -9221, -2355}, -6044)]
        public void TestAverageMonth(long[] totalIncomePerMonth, long expectedAverageIncome, long[] totalExpensesPerMonth, long expectedAverageExpenses)
        {
            ICollection<MonthlySummary> months = new List<MonthlySummary>();
            for(int i = 0; i < totalIncomePerMonth.Length; i++)
            {
                MonthlySummary monthlySummary = new MonthlySummary()
                {
                    Income = totalIncomePerMonth[i],
                    Spent = totalExpensesPerMonth[i]
                };
                months.Add(monthlySummary);
            }

            IAverageMonthCalculator calculator = new AverageMonthCalculator();

            Assert.True(calculator.TryGetAverageMonth(months, out MonthlySummary averageMonth));
            Assert.NotNull(averageMonth);
            Assert.Equal(expectedAverageIncome, averageMonth.Income);
            Assert.Equal(expectedAverageExpenses, averageMonth.Spent);
        }

        [Fact]
        public void EmptyMonthlySummaryList()
        {
            IAverageMonthCalculator calculator = new AverageMonthCalculator();

            Assert.False(calculator.TryGetAverageMonth(null, out MonthlySummary averageMonth));
            Assert.Null(averageMonth);
        }
    }
}
