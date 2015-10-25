$(function () {
    $.getJSON("/JoinUs/AnswerSheet/GetBriefAnswerSheet", "id=" + $("#answer_sheet_id").val(), function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            var list = jsonObj.Data;
            var htmlStr = "";
            $("tbody").empty();
            for (var i = 0; i < list.length; i++) {
                htmlStr += "<tr><td>" + (i + 1) + ". " + list[i].Question.QuestionContent + "</td></tr>";
                htmlStr += "<tr><td>分值："+list[i].Question.QuestionGrade+"</td></tr>";
                htmlStr += "<tr><td>标签：" + list[i].Question.QuestionTag + "</td></tr>";
                htmlStr += "<tr><td>他的答案：" + list[i].Answer + "</td></tr>";
                htmlStr += "<tr><td>您给的分数：<input style='width:200px;' name='" + list[i].Question.ID + "' type='number' min='0' max='" + list[i].Question.QuestionGrade + "'  required=\"required\" /></td></tr><tr><td>&nbsp;</td></tr>";
            }
            htmlStr += "<tr><td>评语（请用一句话谈谈你对该面试者的印象）：<input required=\"required\" name='commentContent' id=\"comment_content\" /></td></tr>";
            $("tbody").append(htmlStr);
        }
    });
    $.getJSON("/JoinUs/AnswerSheet/GetPaperRaterInfo", null, function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            var htmlStr = "";
            list = jsonObj.Data;
            for (var i = 0; i < list.length; i++) {
                htmlStr += "<option value=\"" + list[i].ID + "\">" + list[i].Name + "</option>";
            }
            $("#paper_rater").empty();
            $("#paper_rater").append(htmlStr);
        }
    });
});