using System.Text.Json.Serialization;
using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var name = Environment.GetEnvironmentVariable("FSP_NAME");

var builder = WebApplication.CreateBuilder(args);
var loopBaseUriString = Environment.GetEnvironmentVariable("ITSTILOOP_URI");
var apiId = Environment.GetEnvironmentVariable("ITSTILOOP_API_ID");
var apiKey = Environment.GetEnvironmentVariable("ITSTILOOP_API_KEY");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"{name} API",
        Description = "FSP API for transfers"
    });
});
builder.Services.AddSingleton<AccountService>();
builder.Services.AddTransient<IHttpPostClient, HttpPostClient>();

builder.Services.AddHttpClient("itstiloop", httpClient =>
{
    httpClient.BaseAddress = new Uri(loopBaseUriString);
    httpClient.DefaultRequestHeaders.Add(
        "ApiKey", apiKey);
    httpClient.DefaultRequestHeaders.Add(
        "ApiId", apiId);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        
        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://itstiloop.azurewebsites.net/{name.ToLower()}" } };
    });
});
app.UseSwaggerUI();

//}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
