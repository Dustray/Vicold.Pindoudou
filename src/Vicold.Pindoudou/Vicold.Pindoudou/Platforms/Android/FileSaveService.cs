using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Provider;
using Vicold.Pindoudou.Services;
using Android.App;
using Android.Net;
using Android.Widget;

namespace Vicold.Pindoudou.Platforms.Android
{
    public class FileSaveService : IFileSaveService
    {
        private readonly Context _context;

        public FileSaveService()
        {
            _context = global::Android.App.Application.Context;
        }

        public async Task<bool> SaveImageAsync(byte[] imageData, string fileName)
        {
            try
            {
                bool success;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    // Android 10+ 使用 MediaStore API
                    success = await SaveImageWithMediaStoreAsync(imageData, fileName);
                }
                else
                {
                    // Android 9及以下使用传统方法
                    success = await SaveImageWithTraditionalAsync(imageData, fileName);
                }
                
                // 显示Toast提示
                if (success)
                {
                    ShowToast($"图片保存成功: {fileName}");
                }
                else
                {
                    ShowToast("图片保存失败");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存图片失败: {ex.Message}");
                ShowToast($"保存图片时出错: {ex.Message}");
                return false;
            }
        }
        
        private void ShowToast(string message)
        {
            try
            {
                // 在主线程上显示Toast
                global::Android.App.Application.SynchronizationContext.Post(_ => {
                    Toast.MakeText(_context, message, ToastLength.Short).Show();
                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"显示Toast失败: {ex.Message}");
            }
        }

        public async Task<bool> SaveResourceAsync(string jsonData, string fileName)
        {
            try
            {
                // 确保目录存在
                var directory = await EnsureDirectoryExistsAsync();
                if (string.IsNullOrEmpty(directory))
                {
                    ShowToast("创建目录失败");
                    return false;
                }

                // 保存文件
                var filePath = Path.Combine(directory, fileName);
                await File.WriteAllTextAsync(filePath, jsonData);

                // 显示Toast提示
                ShowToast($"资源保存成功: {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存资源失败: {ex.Message}");
                ShowToast($"保存资源时出错: {ex.Message}");
                return false;
            }
        }

        public bool IsPlatformSupported()
        {
            return true; // Android平台支持
        }

        private async Task<bool> SaveImageWithMediaStoreAsync(byte[] imageData, string fileName)
        {
            try
            {
                var contentValues = new ContentValues();
                contentValues.Put(MediaStore.Images.Media.InterfaceConsts.DisplayName, fileName);
                contentValues.Put(MediaStore.Images.Media.InterfaceConsts.MimeType, "image/png");
                contentValues.Put(MediaStore.Images.Media.InterfaceConsts.RelativePath, "Pictures/pindoudou");

                var resolver = _context.ContentResolver;
                var uri = resolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
                if (uri == null)
                {
                    Console.WriteLine("插入MediaStore失败");
                    return false;
                }

                using (var outputStream = resolver.OpenOutputStream(uri))
                {
                    if (outputStream == null)
                    {
                        Console.WriteLine("打开输出流失败");
                        resolver.Delete(uri, null, null);
                        return false;
                    }
                    await outputStream.WriteAsync(imageData, 0, imageData.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"使用MediaStore保存图片失败: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SaveImageWithTraditionalAsync(byte[] imageData, string fileName)
        {
            try
            {
                // 确保目录存在
                var directory = await EnsureDirectoryExistsAsync();
                if (string.IsNullOrEmpty(directory))
                {
                    return false;
                }

                // 保存文件
                var filePath = Path.Combine(directory, fileName);
                await File.WriteAllBytesAsync(filePath, imageData);

                // 通知媒体扫描器
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                var fileUri = global::Android.Net.Uri.FromFile(new global::Java.IO.File(filePath));
                mediaScanIntent.SetData(fileUri);
                _context.SendBroadcast(mediaScanIntent);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"使用传统方法保存图片失败: {ex.Message}");
                return false;
            }
        }

        private async Task<string> EnsureDirectoryExistsAsync()
        {
            try
            {
                string picturesDirectory;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    // Android 10+ 使用Context API
                    picturesDirectory = Path.Combine(_context.GetExternalFilesDir(null)?.AbsolutePath, "Pictures");
                }
                else
                {
                    // Android 9及以下使用传统API
                    picturesDirectory = global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures).AbsolutePath;
                }
                
                var appDirectory = Path.Combine(picturesDirectory, "pindoudou");

                // 创建目录（如果不存在）
                if (!Directory.Exists(appDirectory))
                {
                    Directory.CreateDirectory(appDirectory);
                }

                return appDirectory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建目录失败: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
