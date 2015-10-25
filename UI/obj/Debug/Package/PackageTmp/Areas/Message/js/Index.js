
//===============<< 删除  >>=======================
function Delete(obj) {
    //alert("点击" + obj);
    if (confirm("您确定要删除该信息吗？")) {
        $.post(
        "/Message/InsideMsg/DeleteReceivedBox",
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
//===============<< 彻底删除  >>=======================
function TrueDelete(obj) {
    alert("点击" + obj);
    if (confirm("您确定要彻底删除该信息吗？")) {
        $.post(
        "/Message/InsideMsg/DelRecMesInRec",
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
//=========================<<  标记未读  >>======================
function SetStatus(obj) {
    //alert("点击" + obj);
    if (confirm("您确定要标记为未读息吗？")) {
        $.post(
        "/Message/InsideMsg/SetStatus",
          {
              UserMessageId: obj
          },
          function (data) {
              //alert(data);
              if (data == "ok") {
                  alert("标记成功！");
                  document.location.reload();
              } else {
                  alert("标记失败！");
              }
          })
    } else {
        return "";
    }
}

//=======================<< 恢复收信  >>==================
function RestoreMsg(obj) {
    //alert("行点击！");
    //alert(obj);
    if (confirm("您确定要恢复该信息吗？")) {
        $.post(
        "/Message/InsideMsg/RestoreMessage",
          {
              UserMessageId: obj,
              Flag: "receive"
          },
          function (data) {
              //alert(data);
              if (data == "ok") {
                  alert("成功恢复！");
                  document.location.reload();
              } else {
                  alert("恢复失败！");
              }
          })
    } else {
        return "";
    }

}