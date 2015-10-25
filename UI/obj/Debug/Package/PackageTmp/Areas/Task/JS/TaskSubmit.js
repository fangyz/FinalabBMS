
function SubmitContent(submition)
{
    $txtSubmitContent = $("#txt-task-content");
    $taskId = $("#txt-task-id");
    $taskFinishTime = $("#txt-finish-time");
    if (submition == "")
    {
        $txtSubmitContent.focus();
    }
    else
    {      
        $.post("/Task/Task/SaveTaskSubmit", {
            TaskSubmition: submition,
            TaskId: $taskId.val(),
            TaskFinishTime:$taskFinishTime.val()
        }, function (data) {
            if (data == "ok") {
                alert("成功提交！");
                location.href = "/Task/Task/Mytask";
            }
            else {
                alert("服务器忙!请稍候再试!")
            }
        });
    }
}

    function SetBorderColor($obj, color) {
        $obj.css("border-color", color);
    }
