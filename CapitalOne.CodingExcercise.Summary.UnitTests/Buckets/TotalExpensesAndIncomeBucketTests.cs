using System;
using System.Linq;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;


namespace CapitalOne.CodingExcercise.Summary.UnitTests.Buckets
{
    public class TotalExpensesAndIncomeBucketTests
    {
        [Theory]
        // Mix of positive and negative amounts.
        [InlineData(new long[] { 1000, 500, -250, -50, -30 })]
        // All positive amounts.
        [InlineData(new long[] { 5000, 100,  200, 600, 400 })]
        // All negative amounts.
        [InlineData(new long[] { -100, -50, -250, -50, -30 })]
        // Mix with positive, negative and zeros.
        [InlineData(new long[] { 1000, 500, 0, -50, 0, -30 })]
        // All zeros.
        [InlineData(new long[] { 0, 0, 0, 0, 0 })]
        public void AddTransactions(long[] amounts)
        {
            TotalIncomeExpenseBucket bucket = new TotalIncomeExpenseBucket();

            // Arrange:
            // Add transactions with the given amounts in random order.
            // We are only counting total amounts, the order shouldn't matter.
            Random random = new Random();
            foreach (long amount in amounts.OrderBy(x => random.Next()))
            {
                // Act
                bucket.Add(new Transaction(){ Amount = amount });
            }

            long expectedIncome = amounts.Where(a => a > 0).Sum();
            long expectedExpenses = amounts.Where(a => a < 0).Sum();

            // Assert
            Assert.Equal(expectedIncome, bucket.Income);
            Assert.Equal(expectedExpenses, bucket.Expenses);
        }
    }
}
