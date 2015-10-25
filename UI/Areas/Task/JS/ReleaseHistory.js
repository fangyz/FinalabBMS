//重写table
function ReloadTable(pageIndex) {
    $.get("/Task/Task/GetReleaseHistoryJsonData?strPageIndex=" + pageIndex + "&strPageSize=10", function (data, status) {
        //alert(data);
        MakeTable(eval(data));
    });
}

//绘制table
function MakeTable(jsonObject) {

    var $table = $("#task-table");
    
    var strTable = '';

    strTable += '<tr>';
    strTable += '<th>任务名称</th>';
    strTable += '<th>任务类型</th>';
    strTable += '<th>发布时间</th>';
    strTable += '<th>截止时间</th>';
    strTable += '<th>评价任务</th>';
    strTable += '<th>修改任务</th>';
    strTable += '<th>删除任务</th>';
    strTable += '</tr>';

    for (var j = 0; j < 10; j++) {
        if (jsonObject.TaskList.length <= j) {
            strTable += '<tr>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '<td></td>';
            strTable += '</tr>';
        } else {
            strTable += '<tr>';
            strTable += '<td><a title ="' + jsonObject.TaskList[j].TaskName + '" href="/Task/Task/TaskDetail?taskId=' + jsonObject.TaskList[j].TaskId + '">' + jsonObject.TaskList[j].TaskName.substr(0,10) + '</a></td>';
            strTable += '<td>' + jsonObject.TaskList[j].TaskTypeName + '</td>';
            strTable += '<td>' + jsonObject.TaskList[j].TaskBegTime + '</td>';
            strTable += '<td>' + jsonObject.TaskList[j].TaskEndTime + '</td>';
            strTable += '<td><a title ="评价任务" href="/Task/Task/TaskEvaluate?taskId=' + jsonObject.TaskList[j].TaskId + '">评价</a></td>';
            if (jsonObject.CanBeModify[j])
            {
                strTable += '<td><a title ="修改任务" href="/Task/Task/ModifyTask?taskId='+jsonObject.TaskList[j].TaskId+'">修改</a></td>';
                strTable += '<td><a onclick="DeleteTask(' + jsonObject.TaskList[j].TaskId + ')" title ="删除任务" href="javascript:void(0)">删除</a></td>';
            } else
            {
                strTable += '<td title="任务已接收无法修改">无法修改</td>';
                strTable += '<td title="任务已接收不可删除">不可删除</td>';
            }
            
            strTable += '</tr>';
        }
    }
    //alert(strTable);
    $table.empty();
    document.getElementById("task-table").innerHTML = strTable;
}

function DeleteTask(taskId)
{
    if (confirm("您确认删除该任务吗？")) {
        $.get("DeleteTask?taskId=" + taskId, function (data) {
            if (data == "ok") {
                sltPageBar = document.getElementById("slt-page-bar");
                $taskCount = $("#span-task-count");
                taskCount = $taskCount.html();
                $taskCount.empty();
                document.getElementById("span-task-count").innerHTML = parseInt(taskCount) - 1;
                ReloadTable(sltPageBar.options[sltPageBar.selectedIndex].value);
            } else {
                alert("服务器忙！请稍后重试");
            }
        });
    } 
}