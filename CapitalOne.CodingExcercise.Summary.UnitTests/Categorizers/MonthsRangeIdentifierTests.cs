using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using CapitalOne.CodingExcercise.Summary.Model;
using CapitalOne.CodingExcercise.Summary.Domain;

namespace CapitalOne.CodingExcercise.Summary.UnitTests.Categorizers
{
    public class MonthsRangeIdentifierTests
    {
        [Fact]
        public void TestSelectMonths()
        {
            IEnumerable<BankTransaction> transactions = new List<BankTransaction>()
            {
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2016,6,12)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2016,6,15)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2016,6,8)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2016,8,12)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2016,9,12)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2017,9,1)
                },
                new BankTransaction()
                {
                    TransactionTime = new DateTime(2017,3,20)
                }
            };

            IMonthsRangeIdentifier monthsRangeIdentifier = new MonthsRangeIdentifier();

            // Select all months.
            var months = monthsRangeIdentifier.GetMonthsInTransaction(transactions);
            Assert.NotNull(months);
            Assert.Equal(expected: 5, actual: months.Count());

            // Only months in 2017
            months = monthsRangeIdentifier.GetMonthsInTransaction(transactions, greaterOrEqualThanDate: new DateTime(2017, 2, 2));
            Assert.NotNull(months);
            Assert.Equal(expected: 2, actual: months.Count());
        }

        [Fact]
        public void EmptyListOfTransaction()
        {
            IMonthsRangeIdentifier monthsRangeIdentifier = new MonthsRangeIdentifier();

            // Select all months.
            var months = monthsRangeIdentifier.GetMonthsInTransaction(transactions: null);
            Assert.NotNull(months);
            Assert.Equal(expected: 0, actual: months.Count());

            // Only months in 2017
            months = monthsRangeIdentifier.GetMonthsInTransaction(transactions: null, greaterOrEqualThanDate: new DateTime(2017, 2, 2));
            Assert.NotNull(months);
            Assert.Equal(expected: 0, actual: months.Count());
        }
    }
}
