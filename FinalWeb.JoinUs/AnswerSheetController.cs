using MVC.Helper;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FinalWeb.JoinUs
{
    /// <summary>
    /// 答卷管理
    /// </summary>
    public class AnswerSheetController : Controller
    {
        #region 1.0 答卷管理首页 +ActionResult Index()
        /// <summary>
        /// 1.0 答卷管理首页
        /// </summary>
        public ActionResult Index()
        {

            int id;
            if (Request.Params["pageid"] == null)
            {
                id = 1;
            }else
            {
                id = int.Parse(Request.Params["pageid"]);
            }
            if (Request.Params["Iscom"] != null)
            {
                Response.Write("<script>alert('您已经评论过这张卷子!');</script>");
            }
            //查询所有记录
            List<MODEL.T_AnswerSheet> list =
                OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).ToList();
            //foreach (MODEL.T_AnswerSheet q in list)
            //{
            //    int totalscore = GetChoiceScore(q.InterviewerID) + GetBriefScore(q.InterviewerID);
            //    MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(a => a.ID == q.ID).First();
            //    model.Score = totalscore;
            //    OperateContext.Current.BLLSession.IAnswerSheetBLL.Modify(model, "Score");
            //}
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
            ViewData["count"] = list.Count;
            return View();
        }
        #endregion

        #region 2.0 获取答卷信息 +ActionResult GetAnswerSheetInfo()
        /// <summary>
        /// 2.0 获取分页答卷信息
        /// </summary>
        public ActionResult GetAnswerSheetInfo()
        {

            //List<MODEL.T_AnswerSheet> list =
            //  OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).ToList();
            //foreach (MODEL.T_AnswerSheet q in list)
            //{
            //    int totalscore = GetChoiceScore(q.InterviewerID) + GetBriefScore(q.InterviewerID);
            //    MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(a => a.ID == q.ID).First();
            //    model.Score = totalscore;
            //    OperateContext.Current.BLLSession.IAnswerSheetBLL.Modify(model, "Score");
            //}

            //页码
            int pageIndex = int.Parse(Request.Params["pageIndex"]);
            //页容量
            int pageSize = int.Parse(Request.Params["pageSize"]);
            int count = int.Parse(Request.Params["count"]);
            //总页数
            int pageCount = 0;
            //总记录条数
            int recordCount = 0;

            //List<MODEL.T_AnswerSheet> list1 =
              //  OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).ToList();
            //foreach (MODEL.T_AnswerSheet q in list1)
            //{
            //    int totalscore = GetChoiceScore(q.InterviewerID) + GetBriefScore(q.InterviewerID);
            //    MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(a => a.ID == q.ID).First();
            //    model.Score = totalscore;
            //    OperateContext.Current.BLLSession.IAnswerSheetBLL.Modify(model, "Score");
            //}
            int recordCount1 = count;
            //计算总页数
            int pageCount1;
            if (recordCount % 10 == 0)
                pageCount1 = recordCount1 / 10;
            else
                pageCount1 = recordCount1 / 10 + 1;
            //获取分页信息
            List<MODEL.T_AnswerSheet> list =
                OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetPagedList(pageIndex, pageSize,ref pageCount1, bs => bs.ID > 0, bs => bs.Score,false).ToList();
            //将答卷转为DTOModel
            List<MODEL.DTOModel.T_AnswerSheetDTO> listDTO =
                new List<MODEL.DTOModel.T_AnswerSheetDTO>() { };
            foreach (MODEL.T_AnswerSheet a in list)
            {
                listDTO.Add(a.ToDTO());
            }

            //根据答卷查找到答卷人的详细信息并计算其得分
            foreach (MODEL.DTOModel.T_AnswerSheetDTO aDTO in listDTO)
            {
                //答卷人信息
                MODEL.T_InterviewerInfo infoModel =
                    OperateContext.Current.BLLSession
                    .IInterviewerInfoBLL.GetListBy(ii => ii.ID == aDTO.InterviewerID).First();
                aDTO.InterviewerInfo = infoModel.ToDTO();
                //计算选择题得分
                aDTO.ChoiceScore = GetChoiceScore(infoModel.ID);
                //计算简答题得分
                aDTO.BriefScore = GetBriefScore(infoModel.ID);
                //计算总分
                aDTO.TotalScore = aDTO.BriefScore + aDTO.ChoiceScore;
                //检查该答卷是否已经被评判
                //object oTemp = OperateContext.Current.BLLSession
                //    .IAnswerSheetCommentBLL.GetListBy(a => a.AnswerSheetID == aDTO.ID).FirstOrDefault();
                //if (oTemp != null)
                //{
                //    aDTO.IsRated = true;
                //}
                List<MODEL.T_AnswerSheetComment> lista = OperateContext.Current.BLLSession
                    .IAnswerSheetCommentBLL.GetListBy(q => q.AnswerSheetID == aDTO.ID).ToList();
                if (lista.Count == 3)
                {
                    aDTO.IsRated = true;
                }
                aDTO.Peoplecount = lista.Count;

                
            }
            ////计算总页数和总记录条数
            //list = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).ToList();
            recordCount = count;
            if (recordCount % pageSize == 0)
                pageCount = recordCount / pageSize;
            else
                pageCount = recordCount / pageSize + 1;
            List<MODEL.DTOModel.T_AnswerSheetDTO>  Data1 = listDTO.ToList();
            //答卷DTO转为JsonModel
            MODEL.FormatModel.AjaxPagedModel jsonModel = new MODEL.FormatModel.AjaxPagedModel()
            {
                //Data = listDTO.OrderByDescending(ld=>ld.TotalScore),//按总分排名（分页内）
               Data = Data1,
                BackUrl = "",
                Msg = "ok",
                Statu = "ok",
                PageCount = pageCount,
                RecordCount = recordCount
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 3.0 GetModelById +ActionResult GetModelById(int id)
        /// <summary>
        /// 3.0 GetModelById
        /// </summary>
        public ActionResult GetModelById(int id)
        {
            //根据ID获取面试者的详细信息
            MODEL.T_InterviewerInfo model =
                OperateContext.Current.BLLSession
                .IInterviewerInfoBLL.GetListBy(ii => ii.ID == id).First();
            //返回json信息
            MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
            {
                Data = model.ToDTO(),
                BackUrl = "",
                Msg = "成功",
                Statu = "ok"
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 4.0 删除答卷 +ActionResult Del()
        /// <summary>
        /// 4.0 删除答卷
        /// </summary>
        public ActionResult Del()
        {
            int id = int.Parse(Request.Params["id"]);
            string pageIndexNow = Request.Params["pageIndexNow"];
            //麻痹，关联性太强，还得删掉引用此ID的其它表中的记录
            //1.删除ChoiceAnswerSheet中的记录
            List<MODEL.T_ChoiceAnswerSheet> casList =
                OperateContext.Current.BLLSession
                .IChoiceAnswerSheetBLL.GetListBy(c => c.AnswerSheetID == id).ToList();
            foreach (MODEL.T_ChoiceAnswerSheet cas in casList)
            {
                OperateContext.Current.BLLSession.IChoiceAnswerSheetBLL.Del(cas);
            }
            //2.删除BriefAnswerSheet中的记录
            List<MODEL.T_BriefAnswerSheet> basList =
                OperateContext.Current.BLLSession
                .IBriefAnswerSheetBLL.GetListBy(b => b.AnswerSheetID == id).ToList();
            foreach (MODEL.T_BriefAnswerSheet bas in basList)
            {
               
                //3.删除BriefScore中的记录
                OperateContext.Current.BLLSession
                    .IBriefScoreBLL.Del(OperateContext.Current.BLLSession
                    .IBriefScoreBLL.GetListBy(bs => bs.BriefAnswerSheetID == bas.ID).FirstOrDefault());
                OperateContext.Current.BLLSession.IBriefAnswerSheetBLL.Del(bas);
            }
            //4.删除AnswerSheetComment中的记录
            List<MODEL.T_AnswerSheetComment> ascList = OperateContext.Current.BLLSession
                .IAnswerSheetCommentBLL.GetListBy(a => a.AnswerSheetID == id).ToList();
            foreach (MODEL.T_AnswerSheetComment asc in ascList)
            {
                OperateContext.Current.BLLSession.IAnswerSheetCommentBLL.Del(asc);
            }
            //5.删除AnswerSheet中的记录
            MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetListBy(a => a.ID == id).FirstOrDefault();
            OperateContext.Current.BLLSession.IAnswerSheetBLL.Del(model);
            //删除成功后跳转到原来的页面
            return Redirect("/JoinUs/AnswerSheet/Index?pageid=" + pageIndexNow);
        }
        #endregion

        #region 5.0 进入评卷页面 +ActionResult RaterPaper(int id)
        /// <summary>
        /// 5.0 进入评卷页面
        /// </summary>
        public ActionResult RaterPaper(int id)
        {
            ViewData["answerSheetId"] = id;
            MODEL.T_MemberInformation mem = Session["ainfo"] as MODEL.T_MemberInformation;
            ViewData["name"] = mem.StuName;
            List<MODEL.T_AnswerSheetComment> list1 = OperateContext.Current.BLLSession.IAnswerSheetCommentBLL.GetListBy(q => q.UserCom == mem.StuName).ToList();

            ViewData["count"] = list1.Count;
            List<MODEL.T_AnswerSheetComment> list = OperateContext.Current.BLLSession.IAnswerSheetCommentBLL.GetListBy(q => q.AnswerSheetID == id).ToList();
            foreach (MODEL.T_AnswerSheetComment q in list)
            {
                if (q.UserCom == mem.StuName)
                {
                    
                    return Redirect("/JoinUs/AnswerSheet/Index?Iscom=1");
                    
                }
            }

            return View();
        }
        #endregion

        #region 6.0 获取简答题答卷 +ActionResult GetBriefAnswerSheet()
        /// <summary>
        /// 6.0 获取简答题答卷
        /// </summary>
        public ActionResult GetBriefAnswerSheet()
        {
            int id = int.Parse(Request.Params["id"]);
            //根据答卷ID找到对应的简答题答卷记录
            List<MODEL.T_BriefAnswerSheet> basList = OperateContext.Current.BLLSession
                .IBriefAnswerSheetBLL.GetListBy(b => b.AnswerSheetID == id).ToList();
            //转为DTO
            List<MODEL.DTOModel.T_BriefAnswerSheetDTO> bDTO = new List<MODEL.DTOModel.T_BriefAnswerSheetDTO>();
            foreach (MODEL.T_BriefAnswerSheet b in basList)
            {
                bDTO.Add(b.ToDTO());
            }
            //查找题目对象
            foreach (MODEL.DTOModel.T_BriefAnswerSheetDTO b in bDTO)
            {
                
                b.Question = OperateContext.Current.BLLSession
                    .IQuestionBLL.GetListBy(q => q.ID == b.QuestionID).FirstOrDefault().ToDTO();
            }
            //转为jsonModel
            MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
            {
                Data = bDTO,
                BackUrl = "",
                Msg = "ok",
                Statu = "ok"
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 7.0 获取评卷人信息 +ActionResult GetPaperRaterInfo()
        /// <summary>
        /// 7.0 获取评卷人信息
        /// </summary>
        public ActionResult GetPaperRaterInfo()
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

        #region 8.0 提交评卷结果 +ActionResult CommitRater()
        /// <summary>
        /// 8.0 提交评卷结果
        /// </summary>
        public ActionResult CommitRater()
        {
            //答卷ID
            int answerSheetId = Convert.ToInt32(Request.Params["answerSheetId"]);
            //评卷人ID
            
            string rater = Request.Params["pname"];
            //评语
            string commentContent = Request.Params["commentContent"];
            //插入简答题评分
            //根据答卷ID找到对应的答卷列表
            List<MODEL.T_BriefAnswerSheet> asList = OperateContext.Current.BLLSession
                .IBriefAnswerSheetBLL.GetListBy(b => b.AnswerSheetID == answerSheetId).ToList();
            //获取简答题评分并插入DB
            foreach (MODEL.T_BriefAnswerSheet a in asList)
            {
                int score = Convert.ToInt32(Request.Params[a.QuestionID.ToString()]);
                MODEL.T_BriefScore bsModel = new MODEL.T_BriefScore()
                {
                    BriefAnswerSheetID = a.ID,
                    PaperRaterID = 1,
                    Score = score,
                    UserB=rater
                };
                OperateContext.Current.BLLSession.IBriefScoreBLL.Add(bsModel);
            }

            //插入评语记录
            MODEL.T_AnswerSheetComment commentModel = new MODEL.T_AnswerSheetComment()
            {
                AnswerSheetID = answerSheetId,
                RaterID = 1,
                CommentContent = commentContent,
                UserCom=rater
            };
            OperateContext.Current.BLLSession.IAnswerSheetCommentBLL.Add(commentModel);


          
            
                
                MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(a => a.ID == answerSheetId).First();
                int totalscore = GetChoiceScore(model.InterviewerID) + GetBriefScore(model.InterviewerID);    
                 model.Score = totalscore;
                OperateContext.Current.BLLSession.IAnswerSheetBLL.Modify(model, "Score");
            

            MODEL.T_MemberInformation mem = Session["ainfo"] as MODEL.T_MemberInformation;
            List<MODEL.T_AnswerSheetComment> lista = OperateContext.Current.BLLSession.IAnswerSheetCommentBLL.GetListBy(q =>  q.UserCom==mem.StuName).ToList();
            List<int> list = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID >0).Select(q=>q.ID).ToList();
            List<int> listas = list;
            foreach (MODEL.T_AnswerSheetComment qo in lista)
            {
                listas.Remove(qo.AnswerSheetID);
            }
            List<int> num = listas;
            if (num.Count != 0)
            {
                
                int[] arr=new int[list.Count];
                int i = 0;
                foreach(int q in list)

                {
                    arr[i] = q;
                    i++;
                   
                }
                return Redirect("/JoinUs/AnswerSheet/RaterPaper/" + arr[0]);
            }
            else
            {
                return Redirect("/JoinUs/AnswerSheet/RaterPaper/1");
            }
        }
        #endregion

        #region 9.0 导出Excel +ActionResult ToExcel()
        /// <summary>
        /// 9.0 导出Excel（有错）
        /// </summary>
        public ActionResult ToExcel()
        {
            //整理要导出面试者的ID
            string idArrStrOrigin = Request.Params["idArr"];
            string[] idArrStr = idArrStrOrigin.Split(',');
            int[] idArr = new int[idArrStr.Length - 1];
            for (int i = 0; i < idArrStr.Length; i++)
            {
                if (idArrStr[i] != "")
                    idArr[i] = int.Parse(idArrStr[i]);
            }
            CreateExcel(Response, Request, idArr);
            return null;
        }
        #endregion

        #region 9.1 全部导出Excel +ActionResult ToExcel()
        /// <summary>
        /// 9.0 导出Excel（有错）
        /// </summary>
        public ActionResult TotalToExcel()
        {
            
            CreateExcel(Response, Request);
            return null;
        }
        #endregion

        #region 9.0 导出Word +ActionResult ToWord()
        /// <summary>
        /// 9.0 导出Excel（有错）
        /// </summary>
        public ActionResult ToWord()
        {
            //整理要导出面试者的ID
            string pageIndexNow = Request.Params["pageIndex"];
            string idArrStrOrigin = Request.Params["idArr"];
            int id = int.Parse(pageIndexNow);
            string[] idArrStr = idArrStrOrigin.Split(',');
            int[] idArr = new int[idArrStr.Length - 1];
            for (int i = 0; i < idArrStr.Length; i++)
            {
                if (idArrStr[i] != "")
                    idArr[i] = int.Parse(idArrStr[i]);
            }

            if (CreateWord(Response, Request, idArr) > 0)
            {
               
                //Response.Write(" <script type='text/javascript'>alert('导入成功!')</script>");
                //return Redirect("/JoinUs/AnswerSheet/Index/" + id);
                MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
                {
                    Data = "导出成功",
                    BackUrl = "",
                    Msg = "ok",
                    Statu = "ok"
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
                {
                    Data = "导出失败",
                    BackUrl = "",
                    Msg = "ok",
                    Statu = "ok"
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
               
            }
        }
        #endregion

        #region 10.0 查看试卷 +ActionResult Lookpaper()
        /// <summary>
        /// 10.0 获取试卷中的题目列表
        /// </summary>
        public ActionResult Lookpaper(int id)
        {
            //int id = int.Parse(Request.Form["answerpaperId"]);
            ViewData["paperId"] = id;
            return View();
        }
        #endregion

        #region 11.0 获取试卷中的题目列表 +ActionResult GetPaperList(int id)
        /// <summary>
        /// 11.0 获取试卷中的题目列表
        /// </summary>
        public ActionResult GetPaperList(int id)
        {
            
            MODEL.T_AnswerSheet modelanswer = OperateContext.Current.BLLSession.IAnswerSheetBLL
                .GetListBy(p => p.ID == id).First();
            int interviewId = modelanswer.InterviewerID;
            int paperId = modelanswer.PaperID;

            List<MODEL.T_Question> list =
                OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                .Select(p => p.T_Question).OrderBy(p => p.QuestionTypeID).ToList();

            if (list.Count == 0)
            {
                MODEL.FormatModel.AjaxMsgModel jsonModel = new MODEL.FormatModel.AjaxMsgModel()
                {
                    Data = null,
                    BackUrl = "",
                    Statu = "null",
                    Msg = "该试卷信息已删除~"
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //DTO格式数据
                List<MODEL.DTOModel.T_QuestionDTO> listDTO = new List<MODEL.DTOModel.T_QuestionDTO>();
                int cout = 0;
                foreach (MODEL.T_Question q in list)
                {

                    //DTO
                    MODEL.DTOModel.T_QuestionDTO qDTO = q.ToDTO();

                    //简答题
                    if (q.QuestionTypeID == 2)
                    {
                        cout++;
                        listDTO.Add(qDTO);
                        //查找题目对象
                        MODEL.T_BriefAnswerSheet model = OperateContext.Current.BLLSession.IBriefAnswerSheetBLL
                         .GetListBy(t => t.AnswerSheetID == id && t.QuestionID == q.ID).First();
                        foreach (MODEL.DTOModel.T_QuestionDTO b in listDTO)
                        {
                            b.BriefAnswerSheet = OperateContext.Current.BLLSession
                                .IBriefAnswerSheetBLL.GetListBy(t => t.AnswerSheetID == id && t.QuestionID == q.ID).FirstOrDefault().ToDTO();
                            b.BriefScore = OperateContext.Current.BLLSession
                               .IBriefScoreBLL.GetListBy(t => t.BriefAnswerSheetID == model.ID).FirstOrDefault().ToDTO();
                            List<MODEL.T_BriefScore> listb = OperateContext.Current.BLLSession.IBriefScoreBLL
                                .GetListBy(qo => qo.BriefAnswerSheetID == model.ID).ToList();
                            int i = 0;
                            foreach (MODEL.T_BriefScore m in listb)
                            {
                                if (i == 0)
                                {
                                   qDTO.ScoreF = m.Score;
                                   qDTO.TeacherF = m.UserB;
                                }
                                if (i == 1)
                                { qDTO.ScoreS = m.Score;
                                qDTO.TeacherS = m.UserB;
                                }
                                if (i == 2)
                                { qDTO.ScoreT = m.Score;
                                qDTO.TeacherT = m.UserB;
                                }
                                i++;
                            }
                            
                        }


                    }
                    else if (q.QuestionTypeID == 1)
                    {
                        cout++;
                        List<MODEL.T_QuestionOption> qoList =
                            OperateContext.Current.BLLSession
                            .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == q.ID).ToList();
                        foreach (MODEL.T_QuestionOption qo in qoList)
                        {
                            qDTO.T_QuestionOption.Add(qo.ToDTO());
                        }
                        listDTO.Add(qDTO);
                        foreach (MODEL.DTOModel.T_QuestionDTO b in listDTO)
                        {
                            b.ChoiceAnswerSheet = OperateContext.Current.BLLSession
                                .IChoiceAnswerSheetBLL.GetListBy(t => t.AnswerSheetID == id && t.QuestionID == q.ID).FirstOrDefault().ToDTO();

                        }

                    }
                    if (cout == list.Count)
                    {
                        foreach (MODEL.DTOModel.T_QuestionDTO b in listDTO)
                        {
                            b.AnswerSheetComment = OperateContext.Current.BLLSession
                                .IAnswerSheetCommentBLL.GetListBy(t => t.AnswerSheetID == id).FirstOrDefault().ToDTO();

                        }
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

        #region 12.0 获取总排名 +ActionResult GetAnswerSheetInfoScore()
        /// <summary>
        /// 12.0 获取分页答卷信息
        /// </summary>
        public ActionResult GetInfoRank()
        {

            List<MODEL.T_AnswerSheet> list =
                OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).OrderBy(q=>q.Score).ToList();
            //将答卷转为DTOModel
            List<MODEL.DTOModel.T_AnswerSheetDTO> listDTO =
                new List<MODEL.DTOModel.T_AnswerSheetDTO>() { };
            foreach (MODEL.T_AnswerSheet a in list)
            {
                listDTO.Add(a.ToDTO());
            }
            //根据答卷查找到答卷人的详细信息并计算其得分
            foreach (MODEL.DTOModel.T_AnswerSheetDTO aDTO in listDTO)
            {
                //答卷人信息
                MODEL.T_InterviewerInfo infoModel =
                    OperateContext.Current.BLLSession
                    .IInterviewerInfoBLL.GetListBy(ii => ii.ID == aDTO.InterviewerID).First();
                aDTO.InterviewerInfo = infoModel.ToDTO();
                //计算选择题得分
                aDTO.ChoiceScore = GetChoiceScore(infoModel.ID);
                //计算简答题得分
                aDTO.BriefScore = GetBriefScore(infoModel.ID);
                //计算总分
                aDTO.TotalScore = aDTO.BriefScore + aDTO.ChoiceScore;
                //检查该答卷是否已经被评判
                object oTemp = OperateContext.Current.BLLSession
                    .IAnswerSheetCommentBLL.GetListBy(a => a.AnswerSheetID == aDTO.ID).FirstOrDefault();
                if (oTemp != null)
                {
                    aDTO.IsRated = true;
                }

                
            }
            //计算总页数和总记录条数
            list = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID > 0).ToList();
           
            List<MODEL.DTOModel.T_AnswerSheetDTO> Data1 = listDTO.ToList();
            //答卷DTO转为JsonModel
            MODEL.FormatModel.AjaxPagedModel jsonModel = new MODEL.FormatModel.AjaxPagedModel()
            {
                //Data = listDTO.OrderByDescending(ld=>ld.TotalScore),//按总分排名（分页内）
                Data = Data1,
                BackUrl = "",
                Msg = "ok",
                Statu = "ok",
               
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 创建Excel +void CreateExcel(HttpResponseBase response, HttpRequestBase request)
        /// <summary>
        /// 创建Excel
        /// </summary>
        public static void CreateExcel(HttpResponseBase response, HttpRequestBase request, int[] idArr)
        {
            // 文件名
            //string fileName = DateTime.Now.ToString("yyyyMMdd") + "测试.xls";、
            string fileName = "面试者信息表.xls";
            string UserAgent = request.ServerVariables["http_user_agent"].ToLower();

            // Firfox和IE下输出中文名显示正常 
            if (UserAgent.IndexOf("firefox") == -1)
            {
                fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            }
            response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            // 新建Excel文档
            HSSFWorkbook excel = new HSSFWorkbook();

            // 新建一个Excel页签
            ISheet sheet = excel.CreateSheet("面试者信息表");

            //设置居中
            ICellStyle cellStyle = excel.CreateCellStyle();
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            // 创建新增行
            IRow row = sheet.CreateRow(0);

            // 设计表头
            ICell cell = row.CreateCell(0);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("ID");

            cell = row.CreateCell(1);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学号");

            cell = row.CreateCell(2);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("姓名");

            cell = row.CreateCell(3);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("性别");

            cell = row.CreateCell(4);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学院");

            cell = row.CreateCell(5);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("专业");

            cell = row.CreateCell(6);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("班级");

            cell = row.CreateCell(7);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("QQ");

            cell = row.CreateCell(8);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("E-mail");

            cell = row.CreateCell(9);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("联系方式");

            cell = row.CreateCell(10);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学习经验");

            cell = row.CreateCell(11);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("自我介绍");

            cell = row.CreateCell(12);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("是否应聘技术方向");

            cell = row.CreateCell(13);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("选择题得分");

            cell = row.CreateCell(14);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("简答题得分");

            cell = row.CreateCell(15);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("总分");

            cell = row.CreateCell(16);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("评语");

            //根据答卷
            //表格添加内容
            List<MODEL.T_InterviewerInfo> list
                = OperateContext.Current.BLLSession
                .IInterviewerInfoBLL.GetListBy(i => idArr.Contains(i.ID)).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                row = sheet.CreateRow(i + 1);

                cell = row.CreateCell(0);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].ID);

                cell = row.CreateCell(1);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Num);

                cell = row.CreateCell(2);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Name);

                cell = row.CreateCell(3);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Gender);

                cell = row.CreateCell(4);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Academy);

                cell = row.CreateCell(5);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Major);

                cell = row.CreateCell(6);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Class);

                cell = row.CreateCell(7);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].QQ);

                cell = row.CreateCell(8);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Email);

                cell = row.CreateCell(9);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].TelNum);

                cell = row.CreateCell(10);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].LearningExperience);

                cell = row.CreateCell(11);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].SelfEvaluation);

                cell = row.CreateCell(12);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].IsRequestTech == true ? "是" : "否");

                cell = row.CreateCell(13);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetChoiceScore(list[i].ID));

                cell = row.CreateCell(14);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetBriefScore(list[i].ID));

                cell = row.CreateCell(15);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetTotalScore(list[i].ID));

                cell = row.CreateCell(16);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetComment(list[i].ID));
            }

            // 设置行宽度
            sheet.SetColumnWidth(1, 18 * 256);
            sheet.SetColumnWidth(4, 22 * 256);
            sheet.SetColumnWidth(5, 22 * 256);
            sheet.SetColumnWidth(7, 15 * 256);
            sheet.SetColumnWidth(8, 21 * 256);
            sheet.SetColumnWidth(9, 16 * 256);
            sheet.SetColumnWidth(12, 18 * 256);
            sheet.SetColumnWidth(16, 30 * 256);

            //将Excel内容写入到流中 
            MemoryStream file = new MemoryStream();
            excel.Write(file);
            
            //输出 
            response.BinaryWrite(file.GetBuffer());
            response.End();
        }
        #endregion

        #region 创建Excel +void CreateExcel(HttpResponseBase response, HttpRequestBase request)
        /// <summary>
        /// 创建Excel
        /// </summary>
        public static void CreateExcel(HttpResponseBase response, HttpRequestBase request)
        {
            // 文件名
            //string fileName = DateTime.Now.ToString("yyyyMMdd") + "测试.xls";、
            string fileName = "面试者信息表.xls";
            string UserAgent = request.ServerVariables["http_user_agent"].ToLower();

            // Firfox和IE下输出中文名显示正常 
            if (UserAgent.IndexOf("firefox") == -1)
            {
                fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            }
            response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            // 新建Excel文档
            HSSFWorkbook excel = new HSSFWorkbook();

            // 新建一个Excel页签
            ISheet sheet = excel.CreateSheet("所有面试者信息表");

            //设置居中
            ICellStyle cellStyle = excel.CreateCellStyle();
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            // 创建新增行
            IRow row = sheet.CreateRow(0);

            // 设计表头
            ICell cell = row.CreateCell(0);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("ID");

            cell = row.CreateCell(1);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学号");

            cell = row.CreateCell(2);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("姓名");

            cell = row.CreateCell(3);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("性别");

            cell = row.CreateCell(4);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学院");

            cell = row.CreateCell(5);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("专业");

            cell = row.CreateCell(6);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("班级");

            cell = row.CreateCell(7);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("QQ");

            cell = row.CreateCell(8);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("E-mail");

            cell = row.CreateCell(9);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("联系方式");

            cell = row.CreateCell(10);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("学习经验");

            cell = row.CreateCell(11);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("自我介绍");

            cell = row.CreateCell(12);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("是否应聘技术方向");

            cell = row.CreateCell(13);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("选择题得分");

            cell = row.CreateCell(14);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("简答题得分");

            cell = row.CreateCell(15);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("总分");

            cell = row.CreateCell(16);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("评语");

            //根据答卷
            //表格添加内容
            List<MODEL.T_InterviewerInfo> list
                = OperateContext.Current.BLLSession
                .IInterviewerInfoBLL.GetListBy(i => i.ID>0).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                row = sheet.CreateRow(i + 1);

                cell = row.CreateCell(0);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].ID);

                cell = row.CreateCell(1);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Num);

                cell = row.CreateCell(2);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Name);

                cell = row.CreateCell(3);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Gender);

                cell = row.CreateCell(4);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Academy);

                cell = row.CreateCell(5);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Major);

                cell = row.CreateCell(6);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Class);

                cell = row.CreateCell(7);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].QQ);

                cell = row.CreateCell(8);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].Email);

                cell = row.CreateCell(9);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].TelNum);

                cell = row.CreateCell(10);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].LearningExperience);

                cell = row.CreateCell(11);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].SelfEvaluation);

                cell = row.CreateCell(12);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(list[i].IsRequestTech == true ? "是" : "否");

                cell = row.CreateCell(13);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetChoiceScore(list[i].ID));

                cell = row.CreateCell(14);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetBriefScore(list[i].ID));

                cell = row.CreateCell(15);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetTotalScore(list[i].ID));

                cell = row.CreateCell(16);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(GetComment(list[i].ID));
            }

            // 设置行宽度
            sheet.SetColumnWidth(1, 18 * 256);
            sheet.SetColumnWidth(4, 22 * 256);
            sheet.SetColumnWidth(5, 22 * 256);
            sheet.SetColumnWidth(7, 15 * 256);
            sheet.SetColumnWidth(8, 21 * 256);
            sheet.SetColumnWidth(9, 16 * 256);
            sheet.SetColumnWidth(12, 18 * 256);
            sheet.SetColumnWidth(16, 30 * 256);

            //将Excel内容写入到流中 
            MemoryStream file = new MemoryStream();
            excel.Write(file);

            //输出 
            response.BinaryWrite(file.GetBuffer());
            response.End();
        }
        #endregion

        #region 创建Word +void CreateWord(HttpResponseBase response, HttpRequestBase request)
        /// <summary>
        /// 创建Word
        /// </summary>
        public int CreateWord(HttpResponseBase response, HttpRequestBase request, int[] idArr)
        {
            string tmppath = "F:\\软件\\学习资料\\实验室网站\\FinalWeb\\FinalWeb\\FinalWeb.JoinUs\\模板3.doc";
            
            Aspose.Words.Document doc = new Aspose.Words.Document(tmppath); //载入模板

            List<MODEL.T_InterviewerInfo> list
               = OperateContext.Current.BLLSession
               .IInterviewerInfoBLL.GetListBy(i => idArr.Contains(i.ID)).ToList();
            if (list.Count == 0)
            {
                return -1;
            }
            else
            {
              
                for (int i = 0; i < list.Count; i++)
                {
                    int id = list[i].ID;
                    String[] fieldNames = new String[] { "num", "name", "acdemic", "profession", "content", "choiceanswer", "briefanswer", "allscore", "comment" };
                    MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.InterviewerID == id).First();
                    int paperId = model.PaperID;
                    int aid = model.ID;

                    List<MODEL.T_Question> listq =
                    OperateContext.Current.BLLSession
                    .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                    .Select(p => p.T_Question).OrderBy(p => p.QuestionTypeID).ToList();

                    string content = "";
                    int cout = 0;
                    foreach (MODEL.T_Question q in listq)
                    {

                        cout++;
                        //简答题
                        if (q.QuestionTypeID == 2)
                        {

                            content = content + (cout).ToString() + "." + q.QuestionContent.ToString() + "(分值：" + q.QuestionGrade.ToString() + " 标签：" + q.QuestionTag.ToString() + ")\n";
                            //查找题目对象
                            MODEL.T_BriefAnswerSheet model1 = OperateContext.Current.BLLSession.IBriefAnswerSheetBLL
                             .GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();

                            MODEL.T_BriefScore BriefScore = OperateContext.Current.BLLSession
                                  .IBriefScoreBLL.GetListBy(t => t.BriefAnswerSheetID == model1.ID).First();
                            content = content + "回答：" + model1.Answer.ToString() + "\n" + "得分：" + BriefScore.Score.ToString() + "\n";


                        }
                        else if (q.QuestionTypeID == 1)
                        {
                            content = content + (i + 1).ToString() + "." + q.QuestionContent.ToString() + "(分值：" + q.QuestionGrade + " 标签：" + q.QuestionTag + ")\n";
                            List<MODEL.T_QuestionOption> qoList =
                                OperateContext.Current.BLLSession
                                .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == q.ID).ToList();
                            foreach (MODEL.T_QuestionOption qo in qoList)
                            {
                                content = content + qo.OptionID.ToString() + "." + qo.OptionContent.ToString() + "(" + qo.OptionWeight.ToString() + ")\n";
                            }

                            MODEL.T_ChoiceAnswerSheet ChoiceAnswerSheet = OperateContext.Current.BLLSession
                                 .IChoiceAnswerSheetBLL.GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();
                            content = content + "回答：" + ChoiceAnswerSheet.Answer.ToString() + "\n";


                        }



                    }

                    int cscore = GetChoiceScore(list[i].ID);
                    int gscore = GetBriefScore(list[i].ID);
                    int allsscore = (GetTotalScore(list[i].ID));
                    string comment = GetComment(list[i].ID);
                    Object[] fieldValues = new Object[] { list[i].Num.ToString(), list[i].Name.ToString(), list[i].Academy.ToString(), list[i].Major.ToString() + list[i].Class.ToString(), content.ToString(), cscore.ToString(), gscore.ToString(), allsscore.ToString(), comment.ToString() };
                    doc.MailMerge.Execute(fieldNames, fieldValues);
                    
                    doc.Save("d:\\" + list[i].Name + "---" + list[i].Num + "成绩单.doc" );


                }
                return 1;
            }
           
           
            
            
           
        }
        #endregion

        #region 计算选择题得分 +int GetChoiceScore(int userId)
        /// <summary>
        /// 计算选择题得分
        /// </summary>
        public static int GetChoiceScore(int userId)
        {
            int count = 0;
            //获取用户所对应的答卷ID
            int answerSheetId =
                OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetListBy(a => a.InterviewerID == userId)
                .Select(a => a.ID).FirstOrDefault();
            //获取用户所有选择题的答案
            List<MODEL.T_ChoiceAnswerSheet> choiceList =
                OperateContext.Current.BLLSession
                .IChoiceAnswerSheetBLL.GetListBy(cas => cas.AnswerSheetID == answerSheetId).ToList();

            //计算选择题答案的得分
            foreach (MODEL.T_ChoiceAnswerSheet c in choiceList)
            {
                if (c.Answer != "")
                {
                    int weight = OperateContext.Current.BLLSession.IQuestionOptionBLL
                        .GetListBy(q => q.QuestionID == c.QuestionID && q.OptionID == c.Answer)
                        .Select(q => q.OptionWeight).First();

                    count = count + weight;
                }
                else { count = count + 0; }
            }
            return count;
        }
        #endregion

        #region 计算简答题得分 +int GetBriefScore(int userId)
        /// <summary>
        /// 计算简答题得分
        /// </summary>
        public static int GetBriefScore(int userId)
        {
            int count = 0;
            //获取用户所对应的答卷ID
            int answerSheetId =
                OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetListBy(a => a.InterviewerID == userId)
                .Select(a => a.ID).FirstOrDefault();
            List<MODEL.T_AnswerSheetComment> lista = OperateContext.Current.BLLSession
                    .IAnswerSheetCommentBLL.GetListBy(q => q.AnswerSheetID == answerSheetId).ToList();
           
            for (int j = 0; j < lista.Count; j++)
            {
                string rid = lista[j].UserCom;
                //获取其简答题答卷ID
                List<int> briefAnswerIds = OperateContext.Current.BLLSession
                    .IBriefAnswerSheetBLL.GetListBy(bas => bas.AnswerSheetID == answerSheetId)
                    .Select(bas => bas.ID).ToList();
                //获取简答题得分并计算
                foreach (int i in briefAnswerIds)
                {
                    count += OperateContext.Current.BLLSession
                        .IBriefScoreBLL.GetListBy(bfs => bfs.BriefAnswerSheetID == i&&bfs.UserB==rid)
                        .Select(bfs => bfs.Score).FirstOrDefault();
                }
            }
            if (lista.Count == 0)
            {
                count = 0;
            }
            else
            {
                int num =  lista.Count;
                count = count / num;
            }
            return count;
        }
        #endregion

        #region 计算总分 +int GetTotalScore(int userId)
        /// <summary>
        /// 计算总分
        /// </summary>
        public static int GetTotalScore(int userId)
        {
            int count = 0;
            count = GetChoiceScore(userId) + GetBriefScore(userId);
            return count;
        }
        #endregion

        #region 获取评语 +string GetComment(int userId)
        /// <summary>
        /// 获取评语
        /// </summary>
        public static string GetComment(int userId)
        {
            int answerSheetId =
                OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetListBy(a => a.InterviewerID == userId)
                .Select(a => a.ID).FirstOrDefault();
            String comment = OperateContext.Current.BLLSession
                .IAnswerSheetCommentBLL.GetListBy(a => a.AnswerSheetID == answerSheetId)
                .Select(a => a.CommentContent).FirstOrDefault();
            return comment;
        }
        #endregion

        #region 另外一种导出Word方法
        [ValidateInput(false)]
        public FileResult ExportWord()
        {
            string idArrStrOrigin = Request.Params["idArr"];
            string[] idArrStr = idArrStrOrigin.Split(',');
            int[] idArr = new int[idArrStr.Length - 1];
            for (int i = 0; i < idArrStr.Length; i++)
            {
                if (idArrStr[i] != "")
                    idArr[i] = int.Parse(idArrStr[i]);
            }

            //byte[] byteArray = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.Append("<style type='text/css'> table {  width:700px; border: 1px solid black;  border-collapse:collapse; } tr, tr td { border: 1px solid black; } </style>");
            sb.Append("<body>");
            List<MODEL.T_InterviewerInfo> list
              = OperateContext.Current.BLLSession
              .IInterviewerInfoBLL.GetListBy(i => idArr.Contains(i.ID)).ToList();
            for (int i = 0; i < list.Count; i++)
            {
               
                
                sb.Append("<table ><tr><td>学号</td><td>" + list[i].Num.ToString() + "</td><td>姓名</td><td>" + list[i].Name.ToString() + "</td></tr>");
                sb.Append("<tr ><td>学院</td><td>" + list[i].Academy.ToString() + "</td><td>专业班级</td><td>" + list[i].Major.ToString() + list[i].Class.ToString() + "</td></tr>");
                int id = list[i].ID;
                MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.InterviewerID == id).First();
                int paperId = model.PaperID;
                int aid = model.ID;

                List<MODEL.T_Question> listq =
                OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                .Select(p => p.T_Question).OrderBy(p => p.QuestionTypeID).ToList();


                int cout = 0;
                sb.Append("<tr><td colspan='4'>试卷内容</td></tr>");
                foreach (MODEL.T_Question q in listq)
                {  
                    cout++;
                    //简答题
                    if (q.QuestionTypeID == 2)
                    {
                        #region MyRegion
                        sb.Append("<tr><td colspan='4'>" + (cout).ToString() + "." + q.QuestionContent.ToString() + "</td></tr>");
                        sb.Append("<tr><td>分值</td><td>" + q.QuestionGrade.ToString() + "</td><td>标签</td><td>" + q.QuestionTag.ToString() + "</td></tr>");

                        //查找题目对象
                        MODEL.T_BriefAnswerSheet model1 = OperateContext.Current.BLLSession.IBriefAnswerSheetBLL
                         .GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();

                        MODEL.T_BriefScore BriefScore = OperateContext.Current.BLLSession
                              .IBriefScoreBLL.GetListBy(t => t.BriefAnswerSheetID == model1.ID).First();

                        sb.Append("<tr><td>回答</td><td colspan='3'>" + model1.Answer.ToString() + "</td></tr>");
                        List<MODEL.T_BriefScore> listb = OperateContext.Current.BLLSession.IBriefScoreBLL
                                .GetListBy(qo => qo.BriefAnswerSheetID == model1.ID).ToList();
                        sb.Append("<tr><td>得分</td>");
                        foreach (MODEL.T_BriefScore h in listb)
                        {
                            sb.Append("<td>"+h.UserB.ToString()+"评分：" + h.Score.ToString() + "</td>");
                        }
                        sb.Append("</tr>"); 
                        #endregion
                    }
                    else if (q.QuestionTypeID == 1)
                    {
                        #region MyRegion
                        sb.Append("<tr><td colspan='4'>" + (cout).ToString() + "." + q.QuestionContent.ToString() + "</td></tr>");
                        sb.Append("<tr><td>分值</td><td>" + q.QuestionGrade.ToString() + "</td><td>标签</td><td>" + q.QuestionTag.ToString() + "</td></tr>");

                        List<MODEL.T_QuestionOption> qoList =
                            OperateContext.Current.BLLSession
                            .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == q.ID).ToList();
                        foreach (MODEL.T_QuestionOption qo in qoList)
                        {
                            sb.Append("<tr><td>" + qo.OptionID.ToString() + "." + "</td><td colspan='3'>" + qo.OptionContent.ToString() + "(" + qo.OptionWeight.ToString() + "）</td></tr>");
                        }
                        MODEL.T_ChoiceAnswerSheet ChoiceAnswerSheet = OperateContext.Current.BLLSession
                             .IChoiceAnswerSheetBLL.GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();
                        sb.Append("<tr><td>回答</td><td colspan='3'>" + ChoiceAnswerSheet.Answer.ToString() + "</td></tr>"); 
                        #endregion
                    }                   
                } 
                int cscore = GetChoiceScore(list[i].ID);
                int gscore = GetBriefScore(list[i].ID);
                int allsscore = (GetTotalScore(list[i].ID));
                string comment = GetComment(list[i].ID);
                sb.Append("<tr><td>选择题得分</td><td >" + cscore.ToString() + "</td><td>简答题得分</td><td >" + gscore.ToString() + "</td></tr>");
                sb.Append("<tr><td>总分</td><td colspan='3'>" + allsscore.ToString() + "</td></tr>");
                sb.Append("<tr><td>评价</td><td colspan='3'>" + comment.ToString() + "</td></tr>");
                sb.Append("</table>");
               
                
            }


            sb.Append("</body>");
            var byteArray = System.Text.Encoding.Default.GetBytes(sb.ToString());
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            return File(byteArray, "application/ms-word", "面试试卷" + ".doc");
           
        } 
        #endregion

        #region 另外一种全部导出Word方法
        [ValidateInput(false)]
        public FileResult TotalToWord()
        {


            //byte[] byteArray = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.Append("<style type='text/css'> table {  width:700px; border: 1px solid black;  border-collapse:collapse; } tr, tr td { border: 1px solid black; } </style>");
            sb.Append("<body>");
            //List<MODEL.T_InterviewerInfo> list
            //  = OperateContext.Current.BLLSession
            //  .IT_InterviewerInfoBLL.GetListBy(i =>i.ID>0).ToList();
            List<MODEL.T_AnswerSheetComment> list
             = OperateContext.Current.BLLSession
             .IAnswerSheetCommentBLL.GetListBy(i => i.ID > 0).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                int id = list[i].AnswerSheetID;
                MODEL.T_AnswerSheet model = OperateContext.Current.BLLSession.IAnswerSheetBLL.GetListBy(q => q.ID == id).First();
                int paperId = model.PaperID;
                int aid = model.ID;
                int infoid = model.InterviewerID;

                MODEL.T_InterviewerInfo infomodel = OperateContext.Current.BLLSession.IInterviewerInfoBLL.GetListBy(q => q.ID == infoid).First();
                sb.Append("<table ><tr><td>学号</td><td>" + infomodel.Num.ToString() + "</td><td>姓名</td><td>" + infomodel.Name.ToString() + "</td></tr>");
                sb.Append("<tr ><td>学院</td><td>" + infomodel.Academy.ToString() + "</td><td>专业班级</td><td>" + infomodel.Major.ToString() + infomodel.Class.ToString() + "</td></tr>");

                List<MODEL.T_Question> listq =
                OperateContext.Current.BLLSession
                .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                .Select(p => p.T_Question).OrderBy(p => p.QuestionTypeID).ToList();


                int cout = 0;
                sb.Append("<tr><td colspan='4'>试卷内容</td></tr>");
                foreach (MODEL.T_Question q in listq)
                {
                    cout++;
                    //简答题
                    if (q.QuestionTypeID == 2)
                    {
                        #region MyRegion
                        sb.Append("<tr><td colspan='4'>" + (cout).ToString() + "." + q.QuestionContent.ToString() + "</td></tr>");
                        sb.Append("<tr><td>分值</td><td>" + q.QuestionGrade.ToString() + "</td><td>标签</td><td>" + q.QuestionTag.ToString() + "</td></tr>");

                        //查找题目对象
                        MODEL.T_BriefAnswerSheet model1 = OperateContext.Current.BLLSession.IBriefAnswerSheetBLL
                         .GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();

                        MODEL.T_BriefScore BriefScore = OperateContext.Current.BLLSession
                              .IBriefScoreBLL.GetListBy(t => t.BriefAnswerSheetID == model1.ID).First();

                        sb.Append("<tr><td>回答</td><td colspan='3'>" + model1.Answer.ToString() + "</td></tr>");
                        List<MODEL.T_BriefScore> listb = OperateContext.Current.BLLSession.IBriefScoreBLL
                               .GetListBy(qo => qo.BriefAnswerSheetID == model1.ID).ToList();
                        sb.Append("<tr><td>得分</td>");
                        foreach (MODEL.T_BriefScore h in listb)
                        {
                            sb.Append("<td>" + h.UserB.ToString() + "评分：" + h.Score.ToString() + "</td>");
                        }
                        sb.Append("</tr>");
                        #endregion
                    }
                    else if (q.QuestionTypeID == 1)
                    {
                        #region MyRegion
                        sb.Append("<tr><td colspan='4'>" + (cout).ToString() + "." + q.QuestionContent.ToString() + "</td></tr>");
                        sb.Append("<tr><td>分值</td><td>" + q.QuestionGrade.ToString() + "</td><td>标签</td><td>" + q.QuestionTag.ToString() + "</td></tr>");

                        List<MODEL.T_QuestionOption> qoList =
                            OperateContext.Current.BLLSession
                            .IQuestionOptionBLL.GetListBy(qo => qo.QuestionID == q.ID).ToList();
                        foreach (MODEL.T_QuestionOption qo in qoList)
                        {
                            sb.Append("<tr><td>" + qo.OptionID.ToString() + "." + "</td><td colspan='3'>" + qo.OptionContent.ToString() + "(" + qo.OptionWeight.ToString() + "）</td></tr>");
                        }
                        MODEL.T_ChoiceAnswerSheet ChoiceAnswerSheet = OperateContext.Current.BLLSession
                             .IChoiceAnswerSheetBLL.GetListBy(t => t.AnswerSheetID == aid && t.QuestionID == q.ID).First();
                        sb.Append("<tr><td>回答</td><td colspan='3'>" + ChoiceAnswerSheet.Answer.ToString() + "</td></tr>");
                        #endregion
                    }
                }
                int cscore = GetChoiceScore(infoid);
                int gscore = GetBriefScore(infoid);
                int allsscore = (GetTotalScore(infoid));
                string comment = GetComment(infoid);
                sb.Append("<tr><td>选择题得分</td><td >" + cscore.ToString() + "</td><td>简答题得分</td><td >" + gscore.ToString() + "</td></tr>");
                sb.Append("<tr><td>总分</td><td colspan='3'>" + allsscore.ToString() + "</td></tr>");
                sb.Append("<tr><td>评价</td><td colspan='3'>" + comment.ToString() + "</td></tr>");
                sb.Append("</table><br>");



            }


            sb.Append("</body>");
            var byteArray = System.Text.Encoding.Default.GetBytes(sb.ToString());
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            return File(byteArray, "application/ms-word", "全部面试试卷" + ".doc");

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
