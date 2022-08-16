using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;

namespace Exercise3.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<string> Categories { get; set; } = new();
        public IndexModel(IConfiguration config) => _config = config;

        public async Task<IActionResult> OnGetAsync()
        {
            var channel = GrpcChannel.ForAddress(_config["grpcUrl"]);
            var client = new Client.Protos.Recipes.RecipesClient(channel);
            var reply = await client.GetAllCategoriesAsync(new EmptyRequest { });
            foreach(var category in reply.Categories)
            {
                Categories.Add(category);
            }
            return Page();
        }
    }
}
