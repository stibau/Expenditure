namespace Services;

public class Expense
{
    public int Id { get; set; }
    
    public int SpenderId { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }
}