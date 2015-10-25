$(function () {
});

//编辑框show&&hide
function ResponseShowAndhide(obj)
{
    $input = $(obj).parent().parent().next();

    if ($input.css("height") == "0px")
    {
        setTimeout(function () { $input.css("borderTop", "1px solid #A6C7CB"); }, 0);
        setTimeout(function () { $(obj).empty(); obj.innerHTML = "收起评语";}, 100);
        $input.animate({ "height": 34 }, 300);
    } else
    {        
        $input.animate({ "height": 0 }, 300);
        setTimeout(function () { $(obj).empty(); obj.innerHTML = "编辑评语"; }, 100);
        setTimeout(function () { $input.css("borderTop", "0px"); }, 300);
    }
}
//改变input框disabled属性
function ResponseCanbeEdit(li)
{
    $input = $(li).children("input");
    $input.attr("disabled", false);
    $input.focus();
}
//input框Blur事件
function InputBlur(input)
{
    $input = $(input);
    $input.attr("disabled","disabled");
}
//sltOnclick事件
function SltTaskGradeOnclick(stuNum,taskId)
{    
    $("#" + stuNum + "-slt-task-grade-" + taskId + " option[value='未评价']").remove();
}

//SltTaskGradOnchange
function SltTaskGradOnchange(stuNum, taskId)
{
    grade = $("#" + stuNum + "-slt-task-grade-" + taskId + " option:selected").val();

    $.post("/Task/Task/SaveTaskGrade", {
        TaskGrade: grade,
        StuNum: stuNum,
        TaskId:taskId
    }, function (data) {
        if (data == "ok") {
            alert("任务评价已保存！");
        } else
        {
            alert("服务器忙！请刷新后重试！");
        }
    });

}

//保存任务评语
function TxtOnchange(txt, stuNum, taskId)
{
    $txt = $(txt);

    $.post("/Task/Task/SaveTaskResponse", {
        TaskResponse: $txt.val(),
        StuNum: stuNum,
        TaskId: taskId
    }, function (data) {
        if (data == "ok") {
            alert("任务评语已保存！");
        } else {
            alert("服务器忙！请刷新后重试！");
        }
    });
}

function GetPagedTaskEvaluateList(pageIndex,taskId)
{
    pageSize = 10;
    $.get("/Task/Task/GetPagedTaskEvaluateList?pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&taskId=" + taskId, function (data) {
        ReloadTable(eval(data));
    });
}

function ReloadTable(listTaskEvaluate)
{
    strHTML = '';
    strHTML += '<ul class="content_panel_ul title_ul">';
    strHTML += '<li>任务名称</li>';
    strHTML += '<li>任务类型</li>';
    strHTML += '<li>发布人</li>';           
    strHTML += '<li>完成时间</li>';
    strHTML += '<li>接收人</li>';
    strHTML += '<li>任务评分</li>';
    strHTML += '<li>任务评阅</li>';
    strHTML += '</ul>';

    for (var i = 0; i < 10; i++) {
        if (listTaskEvaluate.length <= i) {
            strHTML += '<ul class="content_panel_ul coutent_ul">';
            strHTML += '<li></li>';
            strHTML += '<li></li>';
            strHTML += '<li></li>';
            strHTML += '<li></li>';
            strHTML += '<li></li>';
            strHTML += '<li></li>';
            strHTML += '</ul>';
        } else {
            strHTML += '<ul class="content_panel_ul coutent_ul">';
            strHTML += '<li>';
            strHTML += '<a title="查看任务详情" href="/Task/Task/TaskDetail?taskId=' + listTaskEvaluate[i].TaskId + '">';
            strHTML += listTaskEvaluate[i].TaskName;
            strHTML += '</a></li>';
            strHTML += '<li>' + listTaskEvaluate[i].TaskTypeName + '</li>';
            strHTML += '<li>' + listTaskEvaluate[i].TaskSenderName + '</li>';
            if (listTaskEvaluate[i].TaskFinishTime == null) {
                strHTML += '<li>' + "未完成" + '</li>';
            }
            else {
                var date = listTaskEvaluate[i].TaskFinishTime;
                var year = date.getFullYear();
                var month = date.getMonth() + 1;    //js从0开始取 
                var date1 = date.getDate();
                strHTML += '<li>' + year + "年" + month + "月" + date1 + "日" + '</li>';
            }
            strHTML += '<li>' + listTaskEvaluate[i].TaskReceiverName + '</li>';
            strHTML += '<li>';
            //strHTML += '<select onchange="SltTaskGradOnchange(' + listTaskEvaluate[i].TaskReceiver + ', ' + listTaskEvaluate[i].TaskId + ')" onmousedown="SltTaskGradeOnclick(' + listTaskEvaluate[i].TaskReceiver + ',' + listTaskEvaluate[i].TaskId + ')" id="' + listTaskEvaluate[i].TaskReceiver + '-slt-task-grade-' + listTaskEvaluate[i].TaskId + '" class="slt_task_grade">';

            if (listTaskEvaluate[i].TaskGrade == 0 && listTaskEvaluate[i].TaskResponse == "") {
                strHTML += '待评价';
            }
            else {
                strHTML += listTaskEvaluate[i].TaskGrade;
            }
            strHTML += '</li>';
            strHTML += '<li >';
            if (listTaskEvaluate[i].TaskGrade == 0 && listTaskEvaluate[i].TaskResponse == "") {
                strHTML += '<a href="TaskCorrect?taskId=' + (listTaskEvaluate[i].TaskId) + '&taskReciver=' + (listTaskEvaluate[i].TaskReceiver) + '" title="评阅任务">评阅</a>';
            }
            else
            {
                strHTML += '<a href="MyTaskDetail?taskId=' + (listTaskEvaluate[i].TaskId) + '&taskReciver=' + (listTaskEvaluate[i].TaskReceiver) + '" title="查看任务">查看</a>';
            }
            strHTML += '</li>';
            strHTML += '</ul>';
        }
    }
    $("#content-panel").empty();
    document.getElementById("content-panel").innerHTML = strHTML;
}