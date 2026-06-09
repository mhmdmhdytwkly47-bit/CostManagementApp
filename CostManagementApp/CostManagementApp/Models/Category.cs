namespace CostManagementApp.Models;

public class Category
{
    public int id { get; set; }

    public int user_id { get; set; }

    public string name { get; set; } = "";

    public string type { get; set; } = "";
}