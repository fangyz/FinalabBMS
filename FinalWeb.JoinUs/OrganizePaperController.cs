using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalWeb.JoinUs
{
    /// <summary>
    /// 整理试卷（v1.0-未完成）
    /// </summary>
    public class OrganizePaperController : Controller
    {
        #region 1.0 根据id找到指定试卷的"整理试卷界面" +ActionResult Index()
        /// <summary>
        /// 1.0 根据id找到指定试卷的"整理试卷界面"
        /// </summary>
        public ActionResult Index()
        {
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //试卷Id
            int paperId = int.Parse(Request.Params["paperId"]);
            //查询所有记录
            List<MODEL.T_Question> list =
                OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => q.IsDel == false).ToList();
            //默认页容量为10
            int recordCount = list.Count;
            //计算总页数
            int pageCount;
            if (recordCount % 10 == 0)
                pageCount = recordCount / 10;
            else
                pageCount = recordCount / 10 + 1;
            //如果页码越界，则默认访问尾页
            if (pageIndex > pageCount)
                ViewData["pageIndex"] = pageCount;
            else
                ViewData["pageIndex"] = pageIndex;
            ViewData["paperId"] = paperId;
            //查找试卷名称
            MODEL.T_Paper model = OperateContext.Current
                .BLLSession.IPaperBLL.GetListBy(p => p.ID == paperId).FirstOrDefault();
            List<MODEL.T_PaperQuestion> listid =
                OperateContext.Current.BLLSession.IPaperQuestionBLL.GetListBy(p => p.PaperID ==paperId ).OrderBy(p=>p.QuestionID).ToList();
            List<MODEL.DTOModel.T_PaperQuestionDTO> listDTO = new List<MODEL.DTOModel.T_PaperQuestionDTO>();
           
            List<string> arr = new List<string>();
            foreach (MODEL.T_PaperQuestion q in listid)
            {
                arr.Add(q.QuestionID.ToString());
                arr.Add(",");
            }
           
            ViewData["count"] = listid.Count;
            ViewData["paperIdcount"] = arr;
            ViewData["paperName"] = model.PaperName;
            return View();
        }
        #endregion

        #region 2.0 返回分页数据 +ActionResult GetPageData()
        /// <summary>
        /// 2.0 返回分页数据
        /// </summary>
        public ActionResult GetPageData()
        {
            //页码
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //页容量
            int pageSize = int.Parse(Request.Params["pageSize"]);
            //试卷Id
            int paperId = int.Parse(Request.Params["paperId"]);
            //总页数
            int pageCount = 0;
            //总记录条数
            int recordCount = 0;
            //获取分页数据
            List<MODEL.T_Question> list =
                OperateContext.Current.BLLSession.IQuestionBLL
                .GetPagedList(pageIndex, pageSize, q => q.IsDel == false, qb => qb.ID).ToList();
            //转为DTOModel
            List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
            foreach (MODEL.T_Question q in list)
            {
                //限制题目内容长度
                if (q.QuestionContent.Length > 45)
                {
                    q.QuestionContent = q.QuestionContent.Substring(0, 45) + "...";
                }
                //转为DTOmodel
                MODEL.DTOModel.T_QuestionDTO qDTO = q.ToDTO();
                //判断该题目是否存在于试卷中
                if (OperateContext.Current.BLLSession.IPaperQuestionBLL
                    .GetListBy(pq => pq.PaperID == paperId & pq.QuestionID == q.ID)
                    .FirstOrDefault() != null)
                {
                    qDTO.IsAdded = true;
                    listDTO.Add(qDTO);
                }
                else
                    listDTO.Add(qDTO);
            }
            //计算总页数和总记录条数
            list =
                OperateContext.Current.BLLSession
                .IQuestionBLL.GetListBy(q => q.IsDel == false).ToList();
            recordCount = list.Count;
            if (recordCount % pageSize == 0)
                pageCount = recordCount / pageSize;
            else
                pageCount = recordCount / pageSize + 1;
            //封装为Json数据 
            MODEL.FormatModel.AjaxPagedModel jsonData = new MODEL.FormatModel.AjaxPagedModel()
            {
                Data = listDTO,
                BackUrl = null,
                Msg = "ok",
                Statu = "ok",
                PageCount = pageCount,
                RecordCount = recordCount
            };
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageId"] = paperId;
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 3.0 添加到试卷 +ActionResult AddToPaper()
        /// <summary>
        /// 3.0 添加到试卷
        /// </summary>
        public ActionResult AddToPaper()
        {
            int paperId = int.Parse(Request.Params["paperId"]);
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            int questionId = int.Parse(Request.Params["questionId"]);
            //判断是否已经添加，没有添加则添加
            if (OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId && p.QuestionID == questionId)
                .FirstOrDefault() == null)
            {
                MODEL.T_PaperQuestion model = new MODEL.T_PaperQuestion()
                {
                    PaperID = paperId,
                    QuestionID = questionId
                };
                OperateContext.Current.BLLSession.IPaperQuestionBLL.Add(model);
            }
            return Redirect("/JoinUs/OrganizePaper/Index?paperId="
                + paperId + "&pageIndex=" + pageIndex);
        }
        #endregion

        #region 4.0 从试卷删除 +ActionResult DelFromPaper()
        /// <summary>
        /// 4.0 从试卷删除
        /// </summary>
        public ActionResult DelFromPaper()
        {
            int paperId = int.Parse(Request.Params["paperId"]);
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            int questionId = int.Parse(Request.Params["questionId"]);
            //根据关键字查找到对应的model
            MODEL.T_PaperQuestion model = OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId && p.QuestionID == questionId)
                .FirstOrDefault();
            //移除对应的model
            OperateContext.Current.BLLSession.IPaperQuestionBLL.Del(model);
            return Redirect("/JoinUs/OrganizePaper/Index?paperId="
                + paperId + "&pageIndex=" + pageIndex);
        }
        #endregion

        #region 5.0 根据id找到指定试卷的"整理试卷界面" +ActionResult Indextype(int id)
        /// <summary>
        /// 1.0 根据id找到指定试卷的"整理试卷界面"
        /// </summary>
        public ActionResult Indextype()
        {

            int questionTypeId;
            string questioncontent;
            if (Request.Params["questionType"] != null)
            {
                string questionType = Request.Params["questionType"];
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("Type", questionType);
                Response.Cookies.Add(cookie);
                questionTypeId = int.Parse(Request.Params["questionType"]);
            }
            else
            {
                System.Web.HttpCookie cookie = Request.Cookies["Type"];
                questionTypeId = int.Parse(cookie.Value);
            }

            if (Request.Params["questioncontent"] != null)
            {
                string content = Request.Params["questioncontent"];
                System.Web.HttpCookie cookie1 = new System.Web.HttpCookie("content1", content);
                Response.Cookies.Add(cookie1);
                questioncontent = Request.Params["questioncontent"];
            }
            else
            {
                HttpCookie cookie1 = Request.Cookies["content1"];
                if (cookie1 == null)
                {
                    questioncontent = "";
                }
                else
                {
                    questioncontent = cookie1.Value;
                }

            }
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //试卷Id
            int paperId = int.Parse(Request.Params["paperId"]);
            if (questionTypeId != 3)
            {

                //查询所有记录
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => q.IsDel == false && q.QuestionTypeID == questionTypeId && q.QuestionContent.Contains(questioncontent)).ToList();
                //默认页容量为10
                int recordCount = list.Count;
                //计算总页数
                int pageCount;
                if (recordCount % 10 == 0)
                    pageCount = recordCount / 10;
                else
                    pageCount = recordCount / 10 + 1;
                //如果页码越界，则默认访问尾页
                if (pageIndex > pageCount)
                    ViewData["pageIndex"] = pageCount;
                else
                    ViewData["pageIndex"] = pageIndex;
                ViewData["paperId"] = paperId;
                ViewData["questionTypeId"] = questionTypeId;
                ViewData["questioncontent"] = questioncontent;
                //查找试卷名称
                MODEL.T_Paper model = OperateContext.Current
                    .BLLSession.IPaperBLL.GetListBy(p => p.ID == paperId).FirstOrDefault();
                List<MODEL.T_PaperQuestion> listid =
                OperateContext.Current.BLLSession.IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId).OrderBy(p => p.QuestionID).ToList();
                List<MODEL.DTOModel.T_PaperQuestionDTO> listDTO = new List<MODEL.DTOModel.T_PaperQuestionDTO>();

                List<string> arr = new List<string>();
                foreach (MODEL.T_PaperQuestion q in listid)
                {
                    arr.Add(q.QuestionID.ToString());
                    arr.Add(",");
                }

                ViewData["count"] = listid.Count;
                ViewData["paperIdcount"] = arr;
                ViewData["paperName"] = model.PaperName;
                return View();
            }
            else if (questionTypeId == 3 && questioncontent != "")
            {
                //查询所有记录
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => q.IsDel == false && q.QuestionContent.Contains(questioncontent)).ToList();
                //默认页容量为10
                int recordCount = list.Count;
                //计算总页数
                int pageCount;
                if (recordCount % 10 == 0)
                    pageCount = recordCount / 10;
                else
                    pageCount = recordCount / 10 + 1;
                //如果页码越界，则默认访问尾页
                if (pageIndex > pageCount)
                    ViewData["pageIndex"] = pageCount;
                else
                    ViewData["pageIndex"] = pageIndex;
                ViewData["paperId"] = paperId;
                ViewData["questionTypeId"] = questionTypeId;
                ViewData["questioncontent"] = questioncontent;
                //查找试卷名称
                MODEL.T_Paper model = OperateContext.Current
                    .BLLSession.IPaperBLL.GetListBy(p => p.ID == paperId).FirstOrDefault();
                List<MODEL.T_PaperQuestion> listid =
                OperateContext.Current.BLLSession.IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId).OrderBy(p => p.QuestionID).ToList();
            List<MODEL.DTOModel.T_PaperQuestionDTO> listDTO = new List<MODEL.DTOModel.T_PaperQuestionDTO>();
           
            List<string> arr = new List<string>();
            foreach (MODEL.T_PaperQuestion q in listid)
            {
                arr.Add(q.QuestionID.ToString());
                arr.Add(",");
            }
           
            ViewData["count"] = listid.Count;
            ViewData["paperIdcount"] = arr;
                ViewData["paperName"] = model.PaperName;
                return View();
            }
            else {
                return Redirect("/JoinUs/OrganizePaper/Index?paperId=" + paperId + "&pageIndex=" + pageIndex);
            }
        }
        #endregion

        #region 6.0 返回分页数据 +ActionResult GetPageDatatype()
        /// <summary>
        /// 2.0 返回分页数据
        /// </summary>
        public ActionResult GetPageDatatype()
        {

            string questioncontent = Request.Params["content"];
            if (questioncontent == ",")
            {
                questioncontent = "";
            }
            int id;
            if (Request.Params["typeId"] != "")
            {
                id = int.Parse(Request.Params["typeId"]);
            }
            else
            {
                id = 3;
            }
            //页码
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //页容量
            int pageSize = int.Parse(Request.Params["pageSize"]);
            //试卷Id
            int paperId = int.Parse(Request.Params["paperId"]);
            //总页数
            int pageCount = 0;
            //总记录条数
            int recordCount = 0;
            if (id != 3)
            {
                //获取分页数据
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL
                    .GetPagedList(pageIndex, pageSize, q => (q.IsDel == false&& q.QuestionTypeID == id && q.QuestionContent.Contains(questioncontent)), qb => qb.ID).ToList();
                //转为DTOModel
                List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
                foreach (MODEL.T_Question q in list)
                {
                    //限制题目内容长度
                    if (q.QuestionContent.Length > 45)
                    {
                        q.QuestionContent = q.QuestionContent.Substring(0, 45) + "...";
                    }
                    //转为DTOmodel
                    MODEL.DTOModel.T_QuestionDTO qDTO = q.ToDTO();
                    //判断该题目是否存在于试卷中
                    if (OperateContext.Current.BLLSession.IPaperQuestionBLL
                        .GetListBy(pq => pq.PaperID == paperId & pq.QuestionID == q.ID)
                        .FirstOrDefault() != null)
                    {
                        qDTO.IsAdded = true;
                        listDTO.Add(qDTO);
                    }
                    else
                        listDTO.Add(qDTO);
                }
                //计算总页数和总记录条数
                list =
                    OperateContext.Current.BLLSession
                    .IQuestionBLL.GetListBy(q => (q.IsDel == false && q.QuestionTypeID == id && q.QuestionContent.Contains(questioncontent))).ToList();
                recordCount = list.Count;
                if (recordCount % pageSize == 0)
                    pageCount = recordCount / pageSize;
                else
                    pageCount = recordCount / pageSize + 1;
                //封装为Json数据 
                MODEL.FormatModel.AjaxPagedModel jsonData = new MODEL.FormatModel.AjaxPagedModel()
                {
                    Data = listDTO,
                    BackUrl = null,
                    Msg = "ok",
                    Statu = "ok",
                    PageCount = pageCount,
                    RecordCount = recordCount
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //获取分页数据
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL
                    .GetPagedList(pageIndex, pageSize, q => (q.IsDel == false  && q.QuestionContent.Contains(questioncontent)), qb => qb.ID).ToList();
                //转为DTOModel
                List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
                foreach (MODEL.T_Question q in list)
                {
                    //限制题目内容长度
                    if (q.QuestionContent.Length > 45)
                    {
                        q.QuestionContent = q.QuestionContent.Substring(0, 45) + "...";
                    }
                    //转为DTOmodel
                    MODEL.DTOModel.T_QuestionDTO qDTO = q.ToDTO();
                    //判断该题目是否存在于试卷中
                    if (OperateContext.Current.BLLSession.IPaperQuestionBLL
                        .GetListBy(pq => pq.PaperID == paperId & pq.QuestionID == q.ID)
                        .FirstOrDefault() != null)
                    {
                        qDTO.IsAdded = true;
                        listDTO.Add(qDTO);
                    }
                    else
                        listDTO.Add(qDTO);
                }
                //计算总页数和总记录条数
                list =
                    OperateContext.Current.BLLSession
                    .IQuestionBLL.GetListBy(q => (q.IsDel == false &&  q.QuestionContent.Contains(questioncontent))).ToList();
                recordCount = list.Count;
                if (recordCount % pageSize == 0)
                    pageCount = recordCount / pageSize;
                else
                    pageCount = recordCount / pageSize + 1;
                //封装为Json数据 
                MODEL.FormatModel.AjaxPagedModel jsonData = new MODEL.FormatModel.AjaxPagedModel()
                {
                    Data = listDTO,
                    BackUrl = null,
                    Msg = "ok",
                    Statu = "ok",
                    PageCount = pageCount,
                    RecordCount = recordCount
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 7.0 预览试卷 +ActionResult PreviewPaper(int id)
        /// <summary>
        /// 7.0 预览试卷
        /// </summary>
        public ActionResult PreviewPaper(int id)
        {
            ViewData["paperId"] = id;
            return View();
        }
        #endregion

        #region 8.0 获取试卷中的题目列表 +ActionResult GetPaperQuestionList(int id)
        /// <summary>
        /// 8.0 获取试卷中的题目列表
        /// </summary>
        public ActionResult GetPaperQuestionList(int id)
        {
            List<MODEL.T_Question> list =
                OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == id)
                .Select(p => p.T_Question).OrderBy(p => p.QuestionTypeID).ToList();
            //如果试卷中题目为空
            if (list.Count == 0)
            {
                MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
                {
                    Data = null,
                    BackUrl = "",
                    Statu = "null",
                    Msg = "该试卷并没有题目~"
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //DTO格式数据
                List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
                foreach (MODEL.T_Question q in list)
                {
                    //DTO
                    MODEL.DTOModel.T_QuestionDTO qDTO = q.ToDTO();
                    //简答题
                    if (q.QuestionTypeID == 2)
                    {
                        listDTO.Add(qDTO);
                    }
                    else if (q.QuestionTypeID == 1)
                    {
                        List<MODEL.T_QuestionOption> qoList =
                            OperateContext.Current.BLLSession
                            .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == q.ID).ToList();
                        foreach (MODEL.T_QuestionOption qo in qoList)
                        {
                            qDTO.T_QuestionOption.Add(qo.ToDTO());
                        }
                        listDTO.Add(qDTO);
                    }
                }
                //返回JSON
                MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
                {
                    Data = listDTO,
                    BackUrl = "",
                    Statu = "ok",
                    Msg = "ok"
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        //#region 处理异常 protected override void OnException(ExceptionContext filterContext)
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    GetAbnormal ab = new GetAbnormal();
        //    ab.Abnormal(filterContext);
        //    base.OnException(filterContext);
        //}
        //#endregion
    }
}
