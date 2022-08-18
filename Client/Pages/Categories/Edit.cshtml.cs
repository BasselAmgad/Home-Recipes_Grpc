using Grpc.Net.Client;
using Client.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public string NewCategory { get; set; }
        public EditModel(Client.Protos.Recipes.RecipesClient client)
        {
            _client = client;
            Category = "";
            NewCategory = "";
        }

        public void OnGet(string title)
        {
            Category = title;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var reply = await _client.EditCategoryAsync(
                new EditCategoryRequest { OldCategory = Category, NewCategory = NewCategory });
            return RedirectToPage("./Index");
        }
    }
}
