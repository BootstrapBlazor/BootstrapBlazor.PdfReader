// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections;

namespace BootstrapBlazor.Components;

/// <summary>
/// Blazor Pdf Reader PDF阅读器 组件 
/// </summary>
public partial class PdfReader : IAsyncDisposable
{
    [Inject] IJSRuntime? JS { get; set; }

    /// <summary>
    /// 获得/设置 打开文件按钮文字 默认为 打开
    /// </summary>
    [Parameter]
    public string? ButtonGetFile { get; set; } = "打开";

    /// <summary>
    /// 获得/设置 宽
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 700;

    /// <summary>
    /// 获得/设置 文件流
    /// </summary>
    [Parameter]
    public Stream? stream { get; set; }

    /// <summary>
    /// 获得/设置 UrlBase
    /// </summary>
    [Parameter]
    public string? UrlBase { get; set; }

    /// <summary>
    /// 获得/设置 PDF文件
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


    private IJSObjectReference? module;
    private DotNetObjectReference<PdfReader>? instance { get; set; }
    public string? msg = string.Empty;
    /// <summary>
    ///
    /// </summary>
    protected ElementReference pdfElement { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                module = await JS!.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.PdfReader/api.js");
                instance = DotNetObjectReference.Create(this);
                if (stream != null)
                {
                    ShowPdf(stream);
                }
                else if (!string.IsNullOrEmpty(PdfFile))
                {
                    var byteArray = await GetImageAsByteArray(PdfFile, UrlBase!);
                    ShowPdf(new MemoryStream(byteArray));
                }
                else
                {
                    if (OnError != null) await OnError.Invoke("文件在哪?");
                }
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

    /// <summary>
    /// 打开文件
    /// </summary> 
    public virtual async void ShowPdf(Stream stream)
    {
        try
        {
            using var streamRef = new DotNetStreamReference(stream);
            await module!.InvokeVoidAsync("addScript", null);
            await module!.InvokeVoidAsync("showPdf", instance, pdfElement, streamRef);
        }
        catch (Exception e)
        {
            msg += e.Message + Environment.NewLine;
            if (OnError != null) await OnError.Invoke(e.Message);
        }
    }

    /// <summary>
    /// 签名完成回调方法
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task Result(string? val)
    {
        if (OnInfo != null) await OnInfo.Invoke(val ?? "");
    }

}
