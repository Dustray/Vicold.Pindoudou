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
        /// 透明度分量
        /// </summary>
        public int A { get; set; }
        
        /// <summary>
        /// 颜色代码
        /// </summary>
        public string Code { get; set; } = string.Empty;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="r">红色分量</param>
        /// <param name="g">绿色分量</param>
        /// <param name="b">蓝色分量</param>
        /// <param name="a">透明度分量</param>
        /// <param name="code">颜色代码</param>
        public Color(int r, int g, int b, int a = 255, string code = "")
        {
            R = Math.Clamp(r, 0, 255);
            G = Math.Clamp(g, 0, 255);
            B = Math.Clamp(b, 0, 255);
            A = Math.Clamp(a, 0, 255);
            Code = code;
        }
        
        /// <summary>
        /// 从十六进制字符串创建颜色
        /// </summary>
        /// <param name="hex">十六进制颜色字符串</param>
        /// <param name="code">颜色代码</param>
        /// <returns>颜色对象</returns>
        public static Color FromHex(string hex, string code = "")
        {
            // 移除 # 前缀
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }
            
            int r, g, b, a = 255;
            
            // 解析 RGB 值
            if (hex.Length == 6)
            {
                // 格式: RRGGBB
                r = Convert.ToInt32(hex.Substring(0, 2), 16);
                g = Convert.ToInt32(hex.Substring(2, 2), 16);
                b = Convert.ToInt32(hex.Substring(4, 2), 16);
            }
            else if (hex.Length == 8)
            {
                // 假设是 #RRGGBBAA 格式
                r = Convert.ToInt32(hex.Substring(0, 2), 16);
                g = Convert.ToInt32(hex.Substring(2, 2), 16);
                b = Convert.ToInt32(hex.Substring(4, 2), 16);
                a = Convert.ToInt32(hex.Substring(6, 2), 16);
            }
            else
            {
                throw new ArgumentException("Invalid hex color format. Expected 6 or 8 characters.");
            }
            
            return new Color(r, g, b, a, code);
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
                Math.Pow(B - other.B, 2) +
                Math.Pow(A - other.A, 2)
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
            return new Color(R, G, B, A, Code);
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
                return R == other.R && G == other.G && B == other.B && A == other.A;
            }
            return false;
        }
        
        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B, A);
        }
        
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <returns>十六进制颜色字符串</returns>
        public override string ToString()
        {
            if (A == 255)
            {
                return $"#{R:X2}{G:X2}{B:X2}";
            }
            else
            {
                return $"#{R:X2}{G:X2}{B:X2}{A:X2}";
            }
        }
    }
}