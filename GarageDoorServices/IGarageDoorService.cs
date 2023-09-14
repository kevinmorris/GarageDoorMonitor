using GarageDoorModels;

namespace GarageDoorServices
{
    public interface IGarageDoorService
    {
        Task<GarageDoorStatus?> GetAsync(string id);
        Task SetAsync(string id, int isOpen);
    }
}
