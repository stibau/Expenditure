
// ReSharper disable NotAccessedPositionalProperty.Global
namespace Services;

public record Expense(string Initiator, string Purpose, decimal Amount);

public class CostRepartitionService
{
    private readonly List<Expense> _expenses;

    public CostRepartitionService()
    {
        _expenses = new List<Expense>();

        //Add some initial data for testing purposes
        _expenses.Add(new Expense("Stef", "ENGIE", 167.55m));
        _expenses.Add(new Expense("Euge", "Carrefour", 82.3m));
        _expenses.Add(new Expense("Stef", "Carrefour", 50m));
    }

    public List<Expense> GetAllExpenses()
    {
        return _expenses;
    }
}