using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorServices
{
    public class VoltageService : IVoltageService
    {
        private readonly Container _container;

        public VoltageService(string endpointUri, string dbName, string collectionName)
        {
            var cosmosClient = new CosmosClient(endpointUri);
            _container = cosmosClient.GetContainer(dbName, collectionName);
        }

        public async Task SaveAsync(double voltage)
        {
            await _container.CreateItemAsync(new Voltage()
            {
                TimeStamp = DateTime.Now,
                Value = voltage
            });
        }
    }
}
