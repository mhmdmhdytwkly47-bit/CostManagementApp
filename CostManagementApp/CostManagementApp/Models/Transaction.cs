namespace CostManagementApp.Models;

public class Transaction
{
    public int id { get; set; }

    public int user_id { get; set; }

    public int category_id { get; set; }

    public decimal amount { get; set; }

    public string description { get; set; } = "";

    public string type { get; set; } = "";

    public DateTime transaction_date { get; set; }
}