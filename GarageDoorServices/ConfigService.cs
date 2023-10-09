using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorServices
{
    public class ConfigService : IConfigService
    {
        private readonly Container _container;

        public ConfigService(string endpointUri, string dbName, string collectionName)
        {
            var cosmosClient = new CosmosClient(endpointUri);
            _container = cosmosClient.GetContainer(dbName, collectionName);
        }

        public async Task<Config?> GetAsync(string name)
        {
            var query = new QueryDefinition($"select * from c where c.name = @name")
                .WithParameter("@name", name);

            using var iterator = _container.GetItemQueryIterator<Config>(query);

            while (iterator.HasMoreResults)
            {
                var status = await iterator.ReadNextAsync();
                return status.FirstOrDefault();
            }

            return null;
        }
    }
}
