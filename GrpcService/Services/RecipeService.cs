using Google.Protobuf.Collections;
using Grpc.Core;
using GrpcServer.Protos;
using System.Text.Json;
namespace GrpcServer.Services
{
    public class RecipeService : recipe.recipeBase
    {
        private readonly ILogger _logger;

        public RecipeService(ILogger logger) => _logger = logger;

        public override async Task<CategoryList> getAllCategories(EmptyRequest request, ServerCallContext context)
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

        public override async Task<CategoryResponse> addCategory(Category request, ServerCallContext context)
        {
            Data data = new(_logger);
            await data.AddCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> editCategory(EditCategoryRequest request, ServerCallContext context)
        {
            Data data = new(_logger);
            await data.EditCategoryAsync(request.OldCategory,request.NewCategory);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> deleteCategory(Category request, ServerCallContext context)
        {
            Data data = new(_logger);
            await data.RemoveCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }
    }
}
