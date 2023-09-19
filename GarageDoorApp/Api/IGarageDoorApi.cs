using GarageDoorApp.Model;

namespace GarageDoorApp.Api
{
    public interface IGarageDoorApi
    {
        Task<IEnumerable<GarageDoorStatus>> GetAsync();
        Task<GarageDoorStatus> GetAsync(string id);
    }
}
