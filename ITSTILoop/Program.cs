using ITSTILoop.Context;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Helpers;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services;
using ITSTILoop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using ITSTILoopDTOLibrary;
using CBDCHubContract.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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


var connectionStringName = EnvVars.GetEnvironmentVariable(EnvVarNames.DB_CONNECTION, EnvVarDefaultValues.DB_CONNECTION);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString(connectionStringName)));

builder.Services.Configure<EthereumConfig>(
    builder.Configuration.GetSection(EthereumConfig.Ethereum));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IParticipantRepository, ParticipantRepository>();
builder.Services.AddTransient<IPartyRepository, PartyRepository>();
builder.Services.AddTransient<ISettlementWindowRepository, SettlementWindowRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<ITransferRequestRepository, TransferRequestRepository>();
builder.Services.AddTransient<IPartyLookupService, PartyLookupService>();
builder.Services.AddTransient<IHttpPostClient, HttpPostClient>();
builder.Services.AddTransient<IConfirmTransferService, ConfirmTransferService>();
builder.Services.AddTransient<ISampleFspSeedingService, SampleFspSeedingService>();
builder.Services.AddTransient<EthereumEventRetriever>();
builder.Services.AddTransient<CBDCBridgeService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<TimedSettlementWindowService>();
builder.Services.AddHostedService<CBDCBridgeEventWatcherService>();


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
Thread.Sleep(4000);
//let's recreate if it doesn't exist
try
{
    using (var scope = app.Services.CreateScope())
    using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        context?.Database.EnsureDeleted();
        context?.Database.EnsureCreated();
        SampleFspSeedingService? seeding = (SampleFspSeedingService?)scope.ServiceProvider.GetRequiredService<ISampleFspSeedingService>();
        seeding.SeedFsp(EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_1, EnvVarDefaultValues.SAMPLE_FSP_1),
            EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_1_PARTIES, EnvVarDefaultValues.SAMPLE_FSP_1_PARTIES));
        seeding.SeedFsp(EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_2, EnvVarDefaultValues.SAMPLE_FSP_2),
                    EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_2_PARTIES, EnvVarDefaultValues.SAMPLE_FSP_2_PARTIES));
        seeding.SeedFsp(EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_3, EnvVarDefaultValues.SAMPLE_FSP_3),
                    EnvVars.GetEnvironmentVariable(EnvVarNames.SAMPLE_FSP_3_PARTIES, EnvVarDefaultValues.SAMPLE_FSP_3_PARTIES));
    }
}
catch (Exception ex)
{

    logger.LogError(ex, "An error occurred creating the DB.");
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
