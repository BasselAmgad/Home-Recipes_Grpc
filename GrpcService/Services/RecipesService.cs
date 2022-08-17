using Grpc.Core;
using GrpcServer.Protos;

namespace GrpcServer.Services
{
    public class RecipesService : Recipes.RecipesBase
    {
        private readonly Data _Data;

        public RecipesService(Data data)
        {
            _Data = data;
        }

        public override async Task<RecipeList> GetAllRecipes(EmptyRequest request, ServerCallContext context)
        {
            var recipes = await _Data.GetRecipesAsync();
            RecipeList recipeList = new();
            foreach (var recipe in recipes)
            {
                recipeList.Recipes.Add(recipe);
            }
            return recipeList;
        }

        public override async Task<Recipe> GetRecipe(RecipeRequest request, ServerCallContext context)
        {
            var recipe = await _Data.GetRecipeAsync(new Guid(request.Id));
            return recipe;
        }

        public override async Task<RecipeResponse> AddRecipe(Recipe recipe, ServerCallContext context)
        {
            await _Data.AddRecipeAsync(recipe);
            return new RecipeResponse() { Recipe = recipe, StatusCode = StatusCodes.Status200OK };
        }

        public override async Task<RecipeResponse> EditRecipe(EditRecipeRequest request, ServerCallContext context)
        {
            var updatedRecipe = await _Data.EditRecipeAsync(new Guid(request.Id), request.NewRecipe);
            return new RecipeResponse() { StatusCode = StatusCodes.Status200OK, Recipe = updatedRecipe };
        }

        public override async Task<RecipeResponse> DeleteRecipe(RecipeRequest request, ServerCallContext context)
        {
            await _Data.RemoveRecipeAsync(new Guid(request.Id));
            return new RecipeResponse(){ StatusCode = StatusCodes.Status200OK, Recipe = null};
        }

        public override async Task<CategoryList> GetAllCategories(EmptyRequest request, ServerCallContext context)
        {
            var categories = await _Data.GetAllCategoriesAsync();
            CategoryList categoryList = new();
            foreach (var category in categories)
            {
                categoryList.Categories.Add(category);
            }
            return categoryList;
        }

        public override async Task<CategoryResponse> AddCategory(Category request, ServerCallContext context)
        {
            await _Data.AddCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> EditCategory(EditCategoryRequest request, ServerCallContext context)
        {
            await _Data.EditCategoryAsync(request.OldCategory,request.NewCategory);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> DeleteCategory(Category request, ServerCallContext context)
        {
            await _Data.RemoveCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }
    }
}
