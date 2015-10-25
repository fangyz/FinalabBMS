$(function () {

});

function TaskListShrink(id, task) {
    $taskList = $("#task-content-" + id);
    if ($taskList.css("height") == '0px') {
        $taskList.animate({ height: "225px" }, "slow");
        $("#task-message-img-" + id).css("background-position-y", "-15px");
        $(task).css("background-color", "#27B8A5");
        $("#task-message-title-" + id).css("color", "#fff");
    } else {
        $taskList.animate({ height: "0px" }, "slow");
        $("#task-message-img-" + id).css("background-position-y", "0px");
        $(task).css("background-color", "#fff");
        $("#task-message-title-" + id).css("color", "#27B8A5");
    }
}

function RemarkContent() {
    $taskId = $("#txt-task-id");
    $taskReceiver = $("#txt-task-receiver");
    $taskScore = $("#txt-evaluate-score");
    $taskRemark = $("#txt-evaluate-remark");
    if ($taskScore.val() == "" || $taskRemark.val() == "") {
        alert("请填写评分或评语");
    }
    else
    {
    var v = $taskScore.val();
    var w = $taskId.val();
    if (isNaN(v) || v < 0 || v > 100) {
        alert("请给出正确的分数");
    } else {
        $.post("SaveTaskCorret", {
            TaskId: $taskId.val(),
            TaskReceiver: $taskReceiver.val(),
            TaskScore: $taskScore.val(),
            TaskRemark: $taskRemark.val()
        }, function (data) {
            if (data == "ok") {
                alert("任务已批阅！");
                location.href = "/Task/Task/TaskEvaluate?taskId=" + w;
            } else {
                alert("服务器忙，请刷新后重试！");
            }
        });
    }
    }
}
