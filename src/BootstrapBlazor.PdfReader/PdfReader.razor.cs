// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
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
    /// 获得/设置 查询字符串
    /// </summary>
    [Parameter]
    public string? Search { get; set; }

    /// <summary>
    /// 获得/设置 放大倍率 默认为 空
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


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/app.js");
            await Refresh();
        }
    }



    public virtual async Task Refresh(int page = 1)
    {
        ErrorMessage = null;
        try
        {
            if (Stream != null)
            {
                await ShowPdf(Stream);
            }
            else if (!string.IsNullOrEmpty(Filename) && (StreamMode || Filename.StartsWith("http")))
            {
                var byteArray = await GetImageAsByteArray(Filename, UrlBase!);
                await ShowPdf(new MemoryStream(byteArray));

                //var client = new HttpClient();
                //var stream = await client.GetStreamAsync($"{UrlBase ?? ""}{Filename}");
                //if (stream != null)
                //{
                //    await ShowPdf(stream);
                //}
            }
            else
            {
                Page = page;
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
        Url = GenUrl(false);
        using var streamRef = new DotNetStreamReference(stream);
        await Module!.InvokeVoidAsync("showPdf", Url, Element, streamRef);
    }

    static async Task<byte[]> GetImageAsByteArray(string urlImage, string urlBase)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(new Uri(urlBase + urlImage));
        return await response.Content.ReadAsByteArrayAsync();
    }

    static string ConvertToBase64(Stream stream)
    {
        if (stream is MemoryStream memoryStream)
        {
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        var bytes = new Byte[(int)stream.Length];
        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(bytes, 0, (int)stream.Length);
        return Convert.ToBase64String(bytes);
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




