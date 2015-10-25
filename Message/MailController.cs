using MVC.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Message
{
    public class MailController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region 发送邮件请求+public string Send()
        /// <summary>
        /// 发送邮件请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Send()
        {
            string sendEmail = Request["sendEmail"];
            string pwd = Request["pwd"];
            string receiveEmail = Request["receiveEmail"];
            string title = Request["title"];
            string content = Request["content"];
            string hiddenFileAddress = Request["hiddenFileAddress"];
            string result;
            if (hiddenFileAddress != "")
            {
                string[] arr = hiddenFileAddress.Split(';');
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = Server.MapPath("/Areas/uploadFile/" + arr[i]);
                }
                result = SendEmail(receiveEmail, title, content, sendEmail, pwd, arr);
            }
            else
            {
                result = SendEmail(receiveEmail, title, content, sendEmail, pwd);
            }

            return result;
        } 
        #endregion

        #region 发送邮件+private string SendEmail(string receiveEmail, string title, string content, string sendEmail, string pwd, string[] filesPaths = null)
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiveEmail">接收者邮箱</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件正文</param>
        /// <param name="sendEmail">发送者邮箱</param>
        /// <param name="pwd">发送者邮箱密码</param>
        /// <param name="filesPaths">附件</param>
        /// <returns></returns>
        private string SendEmail(string receiveEmail, string title, string content, string sendEmail, string pwd, string[] filesPaths = null)
        {
            string[] email = sendEmail.Split('@');
            WebMail.SmtpServer = "smtp."+email[email.Length-1];//获取或设置要用于发送电子邮件的 SMTP 中继邮件服务器的名称。
            WebMail.SmtpPort = 25;//发送端口
            WebMail.EnableSsl = true;//是否启用 SSL GMAIL 需要 而其他都不需要 具体看你在邮箱中的配置
            WebMail.UserName = sendEmail;//账号名
            WebMail.From = sendEmail;//邮箱名
            WebMail.Password = pwd;//密码
            WebMail.SmtpUseDefaultCredentials = true;//是否使用默认配置

            try
            {
                if (filesPaths!=null)
                {
                    WebMail.Send(
                    to: receiveEmail,
                    subject: title,
                    body: content,
                    filesToAttach: filesPaths
                    );
                }
                else
                {
                    WebMail.Send(
                    to: receiveEmail,
                    subject: title,
                    body: content
                    );
                }
                
            }
            catch (Exception e)
            {
                return "nook";
            }
            return "ok";
        } 
        #endregion

        #region 邮件附件上传+public ActionResult Upload(HttpPostedFileBase fileData)
        /// <summary>
        /// 邮件附件上传
        /// </summary>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Areas/uploadFile");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                    string path = filePath + "\\" + saveName;
                    fileData.SaveAs(path);

                    return Json(new { Success = true, FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 处理异常 protected override void OnException(ExceptionContext filterContext)
        protected override void OnException(ExceptionContext filterContext)
        {
            GetAbnormal ab = new GetAbnormal();
            ab.Abnormal(filterContext);
            base.OnException(filterContext);
        }
        #endregion

    }
}
