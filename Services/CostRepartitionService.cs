
// ReSharper disable NotAccessedPositionalProperty.Global

using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Logging;

namespace Services;

public record Expense(string Initiator, string Purpose, decimal Amount);

public class CostRepartitionService
{
    private readonly List<Expense> _expenses;
    private readonly ILogger<CostRepartitionService> _log;

    public CostRepartitionService(ILogger<CostRepartitionService> log)
    {
        _expenses = new List<Expense>();
        _log = log;

        //Add some initial data for testing purposes
        _expenses.Add(new Expense("Stef", "ENGIE", 167.55m));
        _expenses.Add(new Expense("Euge", "Carrefour", 82.3m));
        _expenses.Add(new Expense("Stef", "Carrefour", 50m));
    }

    public List<Expense> GetAllExpenses()
    {
        _log.LogInformation("Getting all expenses");
        return _expenses;
    }
}