using System;
using System.Text.Json;
using GarageDoorServices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerInfo = Microsoft.Azure.Functions.Worker.TimerInfo;

namespace GarageDoorNotifier
{
    /// <summary>
    /// Azure function to run each evening and report if the garage door is still open.
    /// <remarks>Apparently NotificationHub attribute binding is not supported for isolated worker functions</remarks>
    /// </summary>
    public class NotificationHubNotifier
    {
        private readonly ILogger _logger;

        public NotificationHubNotifier(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NotificationHubNotifier>();
        }

        [Function("NotificationHubNotifier")]
        public void Run(
            [Microsoft.Azure.Functions.Worker.TimerTrigger("*/10 * * * * *")] TimerInfo timerInfo,
            [CosmosDBInput(
                databaseName: "GarageDoors",
                containerName: "Items",
                Connection = "CosmosDBConnection",
                Id = "%ItemId%",
                PartitionKey = "%ItemPartitionKey%")] GarageDoorStatus status)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Id: {status.Id}, IsOpen: {status.IsOpen}, Timestamp: {status.Timestamp}, TimestampSeconds: {status.TimestampSeconds}");
        }
    }
}
