using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FinalWeb.JoinUs
{
    /// <summary>
    /// 试卷管理
    /// </summary>
    public class PaperManageController : Controller
    {
        /////
        #region 1.0 首页 +ActionResult Index()
        /// <summary>
        /// 1.0 首页
        /// </summary>
        public ActionResult Index()
        {
            if (Request.Params["IsNull"] != null)
            {
                Response.Write("<script>alert('空卷子不能发布!');</script>");
            }
            int id;
            if (Request.Params["pageid"] == null)
            {
                id = 1;
            }
            else {
                id = int.Parse(Request.Params["pageid"]);
            }
            //查询所有记录
            List<MODEL.T_Paper> list =
                OperateContext.Current.BLLSession.IPaperBLL.GetListBy(q => q.IsDel == false).ToList();
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
            //获取发布中试卷的信息
            List<MODEL.T_Paper> listcount =
                OperateContext.Current.BLLSession.IPaperBLL.GetListBy(q => q.IsDel == false&&q.IsPublished==true).ToList();
            ViewData["publishingPaperName"] = "无";
            List<string> arr = new List<string>();
            foreach (MODEL.T_Paper p in listcount)
            {

                arr.Add(p.PaperName.ToString());
                arr.Add(",");
               
            }
            ViewData["publishingPaperName"] = arr;
            return View();
        }
        #endregion
        /////
        #region 2.0 返回分页数据 +ActionResult GetPageData()
        /// <summary>
        /// 2.0 返回分页数据
        /// </summary>
        /// <returns></returns>
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
            List<MODEL.T_Paper> list =
                OperateContext.Current.BLLSession.IPaperBLL
                .GetPagedList(pageIndex, pageSize, q => q.IsDel == false, qb => qb.ID).ToList();
            //转为DTOModel
            List<MODEL.DTOModel.T_PaperDTO> listDTO = new List<MODEL.DTOModel.T_PaperDTO>();

            foreach (MODEL.T_Paper p in list)
            {
                //限制试卷名称长度
                //if (p.PaperName.Length > 28)
                //{
                //    p.PaperName = p.PaperName.Substring(0, 40) + "...";
                //}
                MODEL.DTOModel.T_PaperDTO pDTO = new MODEL.DTOModel.T_PaperDTO();
                pDTO = p.ToDTO();
                //查找试卷对应出卷人ID的姓名
                //pDTO.PaperProducerName =
                //    OperateContext.Current.BLLSession.ITeacherInfoBLL.GetListBy(ti => ti.ID == pDTO.PaperProducerID)
                //    .Select(ti => ti.Name).FirstOrDefault();
                listDTO.Add(pDTO);
            }
            //计算总页数和总记录条数
            list =
                OperateContext.Current.BLLSession.IPaperBLL.GetListBy(q => q.IsDel == false).ToList();
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
        #region 3.0 删除试卷 +ActionResult Del(int id)
        /// <summary>
        /// 3.0 删除试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Del()
        {
            int id = int.Parse(Request.Params["id"]);
            string pageIndexNow = Request.Params["pageIndexNow"];
            //1.0 删除试卷
            //根据id查找对应记录
            MODEL.T_Paper model =
                OperateContext.Current.BLLSession
                .IPaperBLL.GetListBy(q => q.ID == id).FirstOrDefault();
            //设置IsDel标记为true
            model.IsDel = true;
            OperateContext.Current.BLLSession.IPaperBLL.Modify(model, "IsDel");
            //2.0 删除该试卷与题目的映射
            List<MODEL.T_PaperQuestion> list = 
                OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(pq => pq.PaperID == id).ToList();
            //foreach (MODEL.T_PaperQuestion p in list)
            //{
            //    OperateContext.Current.BLLSession.IPaperQuestionBLL.Del(p);
            //}
            //删除成功后跳转到原来的页面
            return Redirect("/JoinUs/PaperManage/Index?pageid=" + pageIndexNow);
        }
        #endregion
        /////
        #region 4.1 进入新增页面 +ActionResult Add()
        /// <summary>
        /// 4.1 进入新增页面
        /// </summary>
        public ActionResult Add()
        {
            MODEL.T_MemberInformation mem= Session["ainfo"] as MODEL.T_MemberInformation;
            ViewData["name"] = mem.StuName;
            return View();
        }
        #endregion
        /////
        #region 4.2 确认新增 +ActionResult ConfirmAdd()
        /// <summary>
        /// 4.2 确认新增
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmAdd()
        {
            string paperName = Request.Form["paperName"];
            string producerId = Request.Form["Name"];
            int typeId = int.Parse(Request.Form["typeId"]);
            MODEL.T_Paper model = new MODEL.T_Paper()
            {
                AddDate = DateTime.Now,
                PaperName = paperName,
                PaperProducerID = producerId,
                typeId=typeId
            };
            int result = OperateContext.Current.BLLSession.IPaperBLL.Add(model);
            return Redirect("/JoinUs/PaperManage/Index");
        }
        #endregion
        /////
        #region 5.0 加载出卷人信息 +GetProducerInfo()
        /// <summary>
        /// 5.0 加载出卷人信息
        /// </summary>
        public ActionResult GetProducerInfo()
        {
            //并转为DTO格式
            List<MODEL.DTOModel.T_TeacherInfoDTO> list =
                OperateContext.Current.BLLSession.ITeacherInfoBLL.GetListBy(ti => ti.ID > 0)
                .Select(ti => ti.ToDTO()).ToList();
            MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
            {
                Data = list,
                BackUrl = "",
                Msg = "ok",
                Statu = "ok"
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /////
        #region 6.0 发布试卷 +ActionResult PubPaper()
        /// <summary>
        /// 6.0 发布试卷
        /// </summary>
        public ActionResult PubPaper()
        {
            //当前页码
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //试卷ID
            int paperId = int.Parse(Request.Params["paperId"]);
            MODEL.T_Paper list =
                OperateContext.Current.BLLSession
                .IPaperBLL.GetListBy(p => p.ID == paperId).First();
            int id = list.typeId;
            MODEL.T_Paper listid = OperateContext.Current.BLLSession.IPaperBLL.GetListBy(p => p.typeId == id && p.IsPublished == true).FirstOrDefault();
            if (listid != null)
            {
                listid.IsPublished = false;
                OperateContext.Current.BLLSession.IPaperBLL.Modify(listid, "IsPublished");
            }
            ////修改之前发布中的试卷状态为“false”
            //MODEL.T_Paper modelBefore =
            //    OperateContext.Current.BLLSession
            //    .IT_PaperBLL.GetListBy(p => p.IsPublished == true).FirstOrDefault();
            //if (modelBefore != null)
            //{
            //    modelBefore.IsPublished = false;
            //    OperateContext.Current.BLLSession.IT_PaperBLL.Modify(modelBefore, "IsPublished");
            //}
            //根据id查找对应model
            List<MODEL.T_PaperQuestion> listp = OperateContext.Current.BLLSession.IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId).ToList();
            if (listp.Count != 0)
            {
                MODEL.T_Paper model =
                    OperateContext.Current.BLLSession
                    .IPaperBLL.GetListBy(p => p.ID == paperId && p.typeId == id).First();
                //修改为发布状态并更新到数据库
                model.IsPublished = true;
                OperateContext.Current.BLLSession.IPaperBLL.Modify(model, "IsPublished");
                return Redirect("/JoinUs/PaperManage/Index?pageid=" + pageIndex);
            }
            else
            {
                return Redirect("/JoinUs/PaperManage/Index?IsNull=1&&pageid=" + pageIndex);
            }

        }
        #endregion
        /////
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
        /////
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
        /////
        #region 9.0 取消发布试卷 +ActionResult CancelPubPaper()
        /// <summary>
        /// 6.0 发布试卷
        /// </summary>
        public ActionResult CancelPubPaper()
        {
            //当前页码
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //试卷ID
            int paperId = int.Parse(Request.Params["paperId"]);
            MODEL.T_Paper list =
                OperateContext.Current.BLLSession
                .IPaperBLL.GetListBy(p => p.ID == paperId).First();
            int id = list.typeId;
            MODEL.T_Paper listid = OperateContext.Current.BLLSession.IPaperBLL.GetListBy(p => p.typeId == id && p.IsPublished == true).FirstOrDefault();
            if (listid != null)
            {
                listid.IsPublished = false;
                OperateContext.Current.BLLSession.IPaperBLL.Modify(listid, "IsPublished");
            }
            
            MODEL.T_Paper model =
                OperateContext.Current.BLLSession
                .IPaperBLL.GetListBy(p => p.ID == paperId && p.typeId == id).First();
            //修改为发布状态并更新到数据库
            model.IsPublished = true;
            OperateContext.Current.BLLSession.IPaperBLL.Modify(model, "IsPublished");


            return Redirect("/JoinUs/PaperManage/Index?pageid=" + pageIndex);
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
