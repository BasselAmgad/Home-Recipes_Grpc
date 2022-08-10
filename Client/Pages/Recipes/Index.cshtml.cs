using Grpc.Net.Client;
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
            
        }
    }
}
