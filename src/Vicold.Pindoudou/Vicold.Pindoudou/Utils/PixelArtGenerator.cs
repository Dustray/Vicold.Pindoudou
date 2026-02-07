using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vicold.Pindoudou.Entities;

namespace Vicold.Pindoudou.Utils
{
    public class PixelArtGenerator
    {
        /// <summary>
        /// 生成像素图
        /// </summary>
        /// <param name="originalWidth">原始图像宽度</param>
        /// <param name="originalHeight">原始图像高度</param>
        /// <param name="thumbnailWidth">像素图宽度</param>
        /// <param name="thumbnailHeight">像素图高度</param>
        /// <param name="originalPixels">原始图像素数组</param>
        /// <param name="palette">调色板</param>
        /// <returns>生成的像素图数据</returns>
        public static string[] GeneratePixelArt(int originalWidth, int originalHeight, int thumbnailWidth, int thumbnailHeight, Vicold.Pindoudou.Entities.Color[] originalPixels, PaletteEntity palette)
        {
            var pixelData = new string[thumbnailWidth * thumbnailHeight];
            
            // 如果原始像素数据为空，返回默认白色像素图
            if (originalPixels == null || originalPixels.Length == 0)
            {
                for (int i = 0; i < pixelData.Length; i++)
                {
                    pixelData[i] = "#FFFFFF";
                }
                return pixelData;
            }
            
            for (int y = 0; y < thumbnailHeight; y++)
            {
                for (int x = 0; x < thumbnailWidth; x++)
                {
                    // 获取原始图像中对应区域的所有颜色
                    var colors = ColorUtil.GetColorsFromThumbnail(x, y, thumbnailWidth, thumbnailHeight, originalWidth, originalHeight, originalPixels);
                    
                    // 计算平均颜色
                    var averageColor = ColorUtil.CalculateAverageColor(colors);
                    
                    // 在调色板中找到最接近的颜色
                    var closestColor = palette.FindClosestColor(averageColor);
                    
                    // 转换为十六进制颜色字符串
                    var hexColor = ColorUtil.ColorToHex(closestColor);
                    
                    pixelData[y * thumbnailWidth + x] = hexColor;
                }
            }
            
            return pixelData;
        }
        
        /// <summary>
        /// 生成模拟的像素图（用于测试，当没有原始图像数据时）
        /// </summary>
        /// <param name="width">像素图宽度</param>
        /// <param name="height">像素图高度</param>
        /// <param name="palette">调色板</param>
        /// <returns>生成的像素图数据</returns>
        public static string[] GenerateSimulatedPixelArt(int width, int height, PaletteEntity palette)
        {
            var pixelData = new string[width * height];
            var random = new Random();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 生成模拟的颜色
                    var simulatedColor = ColorUtil.GenerateRandomColorWithStep(random, 50);
                    
                    // 在调色板中找到最接近的颜色
                    var closestColor = palette.FindClosestColor(simulatedColor);
                    
                    // 转换为十六进制颜色字符串
                    var hexColor = ColorUtil.ColorToHex(closestColor);
                    
                    pixelData[y * width + x] = hexColor;
                }
            }
            
            return pixelData;
        }
        
        /// <summary>
        /// 生成像素图实体
        /// </summary>
        /// <param name="originalWidth">原始图像宽度</param>
        /// <param name="originalHeight">原始图像高度</param>
        /// <param name="thumbnailWidth">像素图宽度</param>
        /// <param name="thumbnailHeight">像素图高度</param>
        /// <param name="originalPixels">原始图像素数组</param>
        /// <param name="palette">调色板</param>
        /// <returns>像素图数据和颜色映射</returns>
        public static (string[], Dictionary<string, int>) GeneratePixelArtWithStats(int originalWidth, int originalHeight, int thumbnailWidth, int thumbnailHeight, Vicold.Pindoudou.Entities.Color[] originalPixels, PaletteEntity palette)
        {
            var pixelData = new string[thumbnailWidth * thumbnailHeight];
            var colorStats = new Dictionary<string, int>();
            
            for (int y = 0; y < thumbnailHeight; y++)
            {
                for (int x = 0; x < thumbnailWidth; x++)
                {
                    // 获取原始图像中对应区域的所有颜色
                    var colors = ColorUtil.GetColorsFromThumbnail(x, y, thumbnailWidth, thumbnailHeight, originalWidth, originalHeight, originalPixels);
                    
                    // 计算平均颜色
                    var averageColor = ColorUtil.CalculateAverageColor(colors);
                    
                    // 在调色板中找到最接近的颜色
                    var closestColor = palette.FindClosestColor(averageColor);
                    
                    // 转换为十六进制颜色字符串
                    var hexColor = ColorUtil.ColorToHex(closestColor);
                    
                    pixelData[y * thumbnailWidth + x] = hexColor;
                    
                    // 统计颜色使用情况
                    if (colorStats.ContainsKey(hexColor))
                    {
                        colorStats[hexColor]++;
                    }
                    else
                    {
                        colorStats[hexColor] = 1;
                    }
                }
            }
            
            return (pixelData, colorStats);
        }
    }
}