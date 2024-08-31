using System.Security.Claims;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GarageDoorMonitor;
using GarageDoorServices;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keyVaultUrlStr = builder.Configuration["KeyVaultUrl"];
var keyVault = new SecretClient(new Uri(keyVaultUrlStr), new DefaultAzureCredential());

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasic(options =>
    {
        var basicAuthUserName = keyVault.GetSecret("BasicAuthUserName").Value.Value;
        var basicAuthPassword = keyVault.GetSecret("BasicAuthPassword").Value.Value;
        var authUtil = new AuthUtil(basicAuthUserName, basicAuthPassword);

        options.Realm = "GarageDoorMonitor";
        options.Events = new BasicAuthenticationEvents
        {
            OnValidateCredentials = authUtil.BasicAuthValidateCredentials
        };
    });
builder.Services.AddAuthorization();

var cosmosDbEndpointUri = keyVault.GetSecret("AzureCosmosDbEndpointUri").Value.Value;
builder.Services.AddSingleton<IGarageDoorService>(options =>
{
    var dbName = builder.Configuration["CosmosDb:DatabaseName"];
    var collectionName = builder.Configuration["CosmosDb:GarageDoorsName"];

    return new GarageDoorService(cosmosDbEndpointUri, dbName, collectionName);
});

builder.Services.AddSingleton<IConfigService>(options =>
{
    var dbName = builder.Configuration["CosmosDb:DatabaseName"];
    var collectionName = builder.Configuration["CosmosDb:ConfigsName"];

    return new ConfigService(cosmosDbEndpointUri, dbName, collectionName);
});

builder.Services.AddSingleton<IVoltageService>(options =>
{
    var dbName = builder.Configuration["CosmosDb:DatabaseName"];
    var collectionName = builder.Configuration["CosmosDb:VoltagesName"];

    return new VoltageService(cosmosDbEndpointUri, dbName, collectionName);
});
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
