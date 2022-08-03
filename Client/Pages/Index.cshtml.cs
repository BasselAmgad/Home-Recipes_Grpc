using Grpc.Net.Client;
using GrpcClient;
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
            var input = new HelloRequest { Name = "Bassel" };
            var channel = GrpcChannel.ForAddress(_configuration["url"]);
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(input);
            Message = reply.Message;
        }
    }
}