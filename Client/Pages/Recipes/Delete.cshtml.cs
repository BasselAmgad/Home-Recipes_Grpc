using Client.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Recipes
{
    public class DeleteModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        public Recipe FetchedRecipe { get; set; } = new();
        public IEnumerable<string> DetailedIngredients { get; set; } = new List<string>();
        public IEnumerable<string> DetailedInstructions { get; set; } = new List<string>();
        public IEnumerable<string> DetailedCategories { get; set; } = new List<string>();

        public DeleteModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public async Task<IActionResult> OnGet(Guid id)
        {
            var recipe = await _client.GetRecipeAsync(new RecipeRequest { Id = id.ToString() });
            if (recipe is null)
                return NotFound();
            FetchedRecipe = new Recipe(recipe);
            DetailedIngredients = FetchedRecipe.Ingredients.Split("\n").Select(x => $"{x}");
            DetailedInstructions = FetchedRecipe.Instructions.Split("\n").Select((x, n) => $"{x}");
            DetailedCategories = FetchedRecipe.Categories.Select((x, n) => $"{x}");
            return Page();
                
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var reply = await _client.DeleteRecipeAsync(new RecipeRequest { Id = id.ToString() });
            if(reply.StatusCode != 200)
                return NotFound();
            return RedirectToPage("./Index");
        }
    }
}
