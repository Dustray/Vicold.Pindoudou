namespace Vicold.Pindoudou.Services
{
    public interface IFileSaveService
    {
        Task<bool> SaveImageAsync(byte[] imageData, string fileName);
        Task<bool> SaveResourceAsync(string jsonData, string fileName);
        bool IsPlatformSupported();
    }
}
