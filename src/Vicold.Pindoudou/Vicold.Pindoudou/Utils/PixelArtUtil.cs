using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vicold.Pindoudou.Utils
{
    /// <summary>
    /// 像素艺术工具类，提供像素艺术相关的工具方法
    /// </summary>
    public class PixelArtUtil
    {
        /// <summary>
        /// 根据颜色值获取对应的代码
        /// </summary>
        /// <param name="color">颜色值</param>
        /// <param name="paletteColors">调色板颜色字典</param>
        /// <param name="hideTransparentCodes">是否隐藏透明色代码</param>
        /// <returns>颜色对应的代码</returns>
        public static string GetCodeByColor(string color, Dictionary<string, string> paletteColors, bool hideTransparentCodes)
        {
            // 检查是否需要隐藏透明色代码
            if (hideTransparentCodes)
            {
                // #RRGGBBAA 格式，最后两位为00
                if (color.Length == 9 && color.EndsWith("00", System.StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }
            }
            
            foreach (var kvp in paletteColors)
            {
                if (kvp.Value == color)
                {
                    return kvp.Key;
                }
            }
            return string.Empty;
        }
        
        /// <summary>
        /// 编辑像素
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="width">宽度</param>
        /// <param name="pixelData">像素数据数组</param>
        /// <param name="selectedColor">选中的颜色</param>
        /// <returns>修改后的像素数据数组</returns>
        public static string[] EditPixel(int x, int y, int width, string[] pixelData, string selectedColor)
        {
            if (pixelData == null)
            {
                return Array.Empty<string>();
            }
            
            // 创建一个新的数组副本，以确保Blazor能够检测到变化
            var newPixelData = (string[])pixelData.Clone();
            
            var index = y * width + x;
            if (index >= 0 && index < newPixelData.Length)
            {
                newPixelData[index] = selectedColor;
            }
            return newPixelData;
        }
        
        /// <summary>
        /// 生成调色板代码
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>调色板代码</returns>
        public static string GetPaletteCode(int index)
        {
            // 生成类似 A1、A2、B1、B2 的调色板代码
            char letter = (char)('A' + (index / 10));
            int number = (index % 10) + 1;
            return $"{letter}{number}";
        }
        
        /// <summary>
        /// 获取查询参数（整数类型）
        /// </summary>
        /// <param name="queryString">查询字符串</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>查询参数值</returns>
        public static int GetQueryParameter(string queryString, string parameterName, int defaultValue)
        {
            var parameter = GetQueryParameter(queryString, parameterName, string.Empty);
            if (int.TryParse(parameter, out var value))
            {
                return value;
            }
            return defaultValue;
        }
        
        /// <summary>
        /// 获取查询参数（字符串类型）
        /// </summary>
        /// <param name="queryString">查询字符串</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>查询参数值</returns>
        public static string GetQueryParameter(string queryString, string parameterName, string defaultValue)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return defaultValue;
            }
            
            // 移除开头的 ?
            if (queryString.StartsWith("?"))
            {
                queryString = queryString.Substring(1);
            }
            
            // 分割参数
            var parameters = queryString.Split('&');
            foreach (var param in parameters)
            {
                var parts = param.Split('=');
                if (parts.Length == 2 && parts[0] == parameterName)
                {
                    return System.Uri.UnescapeDataString(parts[1]);
                }
            }
            
            return defaultValue;
        }
    }
}