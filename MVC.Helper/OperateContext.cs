using Common.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Helper
{
   public  class OperateContext
    {
       //常量和对象的声明
        const string Admin_CookiePath = "/admin/";
        const string Admin_InfoKey = "ainfo";//用来把成员信息表的实体对象存入Session
        const string Admin_PermissionKey = "apermission";
        const string Admin_MenuString = "aMenuString";
        const string Admin_LogicSessionKey = "BLLSession";
        const string Admin_Stunum = "StuNum";
        #region 0.1 得到Http上下文 及 相关属性
        /// <summary>
        /// Http上下文
        /// </summary>
        HttpContext ContextHttp
        {
            get
            {
                return HttpContext.Current;
            }
        }

        HttpResponse Response
        {
            get
            {
                return ContextHttp.Response;
            }
        }

        HttpRequest Request
        {
            get
            {
                return ContextHttp.Request;
            }
        }

        System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                return ContextHttp.Session;
            }
        }
        #endregion
        #region 0.2 业务仓储    IBLL.IBLLSession BLLSession
        /// <summary>
        /// 业务仓储
        /// </summary>
        public IBLL.IBLLSession BLLSession;
        #endregion
        #region 0.3 当前用户对象    MODEL.Ou_UserInfo Usr
        // <summary>
        /// 当前用户对象
        /// </summary>
        public MODEL.T_MemberInformation Usr
        {
            get
            {
                return Session[Admin_InfoKey] as MODEL.T_MemberInformation;
            }
            set
            {
                Session[Admin_InfoKey] = value;
            }
        }
        #endregion
        #region 0.4得到用户权限，存入Session中   List<MODEL.Ou_Permission> UsrPermission
        // <summary>
        /// 用户权限
        /// </summary>
        public List<MODEL.T_Permission> UsrPermission
        {
            get
            {
                return Session[Admin_PermissionKey] as List<MODEL.T_Permission>;
            }
            set
            {
                Session[Admin_PermissionKey] = value;
            }
        }
        #endregion
        #region 0.5 实例构造函数 初始化 业务仓储  public OperateContext()
        public OperateContext()
        {
            /*使用反射特别要注意的地方，会到网站根目录下去找*/
            //string pLocal = Path.GetDirectoryName(Environment.CurrentDirectory);
            //Assembly asmb = Assembly.Load( "BLL");
            //Type supType = asmb.GetType("BLL.BLLSession");
            //IBLL.IBLLSession obj = Activator.CreateInstance(supType) as IBLL.IBLLSession; 
            BLLSession = DI.SpringHelper.GetObject<IBLL.IBLLSession>("BLLSession");
        }
        #endregion
        #region 0.6 获取当前 操作上下文     OperateContext Current
        /// <summary>
        /// 获取当前 操作上下文 (为每个处理浏览器请求的服务器线程 单独创建 操作上下文)
        /// </summary>
        public static OperateContext Current//加载OperateContext实例时是不会加载这个成员的，因为是静态变量，不会出现在实例中
        {
            get
            {
                OperateContext oContext = CallContext.GetData(typeof(OperateContext).Name) as OperateContext;
                if (oContext == null)
                {
                    oContext = new OperateContext();
                    CallContext.SetData(typeof(OperateContext).Name, oContext);
                }
                return oContext;
            }
        }
        #endregion

       //唐尼玛测试数据
        public string User
        {
            get
            {
                //获取当前user对象
                return "XiaoBai";
            }
        }


        #region 1.1判断用户登入，并获得用户的权限    bool UserLogin(MODEL.ViewModel.LoginUser usrPara)
        /// <summary>
        /// 判断用户登录
        /// </summary>
        /// <param name="user"></param>
        public bool UserLogin(MODEL.ViewModel.LoginUser user)
        {
            //应该到业务层查询
            MODEL.T_MemberInformation member = BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == user.LoginName).FirstOrDefault();
            //如果登陆成功
            if (user != null&&user.Pwd==member.LoginPwd)
            {
                //Usr是当前用户对象
                Usr = member;
                //存入学号
                Session[Admin_Stunum] = member.StuNum;
                if (user.IsAlways)
                {
                    string strCookieValue = Common.SecurityHelper.EncryptUserInfo(user.LoginName);
                    HttpCookie cookie = new HttpCookie("Admin_InfoKey", strCookieValue);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);
                }
                //2.2 查询当前用户的 权限，并将权限 存入 Session 中
                UsrPermission = GetUserPermission(user.LoginName);
                return true;
            }
            return false;
        } 
        #endregion
        #region 1.2 判断当前用户是否登陆 +bool IsLogin()
        /// <summary>
        /// 判断当前用户是否登陆 而且
        /// </summary>
        /// <returns></returns>E:\Projects\FinalWeb\DALMSSQL\BaseDAL.cs
        public bool IsLogin()
        {
            //1.验证用户是否登陆(Session && Cookie)
            if (Session[Admin_InfoKey] == null)//如果关闭浏览器则session已经没有，但是cookie还在
            {
                if (Request.Cookies[Admin_InfoKey] == null)
                {
                    //重新登陆，内部已经调用了 Response.End(),后面的代码都不执行了！ (注意：如果Ajax请求，此处不合适！)
                    //filterContext.HttpContext.Response.Redirect("/admin/admin/login");
                    return false;
                }
                else//如果有cookie则从cookie中获取用户id并查询相关数据存入 SessionS
                {
                    string strUserInfo = Request.Cookies[Admin_InfoKey].Value;
                    strUserInfo = Common.SecurityHelper.DecryptUserInfo(strUserInfo);
                    //userId即为用户学号
                    string userId = strUserInfo;
                    MODEL.T_MemberInformation usr = BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userId).First();
                    Usr = usr;
                    UsrPermission = OperateContext.Current.GetUserPermission(usr.StuNum);
                }
            }
            return true;
        }
        #endregion





        #region 2.1 根据学号查询权限+  public List<MODEL.T_Permission> GetUserPermission(string userId)
        /// <summary>
        /// 根据学号查询权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MODEL.T_Permission> GetUserPermission(string userId)
        {
            //根据学号插查询角色Id
            List<int> listRoleIds = BLLSession.IRoleActBLL.GetListBy(u => u.RoleActor == userId&&u.IsDel==false).Select(u => u.RoleId).ToList();
            //查询基础权限
            List<int> listPerIdCommon = BLLSession.IPermissionBLL.GetListBy(u => u.IsCommon == true).Select(u => u.PerId).ToList();
            //根据角色Id插查询权限Id
            List<int> listPerId = BLLSession.IRolePermissionBLL.GetListBy(u => listRoleIds.Contains(u.RoleId)).Select(u => u.PerId).ToList();
            //合并权限
            listPerId.AddRange(listPerIdCommon);
            //去除重复元素,得到一个新的集合
            List<int> listPerIds= listPerId.Distinct().ToList();

            //得到权限集合
            List<MODEL.T_Permission> listPers = BLLSession.IPermissionBLL.GetListBy(u => listPerIds.Contains(u.PerId)).Select(u => u.ToPOCO()).ToList();
            return listPers;
        } 
        #endregion
           
        #region  2.2 判断当前用户 是否有 访问当前页面的权限 +bool HasPemission
        /// <summary>
        /// 2.3 判断当前用户 是否有 访问当前页面的权限
        /// </summary> 
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public bool HasPemission(string areaName, string controllerName, string actionName, string httpMethod)
        {
            var listP = from per in UsrPermission
                        where
                            string.Equals(per.PerAreaName, areaName, StringComparison.CurrentCultureIgnoreCase) &&
                            string.Equals(per.PerController, controllerName, StringComparison.CurrentCultureIgnoreCase) &&
                            string.Equals(per.PerActionName, actionName, StringComparison.CurrentCultureIgnoreCase) && (
                                per.PerFormMethod == 3 ||//如果数据库保存的权限 请求方式 =3 代表允许 get/post请求
                                per.PerFormMethod == (httpMethod.ToLower() == "get" ? 1 : 2)
                            )
                        select per;
            return listP.Count() > 0;
        }
        #endregion

        #region 2.3 获取当前登陆用户的权限树Json字符串 +string UsrTreeJsonStr
        /// <summary>
        /// 获取当前登陆用户的权限树Json字符串
        /// </summary>
        public string UsrMenuJsonStr
        {
            get
            {
                //将 登陆用户的 权限集合 转成 树节点 集合（其中 IsShow = false的不要生成到树节点集合中）
                List<MODEL.MenuMODEL.TreeNode> listTree = MODEL.T_Permission.ToTreeNodes(UsrPermission.Where(p => p.PerIsShow == true).ToList());
                Session[Admin_MenuString] = Common.DataHelper.Obj2Json(listTree);
                return Session[Admin_MenuString].ToString();
            }
        }
        #endregion



        //---------------------------------------------3.0 公用操作方法--------------------

        #region 3.1 生成 Json 格式的返回值 +ActionResult RedirectAjax(string statu, string msg, object data, string backurl)
        /// <summary>
        /// 生成 Json 格式的返回值
        /// </summary>
        /// <param name="statu"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <param name="backurl"></param>
        /// <returns></returns>
        public ActionResult RedirectAjax(string statu, string msg, object data, string backurl)
        {
            MODEL.FormatModel.AjaxMsgModel ajax = new MODEL.FormatModel.AjaxMsgModel()
            {
                Statu = statu,
                Msg = msg,
                Data = data,
                BackUrl = backurl
            };
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            res.Data = ajax;
            return res;
        }
        #endregion

        #region 3.2 重定向方法 根据Action方法特性  +ActionResult Redirect(string url, ActionDescriptor action)
       /// <summary>
        /// 重定向方法 有两种情况：如果是Ajax请求，则返回 Json字符串；如果是普通请求，则 返回重定向命令
       /// </summary>
       /// <param name="IsNoLogin">判断是未登录还是没有权限</param>
       /// <param name="url"></param>
       /// <param name="action"></param>
       /// <returns></returns>
        public ActionResult Redirect(bool IsLogin, ActionDescriptor action)
        {
            //如果Ajax请求没有权限，就返回 Json消息
            if (action.IsDefined(typeof(AjaxRequestAttribute), false)
            || action.ControllerDescriptor.IsDefined(typeof(AjaxRequestAttribute), false))
            {
                if (IsLogin)
                {
                    return RedirectAjax("nologin", null, null, "/Login/Login/Index");
                }
                else
                {
                    Uri MyUrl = Request.UrlReferrer;
                    string url = MyUrl.ToString();
                    return RedirectAjax("nopermission", "您没有权限访问此页面", null, url);
                }
            }
            else//如果 超链接或表单 没有权限访问，js代码
            {
                if (IsLogin)
                {
                    ContentResult result = new ContentResult();
                    //跳回登陆页面
                    result.Content = "<script type='text/javascript'>alert('您还没有登陆呦!');parent.location='" +"/Login/Login/Index"+ "'</script>"; ;
                    return result;
                }
                else
                {
                    //返回上一级URL
                    Uri MyUrl = Request.UrlReferrer;
                    string url = MyUrl.ToString();
                    ContentResult result = new ContentResult();
                    result.Content = "<script type='text/javascript'>alert('您没有权限访问此页面!');window.location='" + url + "'</script>";
                    return result;
                }
            }
        }
        #endregion


        #region 4.1发送系统消息
        /// <summary>
        /// 给个别人发送系统消息（如用户注册提示修改密码等）
        /// </summary>
        /// <param name="receiveId">接收者的Id</param>
        /// <param name="title">系统消息的标题</param>
        /// <param name="sysContent">系统消息的内容</param>
        /// <returns>发送成功返回ok ， 否则发送失败</returns>
    /*    public string SendSysMsg(string receiveId, string title, string sysContent)
        {
            MODEL.T_MemberInformation user = Session["ainfo"] as MODEL.T_MemberInformation;
            MODEL.SystemMessage model = new MODEL.SystemMessage();
            model.ReceiveId = receiveId;
            model.SendId = user.StuNum;
            model.Title = title;
            model.Content = sysContent;
            model.SendTime = DateTime.Now;

            int result = OperateContext.Current.BLLSession.IstemMessageBLL.Add(model);
            if (result > 0)
            {
                return "ok";
            }
            else
            {
                return "nook";
            }

        }*/
        /// <summary>
        /// 给多人发送系统消息，
        /// </summary>
        /// <param name="receiveId">接收者的Id用“；”隔开，开头末尾不要加“；”</param>
        /// <param name="title">系统消息的标题</param>
        /// <param name="sysContent">系统消息的内容</param>
        /// <returns>发送成功返回ok ， 否则发送失败</returns>
        public string SendSysMsg(string receiveId, string title, string sysContent)
        {
            MODEL.T_MemberInformation user = Session["ainfo"] as MODEL.T_MemberInformation;
            string[] arr = receiveId.Split(';');
            foreach (var ri in arr)
            {
                MODEL.SystemMessage model = new MODEL.SystemMessage();
                model.ReceiveId = ri;
                model.SendId = user.StuNum;
                model.Title = title;
                model.Content = sysContent;
                model.SendTime = DateTime.Now;

                if (OperateContext.Current.BLLSession.IstemMessageBLL.Add(model) <= 0)
                {
                    return "nook";
                }
            }
            return "ok";
        }
        /// <summary>
        /// 给各部门发送系统消息（如任务发布等）
        /// </summary>
        /// <param name="department">部门Id</param>
        /// <param name="title">系统消息标题</param>
        /// <param name="sysContent">系统消息的内容</param>
        /// <returns></returns>
        public string SendSysMsg(int department, string title, string sysContent)
        {
            MODEL.T_MemberInformation user = Session["ainfo"] as MODEL.T_MemberInformation;
            List<MODEL.T_MemberInformation> list = new List<MODEL.T_MemberInformation>();
            list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.Department == department);
            foreach (var mi in list)
            {
                int result = 0;
                MODEL.SystemMessage model = new MODEL.SystemMessage();
                model.ReceiveId = mi.StuNum;
                model.SendId = user.StuNum;
                model.Title = title;
                model.Content = sysContent;
                model.SendTime = DateTime.Now;
                result = OperateContext.Current.BLLSession.IstemMessageBLL.Add(model);
                if (result <= 0)
                {
                    return "nook";
                }
            }
            return "ok";
        }

        /// <summary>
        /// 给整个实验室全体成员发送系统消息（如部门活动等）
        /// </summary>
        /// <param name="title">系统消息的标题</param>
        /// <param name="sysContent">系统消息的内容</param>
        /// <returns></returns> 
        public string SendSysMsg(string title, string sysContent)
        {
            MODEL.T_MemberInformation user = Session["ainfo"] as MODEL.T_MemberInformation;
            List<MODEL.T_MemberInformation> list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.IsDelete == false);
            foreach (var mi in list)
            {
                MODEL.SystemMessage model = new MODEL.SystemMessage();
                model.SendId = user.StuNum;
                model.ReceiveId = mi.StuNum;
                model.Title = title;
                model.Content = sysContent;
                model.SendTime = DateTime.Now;
                if (OperateContext.Current.BLLSession.IstemMessageBLL.Add(model) <= 0)
                {
                    return "nook";
                }
            }
            return "ok";
        }
        #endregion
    }
}
