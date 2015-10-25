//=====================<< 查询页数导航控制 >>============================
$(function () {
    if (parseInt($("#pageIndex").text()) == parseInt($("#pageCount").text())) {
        $("#nextPage").css("visibility", "hidden");
    }
    if (parseInt($("#pageIndex").text()) == 1) {
        $("#frontPage").css("visibility", "hidden");
    }
});


//===================<<    删除    >>==========================
function Clik(obj) {
    //alert("行点击！");
    //alert(obj);
    if (confirm("您确定要删除该信息吗？")) {
        $.post(
        "/Message/InsideMsg/DeleteSendBox",
          {
              UserMessageId: obj
          },
          function (data) {
              //alert(data);
              if (data == "ok") {
                  alert("成功删除！");
                  document.location.reload();
              } else {
                  alert("删除失败！");
              }
          })
    } else {
        return "";
    }

}
//===================<<    彻底删除    >>==========================
function TrueClik(obj) {
    //alert("行点击！");
    //alert(obj);
    if (confirm("您确定要彻底删除该信息吗？")) {
        $.post(
        "/Message/InsideMsg/DelSendMesInRec",
          {
              UserMessageId: obj
          },
          function (data) {
              //alert(data);
              if (data == "ok") {
                  alert("彻底删除成功！");
                  document.location.reload();
              } else {
                  alert("彻底删除失败！");
              }
          })
    } else {
        return "";
    }

}
//=======================<< 恢复信息   >>=======================
function RestoreMessage(obj) {
    //alert("行点击！");
    //alert(obj);
    if (confirm("您确定要恢复该信息吗？")) {
        $.post(
        "/Message/InsideMsg/RestoreMessage",
          {
              UserMessageId: obj,
              Flag: "send"
          },
          function (data) {
              //alert(data);
              if (data == "ok") {
                  alert("成功恢复信息！");
                  document.location.reload();
              } else {
                  alert("恢复信息失败！");
              }
          })
    } else {
        return "";
    }

}