
//===========<<  删除消息  >>=====================
function Delete(obj) {
    if (confirm("您确定要删除该信息吗？")) {
        $.post(
        "/Message/SysMessage/DelSysMsg",
          {
              MsgId: obj
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
