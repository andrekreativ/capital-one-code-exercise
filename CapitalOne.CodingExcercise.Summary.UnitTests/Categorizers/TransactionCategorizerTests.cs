using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.Categorizers
{
    public class TransactionCategorizerTests
    {
        /// <summary>
        /// Verifies that the categorizer splits the transactions into the appropiate number of buckets.
        /// </summary>
        /// <param name="numberOfBuckets">The number of buckets to create.</param>
        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(1)]
        [InlineData(100)]
        public void VerifyNumberOfBuckets(int numberOfBuckets)
        {
            // Arrange

            // Use the amount % number of buckets formula to split the transactions into buckets.
            Func<Transaction, int> getBucketKey = (Transaction t) =>
            {
                return (int)t.Amount % numberOfBuckets;
            };

            // Use mock buckets, just to verify the number of created buckets.
            Func<ISummaryBucket<Transaction>> createBucket = () =>
            {
                var mockBucket = new Mock<ISummaryBucket<Transaction>>();
                mockBucket
                    .Setup(b => b.Add(It.IsAny<Transaction>()))
                    .Verifiable();
                return mockBucket.Object;
            };

            var categorizer = new TransactionCategorizer<Transaction, int, ISummaryBucket<Transaction>>(createBucket, getBucketKey);

            // Create test transactions.
            int numberOftransaction = 100;
            var list = new List<Transaction>();
            for(int i = 1; i <= numberOftransaction;i++)
            {
                Transaction t = new Transaction()
                {
                    TransactionId = string.Format("T{0}", i),
                    Amount = 500 + i,
                    TransactionTime = DateTime.Now
                };
                list.Add(t);
            }

            // Act
            var buckets = categorizer.Categorize(list);

            // Assert
            Assert.NotNull(buckets);
            Assert.Equal(numberOfBuckets, buckets.Count);
        }
    }
}
