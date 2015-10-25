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
    var TypeId = document.getElementById('question_index_now').value;
    var Content = document.getElementById('question_content_now').value
    $.getJSON("/JoinUs/OrganizePaper/GetPageDatatype?typeId=" + TypeId + "&content=" + Content + "&pageIndex=" + pageIndex + "&pageSize=10" + "&paperId=" + $("#paperId").val(), function (jsonObj) {
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
            history.replaceState(null, null, "/JoinUs/OrganizePaper/Index?paperId=" + $("#paperId").val() + "&pageIndex=" + pageIndex);
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
        //判断是否存在于该试卷，是的话则添加标价√
        if (list[i].IsAdded == true) {
            htmlStr += "<td>" + list[i].ID + " √</td>";
        }
        else {
            htmlStr += "<td>" + list[i].ID + "</td>";
        }
        htmlStr += "<td>" + list[i].QuestionContent + "</td>";
        //判断题目类型
        if (list[i].QuestionTypeID == 1)
            htmlStr += "<td>选择题</td>";
        if (list[i].QuestionTypeID == 2)
            htmlStr += "<td>简答题</td>";
        htmlStr += "<td>";
        if (list[i].IsAdded == true) {
            htmlStr += "<a href=\"/JoinUs/OrganizePaper/DelFromPaper?paperId=" + $("#paperId").val()
                + "&pageIndex=" + $("#page_index_now").val() + "&questionId=" + list[i].ID + "\" style='color:red;' >从试卷删除</a> ";
        } else {
            htmlStr += "<a href=\"/JoinUs/OrganizePaper/AddToPaper?paperId=" + $("#paperId").val()
                + "&pageIndex=" + $("#page_index_now").val() + "&questionId=" + list[i].ID + "\" >添加到试卷</a> ";
        }
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