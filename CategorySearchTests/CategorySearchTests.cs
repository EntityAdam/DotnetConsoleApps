using FluentAssertions;

namespace CategorySearchTests
{
    public class CategorySearchTests
    {
        [Fact]
        public void CategoryFinder_MaximumResultsShouldBe4_WhenSearchTextHasManyResults()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "19");
            results.Count().Should().Be(4);
        }

        [Fact]
        public void CategoryFinder_ShouldReturnOneResult_WhenExactMatch()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "General Knowledge");
            results.Count().Should().Be(1);
        }

        [Fact]
        public void CategoryFinder_StartsWith()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "Auth");
            results.Select(c => c.CategoryName).Should().Contain("Authors");
        }

        [Fact]
        public void CategoryFinder_EndsWith()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "aphy");
            results.Select(c => c.CategoryName).Should().Contain("Geography");
        }

        [Fact]
        public void CategoryFinder_EndsWith_Multiple()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "ure");
            results.Select(c => c.CategoryName).Should().Contain("Pop Culture").And.Contain("Literature");
        }

        [Fact]
        public void CategoryFinder_EndsWithSplit()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "30s");
            results.Select(c => c.CategoryName).Should().Contain("1930s Trivia");
        }

        [Fact(Skip = "Not implemented yet")]
        public void CategoryFinder_IgnoreChars()
        {
            var categories = CategorySearchTestHelper.GenerateCategories().ToList();
            IEnumerable<Category> results = CategoryFinder.Search(categories, "1930's");
            results.Select(c => c.CategoryName).Should().Contain("1930s Trivia");
        }
    }

    public static class CategorySearchTestHelper
    {
        public static IEnumerable<Category> GenerateCategories()
        {
            foreach (string name in TestNames)
            {
                yield return CreateCategoryFromName(name);
            }
        }

        public static Category CreateCategoryFromName(string name)
        {
            return new Category(Guid.NewGuid().ToString(), name, false, "[REDACTED]", new DateTime(2020, 01, 05));
        }


        public static IEnumerable<NewCategory> GenerateNewCategories()
        {
            foreach (string name in TestNames)
            {
                yield return CreateNewCategoryFromName(name);
            }
        }

        public static NewCategory CreateNewCategoryFromName(string categoryName)
        {
            return new NewCategory(categoryName, "[REDACTED]");
        }

        private static string[] TestNames =>
            new string[] {
            "1920s Trivia",
            "1930s Trivia",
            "1940s Trivia",
            "1950s Trivia",
            "1960s Trivia",
            "1970s Trivia",
            "1980s Trivia",
            "1990s Trivia",
            "2000s Trivia",
            "2010s Trivia",
            "Animals",
            "Art",
            "Authors",
            "Cars",
            "Cooking",
            "General Knowledge",
            "Geography",
            "Health",
            "History",
            "Literature",
            "Maths",
            "Music",
            "Pop Culture",
            "Science",
            "Sports",
            "Technology",
            "Video Games"
            };
    }
}