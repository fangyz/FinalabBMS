$(function () {
    $("#search").click(function () {

        alert("点击了");

        $appendedInputButtons = $("#searchText");

        if ($.trim($appendedInputButtons.val()) == "") {
            alert("搜索内容不能为空！");
            $("#appendedInputButtons").css("border", "1px solid red");
        } else {
            alert("进来了");
            //$.post(

            //    "/Home/Home/SearchMember",
            //    {
            //        appendedInputButtons : $appendedInputButtons.val()
            //    },
            //    function (data) {
            //        alert("成功了");
            //        var d = JSON.parse(data);

            //    }
            //    );
            $.ajax({
                url: "/Message/InsideMsg/SearchMember",
                type: "GET",
                dataType: "json",
                data: { appendedInputButtons: $appendedInputButtons.val() },
                success: function (response) {
                    //alert(response);
                    $("#messagaAddress").empty();
                    var obj = strToJson(response);
                }
            })

        }
    });
});