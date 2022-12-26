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
    /// 获得/设置 PDF文件URL
    /// </summary>
    [Parameter]
    public string? Filename { get; set; }

    /// <summary>
    /// 获得/设置 使用流化模式,可跨域读取文件. 默认为 false
    /// </summary>
    [Parameter]
    public bool StreamMode { get; set; }

    /// <summary>
    /// 获得/设置 PDF文件基础路径, (使用流化模式才需要设置)
    /// </summary>
    [Parameter]
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
    /// 获得/设置 页码
    /// </summary> 
    [Parameter]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 获得/设置 显示导航窗格
    /// </summary> 
    [Parameter]
    public int Navpanes { get; set; } = 0;

    /// <summary>
    /// 获得/设置 显示工具栏
    /// </summary> 
    [Parameter]
    public int Toolbar { get; set; } = 0;

    /// <summary>
    /// 获得/设置 显示状态栏
    /// </summary> 
    [Parameter]
    public int Statusbar { get; set; } = 0;

    /// <summary>
    /// 获得/设置 视图模式
    /// </summary>
    [Parameter]
    public string? View { get; set; } = "FitV";

    /// <summary>
    /// 获得/设置 页面模式
    /// </summary>
    [Parameter]
    public string? Pagemode { get; set; } = "thumbs";

    /// <summary>
    /// 获得/设置 查询字符串
    /// </summary>
    [Parameter]
    public string? Search { get; set; }

    /// <summary>
    /// 获得/设置 浏览器路径
    /// </summary> 
    [Parameter]
    public string ViewerBase { get; set; } = "/_content/BootstrapBlazor.PdfReader/web/viewer.html";

    private string? Url { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/app.js");
            await Refresh();
        }
    }



    public virtual async Task Refresh()
    {
        if (Stream != null)
        {
            await ShowPdf(Stream);
        }
        else if (StreamMode && !string.IsNullOrEmpty(Filename))
        {
            var client = new HttpClient();
            var response = await client.GetAsync(UrlBase ?? "" + Filename);

            var byteArray = await response.Content.ReadAsByteArrayAsync();
            await ShowPdf(new MemoryStream(byteArray));
        }

    }

    /// <summary>
    /// 打开 stream
    /// </summary>
    public virtual async Task ShowPdf(Stream stream)
    {
        try
        {
            var url = $"{ViewerBase}?file=(1)#page={Page}?navpanes={Navpanes}&toolbar={Toolbar}&statusbar={Statusbar}&view={View}&pagemode={Pagemode}&search={Search}";
            using var streamRef = new DotNetStreamReference(stream);
            await Module!.InvokeVoidAsync("showPdf", url, Element, streamRef);
        }
        catch
        {
        }
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




