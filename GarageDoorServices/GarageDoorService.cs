using System.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace GarageDoorServices
{
    public class GarageDoorService : IGarageDoorService
    {
        private readonly Container _container;

        public GarageDoorService(string endpointUri, string dbName, string collectionName)
        {
            var cosmosClient = new CosmosClient(endpointUri);
            _container = cosmosClient.GetContainer(dbName, collectionName);
        }

        public async Task<IEnumerable<GarageDoorStatus>> GetAsync()
        {
            var query = new QueryDefinition($"select * from c");

            using var iterator = _container.GetItemQueryIterator<GarageDoorStatus>(query);

            var list = new List<GarageDoorStatus>();
            while (iterator.HasMoreResults)
            {
                var statuses = await iterator.ReadNextAsync();
                list.AddRange(statuses);
            }

            return list;
        }

        public async Task<GarageDoorStatus?> GetAsync(string id)
        {
            var query = new QueryDefinition($"select * from c where c.id = @id")
                .WithParameter("@id", id);

            using var iterator = _container.GetItemQueryIterator<GarageDoorStatus>(query);

            while (iterator.HasMoreResults)
            {
                var status = await iterator.ReadNextAsync();
                return status.FirstOrDefault();
            }

            return null;
        }

        public async Task SetAsync(string id, int isOpen)
        {
            await _container.ReplaceItemAsync(new GarageDoorStatus()
            {
                Id = id,
                IsOpen = isOpen
            }, id);
        }
    }
}
