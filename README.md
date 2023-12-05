# Blazor Pdf Reader PDF阅读器 组件  

![QQ截图20221218143438](https://user-images.githubusercontent.com/8428709/208301379-21e3b760-3f80-4941-9deb-1d34728ca2a5.jpg)

示例:

https://www.blazor.zone/PdfReaders

https://blazor.app1.es/pdfReaders

使用方法:

1.nuget包

```BootstrapBlazor.PdfReader```

2._Imports.razor 文件 或者页面添加 添加组件库引用

```@using BootstrapBlazor.Components```


3.razor页面
```
<PdfReader FileName="pdf/sample.pdf" />
           
<PdfReader FileName="https://blazor.app1.es/_content/DemoShared/sample.pdf" />

<pre>流化方式,可跨域</pre>
<PdfReader FileName="https://densen.es/test/webdev/pdf/sample.pdf" 
           StreamMode="true"/> 
```

4.参数说明 


|  参数   | 说明  | 默认值  | 旧版名称 |
|  ----  | ----  | ----  |  ----  | 
| FileName  | PDF文件路径(Url或相对路径) | null | Filename |
| StreamMode  | 使用流化模式,可跨域读取文件 | false | EnableStreamingMode |
| LocalFileName  | PDF本地文件路径 | null |  |
| Width  | 宽 单位(px/%) | 100% | 
| Height  | 高 单位(px/%) | 500px | 
| StyleString  | 组件外观 Css Style |  | 
| Page | 页码 | 1 |
| Pagemode | 页面模式, EnumPageMode 类型 | Thumbs |
| Zoom | 缩放模式, EnumZoomMode 类型 | Auto |
| Search | 查询字符串 | | 
| Refresh() | 刷新组件 | |
| ShowPdf(Stream stream) | 从 stream 渲染PDF | |
| NavigateToPage(int page) | 跳转页码 | |
| Refresh(int page) | 跳转页码 | |
| Refresh(string? search, int? page, EnumPageMode? pagemode, EnumZoomMode? zoom) | 刷新组件(查询关键字,页码,页面模式,缩放模式) | |
| Stream  | 用于渲染的文件流,为空则用URL参数读取文件 |  | PdfStream |
| ViewerBase | 浏览器页面路径 | 内置 | PDFJS_URL |
| Navpanes | 显示导航窗格 | true |
| Toolbar | 显示工具栏 | true |
| Statusbar | 显示状态栏 | true |
| Debug | 显示调试信息 | | 
| AutoStreamMode | 'http' 开头自动使用流模式读取 | true | 
| Watermark | 水印内容 | | 
| ReadOnly | 禁用复制/打印/下载 | | 
| CompatibilityMode | 兼容模式,兼容旧版浏览器 | false | 
| CompatibilityNoneES5 | 兼容模式,兼容旧版不支持es5的浏览器 | false | 

---
#### 更新历史

v8.0.1
- 添加 LocalFileName 读取本地文件路径
- 添加 Stream 缓存提高性能
- 优化搜索功能

v7.2.0
- 兼容 .pfb 和 .bcmap
 
v7.1.10
- 修复直接使用组件 ShowPdf 方法报错.

v7.1.9 
- 升级兼容模式版本,修复移动端手势缩放,部分文件字体无法正常加载.(感谢Ponderfly的PR)

v7.1.8 
- 添加 CompatibilityNoneES5 : 兼容模式,兼容旧版不支持es5的浏览器
- Chrome < 97 自动使用 2.4.456 版本
- Chrome < 109 自动使用 2.6.347 版本
- 注:ReadOnly 和 Watermark 在这两种兼容模式下不能使用


v7.1.7 
- 添加 CompatibilityMode : 兼容模式,兼容旧版浏览器

v7.1.5 
- 添加 AutoStreamMode: 'http' 开头自动使用流模式读取
- 添加 Watermark : 水印内容
- 添加 ReadOnly : 禁用复制/打印/下载

v7.1.4
- Filename 更改为 FileName

v7.1.3 
- 移除pdfobject, 一些参数也被移除,请注意更改

| 移除参数 | 
| ----  | 
| UrlBase | 
| View |

v7.1
- 移除pdfobject, 一些参数也被移除,请注意更改

| 移除参数 | 
| ----  | 
| Func<string, Task>? OnInfo | 
| Func<string, Task>? OnError | 
| ForceIframe | 
| ForcePDFJS | 
| UrlBase | 
| View |

---
#### Blazor 组件

[条码扫描 ZXingBlazor](https://www.nuget.org/packages/ZXingBlazor#readme-body-tab)
[![nuget](https://img.shields.io/nuget/v/ZXingBlazor.svg?style=flat-square)](https://www.nuget.org/packages/ZXingBlazor) 
[![stats](https://img.shields.io/nuget/dt/ZXingBlazor.svg?style=flat-square)](https://www.nuget.org/stats/packages/ZXingBlazor?groupby=Version)

[图片浏览器 Viewer](https://www.nuget.org/packages/BootstrapBlazor.Viewer#readme-body-tab)
  
[条码扫描 BarcodeScanner](Densen.Component.Blazor/BarcodeScanner.md)
   
[手写签名 Handwritten](Densen.Component.Blazor/Handwritten.md)

[手写签名 SignaturePad](https://www.nuget.org/packages/BootstrapBlazor.SignaturePad#readme-body-tab)

[定位/持续定位 Geolocation](https://www.nuget.org/packages/BootstrapBlazor.Geolocation#readme-body-tab)

[屏幕键盘 OnScreenKeyboard](https://www.nuget.org/packages/BootstrapBlazor.OnScreenKeyboard#readme-body-tab)

[百度地图 BaiduMap](https://www.nuget.org/packages/BootstrapBlazor.BaiduMap#readme-body-tab)

[谷歌地图 GoogleMap](https://www.nuget.org/packages/BootstrapBlazor.Maps#readme-body-tab)

[蓝牙和打印 Bluetooth](https://www.nuget.org/packages/BootstrapBlazor.Bluetooth#readme-body-tab)

[PDF阅读器 PdfReader](https://www.nuget.org/packages/BootstrapBlazor.PdfReader#readme-body-tab)

[文件系统访问 FileSystem](https://www.nuget.org/packages/BootstrapBlazor.FileSystem#readme-body-tab)

[光学字符识别 OCR](https://www.nuget.org/packages/BootstrapBlazor.OCR#readme-body-tab)

[电池信息/网络信息 WebAPI](https://www.nuget.org/packages/BootstrapBlazor.WebAPI#readme-body-tab)

[视频播放器 VideoPlayer](https://www.nuget.org/packages/BootstrapBlazor.VideoPlayer#readme-body-tab)

#### AlexChow

[今日头条](https://www.toutiao.com/c/user/token/MS4wLjABAAAAGMBzlmgJx0rytwH08AEEY8F0wIVXB2soJXXdUP3ohAE/?) | [博客园](https://www.cnblogs.com/densen2014) | [知乎](https://www.zhihu.com/people/alex-chow-54) | [Gitee](https://gitee.com/densen2014) | [GitHub](https://github.com/densen2014)
