using Grpc.Net.Client;
using GrpcClient.Protos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _cofnig;
        public List<Recipe> Recipes { get; set; } = new();
        public IndexModel(IConfiguration config) => _cofnig = config;

        public async Task OnGetAsync()
        {
            var channel = GrpcChannel.ForAddress(_cofnig["url"]);
            var client = new recipe.recipeClient(channel);
            var request = new EmptyRequest();
            RecipeList response = await client.GetAllRecipesAsync(request);
            foreach (var recipe in response.Recipes)
            {
                Recipes.Add(new Recipe(recipe));
            }
        }
    }
}
