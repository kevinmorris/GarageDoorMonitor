using GarageDoorApp.Model;

namespace GarageDoorApp.Api
{
    public interface IGarageDoorApi
    {
        Task<GarageDoorStatus> GetAsync(string id);
    }
}
