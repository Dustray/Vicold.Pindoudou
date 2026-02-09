using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vicold.Pindoudou.Entities;

namespace Vicold.Pindoudou.Utils
{
    public class PaletteUtil
    {
        /// <summary>
        /// 获取颜色列表中与调色板中颜色最相似的颜色
        /// </summary>
        /// <param name="from">颜色列表</param>
        /// <param name="palette">调色板</param>
        /// <returns>最相似的颜色</returns>
        public static Vicold.Pindoudou.Entities.Color GetSimilarColorFromPalette(List<Vicold.Pindoudou.Entities.Color> from, PaletteEntity palette)
        {
            // 根据传入的若干color，获取它们RGB和Alpha的平均值
            var avgR = from.Average(c => c.R);
            var avgG = from.Average(c => c.G);
            var avgB = from.Average(c => c.B);
            var avgA = from.Average(c => c.A);
            
            // 创建平均颜色
            var averageColor = new Vicold.Pindoudou.Entities.Color((int)avgR, (int)avgG, (int)avgB, (int)avgA);
            
            // 在调色板中找到最接近的颜色
            return palette.FindClosestColor(averageColor);
        }
        
    }
}