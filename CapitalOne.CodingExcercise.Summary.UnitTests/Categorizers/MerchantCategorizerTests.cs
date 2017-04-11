using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Domain;
using CapitalOne.CodingExcercise.Summary.Model;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.Categorizers
{
    public class MerchantCategorizerTests
    {
        [Fact]
        public void TestSummarizeByMerchant()
        {
            // Arrange
            const string KrispyKreme = "Krispy Kreme Donuts";
            const string Dunkin = "DUNKIN #336784";

            // Exclude donuts transactions
            ISet<string> ExcludedMerchants = new HashSet<string>()
            {
                { KrispyKreme },
                { Dunkin }
            };

            IList<BankTransaction> includedExpectedList = new List<BankTransaction>()
            {
                new BankTransaction() { RawMerchant = "Star Wars Store", TransactionId = "001" },
                new BankTransaction() { RawMerchant = "7-Eleven", TransactionId = "002" },
                new BankTransaction() { RawMerchant = "Super Store", TransactionId = "003" },
            };

            IList<BankTransaction> excludedExpectedList = new List<BankTransaction>()
            {
                new BankTransaction() { RawMerchant = KrispyKreme, TransactionId = "004" },
                new BankTransaction() { RawMerchant = Dunkin, TransactionId = "005" },
                new BankTransaction() { RawMerchant = "krISpy krEme doNuts", TransactionId = "006" },
                new BankTransaction() { RawMerchant = "KRISPY krEme doNuts", TransactionId = "007" },
                new BankTransaction() { RawMerchant = "DUNkin #336784", TransactionId = "008" },
            };

            // Randomize the order of the transactions
            Random random = new Random();
            IEnumerable<BankTransaction> allTransaction = includedExpectedList.Concat(excludedExpectedList)
                .OrderBy(x => random.Next()).ToList();

            // Act
            ExcludeStringFieldValueCategorizer categorizer = new ExcludeStringFieldValueCategorizer(ExcludedMerchants, (BankTransaction t) => (t.RawMerchant));
            var buckets = categorizer.Categorize(allTransaction);

            // Assert
            Assert.NotNull(buckets);
            Assert.True(buckets.ContainsKey(ExcludeStringFieldValueCategorizer.IncludeKey), "Does not contains bucket of included transactions.");
            Assert.True(buckets.ContainsKey(ExcludeStringFieldValueCategorizer.ExcludeKey), "Does not contains bucket of excluded transactions.");

            var bucketIncluded = buckets[ExcludeStringFieldValueCategorizer.IncludeKey];
            var bucketExcluded = buckets[ExcludeStringFieldValueCategorizer.ExcludeKey];
            Assert.NotNull(bucketIncluded);
            Assert.NotNull(bucketExcluded);

            IList<BankTransaction> actualIncluded = bucketIncluded.Transactions;
            IList<BankTransaction> actualExcluded = bucketExcluded.Transactions;

            Assert.NotNull(actualIncluded);
            Assert.NotNull(actualExcluded);

            Assert.Equal(includedExpectedList.OrderBy(t=> t.TransactionId), actualIncluded.OrderBy(t => t.TransactionId));
            Assert.Equal(excludedExpectedList.OrderBy(t => t.TransactionId), actualExcluded.OrderBy(t => t.TransactionId));
        }
    }
}
