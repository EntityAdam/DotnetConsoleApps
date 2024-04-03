namespace CategorySearchTests;

public static class CategoryFinder
{
    public static IEnumerable<Category> Search(List<Category> categories, string search)
    {
        const int max_results = 4;
        return SearchCore(categories, search).Take(max_results);
    }

    private static IEnumerable<Category> SearchCore(List<Category> categories, string search)
    {
        foreach (Category category in categories)
        {
            string categoryName = category.CategoryName;
            if (IsEqualIgnoreCase(search, categoryName)) { yield return category; yield break; }
            else if (StartsWithIgnoreCase(search, categoryName)) { yield return category; }
            else if (StartsWithAnyTermIgnoreCase(search, categoryName)) { yield return category; }
            else if (EndsWithAnyTermIgnoreCase(search, categoryName)) { yield return category; }
        }
    }

    private static bool StartsWithAnyTermIgnoreCase(string search, string category)
    {
        string[] terms = SplitCategoryByWords(category);
        foreach (string term in terms)
        {
            if (StartsWithIgnoreCase(search, term)) { return true; }
        }
        return false;
    }

    private static bool EndsWithAnyTermIgnoreCase(string search, string category)
    {
        string[] terms = SplitCategoryByWords(category);
        foreach (string term in terms)
        {
            if (EndsWithIgnoreCase(search, term)) { return true; }
        }
        return false;
    }

    private static bool StartsWithIgnoreCase(string search, string category)
    {
        return category.StartsWith(search, StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool EndsWithIgnoreCase(string search, string category)
    {
        return category.EndsWith(search, StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool IsEqualIgnoreCase(string search, string category)
    {
        return string.Equals(search, category, StringComparison.InvariantCultureIgnoreCase);
    }

    private static string[] SplitCategoryByWords(string words)
    {
        return words.Split(' ');
    }
}