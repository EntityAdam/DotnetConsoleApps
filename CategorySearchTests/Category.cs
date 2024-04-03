namespace CategorySearchTests;

public class Category
{
    public Category(string categoryId, string categoryName, bool isEnabled, string createdByUser, DateTime createdTime)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        IsEnabled = isEnabled;
        CreatedByUser = createdByUser;
        CreatedTime = createdTime;
    }

    public Category(NewCategory category, DateTime dateTime)
    {
        CategoryId = Guid.NewGuid().ToString();
        CategoryName = category.CategoryName;
        IsEnabled = false;
        CreatedByUser = category.CreatedByUser;
        CreatedTime = dateTime;
    }

    public string CategoryId { get; }
    public string CategoryName { get; }
    public bool IsEnabled { get; }
    public string CreatedByUser { get; }
    public DateTime CreatedTime { get; }
}

