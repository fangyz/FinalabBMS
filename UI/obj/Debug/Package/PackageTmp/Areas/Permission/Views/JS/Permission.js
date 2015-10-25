//编辑方法
function Edit(stuNum) {

}

//----------------------------------------权限增删改查---------------------------------------------
var objId;//得到每次点击后的权限Id
var objtr;//存储点击每一行后的对象
var lasttr;//存储上一次每一行的对象
var count = 1;//设置未被点击背景颜色
function clickrow(obj) {
    objtr = obj;
    obj.style.backgroundColor = "#16CFF1";
    if (count != 1) {
        lasttr.style.backgroundColor = "rgb(237,241,249)";
    }
    count = count + 1;
    objId = obj.cells[0];
    lasttr = obj;
}
//编辑权限
function edit() {
    if (objtr == null) {
        alert("请您先选择权限！");
        return;
    }
    window.location = "/Permission/Permission/HandlePermission?operate=2&perid=" + objId.innerText;
}
//新增权限
function add() {
    window.location = "/Permission/Permission/HandlePermission?operate=1";
}
//删除权限
function del() {
    if (objtr == null) {
        alert("请您先选择权限！");
        return;
    }
    var perid = objId.innerText;
    if (confirm("权限涉及到具体的代码，删除后可能导致网站无法运行，请谨慎删除！")) {
        $.ajax({
            url: '/Permission/Permission/DelPer',
            type: 'post',
            data: { "perid": perid },
            success: function (data) {
                if (data.Statu == "ok") {
                    alert("删除成功");
                    window.location = "/Permission/Permission/PerIndex";
                }
                if (data.Statu == "err") {
                    alert("删除失败");
                }
            }
        });
    }
}
//查看权限详细信息
function show() {
    if (objtr == null) {
        alert("请您先选择权限！");
        return;
    }
    var perid = objId.innerText;
    window.location = "/Permission/Permission/HandlePermission?operate=3&perid=" + objId.innerText;
}


//分页有关方法--------------------------------------------------------------------------------------
//点击后跳转到第一页
function FirstPage() {
    document.getElementById("firstPage").href = "/Permission/Permission/PerIndex?page=1";
}
//点击后跳转到最后一页s
function LastPage() {
    var allRows = document.getElementById("allRows").value;
    var lastPage = Math.ceil(allRows / 10);
    document.getElementById("lastPage").href = "/Permission/Permission/PerIndex?page=" + lastPage;
}
//点击后跳转到上一页
function FrontPage() {
    var pageNow = document.getElementById("pageNow").textContent;
    var allRows = document.getElementById("allRows").value;
    if (pageNow > 1) {
        var page = Number(pageNow);
        page = page - 1;
        document.getElementById("frontPage").href = "/Permission/Permission/PerIndex?page=" + page;
    }
}
//点击后跳转到下一页
function NextPage() {
    var pageNow = document.getElementById("pageNow").textContent;
    var allRows = document.getElementById("allRows").value;
    var lastPage = Math.ceil(allRows / 10);
    if (pageNow < lastPage) {
        var page = Number(pageNow);
        page = page + 1;
        document.getElementById("nextPage").href = "/Permission/Permission/PerIndex?page=" + page;
    }
}

//删除方法
function Delete(stuNum) {
    if (confirm("确定要删除吗？")) {
        $.ajax({
            url: '/PersonalManger/CheckMember/DeleteBy',
            type: 'post',
            data: { "stuNum": stuNum },
            success: function (data) {
                var jsonObj = JSON.parse(data);
                if (jsonObj.Statu == "ok") {
                    alert(jsonObj.Msg);
                    $("#body").empty();
                    $.Init();
                } else {
                    alert(jsonObj.Msg);
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

$(function () {
    //关闭Jquery的浏览器缓存
    $.ajaxSetup({ cache: false });
})


//生成表格前预处理工作
function PreCreateTable(jsonObj) {
    var data = jsonObj.Data.data;//获得数据data对象
    rowSum = jsonObj.Data.TotalRecord;//总记录数
    pageSum = Math.floor((jsonObj.Data.TotalRecord / (pageSize + 1))) + 1;//总页数    
    rowCurrentSum = data.length;//当前请求得到的总记录数
    rowForm = (pageIndex - 1) * pageSize + 1;//开始的记录数
    rowTO = rowForm + rowCurrentSum - 1;//结束的记录数
    $(".lable-text-right").html("显示" + rowForm + "到" + rowTO + "，总计" + rowSum + "条记录");//从开始纪录到结束纪录显示
    $(".lable-page").html(pageIndex);//当前页显示
    $("#lable-page-sum").html("共" + pageSum + "页");//总页数显示
    $.creatTable(data);
}

$.extend({
    Init: function () {
        $.ajax({
            url: '/Permission/Permission/GetPageData',
            type: 'post',
            data: { "pageindex": pageIndex },
            success: function (json) {
                //转换json，很奇怪............
                //var jsonObj = JSON.parse(json);
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
        var $data = $("#input-text").val();
        /*有查询条件时请求的行为不同*/
        if ($data != "")
        {
            pageIndex = 1;
            Search($data);
        }
        if ($data == "") {
            pageIndex = 1;
            $.Init();
        }
    });

    //点击最后一页按钮触发事件
    $(".a-last").click(function () {
        $("#body").empty();
        var $data = $("#input-text").val();
        /*有查询条件时请求的行为不同*/
        if ($data != "") {
            pageIndex = pageSum;
            Search($data);
        }
        if ($data == "") {
            pageIndex = pageSum;
            $.Init();
        }

    });

    //点击向前翻页按钮触发事件
    $(".a-pre").click(function () {
        if (pageIndex == 1) { }//判断是否到了第一页
        else {
            $("#body").empty();
            var $data = $("#input-text").val();
            /*有查询条件时请求的行为不同*/
            if ($data != "") {
                pageIndex = pageIndex - 1;
                Search($data);
            }
            if ($data == "") {
                pageIndex = pageIndex - 1;
                $.Init();
            }
        }
    });
    //点击向后翻页按钮触发事件
    $(".a-next").click(function () {
        if (pageIndex == pageSum) { }//判断是否到了最后一页
        else {
            $("#body").empty();
            var $data = $("#input-text").val();
            /*有查询条件时请求的行为不同*/
            if ($data != "") {
                pageIndex = pageIndex + 1;
                Search($data);
            }
            if ($data == "") {
                pageIndex = pageIndex + 1;
                $.Init();
            }
        }
    });
    //点击刷新按钮触发事件
    $(".a-refresh").click(function () {
        $("#body").empty();
        var $data = $("#input-text").val();
        /*有查询条件时请求的行为不同*/
        if ($data != "") {
            Search($data);
        }
        $.Init();
    });
})

//生成表格
$.extend({
    creatTable: function (data) {
        var $body = $("#body");
        var IsEdit = $("#IsEdit").val();
        if (IsEdit == "True") {
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
                    "<td>" + data[i].Year + "</td>" + "<td><a href='/PersonalManger/CheckMember/PersonPage?StuNum=" + data[i].StuNum + "'class='a-operate' onclick='Edit(" + data[i].StuNum + ")'>查看</a> <a href='#' class='a-operate' id='a-operate-delete' onclick='Delete(" + data[i].StuNum + ")'>删除</a></td>");
                $tr.append($content);
                $body.append($tr);
            }
        } else {
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
                    "<td>" + data[i].Year + "</td>"+ "<td></td>");
                $tr.append($content);
                $body.append($tr);
            }
        }
    }
});

//进行查询dataBy:查询条件Obj);
function Search(dataBy) {
    $.ajax({
        url: '/PersonalManger/CheckMember/GetPageData',
        type: 'post',
        data: { "dataBy": dataBy, "pageindex": pageIndex },
        success: function (data) {
            var jsonObj = JSON.parse(data);
            if (jsonObj.Statu == "ok") {
                $("#body").empty();
                var pageIndex = 1;//初始化页数为1
                PreCreateTable(jsonObj)
            }
            if (jsonObj.Statu == "nologin") {
                alert(jsonObj.Msg);
                parent.location = jsonObj.BackUrl;

            }
            if (jsonObj.Statu == "nopermission") {
                alert("您没有权限访问此页面!");
                window.location = jsonObj.BackUrl;
            }
            if (jsonObj.Statu == "err") {
                alert("访问出错！");
            }
        }
    })
}

//为button绑定事件
$(function () {
    $(".ser-btn").click(function () {
        var $data = $("#input-text").val();
        Search($data);
    });

    //鼠标焦点移开查寻
    $("#input-text").blur(function () {
        var $data = $("#input-text").val();
        Search($data);
    });
})