//import '/_content/BootstrapBlazor.PdfReader/pdfobject.min.js';
export function addScript() { 
    let url = '/_content/BootstrapBlazor.PdfReader/pdfobject.min.js';
    let scriptTags = document.querySelectorAll('body > script');
    scriptTags.forEach(scriptTag => {
        if (scriptTag) {
            let srcAttribute = scriptTag.getAttribute('src');
            if (srcAttribute && srcAttribute.startsWith(url)) {
                return true;
            }
        }
    });
    let script = document.createElement('script');
    script.src = url;
    //script.defer = true;
    document.body.appendChild(script);
    return false;
}

export async function showPdf(wrapperc, element, stream, options) {
    if (PDFObject !== undefined && PDFObject.supportsPDFs) {
        const arrayBuffer = await stream.arrayBuffer();
        const blob = new Blob([arrayBuffer], { type: 'application/pdf' });//必须要加type
        const url = URL.createObjectURL(blob);
        PDFObject.embed(url, element, { forceIframe: true });//只有iframe可以打开blob链接
        URL.revokeObjectURL(url);
        wrapperc.invokeMethodAsync("Result", "PDFObject OK")
    } else {
        await showPdfjs(wrapperc, element, stream, options);
    }
}

export async function showPdfjs(wrapperc, element, stream, options) {
    const arrayBuffer = await stream.arrayBuffer();
    const blob = new Blob([arrayBuffer], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    PDFObject.embed(url, element, options);
    wrapperc.invokeMethodAsync("Result", "PDFJS OK")
}

export async function showPdfwithUrl(wrapperc, element, url, options) {
    if (PDFObject !== undefined && PDFObject.supportsPDFs) {
        PDFObject.embed(url, element, options);
        wrapperc.invokeMethodAsync("Result", "PDFObject OK")
    } else {
        wrapperc.invokeMethodAsync("Boo, inline PDFs are not supported by this browser,try using pdf.js");
        options.ForcePDFJS = true;
        PDFObject.embed(url, element, options);
    }
}