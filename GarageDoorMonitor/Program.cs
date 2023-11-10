using System.Security.Claims;
using GarageDoorMonitor;
using GarageDoorMonitor.GraphQL;
using GarageDoorServices;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasic(options =>
    {
        var authUtil = new AuthUtil(
            builder.Configuration["BasicAuth:UserName"],
            builder.Configuration["BasicAuth:Password"]);

        options.Realm = "GarageDoorMonitor";
        options.Events = new BasicAuthenticationEvents
        {
            OnValidateCredentials = authUtil.BasicAuthValidateCredentials
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IGarageDoorService>(options =>
{
    var endpointUri = builder.Configuration["AzureCosmosDB:EndpointUri"];
    var dbName = builder.Configuration["CosmosDBName"];
    var collectionName = builder.Configuration["AzureCosmosDB:GarageDoorsName"];

    return new GarageDoorService(endpointUri, dbName, collectionName);
});

builder.Services.AddSingleton<IConfigService>(options =>
{
    var endpointUri = builder.Configuration["AzureCosmosDB:EndpointUri"];
    var dbName = builder.Configuration["AzureCosmosDB:DatabaseName"];
    var collectionName = builder.Configuration["AzureCosmosDB:ConfigsName"];

    return new ConfigService(endpointUri, dbName, collectionName);
});

builder.Services.AddSingleton<IVoltageService>(options =>
{
    var endpointUri = builder.Configuration["AzureCosmosDB:EndpointUri"];
    var dbName = builder.Configuration["AzureCosmosDB:DatabaseName"];
    var collectionName = builder.Configuration["AzureCosmosDB:VoltagesName"];

    return new VoltageService(endpointUri, dbName, collectionName);
});

builder.Services.AddGraphQLServer().AddQueryType<Query>();

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
app.MapGraphQL();

app.Run();
