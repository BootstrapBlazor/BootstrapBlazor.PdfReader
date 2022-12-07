// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;
using static BootstrapBlazor.Components.PdfReaderOptions;

namespace BootstrapBlazor.Components;

/// <summary>
/// Blazor Pdf Reader PDF阅读器 组件 
/// </summary>
public partial class PdfReader : IAsyncDisposable
{
    [Inject] IJSRuntime? JS { get; set; }

    /// <summary>
    /// 获得/设置 文件流
    /// </summary>
    [Parameter]
    public Stream? PdfStream { get; set; }

    /// <summary>
    /// 获得/设置 PDF文件URL
    /// </summary>
    [Parameter]
    public string? PdfFile { get; set; }


    /// <summary>
    /// 获得/设置 信息回调方法
    /// </summary>
    [Parameter]
    public Func<string, Task>? OnInfo { get; set; }

    /// <summary>
    /// 获得/设置 错误回调方法
    /// </summary>
    [Parameter]
    public Func<string, Task>? OnError { get; set; }

    /// <summary>
    /// 获得/设置 使用流化模式,可跨域读取文件. 默认为 false
    /// </summary>
    [Parameter]
    public bool EnableStreamingMode { get; set; }

    /// <summary>
    /// 获得/设置 PDF文件基础路径, (使用流化模式才需要设置)
    /// </summary>
    [Parameter]
    public string? UrlBase { get; set; }

    /// <summary>
    /// 获得/设置 高
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 700;

    /// <summary>
    /// 获得/设置 指定页码,如果浏览器支持，将加载PDF并自动滚动到第n页
    /// </summary>
    [Parameter]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 获得/设置 强制使用 Iframe, 默认：false
    /// </summary> 
    [Parameter]
    public bool ForceIframe { get; set; }

    /// <summary>
    /// 获得/设置 强制使用 PDF.js
    /// </summary> 
    [Parameter]
    public bool ForcePDFJS { get; set; }

    /// <summary>
    /// 获得/设置 PDF.js 浏览器页面路径
    /// </summary> 
    [Parameter]
    public string PDFJS_URL { get; set; } = "https://pdfobject.com/pdfjs/web/viewer.html";

    /// <summary>
    /// 获得/设置 查询字符串 (PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? Search { get; set; }

    /// <summary>
    /// 获得/设置 视图模式 (PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? View { get; set; } = "FitV";

    /// <summary>
    /// 获得/设置 页面模式 (PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? Pagemode { get; set; } = "thumbs";

    private IJSObjectReference? module;
    private DotNetObjectReference<PdfReader>? instance { get; set; }
    public string? msg = string.Empty;
    /// <summary>
    ///
    /// </summary>
    protected ElementReference pdfElement { get; set; }
    protected PdfReaderOptions Options   = new PdfReaderOptions();
        
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                module = await JS!.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/api.js");
                instance = DotNetObjectReference.Create(this);
                await module!.InvokeVoidAsync("addScript", null);
                await Refresh();
            }
        }
        catch (Exception e)
        {
            msg += e.Message + Environment.NewLine;
            if (OnError != null) await OnError.Invoke(e.Message);
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (module is not null)
        {
            await module.DisposeAsync();
        }
        if (instance != null)
        {
            instance.Dispose();
        }

    }

    private async Task<byte[]> GetImageAsByteArray(string urlImage, string urlBase)
    {

        var client = new HttpClient();
        client.BaseAddress = new Uri(urlBase);
        var response = await client.GetAsync(urlImage);

        return await response.Content.ReadAsByteArrayAsync();
    }
    public virtual async Task Refresh()
    {
        Options = new PdfReaderOptions()
        {
            //Height = $"{Height}px",
            ForceIframe = PdfStream != null ? true : ForceIframe,
            ForcePDFJS = ForcePDFJS,
            PDFJS_URL = PDFJS_URL,
            pdfOpenParams = new PdfOpenParams()
            {
                Page = Page,
                View = View,
                Pagemode = Pagemode,
                Search = Search,
            }
        };
        if (PdfStream != null)
        {
            await ShowPdf(PdfStream);
        }
        else if (!string.IsNullOrEmpty(PdfFile))
        {
            if (!EnableStreamingMode)
            {
                await ShowPdfwithUrl($"{UrlBase}{PdfFile}");
            }
            else
            {
                var byteArray = await GetImageAsByteArray(PdfFile, UrlBase!);
                await ShowPdf(new MemoryStream(byteArray));
            }
        }
        else
        {
            if (OnError != null) await OnError.Invoke("文件在哪?");
        }

    }

    /// <summary>
    /// 打开 stream
    /// </summary> 
    public virtual async Task ShowPdf(Stream stream)
    {
        try
        {
            using var streamRef = new DotNetStreamReference(stream);
            await module!.InvokeVoidAsync("showPdf", instance, pdfElement, streamRef);
        }
        catch (Exception e)
        {
            msg += e.Message + Environment.NewLine;
            if (OnError != null) await OnError.Invoke(e.Message);
        }
    }

    /// <summary>
    /// 打开 URL
    /// </summary> 
    public virtual async Task ShowPdfwithUrl(string url)
    {
        try
        {
            await module!.InvokeVoidAsync("showPdfwithUrl", instance, pdfElement, url, Options);
        }
        catch (Exception e)
        {
            msg += e.Message + Environment.NewLine;
            if (OnError != null) await OnError.Invoke(e.Message);
        }
    }

    /// <summary>
    /// 完成回调方法
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task Result(string? val)
    {
        if (OnInfo != null) await OnInfo.Invoke(val ?? "");
    }

}
