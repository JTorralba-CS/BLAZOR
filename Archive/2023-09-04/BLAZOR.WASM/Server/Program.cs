using BLAZOR.WASM.Server.Hubs;
using BLAZOR.WASM.Server.Workers;
using BLAZOR.WASM.Shared;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR(HubOptions =>
{
    HubOptions.EnableDetailedErrors = true;
});

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octest-stream" });
});

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddHostedService<HEARTBEAT>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.MapFallbackToFile("index.html");

app.Run();
