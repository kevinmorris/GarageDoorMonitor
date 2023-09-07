using System.Security.Claims;
using GarageDoorMonitor;
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
    var dbName = builder.Configuration["AzureCosmosDB:DatabaseName"];
    var collectionName = builder.Configuration["AzureCosmosDB:CollectionName"];

    return new GarageDoorService(endpointUri, dbName, collectionName);
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
