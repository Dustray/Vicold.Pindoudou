using System.Threading.Tasks;

namespace Vicold.Pindoudou.Services
{
    public interface IImagePickerService
    {
        Task<byte[]> PickImageAsync();
    }
}