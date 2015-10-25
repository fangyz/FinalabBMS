$(function () {
    $("#file_upload").uploadify({
        //开启调试
        'debug': false,
        //是否自动上传
        'auto': false,
        'buttonText': '选择文件',
        //flash
        'swf': "../../uploadify.swf",
        //文件选择后的容器ID
        'queueID': 'uploadfileQueue',
        'uploader': '../../uploadfiyHelper.ashx',
        'width': '75',
        'height': '24',
        'multi': false,
        'fileTypeDesc': '支持的格式：',
        'fileTypeExts': '*',
        'fileSizeLimit': 1024 * 1024 * 1024,
        'removeTimeout': 100,

        //返回一个错误，选择文件的时候触发
        'onSelectError': function (file, errorCode, errorMsg) {
            switch (errorCode) {
                case -100:
                    alert("上传的文件数量已经超出系统限制的" + $('#file_upload').uploadify('settings', 'queueSizeLimit') + "个文件！");
                    break;
                case -110:
                    alert("文件 [" + file.name + "] 大小超出系统限制的" + $('#file_upload').uploadify('settings', 'fileSizeLimit') + "大小！");
                    break;
                case -120:
                    alert("文件 [" + file.name + "] 大小异常！");
                    break;
                case -130:
                    alert("文件 [" + file.name + "] 类型不正确！");
                    break;
            }
        },
        //检测FLASH失败调用
        'onFallback': function () {
            alert("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。");
        },
        //上传到服务器，服务器返回相应信息到data里
        'onUploadSuccess': function (file, data, response) {
            //alert("上传成功,file:" + file.fileTypeExts + "data:" + data + "response:" + response);
            $("#imgTest").attr("src", data);
            $("#txt-file-name").attr("value", data);
            //window.location = "SaveTaskFile?filename=" + data+"&strTaskId="+ $("#txt-task-id").val();
            $.post("/Task/Task/SaveTaskFile", {
                filename: data,
                strTaskId: $("#txt-task-id").val()               
            }, function (data) {
                if (data == "ok") {
                    alert("上传成功");
                }
                else {
                    alert("服务器忙!请稍候再试!")
                }
            });
        }
    });
});
function doUplaod() {
    $('#file_upload').uploadify('upload', '*');
}

function closeLoad() {
    $('#file_upload').uploadify('cancel', '*');
}

function downloadFile(name) {
   window.location = "../../downloadFile.aspx?filename=" + name;
}