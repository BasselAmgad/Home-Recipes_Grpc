using GrpcServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<Data>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<RecipesService>();
app.UseRouting();
app.UseGrpcWeb(); 
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<RecipesService>().EnableGrpcWeb();
});
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();
