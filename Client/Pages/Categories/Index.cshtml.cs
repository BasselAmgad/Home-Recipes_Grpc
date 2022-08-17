using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
using Grpc.Net.Client;

namespace Exercise3.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Client.Protos.Recipes.RecipesClient _client;
        public List<string> Categories { get; set; } = new();
        public IndexModel(Client.Protos.Recipes.RecipesClient client) => _client = client;

        public async Task<IActionResult> OnGetAsync()
        {
            var reply = await _client.GetAllCategoriesAsync(new EmptyRequest { });
            foreach(var category in reply.Categories)
            {
                Categories.Add(category);
            }
            return Page();
        }
    }
}
