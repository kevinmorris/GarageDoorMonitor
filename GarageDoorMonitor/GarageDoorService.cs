﻿using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace GarageDoorMonitor
{
    public class GarageDoorService : IGarageDoorService
    {
        private readonly Container _container;

        public GarageDoorService(Container container)
        {
            _container = container;
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
            var existingStatus = await GetAsync(id);
            if (existingStatus != null)
            {
                await _container.UpsertItemAsync(new GarageDoorStatus()
                {
                    Id = id,
                    IsOpen = isOpen
                });
            }
        }
    }
}
