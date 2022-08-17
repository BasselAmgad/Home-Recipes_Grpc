using Grpc.Net.Client;
using Client.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        [BindProperty]
        public string Category { get; set; } = default!;
        public CreateModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var request = new Category { CategoryName = Category };
            var reply = await _client.AddCategoryAsync(request);
            return Page();
        }
    }
}
