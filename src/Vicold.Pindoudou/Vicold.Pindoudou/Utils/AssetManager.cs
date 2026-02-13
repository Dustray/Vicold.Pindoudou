using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Storage;
#if ANDROID
using Android.App;
#endif

namespace Vicold.Pindoudou.Utils
{
    public static class AssetManager
    {
        public static void InitializeAssets()
        {
            try
            {
                // 确保应用数据目录中的Data/Palettes目录存在
                string appDataDir = FileSystem.AppDataDirectory;
                var paletteList = new[] { "Mard", "Mard53" };
                string palettesDir = Path.Combine(appDataDir, "Data", "Palettes");
                string userPalettesDir = Path.Combine(appDataDir, "Data", "UserPalettes");

                // 创建必要的目录
                Directory.CreateDirectory(palettesDir);
                Directory.CreateDirectory(userPalettesDir);

                // 加载并设置调色板
                LoadMauiAssets(paletteList).Wait();

            }
            catch (Exception ex)
            {
                // 记录错误但不中断应用启动
                Console.WriteLine($"初始化资产文件失败: {ex.Message}");
            }
        }
        private static async Task LoadMauiAssets(string[] paletteFiles)
        {
            string appDataDir = FileSystem.AppDataDirectory;
            string palettesDir = Path.Combine(appDataDir, "Data", "Palettes");

            foreach (var paletteName in paletteFiles)
            {
                var fileName = paletteName + ".txt";
                try
                {
                    string assetPath = Path.Combine("Palettes", fileName);
                    string destinationPath = Path.Combine(palettesDir, fileName);

                    // 检查目标文件是否已存在
                    if (!File.Exists(destinationPath))
                    {
                        // 尝试打开资产文件
                        using var stream = await FileSystem.OpenAppPackageFileAsync(assetPath);
                        if (stream != null)
                        {
                            // 读取文件内容
                            using var reader = new StreamReader(stream);
                            var contents = reader.ReadToEnd();
                            Console.WriteLine($"成功加载调色板文件: {fileName}, 内容长度: {contents.Length}");

                            // 确保目标目录存在
                            Directory.CreateDirectory(palettesDir);

                            // 将内容写入目标文件
                            File.WriteAllText(destinationPath, contents);
                            Console.WriteLine($"成功保存调色板文件: {destinationPath}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"调色板文件已存在，跳过写入: {destinationPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载调色板文件 {fileName} 失败: {ex.Message}");
                }
            }
        }
    }
}