//------------------------- 窗体加载事件绑定 --------------------------------
$(function () {
    //根据id获取题目详细信息，并添加到表格
    $.getJSON("/JoinUs/QuestionManage/GetModelById", "id=" + $("#questionId").val(), function (jsonObj) {
        if (jsonObj.Statu == "ok") {
            var model = jsonObj.Data;
            //简答题
            if (model.QuestionTypeID == 2) {
                //切换操作面板
                $("#type_select").val(2);
                $("#form_brief").css("display", "block");
                $("#form_choice").css("display", "none");
                //填充题目内容
                $("#brief_question_content").val(model.QuestionContent);
                $("#brief_question_grade").val(model.QuestionGrade);
                $("#brief_question_tag").val(model.QuestionTag);
            }
            //选择题
            if (model.QuestionTypeID == 1) {
                //填充题目内容
                $("#choice_question_content").val(model.QuestionContent);
                $("#choice_question_grade").val(model.QuestionGrade);
                $("#choice_question_tag").val(model.QuestionTag);
                //填充题目选项
                var options = model.T_QuestionOption;
                for (var i = 0; i < options.length; i++) {
                    $("#option_" + options[i].OptionID + "_content").val(options[i].OptionContent);
                    $("#option_" + options[i].OptionID + "_weight").val(options[i].OptionWeight);
                }
            }
        }
    });

    //题目类型改变，则操作面板改变
    //$("#type_select").change(function () {
    //    var typeId = parseInt($(this).find("option:selected").val());
    //    if (typeId == 1) {
    //        $("#form_brief").css("display", "none");
    //        $("#form_choice").css("display", "block");
    //    } else if (typeId == 2) {
    //        $("#form_choice").css("display", "none");
    //        $("#form_brief").css("display", "block");
    //    }
    //});
});