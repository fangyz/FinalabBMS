using MVC.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Message
{
    public class InsideMsgController : Controller
    {

        #region 收件箱,获取所有收件箱的消息+public ActionResult Index()
        public ActionResult Index()
        {
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;// as MODEL.T_MemberInformation;

            #region 真正把消息人和接收人都彻底删除的消息彻底删除
            //用户进入模块主页时，才真正把消息人和接收人都彻底删除的消息彻底删除
            List<MODEL.Tbl_User_Message> trueDel = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => (u.SendId == user.StuNum || u.ReceiveId == user.StuNum) && u.RecTrueDel == true && u.SendTrueDel == true);
            foreach (var del in trueDel)
            {
                OperateContext.Current.BLLSession.Il_User_MessageBLL.DelBy(u => u.Id == del.Id); //删除发送接收表
                OperateContext.Current.BLLSession.Il_MessageBLL.DelBy(u => u.Id == del.MessageId); //删除信息表
            }
            #endregion

            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            MODEL.Tbl_Message message = new MODEL.Tbl_Message();
            List<MODEL.ViewModel.SendBox> sendBoxList = new List<MODEL.ViewModel.SendBox>();
            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();
            string receiveId = user.StuNum;

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

            int pageSize = Request.QueryString["PageSize"] == null ? 5 : int.Parse(Request.QueryString["PageSize"]);
            pageModel.PageIndex = pageIndex;

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.ReceiveId == receiveId && u.ReceiveIsDelete == false && (u.RecTrueDel == false || u.RecTrueDel == null));

            int count = list.Count();

            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / (float)pageSize) == 0 ? 1 : (int)Math.Ceiling(count / (float)pageSize);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }



            foreach (MODEL.Tbl_User_Message userModel in list)
            {
                MODEL.ViewModel.SendBox sendBoxModel = new MODEL.ViewModel.SendBox();

                List<MODEL.T_MemberInformation> menber = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userModel.SendId);
                foreach (MODEL.T_MemberInformation menberModel in menber)
                {
                    sendBoxModel.SendName = menberModel.StuName;

                    List<MODEL.Tbl_Message> messageModel = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(m => m.Id == userModel.MessageId);
                    foreach (MODEL.Tbl_Message mes in messageModel)
                    {
                        sendBoxModel.MessageTitle = mes.Title;
                        sendBoxModel.SendTime = mes.SendTime.ToString();
                        sendBoxModel.MessageId = mes.Id;
                    }
                }
                sendBoxModel.SendId = userModel.SendId;
                sendBoxModel.UserMessageId = userModel.Id;

                sendBoxModel.IsRead = userModel.IsRead;
                sendBoxModel.IsDraft = userModel.IsDraft;
                sendBoxModel.ReceiveIsDelete = userModel.ReceiveIsDelete;
                sendBoxModel.SendIsDelete = userModel.SendIsDelete;

                sendBoxList.Add(sendBoxModel);
            }

            List<MODEL.ViewModel.SendBox> sendBoxList2 = sendBoxList.OrderByDescending(s => Convert.ToDateTime(s.SendTime)).ToList();
            List<MODEL.ViewModel.SendBox> sendBoxList3 = new List<MODEL.ViewModel.SendBox>();

            int p = pageModel.PageIndex * pageSize <= pageModel.MessageCount ? pageModel.PageIndex * pageSize : pageModel.MessageCount;
            for (int i = (pageModel.PageIndex - 1) * pageSize; i < p; i++)
            {
                sendBoxList3.Add(sendBoxList2[i]);
            }


            ViewData["sendBoxList"] = sendBoxList3;
            ViewData["pageModel"] = pageModel;
            return View();
        }
        #endregion

        #region 将收件箱的消息放入垃圾箱+public ActionResult DeleteReceivedBox()
        /// <summary>
        /// 将收件箱的消息放入垃圾箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DeleteReceivedBox(FormCollection form)
        {
            int userMessageId = int.Parse(form["UserMessageId"]);
            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == userMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            userMessage.ReceiveIsDelete = true;
            int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "ReceiveIsDelete");
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

        #region 设置收件箱的消息为未读状态+ public string SetStatus()
        /// <summary>
        /// 设置收件箱的消息为已读状态
        /// </summary>
        /// <returns></returns>
        public string SetStatus(FormCollection form)
        {
            int userMessageId = int.Parse(form["UserMessageId"]);
            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == userMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            userMessage.IsRead = false;
            int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "IsRead");
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

        #region 文件上传+public ActionResult Upload(HttpPostedFileBase fileData)
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
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

        #region 文件下载+public ActionResult DownLoad()
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoad()
        {
            string fileName = Request["fileName"];
            string saveName = Request["saveName"];
            string pathStr = saveName;

            var path = Server.MapPath("~/Areas/uploadFile/" + pathStr);
            return File(path, "application/x-zip-compressed", Url.Encode(fileName));
        }
        #endregion

        #region 发送消息+public ActionResult SendMessage(FormCollection form)
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SendMessage(FormCollection form)
        {
            MODEL.Tbl_Message message = new MODEL.Tbl_Message();
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;

            string receiveName = form["receiveName"];

            string topic = form["topic"];
            string messageContent = form["messageContent"];
            string hiddenFileAddress = form["hiddenFileAddress"];
            //string sendTime = form["sendTime"];

            DateTime sendTime = DateTime.Parse(form["sendTime"]);
            //DateTime sendTime = DateTime.Now;

            message.Title = topic;
            message.Content = messageContent;
            message.SendTime = sendTime;
            message.Atachment = hiddenFileAddress;

            string[] arr = receiveName.Split(';');
            List<string> receiverId = new List<string>();

            //兼容联系人末尾是否有“；”
            if (arr[arr.Length - 1] == "")
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    string[] a = arr[i].Split('|');
                    receiverId.Add(a[a.Length - 1]);
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] a = arr[i].Split('|');
                    receiverId.Add(a[a.Length - 1]);
                }
            }

            if (receiverId.Count() <= 0)
            {
                return "no";
            }
            else
            {
                foreach (string id in receiverId)
                {
                    List<MODEL.T_MemberInformation> model = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == id);
                    if (model.Count() <= 0)
                    {
                        return "nook";
                    }
                }
            }

            //int result =  OperateContext.Current.BLLSession.Il_MessageBLL.Add(message);

            MODEL.Tbl_Message messageList = OperateContext.Current.BLLSession.Il_MessageBLL.AddGetAutoId(message);

            userMessage.MessageId = messageList.Id;
            userMessage.SendId = user.StuNum;
            foreach (string str in receiverId)
            {
                List<MODEL.T_MemberInformation> list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == str);
                userMessage.ReceiveId = str;
                int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Add(userMessage);
                if (result <= 0)
                {
                    return "no";
                }
            }
            return "ok";
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            string receiveId = Request.QueryString["receiveId"] == null ? "" : Request.QueryString["receiveId"];
            string receiveName = Request.QueryString["receiveName"] == null ? "" : Request.QueryString["receiveName"];
            int messageId = Request.QueryString["messageId"] == null ? -1 : int.Parse(Request.QueryString["messageId"]);

            MODEL.ViewModel.SendMessage sendModel = new MODEL.ViewModel.SendMessage();

            if (receiveId != "" && receiveName != "" && messageId != -1)
            {

                sendModel.ReceiveName = receiveName + "|" + receiveId + ";";

                List<MODEL.Tbl_Message> message = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(u => u.Id == messageId);

                foreach (MODEL.Tbl_Message mes in message)
                {
                    sendModel.Title = mes.Title;
                    sendModel.Content = mes.Content;
                    sendModel.Atachment = mes.Atachment;
                }

            }
            else if (receiveId == "" && receiveName == "" && messageId != -1)
            {
                List<MODEL.Tbl_Message> message = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(u => u.Id == messageId);

                foreach (MODEL.Tbl_Message mes in message)
                {
                    sendModel.Title = mes.Title;
                    sendModel.Content = mes.Content;
                    sendModel.Atachment = mes.Atachment;
                }
            }
            else if (receiveId != "" && receiveName != "" && messageId == -1) // 通讯录的写信功能
            {
                sendModel.ReceiveName = receiveName + "|" + receiveId + ";";
            }
            ViewData["sendModel"] = sendModel;
            //加载通讯录
            List<MODEL.T_MemberInformation> list = new List<MODEL.T_MemberInformation>();
            list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum.Length > 0);
            ViewData["MemberInformation"] = list;

            return View();
        }
        #endregion

        #region 保存草稿+public string SaveDraft(FormCollection form)
        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SaveDraft(FormCollection form)
        {
            MODEL.Tbl_Message message = new MODEL.Tbl_Message();
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;

            string receiveName = form["receiveName"];

            string topic = form["topic"];
            string messageContent = form["messageContent"];
            string hiddenFileAddress = form["hiddenFileAddress"];

            message.Title = topic;
            message.Content = messageContent;
            message.SendTime = DateTime.Now;
            message.Atachment = hiddenFileAddress;

            string[] arr = receiveName.Split(';');
            List<string> receiverId = new List<string>();
            for (int i = 0; i < arr.Length - 1; i++)
            {
                string[] a = arr[i].Split('|');
                receiverId.Add(a[a.Length - 1]);
            }

            //int result =  OperateContext.Current.BLLSession.Il_MessageBLL.Add(message);

            MODEL.Tbl_Message messageList = OperateContext.Current.BLLSession.Il_MessageBLL.AddGetAutoId(message);

            userMessage.MessageId = messageList.Id;
            userMessage.IsDraft = true;  //将信息草稿字段激活
            userMessage.SendId = user.StuNum;

            foreach (string str in receiverId)
            {
                userMessage.ReceiveId = str;
                int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Add(userMessage);
                if (result <= 0)
                {
                    return "no";
                }
            }
            return "ok";
        }

        #endregion

        #region 发件箱,获取一发邮件的信息+public ActionResult SendBoxMenu()
        /// <summary>
        /// 加载发信箱
        /// </summary>
        /// <returns></returns>
        public ActionResult SendBoxMenu()
        {
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;

            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            MODEL.Tbl_Message message = new MODEL.Tbl_Message();
            List<MODEL.ViewModel.SendBox> sendBoxList = new List<MODEL.ViewModel.SendBox>();
            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();
            string sendId = user.StuNum;
            int pageIndex = 1;
            int count = 1;
            string flage = Request.QueryString["flage"] == null ? null : Request.QueryString["flage"].ToString();
            if (flage == "next")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) + 1;
            }
            else if (flage == "front")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) - 1;
            }

            int pageSize = Request.QueryString["PageSize"] == null ? 7 : int.Parse(Request.QueryString["PageSize"]);
            pageModel.PageIndex = pageIndex;

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.SendId == sendId && u.SendIsDelete == false && u.IsDraft == false && (u.SendTrueDel == false || u.SendTrueDel == null));

            //总信息数量
            count = list.Count();

            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / (float)pageSize) == 0 ? 1 : (int)Math.Ceiling(count / (float)pageSize);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }

            foreach (MODEL.Tbl_User_Message userModel in list)
            {
                MODEL.ViewModel.SendBox sendBoxModel = new MODEL.ViewModel.SendBox();

                List<MODEL.T_MemberInformation> menber = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userModel.ReceiveId);
                foreach (MODEL.T_MemberInformation menberModel in menber)
                {
                    sendBoxModel.ReceiveName = menberModel.StuName;

                    List<MODEL.Tbl_Message> messageModel = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(m => m.Id == userModel.MessageId);
                    foreach (MODEL.Tbl_Message mes in messageModel)
                    {
                        sendBoxModel.MessageTitle = mes.Title;
                        sendBoxModel.SendTime = mes.SendTime.ToString();
                        sendBoxModel.MessageId = mes.Id;
                    }
                }
                sendBoxModel.ReceiveId = userModel.ReceiveId;
                sendBoxModel.UserMessageId = userModel.Id;

                sendBoxModel.IsRead = userModel.IsRead;
                sendBoxModel.IsDraft = userModel.IsDraft;
                sendBoxModel.ReceiveIsDelete = userModel.ReceiveIsDelete;
                sendBoxModel.SendIsDelete = userModel.SendIsDelete;

                sendBoxList.Add(sendBoxModel);
            }

            //按时间排序
            List<MODEL.ViewModel.SendBox> sendBoxList2 = sendBoxList.OrderByDescending(s => Convert.ToDateTime(s.SendTime.ToString())).ToList();
            //取出每一页的数据
            List<MODEL.ViewModel.SendBox> sendBoxList3 = new List<MODEL.ViewModel.SendBox>();
            int p = (pageModel.PageIndex * pageSize) <= sendBoxList2.Count() ? (pageModel.PageIndex * pageSize) : sendBoxList2.Count();
            for (int i = (pageModel.PageIndex - 1) * pageSize; i < p; i++)
            {
                sendBoxList3.Add(sendBoxList2[i]);
            }
            ViewData["sendBoxList"] = sendBoxList3;
            ViewData["pageModel"] = pageModel;
            return View();
        }
        #endregion

        #region 将发信箱的消息放入垃圾箱+public ActionResult DeleteSendBox()
        /// <summary>
        /// 将发信箱的消息放入垃圾箱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DeleteSendBox(FormCollection form)
        {
            int UserMessageId = int.Parse(form["UserMessageId"]);

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == UserMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            userMessage.SendIsDelete = true;
            int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "SendIsDelete");
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

        #region 垃圾箱,获取垃圾箱中的信息+public ActionResult RecycleBoxMenu()
        public ActionResult RecycleBoxMenu()
        {
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;

            MODEL.Tbl_User_Message sendUserMessage = new MODEL.Tbl_User_Message();
            MODEL.Tbl_Message sendMessage = new MODEL.Tbl_Message();

            List<MODEL.ViewModel.SendBox> sendBoxList = new List<MODEL.ViewModel.SendBox>();
            List<MODEL.ViewModel.SendBox> receiveBoxList = new List<MODEL.ViewModel.SendBox>();
            MODEL.ViewModel.PageModelInMsg sendPpageModel = new MODEL.ViewModel.PageModelInMsg();
            MODEL.ViewModel.PageModelInMsg receivePageModel = new MODEL.ViewModel.PageModelInMsg();

            string sendId = user.StuNum;
            string receiveId = user.StuNum;


            //分页查询处理
            int sendPageIndex = 1;
            int receivePageIndex = 1;
            int sendCount = 1;
            int receiveCount = 1;

            string sendFlage = Request.QueryString["sendFlage"] == null ? null : Request.QueryString["sendFlage"].ToString();
            string receiveFlage = Request.QueryString["receiveFlage"] == null ? null : Request.QueryString["receiveFlage"].ToString();

            if (sendFlage == "next")
            {
                sendPageIndex = Request.QueryString["sendPageIndex"] == null ? 1 : int.Parse(Request.QueryString["sendPageIndex"]) + 1;
            }
            else if (sendFlage == "front")
            {
                sendPageIndex = Request.QueryString["sendPageIndex"] == null ? 1 : int.Parse(Request.QueryString["sendPageIndex"]) - 1;
            }

            if (receiveFlage == "next")
            {
                receivePageIndex = Request.QueryString["receivPageIndex"] == null ? 1 : int.Parse(Request.QueryString["receivPageIndex"]) + 1;
            }
            else if (receiveFlage == "front")
            {
                receivePageIndex = Request.QueryString["receivPageIndex"] == null ? 1 : int.Parse(Request.QueryString["receivPageIndex"]) - 1;
            }

            sendPpageModel.PageIndex = sendPageIndex;
            receivePageModel.PageIndex = receivePageIndex;

            List<MODEL.Tbl_User_Message> sendList = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.SendId == sendId && u.SendIsDelete == true && (u.SendTrueDel == false || u.SendTrueDel == null));
            List<MODEL.Tbl_User_Message> receiveList = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.ReceiveId == receiveId && u.ReceiveIsDelete == true && (u.RecTrueDel == false || u.RecTrueDel == null));

            sendCount = sendList.Count();
            receiveCount = receiveList.Count();

            sendPpageModel.MessageCount = sendCount;
            receivePageModel.MessageCount = receiveCount;

            int sendPageSize = Request.QueryString["sendPageSize"] == null ? 3 : int.Parse(Request.QueryString["sendPageSize"]);
            int receivePageSize = Request.QueryString["receivePageSize"] == null ? 3 : int.Parse(Request.QueryString["receivePageSize"]);

            sendPpageModel.PageCount = (int)Math.Ceiling(sendCount / (float)sendPageSize) == 0 ? 1 : (int)Math.Ceiling(sendCount / (float)sendPageSize);
            receivePageModel.PageCount = (int)Math.Ceiling(receiveCount / (float)receivePageSize) == 0 ? 1 : (int)Math.Ceiling(receiveCount / (float)receivePageSize);

            if (sendPpageModel.PageIndex > sendPpageModel.PageCount)
            {
                sendPpageModel.PageIndex = sendPpageModel.PageCount;
            }
            if (receivePageModel.PageIndex > receivePageModel.PageCount)
            {
                receivePageModel.PageIndex = receivePageModel.PageCount;
            }

            foreach (MODEL.Tbl_User_Message userModel in sendList)
            {
                MODEL.ViewModel.SendBox sendBoxModel = new MODEL.ViewModel.SendBox();

                List<MODEL.T_MemberInformation> menber = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userModel.ReceiveId);
                foreach (MODEL.T_MemberInformation menberModel in menber)
                {
                    sendBoxModel.ReceiveName = menberModel.StuName;

                    List<MODEL.Tbl_Message> messageModel = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(m => m.Id == userModel.MessageId);
                    foreach (MODEL.Tbl_Message mes in messageModel)
                    {
                        sendBoxModel.MessageTitle = mes.Title;
                        sendBoxModel.SendTime = mes.SendTime.ToString();
                        sendBoxModel.MessageId = mes.Id;
                    }
                }
                sendBoxModel.ReceiveId = userModel.ReceiveId;
                sendBoxModel.UserMessageId = userModel.Id;

                sendBoxModel.IsRead = userModel.IsRead;
                sendBoxModel.IsDraft = userModel.IsDraft;
                sendBoxModel.ReceiveIsDelete = userModel.ReceiveIsDelete;
                sendBoxModel.SendIsDelete = userModel.SendIsDelete;

                sendBoxList.Add(sendBoxModel);
            }

            foreach (MODEL.Tbl_User_Message userModel in receiveList)
            {
                MODEL.ViewModel.SendBox sendBoxModel = new MODEL.ViewModel.SendBox();

                List<MODEL.T_MemberInformation> menber = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userModel.SendId);
                foreach (MODEL.T_MemberInformation menberModel in menber)
                {
                    sendBoxModel.SendName = menberModel.StuName;

                    List<MODEL.Tbl_Message> messageModel = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(m => m.Id == userModel.MessageId);
                    foreach (MODEL.Tbl_Message mes in messageModel)
                    {
                        sendBoxModel.MessageTitle = mes.Title;
                        sendBoxModel.SendTime = mes.SendTime.ToString();
                        sendBoxModel.MessageId = mes.Id;
                    }
                }
                sendBoxModel.SendId = userModel.SendId;
                sendBoxModel.UserMessageId = userModel.Id;

                sendBoxModel.IsRead = userModel.IsRead;
                sendBoxModel.IsDraft = userModel.IsDraft;
                sendBoxModel.ReceiveIsDelete = userModel.ReceiveIsDelete;
                sendBoxModel.SendIsDelete = userModel.SendIsDelete;

                receiveBoxList.Add(sendBoxModel);
            }

            List<MODEL.ViewModel.SendBox> sendBoxList2 = sendBoxList.OrderByDescending(s => Convert.ToDateTime(s.SendTime)).ToList();
            List<MODEL.ViewModel.SendBox> receiveBoxList2 = receiveBoxList.OrderByDescending(s => Convert.ToDateTime(s.SendTime)).ToList();

            List<MODEL.ViewModel.SendBox> sendBoxList3 = new List<MODEL.ViewModel.SendBox>();
            List<MODEL.ViewModel.SendBox> receiveBoxList3 = new List<MODEL.ViewModel.SendBox>();

            int p1 = sendPpageModel.PageIndex * sendPageSize <= sendPpageModel.MessageCount ? sendPpageModel.PageIndex * sendPageSize : sendPpageModel.MessageCount;
            int p2 = receivePageModel.PageIndex * receivePageSize <= receivePageModel.MessageCount ? receivePageModel.PageIndex * receivePageSize : receivePageModel.MessageCount;

            for (int i = (sendPpageModel.PageIndex - 1) * sendPageSize; i < p1; i++)
            {
                sendBoxList3.Add(sendBoxList2[i]);
            }

            for (int i = (receivePageModel.PageIndex - 1) * receivePageSize; i < p2; i++)
            {
                receiveBoxList3.Add(receiveBoxList2[i]);
            }

            ViewData["sendBoxList"] = sendBoxList3;
            ViewData["receiveBoxList"] = receiveBoxList3;
            ViewData["sendPpageModel"] = sendPpageModel;
            ViewData["receivePageModel"] = receivePageModel;
            ViewData["user"] = user;
            return View();
        }
        #endregion

        #region 将垃圾箱中的发送消息彻底删除+public ActionResult DelMesInRec()
        /// <summary>
        /// 将垃圾箱中的发送消息彻底删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DelSendMesInRec(FormCollection form)
        {
            int UserMessageId = int.Parse(form["UserMessageId"]);

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == UserMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            userMessage.SendTrueDel = true;
            int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "SendTrueDel");
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

        #region 将垃圾箱中的收件彻底删除+public ActionResult DelRecMesInRec()
        /// <summary>
        /// 将垃圾箱中的收件彻底删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DelRecMesInRec(FormCollection form)
        {
            int UserMessageId = int.Parse(form["UserMessageId"]);

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == UserMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            userMessage.RecTrueDel = true;
            int result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "RecTrueDel");
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

        #region 恢复垃圾箱中的发送消息+ public ActionResult RestoreMessage()
        /// <summary>
        /// 回复垃圾箱中的发送消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string RestoreMessage(FormCollection form)
        {
            int UserMessageId = int.Parse(form["UserMessageId"]);
            string flag = form["Flag"];

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == UserMessageId);
            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            foreach (MODEL.Tbl_User_Message um in list)
            {
                userMessage = um;
            }
            int result = -1;
            if (flag == "send")
            {
                userMessage.SendIsDelete = false;
                result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "SendIsDelete");
            }
            else
            {
                userMessage.ReceiveIsDelete = false;
                result = OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(userMessage, "ReceiveIsDelete");
            }
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

        #region 通讯录,获取通讯录的信息+public ActionResult MessageAddressMenu()
        [HttpGet]
        public ActionResult MessageAddressMenu()
        {
            List<MODEL.T_MemberInformation> list = new List<MODEL.T_MemberInformation>();
            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();
            int pageIndex = 1;
            int count = 1;
            string flage = Request.QueryString["flage"] == null ? null : Request.QueryString["flage"].ToString();
            if (flage == "next")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) + 1;
            }
            else if (flage == "front")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) - 1;
            }

            int pageSize = Request.QueryString["PageSize"] == null ? 7 : int.Parse(Request.QueryString["PageSize"]);
            pageModel.PageIndex = pageIndex;
            count = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum.Length > 0).Count();
            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / (float)pageSize) == 0 ? 1 : (int)Math.Ceiling(count / (float)pageSize);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }


            list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetPagedList(pageModel.PageIndex, pageSize, u => u.StuNum.Length > 0, u => u.StuNum);

            ViewData["list"] = list;
            ViewData["pageModel"] = pageModel;
            return View();
        }
        #endregion

        #region (未实现)联系人的搜索+public ActionResult SearchMember()
        /// <summary>
        /// 联系人的搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchMember()
        {
            string searchText = Request.QueryString["appendedInputButtons"];

            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();
            int pageIndex = 1;
            int count = 1;
            string flage = Request.QueryString["flage"] == null ? null : Request.QueryString["flage"].ToString();
            if (flage == "next")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) + 1;
            }
            else if (flage == "front")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) - 1;
            }

            pageModel.PageIndex = pageIndex;
            count = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum.Length > 0).Count();
            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / 5f) == 0 ? 1 : (int)Math.Ceiling(count / 5f);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }


            List<MODEL.T_MemberInformation> list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetPagedList(pageModel.PageIndex, 5, u => u.StuName.Contains(searchText) || u.StuNum.Contains(searchText) || u.QQNum.Contains(searchText), u => u.StuNum);

            List<MODEL.ViewModel.MenberInfo> listToJson = new List<MODEL.ViewModel.MenberInfo>();
            foreach (MODEL.T_MemberInformation mi in list)
            {
                MODEL.ViewModel.MenberInfo model = new MODEL.ViewModel.MenberInfo();
                model.StuNum = mi.StuNum;
                model.StuName = mi.StuName;
                model.QQNum = mi.QQNum;
                model.TelNum = null;
                model.Email = mi.Email;
                listToJson.Add(model);
            }

            return Json(listToJson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 草稿箱,获取草稿箱中的信息+public ActionResult DraftBoxMenu()
        public ActionResult DraftBoxMenu()
        {
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;

            MODEL.Tbl_User_Message userMessage = new MODEL.Tbl_User_Message();
            MODEL.Tbl_Message message = new MODEL.Tbl_Message();
            List<MODEL.ViewModel.SendBox> sendBoxList = new List<MODEL.ViewModel.SendBox>();
            MODEL.ViewModel.PageModelInMsg pageModel = new MODEL.ViewModel.PageModelInMsg();
            string sendId = user.StuNum;
            int pageIndex = 1;
            int count = 1;
            string flage = Request.QueryString["flage"] == null ? null : Request.QueryString["flage"].ToString();
            if (flage == "next")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) + 1;
            }
            else if (flage == "front")
            {
                pageIndex = Request.QueryString["pageIndex"] == null ? 1 : int.Parse(Request.QueryString["pageIndex"]) - 1;
            }

            int pageSize = Request.QueryString["PageSize"] == null ? 7 : int.Parse(Request.QueryString["PageSize"]);
            pageModel.PageIndex = pageIndex;

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.SendId == sendId && u.SendIsDelete == false && u.IsDraft == true && (u.SendTrueDel == false || u.SendTrueDel == null));

            count = list.Count();
            pageModel.MessageCount = count;
            pageModel.PageCount = (int)Math.Ceiling(count / (float)pageSize) == 0 ? 1 : (int)Math.Ceiling(count / (float)pageSize);

            if (pageModel.PageIndex > pageModel.PageCount)
            {
                pageModel.PageIndex = pageModel.PageCount;
            }


            foreach (MODEL.Tbl_User_Message userModel in list)
            {
                MODEL.ViewModel.SendBox sendBoxModel = new MODEL.ViewModel.SendBox();

                List<MODEL.T_MemberInformation> menber = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userModel.ReceiveId);
                foreach (MODEL.T_MemberInformation menberModel in menber)
                {
                    sendBoxModel.ReceiveName = menberModel.StuName;

                    List<MODEL.Tbl_Message> messageModel = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(m => m.Id == userModel.MessageId);
                    foreach (MODEL.Tbl_Message mes in messageModel)
                    {
                        sendBoxModel.MessageTitle = mes.Title;
                        sendBoxModel.SendTime = mes.SendTime.ToString();
                        sendBoxModel.MessageId = mes.Id;
                    }
                }
                sendBoxModel.ReceiveId = userModel.ReceiveId;
                sendBoxModel.UserMessageId = userModel.Id;

                sendBoxModel.IsRead = userModel.IsRead;
                sendBoxModel.IsDraft = userModel.IsDraft;
                sendBoxModel.ReceiveIsDelete = userModel.ReceiveIsDelete;
                sendBoxModel.SendIsDelete = userModel.SendIsDelete;

                sendBoxList.Add(sendBoxModel);
            }

            List<MODEL.ViewModel.SendBox> sendBoxList2 = sendBoxList.OrderByDescending(s => Convert.ToDateTime(s.SendTime)).ToList();
            List<MODEL.ViewModel.SendBox> sendBoxList3 = new List<MODEL.ViewModel.SendBox>();

            int p = pageModel.PageIndex * pageSize <= pageModel.MessageCount ? pageModel.PageIndex * pageSize : pageModel.MessageCount;

            for (int i = (pageModel.PageIndex - 1) * pageSize; i < p; i++)
            {
                sendBoxList3.Add(sendBoxList2[i]);
            }

            ViewData["sendBoxList"] = sendBoxList3;
            ViewData["pageModel"] = pageModel;
            return View();
        }
        #endregion

        #region 查看收件箱消息的详情+public ActionResult MessageContent()
        /// <summary>
        /// 查看收件箱消息的详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageContent()
        {
            MODEL.ViewModel.MessagaContentModel model = new MODEL.ViewModel.MessagaContentModel();

            MODEL.T_MemberInformation user = OperateContext.Current.Usr;//Session["user"] as MODEL.T_MemberInformation;
            model.ReceiveName = user.StuName + "|" + user.StuNum + ";";

            int userMessgeId = int.Parse(Request.QueryString["userMessgeId"]);
            int messageId = int.Parse(Request.QueryString["messageId"]);
            string sendId = Request.QueryString["sendId"];

            List<MODEL.Tbl_User_Message> list = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == userMessgeId);
            MODEL.Tbl_User_Message usermessage = new MODEL.Tbl_User_Message();

            foreach (var um in list)
            {
                usermessage = um;
            }
            usermessage.IsRead = true;


            //设置消息为已读状态
            OperateContext.Current.BLLSession.Il_User_MessageBLL.Modify(usermessage, "IsRead");


            List<MODEL.Tbl_Message> message = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(u => u.Id == messageId);
            List<MODEL.Tbl_User_Message> userMessage = OperateContext.Current.BLLSession.Il_User_MessageBLL.GetListBy(u => u.Id == userMessgeId);

            string sendIdTemple = null;
            string sendNameTemple = null;
            foreach (MODEL.Tbl_User_Message um in userMessage)
            {
                sendIdTemple = um.SendId;
            }
            List<MODEL.T_MemberInformation> member = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == sendId);
            foreach (MODEL.T_MemberInformation mi in member)
            {
                sendNameTemple = mi.StuName;
            }
            model.SendName = sendNameTemple + "|" + sendIdTemple + ";";

            foreach (MODEL.Tbl_Message mi in message)
            {
                model.Title = mi.Title;
                model.Content = mi.Content;
                model.Atachment = mi.Atachment;
                model.SendTime = mi.SendTime;
            }
            ViewData["messageInfo"] = model;

            return View();
        }
        #endregion

        #region 查看垃圾箱和发件箱信息的详情+public ActionResult MessageInfo()
        /// <summary>
        /// 查看垃圾箱和发件箱信息的详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageInfo()
        {
            string receiveId = Request.QueryString["receiveId"];
            string receiveName = Request.QueryString["receiveName"];
            int messageId = int.Parse(Request.QueryString["messageId"]);
            string pageId = Request.QueryString["pageId"];

            MODEL.ViewModel.MessagaContentModel model = new MODEL.ViewModel.MessagaContentModel();
            model.ReceiveName = receiveName;

            List<MODEL.Tbl_Message> list = OperateContext.Current.BLLSession.Il_MessageBLL.GetListBy(u => u.Id == messageId);
            foreach (MODEL.Tbl_Message info in list)
            {
                model.Title = info.Title;
                model.Content = info.Content;
                model.Atachment = info.Atachment;
                model.SendTime = info.SendTime;
            }
            ViewData["info"] = model;
            ViewData["pageId"] = pageId;
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
