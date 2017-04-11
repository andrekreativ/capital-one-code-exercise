using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.Categorizers
{
    public class GroupByAbsoluteAmountCategorizerTests
    {
        [Theory]
        [InlineData(new long[] { -500, 500, 100, -100, 50, 30 })]
        [InlineData(new long[] { -500, -500, 500, 0, 0, 30, 200, 200, 200 })]
        [InlineData(new long[] { -500, -500, -500, -345, -345 })]
        public void VerifyGroupByAbsAmount(long[] amounts)
        {
            // Arrange
            IList<BankTransaction> transactions = new List<BankTransaction>();

            for(int i = 0; i < amounts.Length; i++)
            {
                var transaction = new BankTransaction()
                {
                    TransactionId = (100 + i).ToString(),
                    Amount = amounts[i]
                };
                transactions.Add(transaction);
            }

            // Randomize the order of the transaction.
            Random random = new Random();
            transactions = transactions.OrderBy(x => random.Next()).ToList();

            // Act
            GroupByAbsoluteAmountCategorizer categorizer = new GroupByAbsoluteAmountCategorizer();
            var buckets = categorizer.Categorize(transactions);

            // Assert
            var distintAmounts = amounts.Select(a => Math.Abs(a)).Distinct();
            foreach(long amount in distintAmounts)
            {
                Assert.True(buckets.ContainsKey(amount));

                var bucket = buckets[amount];
                Assert.NotNull(bucket);
                Assert.NotEmpty(bucket.Transactions);

                var expectedList = transactions
                    .Where(t => Math.Abs(t.Amount) ==  amount)
                    .OrderBy(t => t.TransactionId);

                Assert.Equal(expectedList, bucket.Transactions.OrderBy(t => t.TransactionId));
            }
        }
    }
}
