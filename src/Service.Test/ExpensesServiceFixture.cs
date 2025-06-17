using FluentAssertions;
using Microsoft.Extensions.Logging;
using Services;
using Xunit;

namespace Service.Test;

public class ExpensesServiceFixture
{
    [Fact]
    public void GetAllCosts_Should_ReturnData()
    {
        // Arrange
        var sut = new ExpensesService(new Logger<ExpensesService>(new LoggerFactory()));

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(4)]
    public void GetAllCosts_ShouldReturnTheCorrectNumberOfElements(int expectedCount)
    {
        // Arrange
        var sut = new ExpensesService(new Logger<ExpensesService>(new LoggerFactory()));

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Count.Should().Be(expectedCount);
    }

    [Fact]
    public void GetAllCosts_ShouldReturnTheDataAsExpected()
    {
        // Arrange
        var sut = new ExpensesService(new Logger<ExpensesService>(new LoggerFactory()));
        var expected = new List<Expense>
        {
            new() { Id=1, SpenderId = 1, Description = "Carrefour", Amount = 123m, Date = new DateTime(2025, 03, 15) },
            new() { Id=2, SpenderId = 2, Description = "Carrefour", Amount = 50m, Date = new DateTime(2025, 04, 20) },
            new() { Id=3, SpenderId = 1, Description = "Delhaize", Amount = 253m, Date = new DateTime(2025, 02, 28) },
            new() { Id=4, SpenderId = 2, Description = "Delhaize", Amount = 253m, Date = new DateTime(2025, 02, 28) }
        };

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}