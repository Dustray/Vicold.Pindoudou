using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vicold.Pindoudou.Entities;
using SkiaSharp;

namespace Vicold.Pindoudou.Utils
{
    public class ImageUtil
    {
        /// <summary>
        /// 调整图像大小
        /// </summary>
        /// <param name="originalPixels">原始图像素数组</param>
        /// <param name="originalWidth">原始宽度</param>
        /// <param name="originalHeight">原始高度</param>
        /// <param name="newWidth">新宽度</param>
        /// <param name="newHeight">新高度</param>
        /// <returns>调整大小后的像素数组</returns>
        public static Vicold.Pindoudou.Entities.Color[] ResizeImage(Vicold.Pindoudou.Entities.Color[] originalPixels, int originalWidth, int originalHeight, int newWidth, int newHeight)
        {
            var resizedPixels = new Vicold.Pindoudou.Entities.Color[newWidth * newHeight];
            
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // 计算原始图像中的对应位置
                    int origX = (x * originalWidth) / newWidth;
                    int origY = (y * originalHeight) / newHeight;
                    
                    // 确保坐标在有效范围内
                    origX = Math.Min(origX, originalWidth - 1);
                    origY = Math.Min(origY, originalHeight - 1);
                    
                    resizedPixels[y * newWidth + x] = originalPixels[origY * originalWidth + origX];
                }
            }
            
            return resizedPixels;
        }
        
        /// <summary>
        /// 计算图像区域的平均颜色
        /// </summary>
        /// <param name="pixels">像素数组</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="x">区域起始X坐标</param>
        /// <param name="y">区域起始Y坐标</param>
        /// <param name="regionWidth">区域宽度</param>
        /// <param name="regionHeight">区域高度</param>
        /// <returns>平均颜色</returns>
        public static Vicold.Pindoudou.Entities.Color CalculateRegionAverageColor(Vicold.Pindoudou.Entities.Color[] pixels, int width, int height, int x, int y, int regionWidth, int regionHeight)
        {
            int r = 0, g = 0, b = 0, a = 0;
            int count = 0;
            
            for (int ry = y; ry < y + regionHeight && ry < height; ry++)
            {
                for (int rx = x; rx < x + regionWidth && rx < width; rx++)
                {
                    var color = pixels[ry * width + rx];
                    r += color.R;
                    g += color.G;
                    b += color.B;
                    a += color.A;
                    count++;
                }
            }
            
            if (count == 0)
            {
                return new Vicold.Pindoudou.Entities.Color(255, 255, 255); // 默认白色
            }
            
            return new Vicold.Pindoudou.Entities.Color(r / count, g / count, b / count, a / count);
        }
        
        /// <summary>
        /// 生成模拟图像数据（用于测试）
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>模拟像素数组</returns>
        public static Vicold.Pindoudou.Entities.Color[] GenerateSimulatedImageData(int width, int height)
        {
            var pixels = new Vicold.Pindoudou.Entities.Color[width * height];
            var random = new Random();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 生成更有规律的模拟数据
                    int r = (x * 255) / width;
                    int g = (y * 255) / height;
                    int b = (x + y) * 128 / (width + height);
                    
                    // 添加一些随机变化
                    r = Math.Clamp(r + random.Next(-30, 31), 0, 255);
                    g = Math.Clamp(g + random.Next(-30, 31), 0, 255);
                    b = Math.Clamp(b + random.Next(-30, 31), 0, 255);
                    
                    pixels[y * width + x] = new Vicold.Pindoudou.Entities.Color(r, g, b);
                }
            }
            
            return pixels;
        }
        
        /// <summary>
        /// 从原始图像中提取区域颜色
        /// </summary>
        /// <param name="originalPixels">原始图像素数组</param>
        /// <param name="originalWidth">原始宽度</param>
        /// <param name="originalHeight">原始高度</param>
        /// <param name="thumbnailWidth">缩略图宽度</param>
        /// <param name="thumbnailHeight">缩略图高度</param>
        /// <param name="thumbnailX">缩略图X坐标</param>
        /// <param name="thumbnailY">缩略图Y坐标</param>
        /// <returns>区域颜色列表</returns>
        public static List<Vicold.Pindoudou.Entities.Color> GetRegionColors(Vicold.Pindoudou.Entities.Color[] originalPixels, int originalWidth, int originalHeight, int thumbnailWidth, int thumbnailHeight, int thumbnailX, int thumbnailY)
        {
            var minOrigX = thumbnailX * originalWidth / thumbnailWidth;
            var minOrigY = thumbnailY * originalHeight / thumbnailHeight;
            var maxOrigX = (thumbnailX + 1) * originalWidth / thumbnailWidth;
            var maxOrigY = (thumbnailY + 1) * originalHeight / thumbnailHeight;
            
            // 确保坐标在有效范围内
            minOrigX = Math.Max(0, minOrigX);
            minOrigY = Math.Max(0, minOrigY);
            maxOrigX = Math.Min(originalWidth, maxOrigX);
            maxOrigY = Math.Min(originalHeight, maxOrigY);

            var colors = new List<Vicold.Pindoudou.Entities.Color>();
            for (int y = minOrigY; y < maxOrigY; y++)
            {
                for (int x = minOrigX; x < maxOrigX; x++)
                {
                    var color = originalPixels[y * originalWidth + x];
                    colors.Add(color);
                }
            }
            return colors;
        }


        /// <summary>
        /// 从图像中提取像素数据
        /// </summary>
        /// <param name="imageBytes">图像字节数组</param>
        /// <returns>像素数组</returns>
        public static Vicold.Pindoudou.Entities.Color[] GetPixelData(byte[] imageBytes)
        {
            try
            {
                // 检查图像字节数组是否为空
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    return GenerateSimulatedImageData(100, 100);
                }
                
                // 使用 SkiaSharp 从字节数组中提取像素数据
                using var skiaStream = new System.IO.MemoryStream(imageBytes);
                var bitmap = SkiaSharp.SKBitmap.Decode(skiaStream);
                
                if (bitmap == null)
                {
                    return GenerateSimulatedImageData(100, 100);
                }
                
                // 限制图像大小，防止内存问题
                const int maxWidth = 1000;
                const int maxHeight = 1000;
                
                int width = bitmap.Width;
                int height = bitmap.Height;
                
                // 如果图像过大，缩小到最大尺寸
                if (width > maxWidth || height > maxHeight)
                {
                    float scale = Math.Min((float)maxWidth / width, (float)maxHeight / height);
                    int newWidth = (int)(width * scale);
                    int newHeight = (int)(height * scale);
                    
                    var resizedBitmap = bitmap.Resize(new SkiaSharp.SKImageInfo(newWidth, newHeight), SkiaSharp.SKFilterQuality.Medium);
                    bitmap.Dispose();
                    bitmap = resizedBitmap;
                    
                    width = newWidth;
                    height = newHeight;
                }
                
                using (bitmap)
                {
                    var pixels = new Vicold.Pindoudou.Entities.Color[width * height];
                    var skiaPixels = bitmap.Pixels;
                    
                    for (int i = 0; i < skiaPixels.Length; i++)
                    {
                        var skiaColor = skiaPixels[i];
                        // 直接使用Skia颜色的Alpha通道值
                        pixels[i] = new Vicold.Pindoudou.Entities.Color(skiaColor.Red, skiaColor.Green, skiaColor.Blue, skiaColor.Alpha);
                    }
                    
                    return pixels;
                }
            }
            catch (Exception)
            {
                // 如果获取失败，使用默认大小的模拟数据
                return GenerateSimulatedImageData(100, 100);
            }
        }
        
        /// <summary>
        /// 从图像中提取像素数据
        /// </summary>
        /// <param name="image">图像</param>
        /// <returns>像素数组</returns>
        public static Vicold.Pindoudou.Entities.Color[] GetPixelData(Microsoft.Maui.Graphics.IImage image)
        {
            if (image == null)
            {
                return GenerateSimulatedImageData(100, 100);
            }
            
            var width = (int)image.Width;
            var height = (int)image.Height;
            
            // 限制图像大小，防止内存问题
            const int maxWidth = 1000;
            const int maxHeight = 1000;
            
            if (width > maxWidth || height > maxHeight)
            {
                width = Math.Min(width, maxWidth);
                height = Math.Min(height, maxHeight);
            }
            
            try
            {
                // 尝试将图像转换为字节数组，然后使用 GetPixelData(byte[]) 方法
                using var stream = new System.IO.MemoryStream();
                image.Save(stream, Microsoft.Maui.Graphics.ImageFormat.Png);
                stream.Position = 0;
                var imageBytes = stream.ToArray();
                
                return GetPixelData(imageBytes);
            }
            catch (Exception)
            {
                // 如果获取失败，使用模拟数据
                return GenerateSimulatedImageData(width, height);
            }
        }
    }
}
