using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
using Grpc.Net.Client;

namespace Exercise3.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<Recipe> Recipes { get; set; } = new();
        public string? ChoosenCategory { get; set; }
        public IndexModel(IConfiguration configuration) => _config = configuration;

        public async Task OnGetAsync(string? category)
        {
            var channel = GrpcChannel.ForAddress(_config["grpcUrl"]);
            var client = new Client.Protos.Recipes.RecipesClient(channel);
            var reply = await client.GetAllRecipesAsync(new EmptyRequest { });
            var fetchRecipes = new List<Recipe>();
            foreach(var recipe in reply.Recipes)
            {
                var recipeCategories = new List<string>();
                foreach (var categoryItem in recipe.Categories)
                {
                    recipeCategories.Add(categoryItem);
                }
                var recipeObject = new Recipe(
                    recipe.Id,
                    recipe.Title, 
                    recipe.Ingredients, 
                    recipe.Instructions, 
                    recipeCategories);
                fetchRecipes.Add(recipeObject);
            }
            if (fetchRecipes is not null)
            {
                if (category is not null)
                {
                    fetchRecipes = fetchRecipes.Where(r => r.Categories.Contains(category)).ToList();
                    ChoosenCategory = category;
                }
                Recipes = fetchRecipes;
            }
        }
    }
}
