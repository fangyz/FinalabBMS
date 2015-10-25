$(function () {

    $("#slt-task-type").val($("#slt-task-type").val());
    TaskTypeOnChange($("#slt-task-type").val());

    $txtTaskName = $("#txt-task-name");
    $txtTaskBegTime = $("#txt-task-beg-time");
    $txtTaskEndTime = $("#txt-task-end-time");
    $txtTaskContent = $("#txt-task-content");
    $txtTaskName.click(function () {
        $txtTaskName.parent('.text_border').css("border-color", "#AAAAAA");
    });
    $txtTaskName.keydown(function () {
        $txtTaskName.parent('.text_border').css("border-color", "#AAAAAA");
    });

    $txtTaskBegTime.click(function () {
        $txtTaskBegTime.parent('.text_border').css("border-color", "#AAAAAA");
    });
    $txtTaskBegTime.keydown(function () {
        $txtTaskBegTime.parent('.text_border').css("border-color", "#AAAAAA");
    });

    $txtTaskEndTime.click(function () {
        $txtTaskEndTime.parent('.text_border').css("border-color", "#AAAAAA");
    });
    $txtTaskEndTime.keydown(function () {
        $txtTaskEndTime.parent('.text_border').css("border-color", "#AAAAAA");
    });

    $txtTaskContent.click(function () {
        $txtTaskContent.parent('.text_border').css("border-color", "#AAAAAA");
    });
    $txtTaskContent.keydown(function () {
        $txtTaskContent.parent('.text_border').css("border-color", "#AAAAAA");
    });
});


//改变slt-members成员选择下拉框重新加载成员列表
function ChangeSltMembers(value)
{
    var sltMembers = document.getElementById('slt-members');
    for (var i = 0; i < sltMembers.options.length; i++) {
        if (sltMembers.options[i].value == value) {
            sltMembers.options[i].selected = true;
            LoadMembers(value);
        }
    }
}

//重绘成员列表
function MakeMembersTable(jsonData)
{
    $tb = $("#tb-members");
    
    var strTable = '<table id="tb-members" class="members_table">';                        
    strTable += '<tr><th class="check_td"><input onclick="CheckAll(this)" class="check_box" type="checkbox" name="checkAllBox"/></th>';
    strTable += '<th class="name_td">姓&nbsp&nbsp名</th>';
    strTable += '<th class="num_td">学&nbsp&nbsp号</th>';
    strTable += '</tr>';

    for (var i = 0; i < jsonData.length; i++)
    {
        strTable += '<tr onclick ="MembersTableTrOnclick('+jsonData[i].StuNum+')">';
        strTable += '<td>';
        strTable += '<input onclick="MembersTableTrOnclick(' + jsonData[i].StuNum + ')"  id="check-box-' + jsonData[i].StuNum + '" value="' + jsonData[i].StuNum + '" class="check_box" type="checkbox" name="checkbox"/>';
        strTable += '</td>';
        strTable += '<td>' + jsonData[i].StuName + '</td>';
        strTable += '<td>' + jsonData[i].StuNum + '</td>';
        strTable += '</tr>';
    }

    strTable += '</table>'

    document.getElementById("tb-members").innerHTML= strTable;

    ///任务修改视图函数
    //CheckMembers(members);
}

//请求成员信息
function LoadMembers(membersType)
{
    id = $("#slt-members-option-" + membersType).val();
    $.get("/Task/Task/GetMembersJsonData?membersType=" + membersType + "&id=" +id , function (data) {
        MakeMembersTable(eval(data));
    });
}

//成员列表tr点击事件
function MembersTableTrOnclick(stuNum)
{
    checkBox = document.getElementById("check-box-" + stuNum);
    if (checkBox.checked == true)
    {        
        checkBox.checked = false;
    } else
    {
        checkBox.checked = true;
    }
}

//任务类型改变 加载相应的成员信息
function TaskTypeOnChange(taskTypeId)
{
    //获取成员选择下拉框
    var sltMembers = document.getElementById('slt-members');

    //如果是学习任务、开发任务，项目任务、则不可编辑
    switch (taskTypeId) {
        case '10001':
            ChangeSltMembers(10002);
            sltMembers.disabled = true;
            HideTxtProj();
            break;
        case '10002':
            ChangeSltMembers(10003);
            sltMembers.disabled = true;
            HideTxtProj();
            break;
        case '10003':
            ChangeSltMembers(10008)
            sltMembers.disabled = true;
            ShowTxtProj();
            break;
        case '10004':
            ChangeSltMembers(10001);
            sltMembers.disabled = false;
            HideTxtProj();
            break;
    }
}

//项目改变  加载相应的项目组成员
function OnchangeSltProj(projId)
{
    $("#slt-members-option-10008").val(projId);
    ChangeSltMembers(10008);
}

//隐藏项目信息
function HideTxtProj()
{
    $("#group-proj").hide();
}
//显示项目信息
function ShowTxtProj()
{
    $("#group-proj").show();
}

//选中所有CheckBox
function CheckAll(obj)
{
    var x = document.getElementsByName("checkbox");

    if (obj.checked == true) {
        for (var i = 0; i < x.length; i++) {
            x[i].checked = true;
        }
    } else {
        for (var i = 0; i < x.length; i++) {
            x[i].checked = false;
        }
    }
}

function ReleaseContent(obj)
{
    $txtTaskName = $("#txt-task-name");
    $txtTaskBegTime = $("#txt-task-beg-time");
    $txtTaskEndTime = $("#txt-task-end-time");
    $txtFileName = $("#txt-file-name");
    //$txtTaskContent = $("#txt-task-content");
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
    } else if (obj == "") {
        //SetBorderColor($txtTaskContent.parent('.text_border'), "red");
        //$txtTaskContent.focus();
    } else if (!SelectedMember()) {
        alert("请选择任务接收人！");
    } else {
        $.post("/Task/Task/SaveTaskInfo", {
            TaskName: $txtTaskName.val(),
            TaskBegTime: $txtTaskBegTime.val(),
            TaskEndTime: $txtTaskEndTime.val(),
            TaskContent: obj,
            TaskFileName:$txtFileName.val(),
            TaskTypeId: TaskType.options[TaskType.selectedIndex].value,
            ProjId: Proj.options[Proj.selectedIndex].value,
            ProjPhasesId: ProjPhases.options[ProjPhases.selectedIndex].value,
            Members: GetMemers()
        }, function (data) {
            
            if (data == "nook")
            {
                alert("服务器忙！请稍后重试！");
            } else if (data == "ok") {
                alert("发布成功！");
                location.href = "/Task/Task/ReleaseHistory";
            }
        });

    }

}

function SetBorderColor($obj, color) {
    $obj.css("border-color", color);
}

function GetMemers()
{
    var x = document.getElementsByName("checkbox");

    strStuNum = x[0].value;

    for (var i = 1; i < x.length; i++) {
        strStuNum += "," + x[i].value;
    }
    return strStuNum;
}

function SelectedMember() {
    var x = document.getElementsByName("checkbox");

    for (var i = 0; i < x.length; i++) {
        if (x[i].checked == true) {
            return true
        }
    }
    return false;
}