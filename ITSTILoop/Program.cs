using ITSTILoop.Context;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Authorization by ApiKey inside request's header",
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityDefinition("ApiId", new OpenApiSecurityScheme()
    {
        Name = "ApiId",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Authorization by ApiKey inside request's header",
        Scheme = "ApiIdScheme"
    });

    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var key2 = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiId"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement {
        { key, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);

    var requirement2 = new OpenApiSecurityRequirement {
        { key2, new List<string>() }
    };
    c.AddSecurityRequirement(requirement2);
});

var connectionStringName = Environment.GetEnvironmentVariable("ConnectionStringName");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString(connectionStringName)));


builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IParticipantRepository, ParticipantRepository>();
builder.Services.AddTransient<IPartyRepository, PartyRepository>();
builder.Services.AddTransient<IPartyLookupService, PartyLookupService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<TimedSettlementWindowService>();


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
//let's recreate if it doesn't exist
try
{
    using (var scope = app.Services.CreateScope())
    using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        context?.Database.EnsureDeleted();
        context?.Database.EnsureCreated();
    }
}
catch (Exception ex)
{

    logger.LogError(ex, "An error occurred creating the DB.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
