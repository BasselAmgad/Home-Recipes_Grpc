using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;

namespace Exercise3.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _config;
        [BindProperty]
        public string Category { get; set; } = "";

        public DeleteModel (IConfiguration config) => _config = config;
            
        public void OnGet(string title)
        {
            Category = title;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var channel = GrpcChannel.ForAddress(_config["grpcUrl"]);
            var client = new Client.Protos.Recipes.RecipesClient(channel);
            var request = new Category { CategoryName = Category };
            var reply = await client.DeleteCategoryAsync(request);
            return Page();
        }
    }
}
