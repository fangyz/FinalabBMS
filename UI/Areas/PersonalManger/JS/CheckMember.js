//删除方法
function Delete(stuNum) {
    if (confirm("确定要删除吗？")) {
        $.ajax({
            url: '/PersonalManger/CheckMember/DeleteBy',
            type: 'post',
            data: { "stuNum": stuNum },
            success: function (data) {
                if (data.Statu == "ok") {
                    alert(data.Msg);
                    $("#body").empty();
                    $.Init();
                } else {
                    alert(data.Msg);
                }
            }
        })
    }
    else { }
}


/*分页定义的变量*/
var json = "";//json数据
var pageSize = 10;//页的容量
var pageIndex = 1;//记录翻到了第几页
var rowSum;//记录总行数
var pageSum;//记录总页数
var rowCurrentSum;//当前请求得到的总记录数
var rowForm;//开始的记录数
var rowTO;//结束的记录数
var searchData;//存储查询的条件

var IsNewSearch=1;//得到是否是一个新的查询，是新查询就为1，否则为2

$(function () {
    //关闭Jquery的浏览器缓存
    $.ajaxSetup({ cache: false });
})


//生成表格前预处理工作
function PreCreateTable(jsonObj) {
    var data = jsonObj.Data.data;//获得数据data对象
    rowSum = jsonObj.Data.TotalRecord;//总记录数
    pageSum = Math.ceil((jsonObj.Data.TotalRecord / (pageSize)));//总页数    
    rowCurrentSum = data.length;//当前请求得到的总记录数
    rowForm = (pageIndex - 1) * pageSize + 1;//开始的记录数
    rowTO = rowForm + rowCurrentSum - 1;//结束的记录数
    $(".lable-text-right").html("显示" + rowForm + "到" + rowTO + "，总计" + rowSum + "条记录");//从开始纪录到结束纪录显示
    $("#lable-page").html("第" + pageIndex + "页");//当前页显示
    $("#lable-page-sum").html("共" + pageSum + "页");//总页数显示
    $.creatTable(data);
}

$.extend({
    Init: function () {
        $.ajax({
            url: '/PersonalManger/CheckMember/GetPageData',
            type: 'post',
            data: { "pageindex": pageIndex },
            success: function (json) {
                if (json.Statu == "ok") {
                    PreCreateTable(json);
                }
                if (json.Statu == "nologin") {
                    alert("您还没有登陆，请登录！");
                    parent.location = jsonObj.BackUrl;

                }
                if (json.Statu == "nopermission") {
                    alert(json.Msg);
                    window.location = jsonObj.BackUrl;
                }
                if (json.Statu == "err") {
                    alert(json.Msg);
                }
            }
        })
    }
})

//第一次请求初始化
$(function () {
    //清空表格
    $.Init();
})

//各种点击事件
$(function () {
    //点击第一页按钮触发事件
    $(".a-first").click(function () {
        $("#body").empty();
        pageIndex = 1;
        btnSearch(2);
    });

    //点击最后一页按钮触发事件
    $(".a-last").click(function () {
        $("#body").empty();
        pageIndex = pageSum;
        btnSearch(2);
    });

    //点击向前翻页按钮触发事件
    $(".a-pre").click(function () {
        if (pageIndex == 1) { }//判断是否到了第一页
        else {
            $("#body").empty();
            pageIndex = pageIndex - 1;
            btnSearch(2);
        }
    });
    //点击向后翻页按钮触发事件
    $(".a-next").click(function () {
        if (pageIndex == pageSum) { }//判断是否到了最后一页
        else {
            $("#body").empty();
            pageIndex = pageIndex + 1;
            btnSearch(2);
        }
    });
    //点击刷新按钮触发事件
    $(".a-refresh").click(function () {
        $("#body").empty();
        pageIndex = 1;
        $.Init();
    });
})

//生成表格
$.extend({
    creatTable: function (data) {
        var backurl = document.getElementById("backurl").value;
        var $body = $("#body");
        var IsEdit = $("#IsEdit").val();
        if (IsEdit == "True") {//可以修改又分为系统管理员和普通管理员 为了设置返回按钮
            for (var i = 0; i < data.length; i++) {
                var $tr = $("<tr class='tbody-tr'></tr>");
                /*生成第一列*/
                var $td = $("<td id='body-td-first'></td>");
                var $div = $("<div class='head-table-img'></div>");
                $td.append($div);
                $tr.append($td);
                if (backurl == "fromindex") {
                    var $content = $("<td><a href='" + "/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum +"&backurl=fromindex"+"' class='a-stum'>" + data[i].StuNum + "</a></td>" + "<td>" + data[i].StuName + "</td>" +
                   "<td>" + data[i].TelephoneNumber + "</td>" + "<td>" + data[i].Major + "</td>" + "<td>" + data[i].Department + "</td>" +
                   "<td>" + data[i].Year + "</td>" + "<td id='rightTd'><a href='/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "&backurl=fromindex" + "'class='a-operate'>查看</a> <a href='#' class='a-operate' id='a-operate-delete' onclick='Delete(" + data[i].StuNum + ")'>删除</a></td>");
                }
                if (backurl == "fromrole") {
                    var $content = $("<td><a href='" + "/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "&backurl=fromrole" + "' class='a-stum'>" + data[i].StuNum + "</a></td>" + "<td>" + data[i].StuName + "</td>" +
                   "<td>" + data[i].TelephoneNumber + "</td>" + "<td>" + data[i].Major + "</td>" + "<td>" + data[i].Department + "</td>" +
                   "<td>" + data[i].Year + "</td>" + "<td id='rightTd'><a href='/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "&backurl=fromrole" + "'class='a-operate'>查看</a> <a href='#' class='a-operate' id='a-operate-delete' onclick='Delete(" + data[i].StuNum + ")'>删除</a></td>");
                }
                /*生成其他七列*/
                $tr.append($content);
                $body.append($tr);
            }
        } else {//不能修改
            for (var i = 0; i < data.length; i++) {
                var $tr = $("<tr class='tbody-tr'></tr>");
                /*生成第一列*/
                var $td = $("<td id='body-td-first'></td>");
                var $div = $("<div class='head-table-img'></div>");
                $td.append($div);
                $tr.append($td);
                /*生成其他七列*/
                var $content = $("<td><a href='" + "/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "' class='a-stum'>" + data[i].StuNum + "</a></td>" + "<td>" + data[i].StuName + "</td>" +
                    "<td>" + data[i].TelephoneNumber + "</td>" + "<td>" + data[i].Major + "</td>" + "<td>" + data[i].Department + "</td>" +
                    "<td>" + data[i].Year + "</td>" + "<td id='rightTd'><a href='/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "'class='a-operate' onclick='Edit(" + data[i].StuNum + ")'>查看</a></td>");
                $tr.append($content);
                $body.append($tr);
            }
        }
    }
});
//点击搜索时
function btnSearch(IsNewSearch) {
    var obj1 = document.getElementById("department");
    var index1 = obj1.selectedIndex;
    var valu1 = obj1[index1].value;
    var obj = document.getElementById("grade");
    var index = obj.selectedIndex;
    var valu = obj.options[index].value;
    var txt = document.getElementById("txt").value;
    //还要判断输入格式，学号或姓名那块，输入字符个数不能超过12位
    var reg = /^\s*[\s\S]{1,12}\s*$/;
    if (txt != "") {
        if (!reg.test(txt)) {
            alert("您输入学号或姓名格式错误，请重新输入！");
            document.getElementById("txt").value = "";
            document.getElementById("txt").focus();
            return;
        }
    }
    searchData = valu1 + "," + valu + "," + txt;
    if (IsNewSearch == 1) {
        pageIndex = 1;
    }
    Search(searchData);
}

document.onkeydown=function(){
    if(event.keyCode==13){
        btnSearch(1);
    }
}

//进行查询dataBy:查询条件Obj;
function Search(searchData) {
    $.ajax({
        url: '/PersonalManger/CheckMember/GetPageData',
        type: 'post',
        data: { "dataBy": searchData, "pageindex": pageIndex },
        success: function (data) {
            if (data.Statu == "ok") {
                $("#body").empty();
                PreCreateTable(data)
            }
            if (data.Statu == "nologin") {
                alert(jsonObj.Msg);
                parent.location = jsonObj.BackUrl;
            }
            if (data.Statu == "nopermission") {
                alert("您没有权限访问此页面!");
                window.location = jsonObj.BackUrl;
            }
            if (data.Statu == "err") {
                alert("访问出错！");
            }
        }
    })
}

