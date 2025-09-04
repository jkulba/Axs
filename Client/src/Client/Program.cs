using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Client;
using Client.Services;
using Client.Components.VersionInfo;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();

// Register VersionInfoService with HttpClient pointing to the client root
builder.Services.AddHttpClient<VersionInfoService>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

// Register Refit client for IAccessRequestApi
builder.Services.AddRefitClient<IAccessRequestApi>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5001");
    });

// Register Refit client for IUserApi
builder.Services.AddRefitClient<IUserApi>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5001");
    });


await builder.Build().RunAsync();
