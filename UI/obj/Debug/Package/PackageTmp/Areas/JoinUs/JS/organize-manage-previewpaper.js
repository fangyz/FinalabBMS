$(function () {
    //获取试卷题目信息
    $.getJSON("/JoinUs/OrganizePaper/GetPaperQuestionList/" + $("#paper_id").val(),
        null, function (jsonObj) {
            if (jsonObj.Statu == "ok") {
                var list = jsonObj.Data;
                var htmlStr = "";
                var count = 0;
                for (var i = 0; i < list.length; i++) {
                    //简答题
                    if (list[i].QuestionTypeID == 2) {
                        //题目内容
                        htmlStr += "<tr><td colspan='2'>" + (i + 1) + ". " + list[i].QuestionContent + "</td></tr>";
                        //textarea
                        htmlStr += "<tr><td colspan='2'><textarea></textarea></td></tr>";
                        htmlStr += "<tr><td  >分值</td><td>" + list[i].QuestionGrade + "</td></tr>";
                        htmlStr += "<tr><td >标签</td><td>" + list[i].QuestionTag + "</td></tr>";
                        //空行
                        htmlStr += "<tr><td colspan='2'>--</td></tr>";
                        count = count + list[i].QuestionGrade;
                    }
                        //选择题
                    else if (list[i].QuestionTypeID == 1) {
                        //题目内容
                        htmlStr += "<tr><td colspan='2'>" + (i + 1) + ". " + list[i].QuestionContent + "</td></tr>";
                        var options = list[i].T_QuestionOption;
                        //选项
                        for (var j = 0; j < options.length; j++) {
                            htmlStr += "<tr><td class='option_id'>" + options[j].OptionID + "</td>";
                            htmlStr += "<td>" + options[j].OptionContent + "</td></tr>";
                        }
                        htmlStr += "<tr><td >分值</td><td>" + list[i].QuestionGrade + "</td></tr>";
                        htmlStr += "<tr><td >标签</td><td>" + list[i].QuestionTag + "</td></tr>";
                        //空行
                        htmlStr += "<tr><td colspan='2'>--</td></tr>";
                        count = count + list[i].QuestionGrade;
                    }

                }
                htmlStr += "<tr><td >总分</td><td>" + count + "</td></tr>";
                htmlStr += "<tr><td style='width:100px;'>题目数量</td><td>" + list.length + "</td></tr>";
                htmlStr += "<button type='button' class='am-btn am-btn-default'>默认样式</button>";
                htmlStr += "<tr><td colspan='2'><a href='/JoinUs/OrganizePaper/Index?paperId=" + $("#paper_id").val() + "&pageIndex=1'><button type='button' class='am-btn am-btn-primary'>确定</button></td></tr>";
                $("#tb_list tbody").empty();
                $("#tb_list tbody").append(htmlStr);

            }
            if (jsonObj.Statu == null) {
                var htmlStr = "";
                htmlStr += "<tr><td>" + jsonObj.Msg + "</td></tr>";
                $("#tb_list tbody").empty();
                $("#tb_list tbody").append(htmlStr);
            }
        });
});