
(function ($) {
    $.extend($, {
        procAjaxData: function (data,funcSuc,funcErr) {
            if (!data.Statu) {
                return;
            }

            switch (data.Statu)
            {
                case "ok":
                    alert("OK:" + data.Msg);
                    if (funcSuc) funcSuc(data);
                    break;
                case "err":
                    alert("ERR:" + data.Msg);
                    if (funcErr) funcErr(data);
                    break;
                case "nologin":
                    alert(data.Msg);
                    window.location = data.BackUrl;
                    break;
            }
        }
    });
}(jQuery));