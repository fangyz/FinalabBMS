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
    $("#pub_paper").click(function () {
        pubPaper();
    });
});

/* 获取分页数据（默认页容量为10） */
function getPageData(pageIndex) {
    $.getJSON("/JoinUs/PaperManage/GetPageData", "pageIndex=" + pageIndex + "&pageSize=10", function (jsonObj) {
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
            history.replaceState(null, null, "/JoinUs/PaperManage/Index/" + pageIndex);
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
        htmlStr += "<td>" + list[i].PaperName + "</td>";
        if (list[i].typeId == 1)
        {
            htmlStr += "<td>技术方向</td>";
        }
        else if (list[i].typeId == 2)
        { htmlStr += "<td>市场部</td>"; }
        else
        { htmlStr += "<td>设计部</td>"; }
        htmlStr += "<td>" + list[i].PaperProducerID + "</td>";
        //JSON日期时间格式转换
        //使用正则表达式将生日属性中的非数字（\D）删除
        //并把得到的毫秒数转换成数字类型
        var birthdayMilliseconds = parseInt(list[i].AddDate.replace(/\D/igm, ""));
        //实例化一个新的日期格式，使用1970 年 1 月 1 日至今的毫秒数为参数
        var date = new Date(birthdayMilliseconds);
        htmlStr += "<td>" + date.toLocaleString() + "</td>";
        htmlStr += "<td>";
        htmlStr += "<a href=\"/JoinUs/OrganizePaper/Index?pageIndex=1&paperId=" + list[i].ID + "\">整理</a> ";
        htmlStr += "<a href=\"/JoinUs/PaperManage/PreviewPaper/" + list[i].ID + "\">预览</a> ";
        //htmlStr += "<a href=\"/JoinUs/PaperManage/PubPaper?pageIndex=1&paperId=" + list[i].ID + "\">发布</a> ";

        if (list[i].IsPublished == true) {
            htmlStr += "<font style='color:red;' >取消</font> ";
        } else {
            htmlStr += "<a href=\"/JoinUs/PaperManage/PubPaper?pageIndex=1&paperId=" + list[i].ID + "\">发布</a> ";
        }

        htmlStr += "<a href=\"#\" onclick=\"del(" + list[i].ID + ")\">删除</a>";
        htmlStr += "</td>";
        htmlStr += "</tr>";
    }
    //如果不够10条数据，则生成空行
    if (list.length < 10)
        for (var i = 0; i < 10 - list.length; i++) {
            htmlStr += "<tr><td>-</td><td></td><td></td><td></td><td></td></tr>";
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
            window.location = "/JoinUs/PaperManage/Del?id=" + id + "&pageIndexNow=" + pageIndexNow;
        }
        //,
        //onCancel: function () {
        //    alert('算求，不弄了');
        //}
    });
    //if (confirm("您确定要删除这条数据？")) {
    //    var pageIndexNow = $("#page_index_now").val();
    //    window.location = "/JoinUs/QuestionManage/Del?id=" + id + "&pageIndexNow=" + pageIndexNow;
    //}
}
