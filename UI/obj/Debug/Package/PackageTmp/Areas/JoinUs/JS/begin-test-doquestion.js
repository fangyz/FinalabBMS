$(function () {
   
    //下一步-表单验证
    $("#btn_next_step").click(function () {
        //选择题
        if ($("#questionTypeId").val() == 1) {
            var val = $('input:radio[name="answerContent"]:checked').val();
            if (val == null) {
                alert("还没选答案呢！");
                return;
            }
        }
            //简答题
        else if ($("#questionTypeId").val() == 2) {
            if ($("textarea").val().trim() == "") {
                alert("简答题还没写呢！");
                return;
            }
        }
        
        if ($("#questionindex").val() == $("#questioncount").val()) {
            window.name = '';
           
        }
        $("#questionForm").submit();
    });
    //获取题目
    $.getJSON("/JoinUs/BeginTest/GetQuestionById/" + $("#question_index").val(),
        null, function (jsonObj) {
            if (jsonObj.Statu == "ok") {
                var model = jsonObj.Data;
                var thead = "";
                var tbody = "";
                //简答题
                if (model.QuestionTypeID == 2) {
                    thead += "<tr><th colspan='2'>" + model.QuestionContent + "</th></tr>";
                    tbody += "<tr><td><textarea name='answerContent'></textarea></td></tr>";
                }
                    //选择题
                else if (model.QuestionTypeID == 1) {
                    thead += "<tr><th colspan='2'>" + model.QuestionContent + "</th></tr>";
                    var options = model.T_QuestionOption;
                    for (var i = 0; i < options.length; i++) {
                        tbody += "<tr><td colspan='2'><input type='radio' name='answerContent' value=" + options[i].OptionID + " />";
                        tbody += options[i].OptionID + " " + options[i].OptionContent + "</td></tr>";
                    }
                }
                $("#questionTypeId").val(model.QuestionTypeID);
                $("#questionId").val(model.ID);
                $("#question table thead").empty();
                $("#question table thead").append(thead);
                $("#question table tbody").empty();
                $("#question table tbody").append(tbody);
            }
        });
});