using System.Collections.Generic;

namespace Vicold.Pindoudou.Entities
{
    /// <summary>
    /// 导航数据模型，用于页面间传递像素艺术相关参数
    /// </summary>
    public class NavigationData
    {
        /// <summary>
        /// 像素艺术宽度
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// 像素艺术高度
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// 调色板名称
        /// </summary>
        public string Palette { get; set; }
        
        /// <summary>
        /// 像素数据字符串，逗号分隔的颜色值
        /// </summary>
        public string PixelData { get; set; }
        
        /// <summary>
        /// 当前调色板颜色字典，键为颜色代码，值为颜色值
        /// </summary>
        public Dictionary<string, string> CurrentPalette { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public NavigationData()
        {
            CurrentPalette = new Dictionary<string, string>();
        }
    }
}