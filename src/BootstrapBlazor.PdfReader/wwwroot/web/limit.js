//屏蔽功能按钮
function onBodyLoad() {
    var appConfig = PDFViewerApplication.appConfig;
    //appConfig.toolbar.viewBookmarkviewBookmark.setAttribute('hidden', 'true');
    appConfig.secondaryToolbar.viewBookmarkButton.setAttribute('hidden', 'true');
    appConfig.toolbar.openFile.setAttribute('hidden', 'true');
    appConfig.secondaryToolbar.openFileButton.setAttribute('hidden', 'true');
    appConfig.toolbar.download.setAttribute('hidden', 'true');
    appConfig.secondaryToolbar.downloadButton.setAttribute('hidden', 'true');
    appConfig.toolbar.print.setAttribute('hidden', 'true');
    appConfig.secondaryToolbar.printButton.setAttribute('hidden', 'true');

    //禁止键盘操作
    window.onkeydown = window.onkeyup = window.onkeypress = function (e) {
        e.preventDefault(); // 阻止默认事件行为
        window.event.returnValue = false;
    }
}
