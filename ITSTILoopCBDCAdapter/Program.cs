var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
