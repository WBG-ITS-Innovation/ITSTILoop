using System.Text.Json.Serialization;
using ITSTILoopDTOLibrary;
using ITSTILoopLibrary.Utility;
using ITSTILoopLibrary.UtilityServices;
using ITSTILoopLibrary.UtilityServices.Interfaces;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using CBDCTransferContract;

var name = Environment.GetEnvironmentVariable("FSP_NAME");

var builder = WebApplication.CreateBuilder(args);


//let's configure for participant fsp
if (EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.IS_LOOP_PARTICIPANT, EnvironmentVariableDefaultValues.IS_LOOP_PARTICIPANT_DEFAULT_VALUE).ToLower() == "true")
{
    var loopBaseUriString = Environment.GetEnvironmentVariable("ITSTILOOP_URI");
    var apiId = Environment.GetEnvironmentVariable("ITSTILOOP_API_ID");
    var apiKey = Environment.GetEnvironmentVariable("ITSTILOOP_API_KEY");
    builder.Services.AddHttpClient("itstiloop", httpClient =>
    {
        httpClient.BaseAddress = new Uri(loopBaseUriString);
        httpClient.DefaultRequestHeaders.Add(
            "ApiKey", apiKey);
        httpClient.DefaultRequestHeaders.Add(
            "ApiId", apiId);
    });
}
else
{
    //non patricipant fsp
    builder.Services.AddHttpClient();
}

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<CBDCTransferConfig>(
    builder.Configuration.GetSection("CBDCTransferConfig"));

builder.Services.AddTransient<CBDCTransferService>();
builder.Services.AddTransient<GlobalPartyLookupService>();


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
