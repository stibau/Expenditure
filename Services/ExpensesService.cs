// ReSharper disable NotAccessedPositionalProperty.Global

using Microsoft.Extensions.Logging;

namespace Services;

public class ExpensesService : IExpensesService
{
    private readonly List<Expense> _expenses;
    private readonly ILogger<ExpensesService> _log;

    public ExpensesService(ILogger<ExpensesService> log)
    {
        _expenses = [];
        _log = log;

        // Add some initial data for testing purposes
        _expenses.Add(new Expense { Id=1, SpenderId = 1, Description = "Carrefour", Amount = 123m, Date = new DateTime(2025,03,15)});
        _expenses.Add(new Expense { Id=2, SpenderId = 2, Description = "Carrefour", Amount = 50m, Date = new DateTime(2025,04,20)});
        _expenses.Add(new Expense { Id=3, SpenderId = 1, Description = "Delhaize", Amount = 253m, Date = new DateTime(2025,02,28)});
        _expenses.Add(new Expense { Id=4, SpenderId = 2, Description = "Delhaize", Amount = 253m, Date = new DateTime(2025,02,28)});
    }

    public List<Expense> GetAllExpenses()
    {
        _log.LogInformation("Getting all expenses at {Time}", DateTime.UtcNow);
        return _expenses;
    }
}