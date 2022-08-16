using Grpc.Net.Client;
using Client.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _config;
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public string NewCategory { get; set; }
        public EditModel(IConfiguration config)
        {
            _config = config;
            Category = "";
            NewCategory = "";
        }

        public void OnGet(string title)
        {
            Category = title;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var channel = GrpcChannel.ForAddress(_config["grpcUrl"]);
            var client = new Client.Protos.Recipes.RecipesClient(channel);
            var reply = await client.EditCategoryAsync(
                new EditCategoryRequest { OldCategory = Category, NewCategory = NewCategory });
            return Page();
        }
    }
}
