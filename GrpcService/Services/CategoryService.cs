using Google.Protobuf.Collections;
using Grpc.Core;
using GrpcServer.Protos;
using System.Text.Json;
namespace GrpcServer.Services
{
    public class CategoryService : recipe.recipeBase
    {
        private readonly ILogger _logger;
        private List<Recipe> _recipes { get; set; } = new();
        private List<string> _categories { get; set; } = new();
        private string _recipesFilePath;
        private string _categoriesFilePath;
        public CategoryService(ILogger logger)
        {
            _recipesFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "Recipes.json");
            _categoriesFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "Categories.json");
            _logger = logger;
        }

        public override async Task<CategoryList> getAllCategories(GetCategoriesRequest request, ServerCallContext context)
        {
            Data data = new(_logger);
            var categories = await data.GetAllCategoriesAsync();
            CategoryList categoryList = new();
            foreach (var category in categories)
            {
                categoryList.Categories.Add(category);
            }
            return categoryList;
        }
    }
}
