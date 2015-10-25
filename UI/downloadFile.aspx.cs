using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI
{
    public partial class downloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileName = Context.Request.Params["filename"];
            string fullFilePath = Context.Server.MapPath("/uploads/" + fileName);
            //string fullFilePath = Context.Server.MapPath("/uploads/2222.jpg");
            FileInfo info = new FileInfo(fullFilePath);
            try
            {
                long fileSize = info.Length;
                Response.Clear();
                Response.ContentType = "application/x-zip-compressed";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileSize.ToString());
                Response.TransmitFile(fullFilePath, 0, fileSize);
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), " message", "<script language='javascript' >alert('您下载的文件不存在或已被删除！');</script>"); 
                Response.Redirect("Task/Task/Mytask");
            }
            
        }
    }
}