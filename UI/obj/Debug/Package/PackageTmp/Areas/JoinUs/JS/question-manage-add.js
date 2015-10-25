//------------------------- 窗体加载事件绑定 --------------------------------
$(function () {
    $("#type_select").change(function () {
        var typeId = parseInt($(this).find("option:selected").val());
        if (typeId == 1) {
            $("#form_brief").css("display", "none");
            $("#form_choice").css("display", "block");
        } else if (typeId == 2) {
            $("#form_choice").css("display", "none");
            $("#form_brief").css("display", "block");
        }
    });
});