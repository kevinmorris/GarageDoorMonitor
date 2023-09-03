using System.Security.Claims;
using GarageDoorMonitor;
using idunno.Authentication.Basic;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasic(options =>
    {
        options.Realm = "GarageDoorMonitor";
        options.Events = new BasicAuthenticationEvents
        {
            OnValidateCredentials = context =>
            {
                if (context.Username == builder.Configuration["BasicAuth:Username"] &&
                    context.Password == builder.Configuration["BasicAuth:Password"])
                {
                    context.Principal = new ClaimsPrincipal(
                        new ClaimsIdentity(new[]
                        {
                            new Claim(
                                ClaimTypes.Name, 
                                context.Username,
                                context.Options.ClaimsIssuer)
                        }, context.Scheme.Name));
                    context.Success();
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IGarageDoorService>(options =>
{
    var cosmosClient = new CosmosClient(builder.Configuration["AzureCosmosDB:EndpointUri"]);
    var dbName = builder.Configuration["AzureCosmosDB:DatabaseName"];
    var collectionName = builder.Configuration["AzureCosmosDB:CollectionName"];

    return new GarageDoorService(
        cosmosClient.GetContainer(dbName, collectionName));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
