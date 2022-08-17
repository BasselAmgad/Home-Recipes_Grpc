using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
using Grpc.Net.Client;

namespace Exercise3.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        public List<Recipe> Recipes { get; set; } = new();
        public string? ChoosenCategory { get; set; }
        public IndexModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public async Task OnGetAsync(string? category)
        {
            var reply = await _client.GetAllRecipesAsync(new EmptyRequest { });
            var fetchRecipes = new List<Recipe>();
            foreach(var recipe in reply.Recipes)
            {
                var recipeObject = new Recipe(recipe);
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
