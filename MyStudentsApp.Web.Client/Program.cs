using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyStudentsApp.Shared.Services;
using MyStudentsApp.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the MyStudentsApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
