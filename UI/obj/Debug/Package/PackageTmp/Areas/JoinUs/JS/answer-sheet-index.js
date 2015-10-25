/* 窗口载入绑定事件 */
$(function () {

    var allidArr = "";
    var Arr="";
    getPageDateDefaul();
   
   
        $("#prev").click(function () {
            prev();
        });
   
    $("#next").click(function () {     
        next();
    });

    $("#first").click(function () {
        first();       
    });

    $("#last").click(function () {
        last();
      
    });
    $(".am-pagination-select select").change(function () {
        pagerChanged();
    });
    //全选/反选
    $("#chk_all").click(function () {
        var flag = $(this).is(':checked');
       
        $("[name=chk_list]:checkbox").each(function () {
            if (flag) {
                $(this).attr("checked", true);
            } else {
                $(this).attr("checked", false);
            }
        });
      
    });
    //发布试卷
    
    $("#pub_interviewer_info").click(function () {
        var idArr = "";
        var flag = $("#chk_all").is(':checked');
        if (flag) {
            window.location = "/JoinUs/AnswerSheet/TotalToExcel";
        }
        else {
            $("[name=chk_list]:checkbox").each(function () {
                if ($(this).is(':checked')) {
                    idArr += $(this).val() + ",";
                    var idarr = ""
                    for (var i = 0; i < arr.length; i++) {
                        idarr = arr[i] + ",";
                    }
                    idarr = idarr + idArr;
                   
                }
            });
            if (idArr == "") {
                alert("你还没选呢！");
            } else {
                window.location = "/JoinUs/AnswerSheet/ToExcel?idArr=" + idArr;
            }
        }
    });
    $("#toword").click(function () {
        var idArr = "";
        var flag = $("#chk_all").is(':checked');
        if (flag) {
            window.location = "/JoinUs/AnswerSheet/TotalToWord";
        }
        else {
            $("[name=chk_list]:checkbox").each(function () {
                if ($(this).is(':checked')) {
                    idArr += $(this).val() + ",";
                }
            });
            if (idArr == "") {
                alert("你还没选呢！");
            } else {
                var idarr="";
                var pageIndexNow = $("#page_index_now").val();
                strarr = idArr.split(",");
               
                for (var i = 0; i < arr.length; i++)
                {
                    idarr = arr[i] + ",";
                }
                idarr = idarr + idArr;
                window.location = "/JoinUs/AnswerSheet/ExportWord?idArr=" + idarr + "&pageIndex=" + pageIndexNow;
                //$.getJSON("/JoinUs/AnswerSheet/Toword?idArr=" + idArr + "&pageIndex=" + pageIndexNow, function (jsonObj) {
                //    if (jsonObj.Statu == "ok") {

                //        //取出列表数据
                //        list = jsonObj.Data;
                //        alert(list);
                //        window.location = "/JoinUs/AnswerSheet/Index/" + pageIndexNow;

                //    }
                //    else {
                //        list = jsonObj.Data;
                //        alert(list);
                //        window.location = "/JoinUs/AnswerSheet/Index/" + pageIndexNow;
                //    }
                //});
            }
        }
    });

});

/* 获取分页数据（默认页容量为10） */
function getPageData(pageIndex) {
    var count = $("#count").val();
    $.getJSON("/JoinUs/AnswerSheet/GetAnswerSheetInfo", "pageIndex=" + pageIndex + "&pageSize=10&count="+count, function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            //隐藏域存储信息
            $("#page_index_now").val(pageIndex);
            $("#page_count").val(jsonObj.PageCount);
            //取出列表数据
            list = jsonObj.Data;
            //json数据放入表格
            jsonIntoTb(list);
            //更新分页
            updatePager(jsonObj.PageCount, pageIndex);
            //url同步改变
            history.replaceState(null, null, "/JoinUs/AnswerSheet/Index/" + pageIndex);
        }
    });
}
var Id;
/* 获取分页数据（初次加载） */
function getPageDateDefaul() {
    var pageIndex = $("#page_index_now").val();
    getPageData(pageIndex);
}

var arr = [];
/* json数据放入表格 */
function jsonIntoTb(list) {
    //清空table
    $("tbody").empty();
    var htmlStr;
   
   // var arr = new Array();
    //生成htmlStr
    var j = ($("#page_index_now").val() - 1) * 10 + 1;
    for (var i = 0; i < list.length; i++) {

        htmlStr += "<tr>";
        var isCheck = 0;
        for (var k = 0; k < arr.length; k++) {
            if (list[i].InterviewerInfo.ID == arr[k]) {
                isCheck = 1;
            }
        }
        if (isCheck == 1) {
            htmlStr += "<td><input type='checkbox' checked name='chk_list' value='" + list[i].InterviewerInfo.ID + "' /></td>";
        } else {
            htmlStr += "<td><input type='checkbox'  name='chk_list' value='" + list[i].InterviewerInfo.ID + "' /></td>";
        }

      
        htmlStr += "<td>" + j++ + "</td>";
        htmlStr += "<td>" + list[i].ID + "</td>";
        var info = list[i].InterviewerInfo;
        htmlStr += "<td><a href='#' onclick='getModelById(" + list[i].InterviewerInfo.ID + ")'>" + info.Name + "</a></td>";
        htmlStr += "<td>" + info.Major + info.Class + "</td>";
        htmlStr += "<td>" + list[i].ChoiceScore + "</td>";
        htmlStr += "<td>" + list[i].BriefScore + "</td>";
        htmlStr += "<td>" + list[i].TotalScore + "</td>";
        if (list[i].IsRated == false) {
            htmlStr += "<td><a href='/JoinUs/AnswerSheet/RaterPaper/" + list[i].ID + "'>评卷(" + list[i].Peoplecount + ")</a></td>";
            htmlStr += "<td>查看答卷&emsp;<a href=\"#\" onclick=\"del(" + list[i].ID + ")\">删除</a></td>";

        } else if (list[i].IsRated == true) {
            htmlStr += "<td><a style='color:red;'>已阅</a></td>";
            htmlStr += "<td><a href=\"/JoinUs/AnswerSheet/Lookpaper/" + list[i].ID + "\">查看答卷</a>&emsp;<a href=\"#\" onclick=\"del(" + list[i].ID + ")\">删除</a></td>";
        }

    }
    //如果不够10条数据，则生成空行
    if (list.length < 10)
        for (var i = 0; i < 10 - list.length; i++) {
            htmlStr += "<tr><td>-</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        }
    //htmlStr添加至tbody
    $("#tbody_list").append(htmlStr);
}

/* 更新分页 */
function updatePager(pageCount, pageIndex) {
    //清空分页
    $(".am-pagination-select select").empty();
    //重新生成分页
    var htmlStr;
    for (var i = 1; i <= pageCount; i++) {
        if (pageIndex == i)
            //设置当前页为“选中”状态
            htmlStr += "<option value=\"" + i + "\" selected=\"selected\">" + i + " / " + pageCount + "</option>";
        else
            htmlStr += "<option value=\"" + i + "\">" + i + " / " + pageCount + "</option>";
    }
    //添加到分页
    $(".am-pagination-select select").append(htmlStr);
}

/* 分页改变事件 */
function pagerChanged(obj) {
    var pageIndex = $(".am-pagination-select select").find("option:selected").val();
    getPageData(pageIndex);
}

/* 上一页 */
function prev() {
    var table = document.getElementById("tbody_list").childNodes;
    for (var i = 0; i < 10; i++) {
        var checkEle = table[i].childNodes[0].firstChild;
        if (checkEle.checked == true) {
            var len = arr.length;
            //arr[len] = checkEle.value;
            for (var j = 0; j <=arr.length; j++) {
                if (arr[j] != checkEle.value) {
                    arr[len] = checkEle.value;
                }
            }
        }
        if (checkEle.checked == false) {

            for (var k = 0; k <= arr.length; k++) {
                if (arr[k] == checkEle.value) {
                    // arr[i] = arr[i].replace(checkEle.value,"");
                    //alert(arr[k]);
                    arr.splice((arr.indexOf(arr[k])), 1);
                }
            }
        }
    }

    
    var pageIndexNow = $("#page_index_now").val();
    var pageIndexPrev = parseInt(pageIndexNow) - 1;
    if (pageIndexPrev > 0) {
        getPageData(pageIndexPrev);
       
    }
   
   
}

/* 下一页 */
function next() {
   
    var table = document.getElementById("tbody_list").childNodes;
    for (var i = 0; i < 10; i++) {
        var checkEle = table[i].childNodes[0].firstChild;
        
      
        if (checkEle.checked == true) {
            
           var len = arr.length;
            //arr[len] = checkEle.value;
            //第一个值没有比较的对象，报错，卡死。。
            for (var j = 0; j <=arr.length; j++) {
                if (arr[j] != checkEle.value) {
                    arr[len] = checkEle.value;
                  
                }
            }

        } 
        if (checkEle.checked == false) {
            
            for (var k = 0; k <=arr.length; k++)
            {
                if (arr[k] == checkEle.value)
                {
                    // arr[i] = arr[i].replace(checkEle.value,"");
                    //alert(arr[k]);
                    arr.splice((arr.indexOf(arr[k])), 1);
                }
            }
        }
    }

    var pageIndexNow = $("#page_index_now").val();
    var pageIndexNext = parseInt(pageIndexNow) + 1;
    var pageCount = $("#page_count").val();
    if (pageIndexNext <= pageCount) {
        getPageData(pageIndexNext);
    }
}

/* 首页 */
function first() {
    getPageData(1);
}

/* 尾页 */
function last() {
    var pageCount = $("#page_count").val();
    getPageData(pageCount);
}

/* 根据id查找对应实体模型 */
function getModelById(id) {
    $.getJSON("/JoinUs/AnswerSheet/GetModelById", "id=" + id, function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            var model = jsonObj.Data;
            //将查询到的对象放入模态窗体
            modelIntoModal(model);
            //打开模态窗体（先填充后打开，位置不会乱）
            $("#my-modal").modal();
        }
    });
}

/* 将查询到的对象放入模态窗体 */
function modelIntoModal(model) {
    //清空内容
    $("#details_thead").empty();
    //定义htmlStr
    var htmlStr;
    //组织个人资料
    //学号
    htmlStr += "<tr><td>";
    htmlStr += "学号：" + model.Num;
    htmlStr += "</td></tr>";
    //姓名
    htmlStr += "<tr><td>";
    htmlStr += "姓名：" + model.Name;
    htmlStr += "</td></tr>";
    //性别
    htmlStr += "<tr><td>";
    htmlStr += "性别：" + model.Gender;
    htmlStr += "</td></tr>";
    //学院
    htmlStr += "<tr><td>";
    htmlStr += "学院：" + model.Academy;
    htmlStr += "</td></tr>";
    //专业
    htmlStr += "<tr><td>";
    htmlStr += "专业：" + model.Major;
    htmlStr += "</td></tr>";
    //班级
    htmlStr += "<tr><td>";
    htmlStr += "班级：" + model.Class;
    htmlStr += "</td></tr>";
    //电话
    htmlStr += "<tr><td>";
    htmlStr += "电话：" + model.TelNum;
    htmlStr += "</td></tr>";
    //QQ
    htmlStr += "<tr><td>";
    htmlStr += "QQ：" + model.QQ;
    htmlStr += "</td></tr>";
    //E-mail
    htmlStr += "<tr><td>";
    htmlStr += "E-mail：" + model.Email;
    htmlStr += "</td></tr>";
    //学习经历
    htmlStr += "<tr><td>";
    htmlStr += "学习经历：" + model.LearningExperience;
    htmlStr += "</td></tr>";
    //自我评价
    htmlStr += "<tr><td>";
    htmlStr += "自我评价：" + model.SelfEvaluation;
    htmlStr += "</td></tr>";

    //内容填充进模态窗体
    $("#details_thead").append(htmlStr);
}

/* 删除 */
function del(id) {
    $('#my-confirm').modal({
        relatedTarget: this,
        onConfirm: function (options) {
            var pageIndexNow = $("#page_index_now").val();
            window.location = "/JoinUs/AnswerSheet/Del?id=" + id + "&pageIndexNow=" + pageIndexNow;
        }
    });
}


function getRank() {
    $.getJSON("/JoinUs/AnswerSheet/GetInfoRank", function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            var list = jsonObj.Data;
            //将查询到的对象放入模态窗体
            getScoreRank(list);
            //打开模态窗体（先填充后打开，位置不会乱）
            $("#my-score").modal();
        }
    });
}
function getScoreRank(list) {
    $("#score").empty();
    var htmlStr;
    //生成htmlStr
    htmlStr += "<tr><td>名次</td><td>试卷号</td><td>姓名</td><td></td>总分</tr>"
    var j = 0;
    for (var i = 0; i < list.length; i++) {

        htmlStr += "<tr>";

        htmlStr += "<td>" + j++ + "</td>";
        htmlStr += "<td>" + list[i].ID + "</td>";
        var info = list[i].InterviewerInfo;
        htmlStr += "<td>" + info.Name + "</td>";
        htmlStr += "<td>" + list[i].TotalScore + "</td>";
    }
    $("#score").append(htmlStr);
}