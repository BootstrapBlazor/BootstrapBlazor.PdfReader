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

    /// <summary>
    /// 获得/设置 宽
    /// </summary>
    public string? Height { get; set; } = "700px";

    /// <summary>
    /// 获得/设置 指定页码
    /// </summary> 
    public string? Page { get; set; } = "1";

    /// <summary>
    /// 获得/设置 强制使用 Iframe
    /// </summary> 
    public bool ForceIframe { get; set; } = true;

    
    /// <summary>
    /// 获得/设置 打开参数
    /// </summary> 
    public PdfOpenParams? pdfOpenParams { get; set; }

    /// <summary>
    /// 打开参数
    /// </summary>
    public class PdfOpenParams
    {
        /// <summary>
        /// 视图模式
        /// </summary>
        public string? View { get; set; } = "FitV";

        /// <summary>
        /// 页面模式
        /// </summary>
        public string? Pagemode { get; set; } = "thumbs";

        /// <summary>
        /// 查询字符串的
        /// </summary>
        public string? Search { get; set; }

    }
}
