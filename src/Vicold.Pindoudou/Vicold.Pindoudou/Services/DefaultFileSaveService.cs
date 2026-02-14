using System;
using System.IO;
using System.Threading.Tasks;

namespace Vicold.Pindoudou.Services
{
    public class DefaultFileSaveService : IFileSaveService
    {
        public Task<bool> SaveImageAsync(byte[] imageData, string fileName)
        {
            // 默认实现，仅在支持的平台上使用
            return Task.FromResult(false);
        }

        public Task<bool> SaveResourceAsync(string jsonData, string fileName)
        {
            // 默认实现，仅在支持的平台上使用
            return Task.FromResult(false);
        }

        public bool IsPlatformSupported()
        {
            // 默认实现，返回false
            return false;
        }
    }
}
