

//=============================<< 消息发送  >>===========================
function SendNow() {

    //alert("点击了发送！");
    if (confirm("您确定要发送该信息吗？")) {
        $receiveName = $("#receiveName");
        $topic = $("#topic");
        $messageContent = $("#messageContent");
        $hiddenFileAddress = $("#hiddenFileAddress");

        //alert($receiveName.val() + "+" + $topic.val() + "+" + $messageContent.val() + "!");

        if ($.trim($receiveName.val()) == "") {
            alert("收件人不能为空！");
            $("#receiveName").css("border", "1px solid red");
        } else if ($.trim($topic.val()) == "") {
            alert("主题不能为空！");
            $("#topic").css("border", "1px solid red");
        } else if ($.trim($messageContent.val()) == "") {
            alert("正文不能为空");
            $("#messageContent").css("border", "1px solid red");
        } else {
            //alert("进入了");
            $.post(
                "/Message/InsideMsg/SendMessage",
                {
                    receiveName: $receiveName.val(),
                    topic: $topic.val(),
                    messageContent: $messageContent.val(),
                    hiddenFileAddress: $hiddenFileAddress.val(),
                    sendTime: GetCurrentTime(1)
                },
                function (data) {
                    if (data == "ok") {
                        alert("发送成功！");
                    } else if (data == "nook") {
                        alert("发送失败！");
                        alert("原因可能是1. 用户不存才 2.多个联系人不用“；”隔开");
                    }
                    else {
                        alert("发送失败！");
                    }
                }
                );
        }
    }

}

//=============================<< 定时发送 >>============================
$(function () {
    $("#sendTimer").click(function () {

        //alert("点击了");
        if (confirm("您确定要设置定时发送发送该信息吗？")) {
            $receiveName = $("#receiveName");
            $topic = $("#topic");
            $messageContent = $("#messageContent");
            $hiddenFileAddress = $("#hiddenFileAddress");

            $sendTime = $("input[name='act_time']");
            //alert($sendTime.val());

            //alert($receiveName.val() + "+" + $topic.val() + "+" + $messageContent.val() + "!");

            if ($.trim($receiveName.val()) == "") {
                alert("收件人不能为空！");
                $("#receiveName").css("border", "1px solid red");
            } else if ($.trim($topic.val()) == "") {
                alert("主题不能为空！");
                $("#topic").css("border", "1px solid red");
            } else if ($.trim($messageContent.val()) == "") {
                alert("正文不能为空");
                $("#messageContent").css("border", "1px solid red");
            } else if ($sendTime.val() == "") {
                alert("发送时间不能为空");
                $("input[name='act_time']").css("border", "1px solid red");
            } else {
                //alert("开始定时发送");
                $.post(
                    "/Message/InsideMsg/SendMessage",
                    {
                        receiveName: $receiveName.val(),
                        topic: $topic.val(),
                        messageContent: $messageContent.val(),
                        hiddenFileAddress: $hiddenFileAddress.val(),
                        sendTime: $("input[name='act_time']").val() + ":00"
                    },
                    function (data) {
                        if (data == "ok") {
                            alert("成功设置定时发送！");
                        } else {
                            alert("设置定时发送失败！");
                        }
                    }
                    );
            }
        }
    });
});

//=============================<< 保存草稿 >>============================
$(function () {

    $("#saveDraft").click(function () {
        //alert("点击了！");

        $receiveName = $("#receiveName");
        $topic = $("#topic");
        $messageContent = $("#messageContent");
        $hiddenFileAddress = $("#hiddenFileAddress");

        if ($.trim($receiveName.val()) == "") {
            alert("收件人必填，不能为空！");
            $("#receiveName").css("border", "1px solid red");
        } else {
            $.post(
                "/Message/InsideMsg/SaveDraft",
                {
                    receiveName: $receiveName.val(),
                    topic: $topic.val(),
                    messageContent: $messageContent.val(),
                    hiddenFileAddress: $hiddenFileAddress.val()
                },
                function (data) {
                    if (data == "ok") {
                        alert("保存草稿成功！");
                    } else {
                        alert("保存草稿失败！");
                    }
                });
        }
    });
});

//==============================<< 通讯录树状列表 >>=====================
$(function () {
    $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
    $('.tree li.parent_li > span').on('click', function (e) {
        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(":visible")) {
            children.hide('fast');
            $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
        } else {
            children.show('fast');
            $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
        }
        e.stopPropagation();
    });
});

//==============================<< 关闭窗口 >>===========================
$(function () {
    $("#shutdown").click(function () {
        if (confirm("编辑的消息内容将丢失，是否确认离开！")) {
            location.href = "/Message/InsideMsg/Index";
        }
        return false;
    });
});

//==============================<< 联系人选择  >>========================
$(function () {
    $(".addreesClick").click(function () {

        //alert("1"+$(this).text());

        var str1 = $(this).text();

        //alert("2"+str1);

        var arr = new Array();
        var str2 = $("#receiveName").val();

        //alert("3"+str2);
        if (str2 == "") {
            //alert("4");
            $("#receiveName").val(str2 + str1 + ";");
        } else {
            //alert("5");
            arr = str2.split(";");
            //alert("6");
            var count = 0;
            for (i = 0 ; i < arr.length - 1; i++) {
                //alert("7进入循环");
                //alert("8"+arr[i]);
                if (arr[i] == str1) {
                    count++;
                    break;
                }
            }
            if (count == 0) {
                $("#receiveName").val(str2 + str1 + ";");
            }
            else {
                alert("重复的联系人!");
            }
        }
        return false;
    });
});

//==============================<< 时间选择器 >>=========================
$(function () {
    $("input[name='act_time']").datetimepicker();
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




