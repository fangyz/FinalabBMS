/* 窗口载入绑定事件 */
$(function () {
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
});

/* 获取分页数据（默认页容量为10） */
function getPageData(pageIndex) {
    $.getJSON("/JoinUs/QuestionManage/GetPageData", "pageIndex=" + pageIndex + "&pageSize=10", function (jsonObj) {
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
            history.replaceState(null, null, "/JoinUs/QuestionManage/Index/" + pageIndex);
        }
    });
}

/* 获取分页数据（初次加载） */
function getPageDateDefaul() {
    var pageIndex = $("#page_index_now").val();
    getPageData(pageIndex);
}

/* json数据放入表格 */
function jsonIntoTb(list) {
    //清空table
    $("tbody").empty();
    var htmlStr;
    //生成htmlStr
    for (var i = 0; i < list.length; i++) {
        htmlStr += "<tr>";
        htmlStr += "<td>" + list[i].ID + "</td>";
        htmlStr += "<td>" + list[i].QuestionContent + "</td>";
        //判断题目类型
        if (list[i].QuestionTypeID == 1)
            htmlStr += "<td>选择题</td>";
        if (list[i].QuestionTypeID == 2)
            htmlStr += "<td>简答题</td>";
        htmlStr += "<td>";
        htmlStr += "<a href=\"#\" onclick=\"getModelById(" + list[i].ID + ")\">查看</a> ";
        htmlStr += "<a href=\"/JoinUs/QuestionManage/Modify/" + list[i].ID + "\">修改</a> ";
        htmlStr += "<a href=\"#\" onclick=\"del(" + list[i].ID + ")\">删除</a>";
        htmlStr += "</td>";
        htmlStr += "</tr>";
    }
    //如果不够10条数据，则生成空行
    if (list.length < 10)
        for (var i = 0; i < 10 - list.length; i++) {
            htmlStr += "<tr><td>-</td><td></td><td></td><td></td></tr>";
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
    var pageIndexNow = $("#page_index_now").val();
    var pageIndexPrev = parseInt(pageIndexNow) - 1;
    if (pageIndexPrev > 0) {
        getPageData(pageIndexPrev);
    }
}

/* 下一页 */
function next() {
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

/* 删除 */
function del(id) {
    $('#my-confirm').modal({
        relatedTarget: this,
        onConfirm: function (options) {
            var pageIndexNow = $("#page_index_now").val();
            window.location = "/JoinUs/QuestionManage/Del?id=" + id + "&pageIndexNow=" + pageIndexNow;
        }
      
    });
   
}

/* 根据id查找对应实体模型 */
function getModelById(id) {
    $.getJSON("/JoinUs/QuestionManage/GetModelById", "id=" + id, function (jsonObj) {
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
    //简答题
    if (model.QuestionTypeID == 2) {
        htmlStr += "<tr><td>";
        htmlStr += "题目：" + model.QuestionContent;
        htmlStr += "</td></tr>";
        htmlStr += "<tr><td>";
        htmlStr += "分数：" + model.QuestionGrade;
        htmlStr += "</td></tr>";
        htmlStr += "<tr><td>";
        htmlStr += "标签：" + model.QuestionTag;
        htmlStr += "</td></tr>";
    }
    if (model.QuestionTypeID == 1) {
        //题目内容
        htmlStr += "<tr><td>";
        htmlStr += "题目：" + model.QuestionContent;
        htmlStr += "</td></tr>";
        //题目选项
        var options = model.T_QuestionOption;
        for (var i = 0; i < options.length; i++) {
            htmlStr += "<tr><td>";
            htmlStr += "【" + options[i].OptionWeight + "】";
            htmlStr += options[i].OptionID + ". ";
            htmlStr += options[i].OptionContent;
            htmlStr += "</td></tr>";
        }
        htmlStr += "<tr><td>";
        htmlStr += "分数：" + model.QuestionGrade;
        htmlStr += "</td></tr>";
        htmlStr += "<tr><td>";
        htmlStr += "标签：" + model.QuestionTag;
        htmlStr += "</td></tr>";
    }
    //内容填充进模态窗体
    $("#details_thead").append(htmlStr);
}