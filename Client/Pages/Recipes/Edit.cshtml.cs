using Client.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Recipes
{
    public class EditModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        [BindProperty]
        public Recipe Recipe { get; set; } = new();
        [BindProperty]
        public List<string> Categories { get; set; } = new();
        public EditModel(Client.Protos.Recipes.RecipesClient client) => _client = client;


        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var categoriesReq = await _client.GetAllCategoriesAsync(new EmptyRequest { });
            var recipeReq = await _client.GetRecipeAsync(new RecipeRequest { Id = id.ToString()});
            if (recipeReq == null || categoriesReq == null)
                return NotFound();
            Recipe = new Recipe(recipeReq);
            foreach(var category in categoriesReq.Categories)
            {
                Categories.Add(category);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRecipe = new Client.Protos.Recipe 
            {
                Id = Recipe.Id.ToString(),
                Ingredients = Recipe.Ingredients,
                Instructions = Recipe.Instructions
            };
            foreach(var category in Recipe.Categories)
            {
                newRecipe.Categories.Add(category);
            }
            var reply = await _client.EditRecipeAsync(
                new EditRecipeRequest 
                { 
                    Id = Recipe.Id.ToString(),
                    NewRecipe = newRecipe,
                });
            if (reply.StatusCode != 200 || !ModelState.IsValid)
                return Page();
            return RedirectToPage("./Index");
        }
    }
}
