// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************


using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BootstrapBlazor.Components;

/// <summary>
/// 选项
/// </summary>
public class PdfReaderOptions
{

    ///// <summary>
    ///// 获得/设置 高
    ///// </summary>
    //public string? Height { get; set; } = "700px";

    /// <summary>
    /// 获得/设置 强制使用 Iframe, 默认：false
    /// </summary> 
    public bool ForceIframe { get; set; }

    /// <summary>
    /// 获得/设置 强制使用 pdf.js, 默认：false
    /// </summary> 
    public bool ForcePDFJS { get; set; }

    /// <summary>
    /// 获得/设置 PDF.js 浏览器页面路径
    /// </summary>
    [JsonPropertyName("PDFJS_URL")]
    public string PDFJS_URL { get; set; } = "/_content/BootstrapBlazor.PdfReader/web/viewer.html";

    /// <summary>
    /// 获得/设置 打开参数
    /// </summary> 
    public PdfOpenParams? pdfOpenParams { get; set; }

    /// <summary>
    /// 打开参数, 警告：大部分参数是 PDF.js 专有的。
    /// </summary>
    public class PdfOpenParams
    {

        /// <summary>
        /// 获得/设置 指定页码,如果浏览器支持，将加载PDF并自动滚动到第n页
        /// </summary> 
        public int Page { get; set; } = 1;

        /// <summary>
        /// 获得/设置 Navpanes (PDF.js 专有)
        /// </summary> 
        public int Navpanes { get; set; } = 0;

        /// <summary>
        /// 获得/设置 Toolbar (PDF.js 专有)
        /// </summary> 
        public int Toolbar { get; set; } = 0;

        /// <summary>
        /// 获得/设置 Statusbar (PDF.js 专有)
        /// </summary> 
        public int Statusbar { get; set; } = 0;

        /// <summary>
        /// 视图模式 (PDF.js 专有)
        /// </summary>
        public string? View { get; set; } = "FitV";

        /// <summary>
        /// 页面模式 (PDF.js 专有)
        /// </summary>
        public string? Pagemode { get; set; } = "thumbs";

        /// <summary>
        /// 查询字符串的(PDF.js 专有)
        /// </summary>
        public string? Search { get; set; }

    }
}
