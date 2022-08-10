using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercise3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Message { get; set; }
        public IndexModel(IConfiguration config) => _configuration = config;

        public async Task OnGetAsync()
        {
            
        }
    }
}