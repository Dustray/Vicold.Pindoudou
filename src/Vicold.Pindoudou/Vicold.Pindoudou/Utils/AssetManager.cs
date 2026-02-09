using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Storage;

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
                string palettesDir = Path.Combine(appDataDir, "Data", "Palettes");
                string userPalettesDir = Path.Combine(appDataDir, "Data", "UserPalettes");
                
                Directory.CreateDirectory(palettesDir);
                Directory.CreateDirectory(userPalettesDir);
                
                // 复制Data/Palettes目录下的所有文件
                CopyAllPaletteFiles();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断应用启动
                Console.WriteLine($"初始化资产文件失败: {ex.Message}");
            }
        }
        
        private static void CopyAllPaletteFiles()
        {
            try
            {
                // 尝试从应用程序基础目录复制（适用于Windows调试）
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string sourcePalettesDir = Path.Combine(baseDir, "Data", "Palettes");
                
                if (Directory.Exists(sourcePalettesDir))
                {
                    // 获取所有调色板文件
                    var paletteFiles = Directory.GetFiles(sourcePalettesDir, "*.txt");
                    
                    foreach (var file in paletteFiles)
                    {
                        // 获取相对路径
                        string relativePath = Path.GetRelativePath(baseDir, file);
                        // 复制文件
                        CopyAssetToAppData(relativePath);
                    }
                }
                else
                {
                    // 尝试从当前工作目录复制
                    string currentDir = Directory.GetCurrentDirectory();
                    sourcePalettesDir = Path.Combine(currentDir, "Data", "Palettes");
                    
                    if (Directory.Exists(sourcePalettesDir))
                    {
                        // 获取所有调色板文件
                        var paletteFiles = Directory.GetFiles(sourcePalettesDir, "*.txt");
                        
                        foreach (var file in paletteFiles)
                        {
                            // 获取相对路径
                            string relativePath = Path.GetRelativePath(currentDir, file);
                            // 复制文件
                            CopyAssetToAppData(relativePath);
                        }
                    }
                    else
                    {
                        Console.WriteLine("找不到Data/Palettes目录");
                        
                        // 如果都找不到，创建默认的Mard.txt文件
                        string appDataDir = FileSystem.AppDataDirectory;
                        string destinationPath = Path.Combine(appDataDir, "Data", "Palettes", "Mard.txt");
                        CreateDefaultMardPalette(destinationPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"复制所有调色板文件失败: {ex.Message}");
            }
        }
        
        private static void CopyAssetToAppData(string assetPath)
        {
            try
            {
                string appDataDir = FileSystem.AppDataDirectory;
                string destinationPath = Path.Combine(appDataDir, assetPath);
                
                // 只在目标文件不存在时复制
                if (!File.Exists(destinationPath))
                {
                    // 尝试从应用程序基础目录复制（适用于Windows调试）
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string sourcePath = Path.Combine(baseDir, assetPath);
                    
                    if (File.Exists(sourcePath))
                    {
                        // 确保目标目录存在
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        
                        // 复制文件
                        File.Copy(sourcePath, destinationPath, true);
                        Console.WriteLine($"从应用程序基础目录复制资产文件: {assetPath}");
                    }
                    else
                    {
                        // 尝试从当前工作目录复制
                        string currentDir = Directory.GetCurrentDirectory();
                        sourcePath = Path.Combine(currentDir, assetPath);
                        
                        if (File.Exists(sourcePath))
                        {
                            // 确保目标目录存在
                            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                            
                            // 复制文件
                            File.Copy(sourcePath, destinationPath, true);
                            Console.WriteLine($"从当前工作目录复制资产文件: {assetPath}");
                        }
                        else
                        {
                            Console.WriteLine($"找不到资产文件: {assetPath}");
                            
                            // 如果都找不到，创建默认的Mard.txt文件
                            if (assetPath.Equals("Data/Palettes/Mard.txt", StringComparison.OrdinalIgnoreCase))
                            {
                                CreateDefaultMardPalette(destinationPath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"复制资产文件失败 {assetPath}: {ex.Message}");
            }
        }
        
        private static void CreateDefaultMardPalette(string destinationPath)
        {
            try
            {
                // 确保目标目录存在
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                
                // 创建默认的Mard.txt文件内容
                string[] defaultPalette = new string[]
                {
                    "Color0\t#FFFFFF", // 白色
                    "Color1\t#000000", // 黑色
                    "Color2\t#FF0000", // 红色
                    "Color3\t#00FF00", // 绿色
                    "Color4\t#0000FF", // 蓝色
                    "Color5\t#FFFF00", // 黄色
                    "Color6\t#FF00FF", // 品红色
                    "Color7\t#00FFFF", // 青色
                    "Color8\t#FFA500", // 橙色
                    "Color9\t#800080", // 紫色
                    "Color10\t#808080", // 灰色
                    "Color11\t#A52A2A", // 棕色
                    "Color12\t#FFC0CB", // 粉色
                    "Color13\t#87CEEB", // 天蓝色
                    "Color14\t#98FB98", // 淡绿色
                    "Color15\t#FFD700"  // 金色
                };
                
                // 写入文件
                File.WriteAllLines(destinationPath, defaultPalette);
                Console.WriteLine($"创建默认的Mard.txt文件: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建默认Mard.txt文件失败: {ex.Message}");
            }
        }
    }
}