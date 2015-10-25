
//===============================<< 回复 >>==============================
$(function () {
    $("#reply").click(function () {
        //alert("点击了");
        $receiveName = $("#sendName");
        $topic = $("#title");
        $messageContent = $("#content");
        $hiddenFileAddress = $("#atachmentHidden");
        $replyContent = $("#replyContent");

        if ($.trim($replyContent.val()) == "") {
            alert("回复内容不能为空！");
            $("#replyContent").css("border", "1px solid red");
        } else {
            //alert("进入了");
            var mc = "##" + $.trim($topic.text()) + "##" + $replyContent.val();
            //alert(mc);
            $.post(
                "/Message/InsideMsg/SendMessage",
                {
                    receiveName: $receiveName.text(),
                    topic: $topic.text(),
                    messageContent: mc,
                    hiddenFileAddress: null,
                    sendTime: GetCurrentTime(1)
                },
                function (data) {
                    if (data == "ok") {
                        alert("回复成功！");
                    } else {
                        alert("回复失败！");
                    }
                }
                );
        }
    });
});

//==============================<< 获取当前时间  >>======================
function GetCurrentTime(flag) {
    var currentTime = "";
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = parseInt(myDate.getMonth().toString()) + 1; //month是从0开始计数的，因此要 + 1
    if (month < 10) {
        month = "0" + month.toString();
    }
    var date = myDate.getDate();
    if (date < 10) {
        date = "0" + date.toString();
    }
    var hour = myDate.getHours();
    if (hour < 10) {
        hour = "0" + hour.toString();
    }
    var minute = myDate.getMinutes();
    if (minute < 10) {
        minute = "0" + minute.toString();
    }
    var second = myDate.getSeconds();
    if (second < 10) {
        second = "0" + second.toString();
    }
    if (flag == "0") {
        currentTime = year.toString() + month.toString() + date.toString() + hour.toString() + minute.toString() + second.toString(); //返回时间的数字组合
    }
    else if (flag == "1") {
        currentTime = year.toString() + "/" + month.toString() + "/" + date.toString() + " " + hour.toString() + ":" + minute.toString() + ":" + second.toString(); //以时间格式返回
    }
    return currentTime;
}
//=============================<<  返回 >>===============================
$(function () {
    $("#return").click(function () {
        if (confirm("编辑的消息内容将丢失，是否确认离开！")) {
            location.href = "/Message/InsideMsg/Index";
        }
        return false;
    });
});