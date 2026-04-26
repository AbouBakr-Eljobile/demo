using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GovContracts.Web;
using GovContracts.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5025";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<AuthSessionService>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<ContractsApiService>();
builder.Services.AddScoped<AttachmentTemplatesApiService>();

await builder.Build().RunAsync();
