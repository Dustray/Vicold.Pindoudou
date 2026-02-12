using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Vicold.Pindoudou.Entities;

namespace Vicold.Pindoudou.Utils
{
    public class ResourceUtil
    {
        /// <summary>
        /// 保存资源文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="width">像素图宽度</param>
        /// <param name="height">像素图高度</param>
        /// <param name="palette">调色板</param>
        /// <param name="pixelData">像素数据</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveResource(string filePath, int width, int height, PaletteEntity palette, string[] pixelData)
        {
            try
            {
                // 创建资源数据对象
                var resourceData = new ResourceData
                {
                    Width = width,
                    Height = height,
                    Palette = new List<PaletteColor>(),
                    PixelData = pixelData
                };

                // 添加调色板颜色
                foreach (var (name, color) in palette.ColorsList)
                {
                    resourceData.Palette.Add(new PaletteColor
                    {
                        Name = name,
                        R = color.R,
                        G = color.G,
                        B = color.B,
                        A = color.A
                    });
                }

                // 序列化为JSON
                var json = JsonSerializer.Serialize(resourceData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // 写入文件
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存资源失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 打开资源文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="width">输出像素图宽度</param>
        /// <param name="height">输出像素图高度</param>
        /// <param name="palette">输出调色板</param>
        /// <param name="pixelData">输出像素数据</param>
        /// <returns>是否打开成功</returns>
        public static bool OpenResource(string filePath, out int width, out int height, out PaletteEntity palette, out string[] pixelData)
        {
            width = 0;
            height = 0;
            palette = new PaletteEntity();
            pixelData = Array.Empty<string>();

            try
            {
                // 读取文件内容
                var json = File.ReadAllText(filePath);

                // 反序列化为资源数据对象
                var resourceData = JsonSerializer.Deserialize<ResourceData>(json);

                if (resourceData == null)
                {
                    return false;
                }

                // 提取数据
                width = resourceData.Width;
                height = resourceData.Height;
                pixelData = resourceData.PixelData;

                // 重建调色板
                foreach (var paletteColor in resourceData.Palette)
                {
                    var color = new Vicold.Pindoudou.Entities.Color(paletteColor.R, paletteColor.G, paletteColor.B, paletteColor.A);
                    palette.AddColor(paletteColor.Name, color);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开资源失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 资源数据结构
        /// </summary>
        private class ResourceData
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public List<PaletteColor> Palette { get; set; } = new List<PaletteColor>();
            public string[] PixelData { get; set; } = Array.Empty<string>();
        }

        /// <summary>
        /// 调色板颜色结构
        /// </summary>
        private class PaletteColor
        {
            public string Name { get; set; } = string.Empty;
            public int R { get; set; }
            public int G { get; set; }
            public int B { get; set; }
            public int A { get; set; }
        }
    }
}