using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Services;
using Xunit;

namespace Service.Test;

public class CostRepartitionServiceFixture
{
    [Fact]
    public void GetAllCosts_Should_ReturnData()
    {
        // Arranged
        var sut = new CostRepartitionService(new Logger<CostRepartitionService>(new LoggerFactory()));

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(3)]
    public void GetAllCosts_ShouldReturnTheCorrectNumberOfElements(int expectedCount)
    {
        // Arrange
        var sut = new CostRepartitionService(new Logger<CostRepartitionService>(new LoggerFactory()));

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Count.Should().Be(expectedCount);
    }

    [Theory]
    [AutoData]
    public void GetAllCosts_ShouldReturnTheDataAsExpected(CostRepartitionService sut)
    {
        // Arrange
        var expected = new List<Expense>
        {
            new("Stef", "ENGIE", 167.55m),
            new("Euge", "Carrefour", 82.3m),
            new("Stef", "Carrefour", 50m)
        };

        // Act
        var actual = sut.GetAllExpenses();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}