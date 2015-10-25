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
    /// 题库管理
    /// </summary>
    public class QuestionManageController : Controller
    {
        #region 1.0 首页 +ActionResult Index()
        /// <summary>
        /// 1.0 首页
        /// </summary>
        public ActionResult Index()
        {
                
                int id;
                if (Request.Params["pageid"] == null)
                {
                    id = 1;
                }
                else
                {
                   id = int.Parse(Request.Params["pageid"]);
                }
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
                if (id > pageCount)
                    ViewData["pageIndex"] = pageCount;
                else
                    ViewData["pageIndex"] = id;
                return View();
            
           
            
        }
        #endregion
        /////
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
                listDTO.Add(q.ToDTO());
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
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /////
        #region 3.0 删除题目 +ActionResult Del()
        /// <summary>
        /// 3.0 删除题目
        /// </summary>
        public ActionResult Del()
        {
            int id = int.Parse(Request.Params["id"]);
            string pageIndexNow = Request.Params["pageIndexNow"];
            //根据id查找对应记录
            MODEL.T_Question model =
                OperateContext.Current.BLLSession
                .IQuestionBLL.GetListBy(q => q.ID == id).FirstOrDefault();
            //设置IsDel标记为true
            model.IsDel = true;
            int result = OperateContext.Current.BLLSession.IQuestionBLL.Modify(model, "IsDel");
            //删除成功后跳转到原来的页面
            return Redirect("/JoinUs/QuestionManage/Index?pageid="+ pageIndexNow);
        }
        #endregion
        /////
        #region 4.0 GetModelById +ActionResult GetModelById()
        /// <summary>
        /// 4.0 GetModelById
        /// </summary>
        public ActionResult GetModelById()
        {
            int id = int.Parse(Request.QueryString["id"]);
            MODEL.T_Question model
                = OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => q.ID == id).First();
            MODEL.DTOModel.T_QuestionDTO modelDTO = model.ToDTO();
            //如果是选择题，则需要查找选项
            if (model.QuestionTypeID == 1)
            {
                List<MODEL.T_QuestionOption> qoList =
                    OperateContext.Current.BLLSession
                    .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == model.ID).ToList();
                foreach (MODEL.T_QuestionOption qo in qoList)
                {
                    modelDTO.T_QuestionOption.Add(qo.ToDTO());
                }
            }
            MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
            {
                BackUrl = null,
                Data = modelDTO,
                Msg = "ok",
                Statu = "ok"
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /////
        #region 5.1 进入新增页面 +ActionResult Add()
        /// <summary>
        /// 5.1 进入新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }
        #endregion
        /////
        #region 5.2 确认新增 +ActionResult ConfirmAdd()
        /// <summary>
        /// 5.2 确认新增
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmAdd()
        {
            //题目类型：1选择题；2简答题；
            int questionType = int.Parse(Request.Form["questionType"]);
            //题目内容
            string questionContent = questionContent = Request.Form["questionContent"];
            int questionGrade = int.Parse(Request.Form["questionGrade"]);
            string questionTag = Request.Form["questionTag"];
            MODEL.T_Question model = new MODEL.T_Question()
            {
                QuestionTypeID = questionType,
                QuestionContent = questionContent,
                QuestionGrade=questionGrade,
                QuestionTag=questionTag
            };
            //题目插入DB，返回result
            int result = OperateContext.Current.BLLSession.IQuestionBLL.Add(model);
            //如果是选择题，则分别插入选项
            if (questionType == 1)
            {
                string optionIDs = "ABCD";
                string optionContent;
                string optionWeight;
                foreach (char id in optionIDs)
                {
                    optionContent = Request.Form["option" + id + "Content"];
                    optionWeight = Request.Form["option" + id + "Weight"];
                    MODEL.T_QuestionOption qo = new MODEL.T_QuestionOption()
                    {
                        QuestionID = model.ID,
                        OptionID = id.ToString(),
                        OptionContent = optionContent,
                        OptionWeight = int.Parse(optionWeight)
                    };
                    OperateContext.Current.BLLSession.IQuestionOptionBLL.Add(qo);
                }
            }
            //跳转到列表第一页
            return Redirect("/JoinUs/QuestionManage/Index");
        }
        #endregion
        /////
        #region 6.1 进入修改页面 +ActionResult Modify()
        /// <summary>
        /// 6.1 进入修改页面
        /// </summary>
        public ActionResult Modify(int id)
        {
            ViewData["questionId"] = id;
            return View();
        }
        #endregion
        /////
        #region 6.2 确定修改 +ActionResult ConfirmModify(int id)
        /// <summary>
        /// 6.2 确定修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmModify(int id)
        {
            //题目类型：1选择题；2简答题；
            int questionType = int.Parse(Request.Form["questionType"]);
            //题目内容
            string questionContent = questionContent = Request.Form["questionContent"];
            int questionGrade = int.Parse(Request.Form["questionGrade"]);
            string questionTag = Request.Form["questionTag"];

            MODEL.T_Question model
                = OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => q.ID == id).First();
            model.QuestionContent = questionContent;
            model.QuestionGrade = questionGrade;
            model.QuestionTag = questionTag;
            //确认修改题目内容
            OperateContext.Current.BLLSession.IQuestionBLL.Modify(model, "QuestionContent");
            OperateContext.Current.BLLSession.IQuestionBLL.Modify(model, "QuestionGrade");
            OperateContext.Current.BLLSession.IQuestionBLL.Modify(model, "QuestionTag");
            //如果是选择题，则需要修改选项
            if (questionType == 1)
            {
                List<MODEL.T_QuestionOption> qoList =
                    OperateContext.Current.BLLSession
                    .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == model.ID).ToList();
                foreach (MODEL.T_QuestionOption qo in qoList)
                {
                    qo.OptionContent = Request.Form["option" + qo.OptionID + "Content"];
                    qo.OptionWeight = int.Parse(Request.Form["option" + qo.OptionID + "Weight"]);
                    OperateContext.Current.BLLSession
                        .IQuestionOptionBLL.Modify(qo, "OptionContent", "OptionWeight");
                }
            }
            //跳转到列表第一页
            return Redirect("/JoinUs/QuestionManage/Index");
        }
        #endregion
        /////
        #region 7.0 首页 +ActionResult Indextype()
        /// <summary>
        /// 1.0 首页
        /// </summary>
        public ActionResult Indextype()
        {
            int id = 1;
            int questionTypeId;
            string questioncontent;//= Request.Params["questioncontent"];
            //int questionTypeId = int.Parse(Request.Params["questionType"]);
            if (Request.Params["questionType"] != null)
            {
                string questionType = Request.Params["questionType"];
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("Type", questionType);
                Response.Cookies.Add(cookie);
               
                questionTypeId = int.Parse(Request.Params["questionType"]);
            }
            else {
                HttpCookie cookie=Request.Cookies["Type"];
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

            if (questionTypeId != 3)
            {
                //查询所有记录
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => (q.IsDel == false && q.QuestionTypeID == questionTypeId&&q.QuestionContent.Contains(questioncontent))).ToList();
                //默认页容量为10
                int recordCount = list.Count;
                //计算总页数
                int pageCount;
                if (recordCount % 10 == 0)
                    pageCount = recordCount / 10;
                else
                    pageCount = recordCount / 10 + 1;
                //如果页码越界，则默认访问尾页
                if (id > pageCount)
                    ViewData["pageIndex"] = pageCount;
                else
                {
                    ViewData["pageIndex"] = id;
                    ViewData["questionTypeId"] = questionTypeId;
                    ViewData["questioncontent"] = questioncontent;


                }
                return View();
            }
            else if (questionTypeId == 3&&questioncontent!="")
                {
                    //查询所有记录
                    List<MODEL.T_Question> list =
                        OperateContext.Current.BLLSession.IQuestionBLL.GetListBy(q => (q.IsDel == false  && q.QuestionContent.Contains(questioncontent))).ToList();
                    //默认页容量为10
                    int recordCount = list.Count;
                    //计算总页数
                    int pageCount;
                    if (recordCount % 10 == 0)
                        pageCount = recordCount / 10;
                    else
                        pageCount = recordCount / 10 + 1;
                    //如果页码越界，则默认访问尾页
                    if (id > pageCount)
                        ViewData["pageIndex"] = pageCount;
                    else
                    {
                        ViewData["pageIndex"] = id;
                        ViewData["questionTypeId"] = questionTypeId;
                        ViewData["questioncontent"] = questioncontent;


                    }
                    return View();
            }
            else {
                return Redirect("/JoinUs/QuestionManage/Index");
            }
        }
        #endregion
        /////
        #region 8.0 返回分页数据 +ActionResult GetPageDatatype()
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
            if (id != 3)
            {
                //页码
                int pageIndex = int.Parse(Request.Params["pageIndex"]);
                //页容量
                int pageSize = int.Parse(Request.Params["pageSize"]);
                //总页数
                int pageCount = 0;
                //总记录条数
                int recordCount = 0;
                //获取分页数据
                List<MODEL.T_Question> list =
                    OperateContext.Current.BLLSession.IQuestionBLL
                    .GetPagedList(pageIndex, pageSize, q => (q.IsDel == false && q.QuestionTypeID == id && q.QuestionContent.Contains(questioncontent)), qb => qb.ID).ToList();
                //转为DTOModel
                List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
                foreach (MODEL.T_Question q in list)
                {
                    //限制题目内容长度
                    if (q.QuestionContent.Length > 45)
                    {
                        q.QuestionContent = q.QuestionContent.Substring(0, 45) + "...";
                    }
                    listDTO.Add(q.ToDTO());
                }
                //计算总页数和总记录条数
                list =
                    OperateContext.Current.BLLSession
                    .IQuestionBLL.GetListBy(q => q.IsDel == false && q.QuestionTypeID == id && q.QuestionContent.Contains(questioncontent)).ToList();
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
                //页码
                int pageIndex = int.Parse(Request.Params["pageIndex"]);
                //页容量
                int pageSize = int.Parse(Request.Params["pageSize"]);
                //总页数
                int pageCount = 0;
                //总记录条数
                int recordCount = 0;
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
                    listDTO.Add(q.ToDTO());
                }
                //计算总页数和总记录条数
                list =
                    OperateContext.Current.BLLSession
                    .IQuestionBLL.GetListBy(q => q.IsDel == false && q.QuestionContent.Contains(questioncontent)).ToList();
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
