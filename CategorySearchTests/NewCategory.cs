namespace CategorySearchTests;

public class NewCategory
{
    public NewCategory(string categoryName, string createdByUser)
    {
        CategoryName = categoryName;
        CreatedByUser = createdByUser;
    }

    public string CategoryName { get; }
    public string CreatedByUser { get; }
}
