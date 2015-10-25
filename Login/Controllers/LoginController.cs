using Common.Attributes;
using MODEL.FormatModel;
using MVC.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Login.Controllers
{
    public class LoginController : Controller
    {
        #region 1.1 获取登陆页面+public ActionResult Index()
        // GET: /Login/
        /// <summary>
        /// 获取登陆页面，不需要验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Common.Attributes.Skip]
        public ActionResult Index()
        {
            //这是判断是否免登入，即有用户的对象
            if (Request.Cookies["Admin_InfoKey"] != null)
            {
                string strCookieValue = Request.Cookies["Admin_InfoKey"].Value;
                string User = Common.SecurityHelper.DecryptUserInfo(strCookieValue);
                MODEL.T_MemberInformation user =OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == User).First();
                ViewData["Name"] = user.StuNum;
                ViewData["Pwd"] = user.LoginPwd;
            }
            return View();
        }
        #endregion
        #region 1.2 获取验证码，不需要验证+public ActionResult GetVCode()
        /// <summary>
        /// 获取验证码，不需要验证
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult GetVCode()
        {
            VCode v = new VCode();
            byte[] arrImg = v.GetVCode();
            return File(arrImg, "image/jpeg");
        }
        #endregion
        #region 1.3登陆，表单提交过来的数据+public ActionResult Login(MODEL.ViewModel.LoginUser user)
         [Common.Attributes.Skip]
        public ActionResult Login(MODEL.ViewModel.LoginUser user)
        {
            //检查模型绑定器能否成功绑定“Date”属性，如果数据不合法使用ModelState.AddModelError()添加错误消息。如果没有任何错误
            //ModelState.IsValid则为true
            if (ModelState.IsValid)
            {
                //这里Remember是得到是否记住密码
                string remember = Request.Form["Remember"];
                if (remember.Equals("on"))
                {
                    user.IsAlways = true;
                }
                string vCode = Request.Form["VCode"];
                string vCodeSer = (string)Session["VCode"];
                /*自动登陆时*/
                if (vCode.Equals(vCodeSer))
                {
                    //登陆成功进入主页
                    if (OperateContext.Current.UserLogin(user))
                    {
                        return OperateContext.Current.RedirectAjax("ok", null, null, "/Login/Login/MainPage");
                    }
                    else
                    {
                        return OperateContext.Current.RedirectAjax("err", "登陆失败", null, null);
                    }
                }
                else
                {
                    return OperateContext.Current.RedirectAjax("err", "验证码错误", null, null);
                }
                //如果登入成功，再判断验证码是否正确
                
            }
            else
            {
                return OperateContext.Current.RedirectAjax("err", "登陆失败", null, null);
            }
        } 
        #endregion
        #region 1.4登陆成功进入主页+ public ActionResult MainPage()
        public ActionResult MainPage()
        {
            ViewBag.name = OperateContext.Current.Usr.StuName;
            //在这个地方进行人员角色的更新
            string dtone;
            string dttwo;
            string dtthree;
            string dtfour;
            DateTime dt = DateTime.Now;
            if (dt.Month == 7)
            {
                dtone = dt.Year.ToString();
                dttwo = (dt.Year - 1).ToString();
                dtthree = (dt.Year - 2).ToString();
                dtfour = (dt.Year - 3).ToString();
                //1.将所有退役人员设置为毕业人员
                List<MODEL.T_RoleAct> rolelist=new List<MODEL.T_RoleAct>();
                rolelist=OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u=>u.RoleId==10012).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u=>u.RoleId==10012);
                foreach(MODEL.T_RoleAct ra in rolelist){
                    ra.RoleId=10011;
                    OperateContext.Current.BLLSession.IRoleActBLL.Add(ra);
                }
                //2.所有大四成员角色设置为退役人员
                List<string> numlist=new List<string>();
                numlist=OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u=>u.RoleActor.Contains(dtfour)).Select(p=>p.RoleActor).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u=>u.RoleActor.Contains(dtfour));
                MODEL.T_RoleAct ro;
                foreach(string stunum in numlist)
                {
                    ro=new MODEL.T_RoleAct();
                    ro.RoleId=10012;
                    ro.RoleActor=stunum;
                    ro.IsDel=false;
                    OperateContext.Current.BLLSession.IRoleActBLL.Add(ro);
                }

            }
           
            return View();
        } 
        #endregion

        #region 2.1忘记密码首页+ActionResult ForgetPwd()
        [Common.Attributes.Skip]
        public ActionResult ForgetPwd()
        {
            //跳转到一个忘记密码页面
            return View();
        }
        #endregion
        #region 2.2将信息发送到邮箱 ActionResult GetPwd()
        [Common.Attributes.Skip]
        public ActionResult GetPwd()
        {
            string num = Request.QueryString["stunum"];
            List<MODEL.T_MemberInformation> mem= OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == num).ToList();
            if (mem.Count > 0)
            {
                //发送内容到这个用户的邮箱
                string mail=mem[0].Email;
                if(!string.IsNullOrEmpty(mail)){
                    string href = "http://www.finalab.cn/login/login/ModifyPwd";
                    if (SendEmail(mail, "修改密码", href, "1666196950@qq.com", "lrhLRH072131") == "ok")
                    {
                        return Content("<script>alert('信息已发送到您的邮箱，请注意查看！');window.location='/Login/Login/Index'</script>");
                    }
                }
               
            }
            return Content("<script>alert('不存在这个学号，请您重新输入学号！');window.location='/Login/Login/ForgetPwd'</script>");
        }
        #endregion
        #region 2.3修改密码页面+ActionResult ModifyPwd()
        [Common.Attributes.Skip]
        public ActionResult ModifyPwd()
        {
            return View();
        }
        #endregion
       
        #region 2.4修改密码到数据库+ActionResult ModifyPwd()
        [Common.Attributes.Skip]
        public ActionResult ReallyModifyPwd()
        {
            string pwd = Request.Form["newPwd"].ToString();
            string num = Request.Form["stunum"].ToString();
            if (!string.IsNullOrEmpty(pwd))
            {
                MODEL.T_MemberInformation mem = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == num).FirstOrDefault();
                mem.LoginPwd = pwd;
                if (OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(mem, "LoginPwd") > 0)
                {
                    return Content("<script>alert('修改成功！');window.location='/Login/Login/index'</script>");
                }
            }
            return Content("<script>alert('不存在这个学号，请您重新输入学号！');window.location='/Login/Login/ForgetPwd'</script>");
        }
        #endregion
        
        #region 2.5发送邮件+private string SendEmail(string receiveEmail, string title, string content, string sendEmail, string pwd, string[] filesPaths = null)
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
            WebMail.SmtpServer = "smtp." + email[email.Length - 1];//获取或设置要用于发送电子邮件的 SMTP 中继邮件服务器的名称。
            WebMail.SmtpPort = 25;//发送端口
            WebMail.EnableSsl = true;//是否启用 SSL GMAIL 需要 而其他都不需要 具体看你在邮箱中的配置
            WebMail.UserName = sendEmail;//账号名
            WebMail.From = sendEmail;//邮箱名
            WebMail.Password = pwd;//密码
            WebMail.SmtpUseDefaultCredentials = true;//是否使用默认配置

            try
            {
                if (filesPaths != null)
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



        #region 3.1 根据当前登陆用户 权限 生成菜单 +GetMenuData()
        /// <summary>
        /// 根据当前登陆用户 权限 生成菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuData()
        {
            return Content(OperateContext.Current.UsrMenuJsonStr);
        }
        #endregion
        #region 3.2登入后的首页 ActionResult Welcome()
        [Common.Attributes.Skip]
        public ActionResult Welcome()
        {
            string name = Request.QueryString["name"];
            ViewBag.name = name;
            return View();
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
