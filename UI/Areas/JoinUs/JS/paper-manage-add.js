$(function () {
    $.getJSON("/JoinUs/PaperManage/GetProducerInfo", null, function (jsonData) {
        if (jsonData.Statu == "ok") {
            var htmlStr = "";
            list = jsonData.Data;
            for (var i = 0; i < list.length; i++) {
                htmlStr += "<option value=\"" + list[i].ID + "\">" + list[i].Name + "</option>";
            }
            $("#select_producer").empty();
            $("#select_producer").append(htmlStr);
        }
    });
});