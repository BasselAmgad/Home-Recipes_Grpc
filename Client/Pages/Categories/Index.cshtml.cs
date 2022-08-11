using Grpc.Net.Client;
using GrpcClient.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<string> Categories { get; set; } = new();
        public IndexModel(IConfiguration config) => _config = config;

        public async Task<IActionResult> OnGetAsync()
        {
            var channel = GrpcChannel.ForAddress(_config["url"]);
            var client = new recipe.recipeClient(channel);
            var request = new EmptyRequest();
            CategoryList response = await client.GetAllCategoriesAsync(request);
            foreach (var category in response.Categories)
            {
                Categories.Add(category);
            }
            return Page();
        }
    }
}
