using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
using Grpc.Net.Client;

namespace Exercise3.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        [BindProperty]
        public Recipe Recipe { get; set; } = new();

        public CreateModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public async Task<IActionResult> OnGetAsync()
        {
            var reply = await _client.GetAllCategoriesAsync(new EmptyRequest { });
            var fetchCategories = reply.Categories;
            foreach (var category in fetchCategories)
            {
                Recipe.Categories.Add(category);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Recipe == null)
                return Page();
            var newRecipe = new Client.Protos.Recipe
            {
                Id = Guid.NewGuid().ToString(),
                Title = Recipe.Title,
                Ingredients = Recipe.Ingredients,
                Instructions = Recipe.Instructions
            };
            foreach(var category in Recipe.Categories)
            {
                newRecipe.Categories.Add(category);
            }
            var reply = await _client.AddRecipeAsync(newRecipe);
            if(reply.StatusCode.Equals(200))
                return RedirectToPage("./Index");
            return Page();
        }
    }
}