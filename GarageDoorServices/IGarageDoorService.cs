namespace GarageDoorServices
{
    public interface IGarageDoorService
    {
        Task<IEnumerable<GarageDoorStatus>> GetAsync();
        Task<GarageDoorStatus?> GetAsync(string id);
        Task SetAsync(string id, int isOpen);
    }
}
