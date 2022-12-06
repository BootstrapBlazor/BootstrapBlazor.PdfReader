# Blazor Pdf Reader PDF阅读器 组件  

![image](https://user-images.githubusercontent.com/8428709/205808008-b3898c07-3f26-4f88-be5c-7836f8985174.png)

示例:

https://blazor.app1.es/pdfReaders

使用方法:

1.nuget包

```BootstrapBlazor.PdfReader```

2._Imports.razor 文件 或者页面添加 添加组件库引用

```@using BootstrapBlazor.Components```


3.razor页面
```
<PdfReader UrlBase="https://blazor.app1.es/"
           PdfFile="_content/BootstrapBlazor.PdfReader/sample.pdf" />
```
