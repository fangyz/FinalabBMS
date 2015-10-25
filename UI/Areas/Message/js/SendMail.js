
//=========================<< 邮件发送验证 >>================================
$(function () {
    $("#sendNow").click(function () {
        //alert("点击了");

        if (confirm("您确定要发送该邮件吗？")) {
            $sendEmail = $("#sendEmail");
            $pwd = $("#pwd");
            $receiveEmail = $("#receiveEmail");
            $title = $("#title");
            $content = $("#content");
            $hiddenFileAddress = $("#hiddenFileAddress");

            var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
            if (!reg.test($sendEmail.val())) {
                alert("请输入正确的发送邮箱格式");
                $("#sendEmail").css("border", "1px solid red");
            } else if ($pwd.val == "") {
                alert("请输入邮箱密码");
                $("#pwd").css("border", "1px solid red");
            } else if (!reg.test($receiveEmail.val())) {
                alert("请输入正确的收件邮箱款式");
                $("#receiveEmail").css("border", "1px solid red");
            } else if ($title.val().trim() == "") {
                alert("请输入主题！");
                $("#title").css("border", "1px solid red");
            } else if ($content.val().trim() == "") {
                alert("请输入正文！");
                $("#content").css("border", "1px solid red");
            } else {
                $("#statusSend").css({ "display": "block" });
                $.post(
                "/Message/Mail/Send",
                {
                    sendEmail: $sendEmail.val(),
                    pwd: $pwd.val(),
                    receiveEmail: $receiveEmail.val(),
                    title: $title.val(),
                    content: $content.val(),
                    hiddenFileAddress: $hiddenFileAddress.val()
                },
                function (data) {
                    if (data == "ok") {
                        alert("发送成功！");
                        $("#statusSend").css({ "display": "none" });
                    } else {
                        alert("发送失败！");
                        alert("原因可能是:发送邮箱和密码不对！");
                        $("#statusSend").css({ "display": "none" });
                    }
                }
                );
            }
        }
    });
});