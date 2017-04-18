using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWriter
{

    /// <summary>
    /// 水印的类型
    /// </summary>
    public enum WaterMarkType
    {
        /// <summary>
        /// 文字水印
        /// </summary>
        TextMark,
        /// <summary>
        /// 图片水印
        /// </summary>
        ImageMark // 暂时只能添加文字水印
    }
}
