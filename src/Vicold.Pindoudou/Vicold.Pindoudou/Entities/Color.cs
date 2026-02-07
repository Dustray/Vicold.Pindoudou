using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vicold.Pindoudou.Entities
{
    public class Color
    {
        /// <summary>
        /// 红色分量
        /// </summary>
        public int R { get; set; }
        
        /// <summary>
        /// 绿色分量
        /// </summary>
        public int G { get; set; }
        
        /// <summary>
        /// 蓝色分量
        /// </summary>
        public int B { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="r">红色分量</param>
        /// <param name="g">绿色分量</param>
        /// <param name="b">蓝色分量</param>
        public Color(int r, int g, int b)
        {
            R = Math.Clamp(r, 0, 255);
            G = Math.Clamp(g, 0, 255);
            B = Math.Clamp(b, 0, 255);
        }
        
        /// <summary>
        /// 从十六进制字符串创建颜色
        /// </summary>
        /// <param name="hex">十六进制颜色字符串</param>
        /// <returns>颜色对象</returns>
        public static Color FromHex(string hex)
        {
            // 移除 # 前缀
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }
            
            // 解析 RGB 值
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);
            
            return new Color(r, g, b);
        }
        
        /// <summary>
        /// 计算与另一个颜色的距离
        /// </summary>
        /// <param name="other">另一个颜色</param>
        /// <returns>颜色距离</returns>
        public double DistanceTo(Color other)
        {
            return Math.Sqrt(
                Math.Pow(R - other.R, 2) +
                Math.Pow(G - other.G, 2) +
                Math.Pow(B - other.B, 2)
            );
        }
        
        /// <summary>
        /// 获取亮度值
        /// </summary>
        /// <returns>亮度值 (0-255)</returns>
        public int GetBrightness()
        {
            return (int)(0.299 * R + 0.587 * G + 0.114 * B);
        }
        
        /// <summary>
        /// 克隆颜色
        /// </summary>
        /// <returns>颜色副本</returns>
        public Color Clone()
        {
            return new Color(R, G, B);
        }
        
        /// <summary>
        /// 比较两个颜色是否相等
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color other)
            {
                return R == other.R && G == other.G && B == other.B;
            }
            return false;
        }
        
        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }
        
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <returns>十六进制颜色字符串</returns>
        public override string ToString()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
    }
}