$(function () {
    //获取试卷题目信息
    $.getJSON("/JoinUs/AnswerSheet/GetPaperList/" + $("#paper_id").val(),
        null, function (jsonObj) {
            if (jsonObj.Statu == "ok") {
                var list = jsonObj.Data;
                var htmlStr = "";
                var count = 0;
                var num = 0;
                for (var i = 0; i < list.length; i++) {
                //简答题
                    if (list[i].QuestionTypeID == 2) {
                        num++;
                        
                        //题目内容
                        htmlStr += "<tr><td colspan='2'>" + (i + 1) + ". " + list[i].QuestionContent + "</td></tr>";
                       
                        htmlStr += "<tr><td  class='option_id'>分值</td><td>" + list[i].QuestionGrade + "</td></tr>";
                        htmlStr += "<tr><td class='option_id'>标签</td><td>" + list[i].QuestionTag + "</td></tr>";
                        htmlStr += "<tr><td  class='option_id'>回答</td><td>" + list[i].BriefAnswerSheet.Answer + "</td></tr>";
                        htmlStr += "<tr><td class='option_id'>得分</td><td>"+list[i].TeacherF+"评分：" + list[i].ScoreF + ",&nbsp;&nbsp;"+list[i].TeacherS+"评分：" + list[i].ScoreS + ",&nbsp;&nbsp;" +list[i].TeacherT+"评分："+ list[i].ScoreT + "</td></tr>";
                       
                        //空行
                        htmlStr += "<tr><td colspan='2'>--</td></tr>";
                        count = count + list[i].BriefScore.Score;

                        
                    }
                        //选择题
                    else if (list[i].QuestionTypeID == 1) {
                        num++;
                        //题目内容
                        htmlStr += "<tr><td colspan='2'>" + (i + 1) + ". " + list[i].QuestionContent + "</td></tr>";
                        var options = list[i].T_QuestionOption;
                        var str;
                        //选项
                        for (var j = 0; j < options.length; j++) {
                            htmlStr += "<tr><td class='option_id'>" + options[j].OptionID + "</td>";
                            htmlStr += "<td>" + options[j].OptionContent + "</td></tr>";
                            if (list[i].ChoiceAnswerSheet.Answer == options[j].OptionID)
                            {
                                str = options[j].OptionWeight;
                            }
                        }
                        htmlStr += "<tr><td class='option_id'>分值</td><td>" + list[i].QuestionGrade + "</td></tr>";
                        htmlStr += "<tr><td class='option_id'>标签</td><td>" + list[i].QuestionTag + "</td></tr>";
                        htmlStr += "<tr><td  class='option_id'>回答</td><td>" + list[i].ChoiceAnswerSheet.Answer + "</td></tr>";
                        htmlStr += "<tr><td class='option_id'>得分</td><td>" + str + "</td></tr>";
                        //空行
                        htmlStr += "<tr><td colspan='2'>--</td></tr>";
                        count = count + str;
                       
                    }
                    if (i == list.length-1)
                    {
                        htmlStr += "<tr><td class='option_id'>评价</td><td>" + list[num - 1].AnswerSheetComment.CommentContent + "</td></tr>";
                    }

                }
               
                htmlStr += "<tr><td style='width:100px;' class='option_id'>总分</td><td>" + count + "</td></tr>";
                htmlStr += "<tr><td colspan='2'><a href='/JoinUs/AnswerSheet/Index'><button type='button' class='am-btn am-btn-primary'>确定</button></td></tr>";
                $("#tb_list tbody").empty();
                $("#tb_list tbody").append(htmlStr);

            }
            if (jsonObj.Statu == "null") {
                var htmlStr = "";
                htmlStr += "<tr><td>" + jsonObj.Msg + "</td></tr>";
                htmlStr += "<tr><td colspan='2'><a href='/JoinUs/AnswerSheet/Index'><button type='button' class='am-btn am-btn-primary'>确定</button></td></tr>";
                $("#tb_list tbody").empty();
                $("#tb_list tbody").append(htmlStr);
            }
        });
});