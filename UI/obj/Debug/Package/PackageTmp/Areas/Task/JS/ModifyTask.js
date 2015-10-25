function CheckMembers(jsonMembers)
{
    var x = document.getElementsByName("checkbox");
    for (var i = 0; i < x.length; i++) {
        for (var j = 0; j < jsonMembers.length;j++)
        {
            if (jsonMembers[j].StuNum == x[i].value)
            {
                x[i].checked = true;
                break;
            }
        }
    }
}

function ModifySubmit()
{
    $txtTaskName = $("#txt-task-name");
    $txtTaskBegTime = $("#txt-task-beg-time");
    $txtTaskEndTime = $("#txt-task-end-time");
    $txtTaskContent = $("#txt-task-content");
    $txtTaskId = $("#txt-task-id");
    $txtTaskFile = $("#txt-file-name");

    TaskType = document.getElementById("slt-task-type");
    Proj = document.getElementById("slt-proj-name");
    ProjPhases = document.getElementById("txt-proj-phases");
    if ($.trim($txtTaskName.val()) == "") {
        SetBorderColor($txtTaskName.parent('.text_border'), "red");
        $txtTaskName.focus();
    } else if ($.trim($txtTaskBegTime.val()) == "") {
        SetBorderColor($txtTaskBegTime.parent('.text_border'), "red");
        $txtTaskBegTime.focus();
    } else if ($.trim($txtTaskEndTime.val()) == "") {
        SetBorderColor($txtTaskEndTime.parent('.text_border'), "red");
        $txtTaskEndTime.focus();
    } else if ($.trim($txtTaskContent.val()) == "") {
        SetBorderColor($txtTaskContent.parent('.text_border'), "red");
        $txtTaskContent.focus();
    } else if (!SelectedMember()) {
        alert("请选择任务接收人！");
    } else {
        $.post("/Task/Task/SaveTaskInfo", {
            TaskName: $txtTaskName.val(),
            TaskBegTime: $txtTaskBegTime.val(),
            TaskEndTime: $txtTaskEndTime.val(),
            TaskContent: $txtTaskContent.val(),
            TaskId: $txtTaskId.val(),
            TaskFile:$txtTaskFile.val(),
            TaskTypeId: TaskType.options[TaskType.selectedIndex].value,
            ProjId: Proj.options[Proj.selectedIndex].value,
            ProjPhasesId: ProjPhases.options[ProjPhases.selectedIndex].value,
            Members: GetMemers()
        }, function (data) {

            if (data == "ok") {
                alert("修改成功");
                location.href = "/Task/Task/ReleaseHistory";
            } else {
                alert("服务器忙！请稍后重试！");
            }
        });
    }
}

function ChangeSltProjPhases(phasesjId)
{    
    var slt = document.getElementById('txt-proj-phases');
    for (var i = 0; i < slt.options.length; i++) {
        if (slt.options[i].value == phasesjId) {
            slt.options[i].selected = true;
        }
    }
}
function ChangeSltProj(projId) {
    var slt = document.getElementById('slt-proj-name');
    for (var i = 0; i < slt.options.length; i++) {
        if (slt.options[i].value == projId) {
            slt.options[i].selected = true;
        }
    }
    OnchangeSltProj(projId);
}