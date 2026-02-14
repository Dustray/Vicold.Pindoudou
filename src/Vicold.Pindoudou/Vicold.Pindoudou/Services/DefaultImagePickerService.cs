using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

namespace Vicold.Pindoudou.Services
{
    public class DefaultImagePickerService : IImagePickerService
    {
        public async Task<byte[]> PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "选择图片"
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}