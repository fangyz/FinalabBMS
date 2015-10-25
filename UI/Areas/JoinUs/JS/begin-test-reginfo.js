
$(function () {
    $("#btn_next").click(function () {
        $('#my-confirm').modal({
            relatedTarget: this,
            onConfirm: function () {

                if ($("#num").val() == "") {
                    alert("学号不能为空");
                    return;
                }
                if ($("#name").val() == "") {
                    alert("姓名不能为空");
                    return;
                }

                if ($("#academy").val() == "") {
                    alert("学院不能为空");
                    return;
                }
                if ($("#major").val() == "") {
                    alert("专业不能为空");
                    return;
                }
                if ($("#qq").val() == "") {
                    alert("QQ不能为空");
                    return;
                }
                if ($("#email").val() == "") {
                    alert("Email不能为空");
                    return;
                }
                if ($("#tel_num").val() == "") {
                    alert("联系方式不能为空");
                    return;
                }
                if ($("#learning_experience").val() == "") {
                    alert("学习经验不能为空");
                    return;
                }
                if ($("#self_evaluation").val() == "") {
                    alert("自我评价不能为空");
                    return;
                }
                $("#form_reginfo").submit();
            }
        });
    });
});
