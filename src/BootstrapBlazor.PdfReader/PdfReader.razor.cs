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
    
    private DotNetObjectReference<PdfReader>? Instance { get; set; }

    private ElementReference Element { get; set; }

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
    /// 获得/设置 宽 单位(px|%) 默认 100%
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// 获得/设置 高 单位(px|%) 默认 500px
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "500px";

    /// <summary>
    /// 获得/设置 指定页码,如果浏览器支持，将加载PDF并自动滚动到第n页
    /// </summary> 
    [Parameter]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 获得/设置 Navpanes (PDF.js 专有)
    /// </summary> 
    [Parameter]
    public int Navpanes { get; set; } = 0;

    /// <summary>
    /// 获得/设置 Toolbar (PDF.js 专有)
    /// </summary> 
    [Parameter]
    public int Toolbar { get; set; } = 0;

    /// <summary>
    /// 获得/设置 Statusbar (PDF.js 专有)
    /// </summary> 
    [Parameter]
    public int Statusbar { get; set; } = 0;

    /// <summary>
    /// 视图模式 (PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? View { get; set; } = "FitV";

    /// <summary>
    /// 页面模式 (PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? Pagemode { get; set; } = "thumbs";

    /// <summary>
    /// 查询字符串的(PDF.js 专有)
    /// </summary>
    [Parameter]
    public string? Search { get; set; }
    
    /// <summary>
    /// 获得/设置 PDF.js 浏览器页面路径
    /// </summary> 
    [Parameter]
    public string PDFJS_URL { get; set; } = "/_content/BootstrapBlazor.PdfReader/web/viewer.html";
    private string? Url { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
            if (firstRender)
            {
             Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/app.js");
             Instance = DotNetObjectReference.Create(this);
             await Refresh();
            }
    }



    public virtual async Task Refresh()
    {
        if (PdfStream != null)
        {
            await ShowPdf(PdfStream);
        }
        else if (!string.IsNullOrEmpty(PdfFile))
        {
            if (!EnableStreamingMode)
            {
                var url = $"{PDFJS_URL}#page={Page}&navpanes={Navpanes}&toolbar={Toolbar}&statusbar={Statusbar}&view={View}&pagemode={Pagemode}&search={Search}&file=";
                Url = $"{url}{UrlBase}{PdfFile}";
                Url = HttpUtility.UrlEncode(Url);
                StateHasChanged();
            }
            else
            {
                var byteArray = await GetImageAsByteArray(PdfFile, UrlBase!);
                await ShowPdf(new MemoryStream(byteArray));
            }
         }
        
    }

    /// <summary>
    /// 打开 stream
    /// </summary>
    public virtual async Task ShowPdf(Stream stream)
    {
        try
        {
            var url = $"{PDFJS_URL}#page={Page}&navpanes={Navpanes}&toolbar={Toolbar}&statusbar={Statusbar}&view={View}&pagemode={Pagemode}&search={Search}&file=";
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
    
    static async Task<byte[]> GetImageAsByteArray(string urlImage, string urlBase)
    {

        var client = new HttpClient();
        client.BaseAddress = new Uri(urlBase);
        var response = await client.GetAsync(urlImage);

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

}




