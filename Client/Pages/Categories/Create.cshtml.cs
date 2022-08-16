using Grpc.Net.Client;
using Client.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;
        [BindProperty]
        public string Category { get; set; } = default!;
        public CreateModel(IConfiguration configuration) => _config = configuration;

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var channel = GrpcChannel.ForAddress(_config["grpcUrl"]);
            var client = new Client.Protos.Recipes.RecipesClient(channel);
            var request = new Category { CategoryName = Category };
            var reply = client.AddCategory(request);
            return Page();
        }
    }
}
