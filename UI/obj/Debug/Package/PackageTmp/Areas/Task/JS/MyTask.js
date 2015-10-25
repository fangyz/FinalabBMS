$(function () {

});

//list收缩事件
function TaskListShrink(id,task)
{
    $taskList = $("#task-content-" + id);
    if ($taskList.css("height") == '0px')
    {
        $taskList.animate({ height: 225 }, "slow");
        $("#task-type-img-" + id).css("background-position-y", "-15px");
        $(task).css("background-color", "aliceblue");
        $("#task-type-title-" + id).css("color", "burlywood");

    } else
    {
        $taskList.animate({ height: 0 }, "slow");
        $("#task-type-img-" + id).css("background-position-y", "0px");
        $(task).css("background-color", "#fff");
        $("#task-type-title-" + id).css("color", "burlywood");
    }
    
}

//重写table
function ReloadTable(pageIndex, complete) {
    $.get("/Task/Task/GetMyTaskJsonData?strPageIndex=" + pageIndex + "&strPageSize=4&complete=" + complete, function (data, status) {
        //alert(data);
        MakeTable(eval(data), complete);
    });    
}

//绘制table
function MakeTable(jsonObject, complete) {
    
    var $table = $("#task-table-" + complete);
    

    var strTable = '';

    strTable += '<tr>';
    strTable += '<th>任务名称</th>';
    strTable += '<th>发布时间</th>';
    strTable += '<th>截止时间</th>';
    strTable += '<th>评价等级</th>';
    strTable += '<th>发布人</th>';
    strTable += '<th>是否完成</th>';
    strTable += '</tr>';

    for (var j = 0; j < 4;j++ )
    {
        if (jsonObject.length <= j)
        {
            strTable += '<tr>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '</tr>';
        } else
        {
            strTable += '<tr>';
            if (jsonObject[j].TaskGrade == 0)

            { strTable += '<td><a title="查看任务详情" class="task_detail_link" href="TaskDetail?taskid=' + jsonObject[j].TaskId + '">' + jsonObject[j].TaskName + '</a></td>'; }
            else
            { strTable += '<td><a title="查看我的任务详情" class="task_detail_link" href="MyTaskDetail?taskId=' + jsonObject[j].TaskId + '&taskReciver=' + jsonObject[j].TaskReceiver + '">' + jsonObject[j].TaskName + '</a>';}
            strTable += '<td>' + ChangeDateFormat(jsonObject[j].TaskBegTime) + '</td>';
            strTable += '<td>' + ChangeDateFormat(jsonObject[j].TaskEndTime) + '</td>';
            if (jsonObject[j].TaskGrade == "") {
                strTable += '<td>未评价</td>';
            } else {
                strTable += '<td>' + jsonObject[j].TaskGrade + '</td>';
            }            
            strTable += '<td>' + jsonObject[j].TaskSenderName + '</td>';

            if (jsonObject[j].IsComplete == true)
            {
                strTable += '<td><span>已完成</span></td>';
            } else
            {
                strTable += '<td><a title="完成任务" class="task_complete_link" href="Tasksubmit?taskid=' + jsonObject[j].TaskId + '">' + '未完成' + '</a></td>';
            }
            strTable += '</tr>';
        }
    }
    $table.empty();
    document.getElementById("task-table-" + complete).innerHTML = strTable;
}

function TaskCompleteConfirmation(taskId)
{
    $.get("TaskCompleteConfirmation?taskId=" + taskId, function (data) {
        if (data == "ok")
        {
            $("#task-is-complete-" + taskId).empty();
            document.getElementById("task-is-complete-" + taskId).innerHTML = "<span>已完成</span>";
            var date = new Date();
            var myTime = date.toLocaleDateString();
            window.location.reload();
        } else
        {
            alert("服务器忙！请稍后重试哟~");
        }
    });
}

