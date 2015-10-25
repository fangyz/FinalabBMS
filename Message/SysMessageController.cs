using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Message
{
    public class SysMessageController : Controller
    {
        #region 加载系统消息+public ActionResult Index()
        /// <summary>
        /// 加载系统消息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;
            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();

            int pageIndex = 1;
            string flage = Request.QueryString["flage"] == null ? null : Request.QueryString["flage"].ToString();
            if (flage == "next")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) + 1;
            }
            else if (flage == "front")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) - 1;
            }

            int pageSize = Request.QueryString["PageSize"] == null ? 8 : int.Parse(Request.QueryString["PageSize"]);
            pageModel.PageIndex = pageIndex;
            int count = OperateContext.Current.BLLSession.IstemMessageBLL.GetListBy(u => u.ReceiveId == user.StuNum).Count();
            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / (float)pageSize) == 0 ? 1 : (int)Math.Ceiling(count / (float)pageSize);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }

            List<MODEL.SystemMessage> list = OperateContext.Current.BLLSession.IstemMessageBLL.GetPagedList(pageIndex, pageSize, u => u.ReceiveId == user.StuNum, u => u.SendTime);

            ViewData["sysMsgList"] = list;
            ViewData["pageModel"] = pageModel;
            return View();
        }
        #endregion

        #region 删除系统消息+public string DelSysMsg(FormCollection form)
        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelSysMsg(FormCollection form)
        {
            int msgId = int.Parse(form["MsgId"]);
            int result = OperateContext.Current.BLLSession.IstemMessageBLL.DelBy(u => u.Id == msgId);
            if (result > 0)
            {
                return "ok";
            }
            else
            {
                return "nook";
            }
        }
        #endregion

        #region 系统消息详细内容+ public ActionResult MsgInfo()
        /// <summary>
        /// 系统消息详细内容
        /// </summary>
        /// <returns></returns>
        public ActionResult MsgInfo()
        {
            int msgId = int.Parse(Request.QueryString["msgId"]);
            string result = SetStatus(msgId);
            List<MODEL.SystemMessage> list = OperateContext.Current.BLLSession.IstemMessageBLL.GetListBy(u => u.Id == msgId);
            MODEL.SystemMessage model = new MODEL.SystemMessage();
            foreach (var sm in list)
            {
                model.Title = sm.Title;
                model.Content = sm.Content;
                model.SendTime = sm.SendTime;
                model.SendId = sm.SendId;
                model.ReceiveId = sm.ReceiveId;
            }

            ViewData["msgInfo"] = model;
            return View();
        }
        #endregion

        #region 设置消息为已读状态+public string SetStatus(FormCollection form)
        /// <summary>
        /// 设置消息为已读状态
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SetStatus(int msgId)
        {
            List<MODEL.SystemMessage> list = OperateContext.Current.BLLSession.IstemMessageBLL.GetListBy(u => u.Id == msgId);
            MODEL.SystemMessage msg = new MODEL.SystemMessage();
            foreach (var sm in list)
            {
                msg = sm;
            }
            msg.IsRead = true;
            int result = OperateContext.Current.BLLSession.IstemMessageBLL.Modify(msg, "IsRead");
            if (result > 0)
            {
                return "ok";
            }
            else
            {
                return "nook";
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
