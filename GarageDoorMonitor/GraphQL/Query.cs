using GarageDoorServices;

namespace GarageDoorMonitor.GraphQL
{
    public class Query
    {
        private readonly IGarageDoorService _service;
        public Query(IGarageDoorService service)
        {
            _service = service;
        }

        public async Task<GarageDoorStatus?> GetGarageDoorStatusAsync(string id)
        {
            var status = await _service.GetAsync(id);
            return status;
        }
    }
}
