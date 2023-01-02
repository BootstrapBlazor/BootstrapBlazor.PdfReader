// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.Arm;
using System.Web;

namespace BootstrapBlazor.Components;

/// <summary>
/// Blazor Pdf Reader PDF阅读器 组件 
/// </summary>
public partial class PdfReader : IAsyncDisposable
{
    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    [NotNull]
    private IJSObjectReference? Module { get; set; }

    private ElementReference Element { get; set; }

    /// <summary>
    /// 获得/设置 用于渲染的文件流,为空则用Filename参数读取文件
    /// </summary>
    [Parameter]
    public Stream? Stream { get; set; }

    /// <summary>
    /// 获得/设置 PDF文件URL, 'http' 开头自动使用流模式读取
    /// </summary>
    [Parameter]
    public string? Filename { get; set; }

    /// <summary>
    /// 获得/设置 使用流化模式,可跨域读取文件. 默认为 false
    /// </summary>
    [Parameter]
    public bool StreamMode { get; set; }

    /// <summary>
    /// [已过时,统一使用 Filename 简化参数] 获得/设置 PDF文件基础路径, (使用流化模式才需要设置)
    /// </summary>
    [Parameter]
    [Obsolete]
    public string? UrlBase { get; set; }

    /// <summary>
    /// 获得/设置 宽 单位(px|%) 默认 100%
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// 获得/设置 高 单位(px|%) 默认 500px
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "700px";

    /// <summary>
    /// 获得/设置 组件外观 Css Style
    /// </summary>
    [Parameter]
    public string? StyleString { get; set; }

    /// <summary>
    /// 获得/设置 页码
    /// </summary> 
    [Parameter]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 获得/设置 显示导航窗格
    /// </summary> 
    [Parameter]
    public bool Navpanes { get; set; } = true;

    /// <summary>
    /// 获得/设置 显示工具栏
    /// </summary> 
    [Parameter]
    public bool Toolbar { get; set; } = true;

    /// <summary>
    /// 获得/设置 显示状态栏
    /// </summary> 
    [Parameter]
    public bool Statusbar { get; set; } = true;

    /// <summary>
    /// [已过时,使用 Zoom 代替] 获得/设置 视图模式, 
    /// </summary>
    [Parameter]
    [Obsolete]
    public string? View { get; set; }

    /// <summary>
    /// 获得/设置 页面模式
    /// </summary>
    [Parameter]
    public EnumPageMode? Pagemode { get; set; } = EnumPageMode.Thumbs;

    /// <summary>
    /// 获得/设置 查询关键字
    /// </summary>
    [Parameter]
    public string? Search { get; set; }

    /// <summary>
    /// 获得/设置 缩放模式 默认为 自动
    /// </summary>
    [Parameter]
    public EnumZoomMode? Zoom { get; set; } = EnumZoomMode.Auto;

    /// <summary>
    /// 获得/设置 浏览器路径
    /// </summary> 
    [Parameter]
    public string ViewerBase { get; set; } = "/_content/BootstrapBlazor.PdfReader/web/viewer.html";


    /// <summary>
    /// Debug
    /// </summary>
    [Parameter]
    public bool Debug { get; set; }

    string? ErrorMessage { get; set; }

    private string? Url { get; set; }
    private string? UrlDebug { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/app.js");
            await Refresh();
        }
    }

    /// <summary>
    /// 刷新组件
    /// </summary>
    /// <returns></returns>
    public virtual async Task Refresh() => await Refresh(null,null,null,null);

    /// <summary>
    /// 跳转页码
    /// </summary>
    /// <param name="page">页码</param>
    /// <returns></returns>
    public virtual async Task NavigateToPage(int page) => await Refresh(page:page);

    /// <summary>
    /// 刷新组件
    /// </summary>
    /// <param name="page">页码</param>
    /// <returns></returns>
    public virtual async Task Refresh(int page) => await Refresh(page:page);

    /// <summary>
    /// 刷新组件
    /// </summary>
    /// <param name="search">查询关键字</param>
    /// <param name="page">页码</param>
    /// <param name="pagemode">页面模式</param>
    /// <param name="zoom">缩放模式</param>
    /// <returns></returns>
    public virtual async Task Refresh(string? search = null, int? page = null, EnumPageMode? pagemode = null, EnumZoomMode? zoom = null)
    {
        ErrorMessage = null;
        try
        {
            Search= search ?? Search;
            Page = page?? Page;
            Pagemode = pagemode?? Pagemode;
            Zoom = zoom ?? Zoom;

            if (Stream != null)
            {
                await ShowPdf(Stream);
            }
            else if (!string.IsNullOrEmpty(Filename) && StreamMode) //|| Filename.StartsWith("http")
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(UrlBase ?? "" + Filename);
                if (stream != null)
                {
                    await ShowPdf(stream);
                }
                else
                {
                    ErrorMessage = "No data";
                }
            }
            else
            {
                Url = GenUrl();
            }

        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
        StateHasChanged();

    }

    private string GenUrl(bool filemode = true) => $"{ViewerBase}?file={(filemode ? HttpUtility.UrlEncode(Filename) : "(1)")}#page={Page}&navpanes={(Navpanes ? 0 : 1)}&toolbar={(Toolbar ? 0 : 1)}&statusbar={(Statusbar ? 0 : 1)}&pagemode={(Pagemode ?? EnumPageMode.Thumbs).ToString().ToLower()}&search={Search}" + (Zoom != null ? $"&zoom={Zoom.GetEnumName()}" : "");
 

    /// <summary>
    /// 打开 stream
    /// </summary>
    public virtual async Task ShowPdf(Stream stream)
    {
        Url = null;
        var url = GenUrl(false);
        UrlDebug= url;
        using var streamRef = new DotNetStreamReference(stream);
        await Module!.InvokeVoidAsync("showPdf", url, Element, streamRef);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (Module is not null)
        {
            await Module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }


}




