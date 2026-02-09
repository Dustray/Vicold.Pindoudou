using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vicold.Pindoudou.Entities;

namespace Vicold.Pindoudou.Utils
{
    public class ColorUtil
    {
        /// <summary>
        /// 根据原始图像大小和像素图（缩略图），获取像素图的单个像素对应原始图像的所有颜色
        /// </summary>
        /// <param name="thumbnailX">像素图X</param>
        /// <param name="thumbnailY">像素图Y</param>
        /// <param name="thumbnailWidth">像素图X</param>
        /// <param name="thumbnailHeight">像素图Y</param>
        /// <param name="originalWidth">原始图像宽度</param>
        /// <param name="originalHeight">原始图像高度</param>
        /// <param name="originalPixels">原始图像素数组</param>
        /// <returns>每个像素对应原始图像的所有颜色</returns>
        public static List<Vicold.Pindoudou.Entities.Color> GetColorsFromThumbnail(int thumbnailX, int thumbnailY, int thumbnailWidth, int thumbnailHeight, int originalWidth, int originalHeight, Vicold.Pindoudou.Entities.Color[] originalPixels)
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
                    if(color.A > 0){
                        colors.Add(color);
                    }
                }
            }
            return colors;
        }
        
        /// <summary>
        /// 计算颜色列表的平均颜色
        /// </summary>
        /// <param name="colors">颜色列表</param>
        /// <returns>平均颜色</returns>
        public static Vicold.Pindoudou.Entities.Color CalculateAverageColor(List<Vicold.Pindoudou.Entities.Color> colors)
        {
            if (colors == null || colors.Count == 0)
            {
                return new Vicold.Pindoudou.Entities.Color(255, 255, 255, 0); // 默认透明
            }
            
            int r = 0, g = 0, b = 0, a = 0;
            
            foreach (var color in colors)
            {
                r += color.R;
                g += color.G;
                b += color.B;
                a += color.A;
            }
            
            r /= colors.Count;
            g /= colors.Count;
            b /= colors.Count;
            a /= colors.Count;
            
            return new Vicold.Pindoudou.Entities.Color(r, g, b, a);
        }
        
        /// <summary>
        /// 计算两个颜色之间的距离
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <returns>颜色距离</returns>
        public static double CalculateColorDistance(Vicold.Pindoudou.Entities.Color color1, Vicold.Pindoudou.Entities.Color color2)
        {
            return Math.Sqrt(
                Math.Pow(color1.R - color2.R, 2) +
                Math.Pow(color1.G - color2.G, 2) +
                Math.Pow(color1.B - color2.B, 2) +
                Math.Pow(color1.A - color2.A, 2)
            );
        }
        
        /// <summary>
        /// 将颜色转换为十六进制字符串
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>十六进制颜色字符串</returns>
        public static string ColorToHex(Vicold.Pindoudou.Entities.Color color)
        {
            return color.ToString();
        }
        
        /// <summary>
        /// 从十六进制字符串解析颜色
        /// </summary>
        /// <param name="hex">十六进制颜色字符串</param>
        /// <returns>颜色对象</returns>
        public static Vicold.Pindoudou.Entities.Color FromHex(string hex)
        {
            return Vicold.Pindoudou.Entities.Color.FromHex(hex);
        }
        
        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <param name="random">随机数生成器</param>
        /// <returns>随机颜色</returns>
        public static Vicold.Pindoudou.Entities.Color GenerateRandomColor(Random random)
        {
            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);
            int a = 255; // 默认完全不透明
            return new Vicold.Pindoudou.Entities.Color(r, g, b, a);
        }
        
        /// <summary>
        /// 生成带步长的随机颜色
        /// </summary>
        /// <param name="random">随机数生成器</param>
        /// <param name="step">步长</param>
        /// <returns>带步长的随机颜色</returns>
        public static Vicold.Pindoudou.Entities.Color GenerateRandomColorWithStep(Random random, int step)
        {
            int r = GetRandomWithStep(random, 0, 256, step);
            int g = GetRandomWithStep(random, 0, 256, step);
            int b = GetRandomWithStep(random, 0, 256, step);
            int a = 255; // 默认完全不透明
            return new Vicold.Pindoudou.Entities.Color(r, g, b, a);
        }
        
        /// <summary>
        /// 带步长的随机数生成
        /// </summary>
        /// <param name="random">随机数生成器</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="step">步长</param>
        /// <returns>随机数</returns>
        private static int GetRandomWithStep(Random random, int minValue, int maxValue, int step)
        {
            int range = (maxValue - minValue + step - 1) / step;
            int randomIndex = random.Next(range);
            return minValue + randomIndex * step;
        }
        
        /// <summary>
        /// 计算十六进制颜色的亮度
        /// </summary>
        /// <param name="color">十六进制颜色字符串</param>
        /// <returns>亮度值 (0-1)</returns>
        public static double CalculateColorLuminance(string color)
        {
            // 解析颜色值
            int r, g, b;
            
            if (color.StartsWith("#"))
            {
                color = color.Substring(1);
            }
            
            if (color.Length == 6)
            {
                // #RRGGBB 格式
                r = Convert.ToInt32(color.Substring(0, 2), 16);
                g = Convert.ToInt32(color.Substring(2, 2), 16);
                b = Convert.ToInt32(color.Substring(4, 2), 16);
            }
            else if (color.Length == 8)
            {
                r = Convert.ToInt32(color.Substring(0, 2), 16);
                g = Convert.ToInt32(color.Substring(2, 2), 16);
                b = Convert.ToInt32(color.Substring(4, 2), 16);
            }
            else if (color.Length == 3)
            {
                // #RGB 格式
                r = Convert.ToInt32(color.Substring(0, 1) + color.Substring(0, 1), 16);
                g = Convert.ToInt32(color.Substring(1, 1) + color.Substring(1, 1), 16);
                b = Convert.ToInt32(color.Substring(2, 1) + color.Substring(2, 1), 16);
            }
            else
            {
                // 默认黑色
                return 0;
            }
            
            // 计算亮度（使用相对亮度公式）
            return (0.299 * r + 0.587 * g + 0.114 * b) / 255;
        }
        
        /// <summary>
        /// 根据背景颜色获取对比度高的文本颜色（黑色或白色）
        /// </summary>
        /// <param name="color">十六进制颜色字符串</param>
        /// <returns>对比度高的文本颜色</returns>
        public static string GetContrastTextColor(string color)
        {
            // 计算亮度
            double luminance = CalculateColorLuminance(color);
            
            // 如果亮度大于0.5，使用黑色文本；否则使用白色文本
            return luminance > 0.5 ? "#000000" : "#FFFFFF";
        }
        
        /// <summary>
        /// 判断颜色是否为浅色
        /// </summary>
        /// <param name="color">十六进制颜色字符串</param>
        /// <returns>是否为浅色</returns>
        public static bool IsLightColor(string color)
        {
            // 计算亮度
            double luminance = CalculateColorLuminance(color);
            
            // 亮度大于0.5视为浅色
            return luminance > 0.5;
        }
    }
}