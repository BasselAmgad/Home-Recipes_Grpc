using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;

namespace Exercise3.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        [BindProperty]
        public string Category { get; set; } = "";

        public DeleteModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public void OnGet(string title)
        {
            Category = title;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var request = new Category { CategoryName = Category };
            var reply = await _client.DeleteCategoryAsync(request);
            return Page();
        }
    }
}
