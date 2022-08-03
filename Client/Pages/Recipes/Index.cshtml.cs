using Grpc.Net.Client;
using GrpcClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Recipe> Recipes { get; set; } = new();
        public IndexModel(IConfiguration config) => _configuration = config;

        public async Task OnGetAsync()
        {
            var input = new HelloRequest { Name = "Bassel" };
            var channel = GrpcChannel.ForAddress(_configuration["url"]);
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(input);
/*            var fetchRecipes;
            if (fetchRecipes is not null)
                Recipes = fetchRecipes;*/
        }
    }
}
