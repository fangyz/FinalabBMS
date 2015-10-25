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
    /// 测评系统（前台）（v1.0-未开始）
    /// </summary>
    public class BeginTestController : Controller
    {
        [Common.Attributes.Skip]
        public ActionResult TestIndex()
        {
            return View();
        }
        [Common.Attributes.Skip]
        public ActionResult Index2()
        {
            return View();
        }
        #region 1.0 前台首页 +ActionResult Index()
        /// <summary>
        /// 1.0 前台首页
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 2.0 注册个人信息 +ActionResult RegInfo()
        /// <summary>
        /// 2.0 注册个人信息
        /// </summary>
        [Common.Attributes.Skip]
        public ActionResult RegInfo()
        {
            return View();
        }
        #endregion

        #region 3.0 确定录入信息 +ActionResult ConfirmReg()
        /// <summary>
        /// 3.0 确定录入信息
        /// </summary>
        [Common.Attributes.Skip]
        public ActionResult ConfirmReg()
        {
            int isRequestTech = int.Parse(Request.Params["isRequestTech"]);
            string num = Request.Params["num"];
            string name = Request.Params["name"];
            int gender = int.Parse(Request.Params["gender"]);
            string academy = Request.Params["academy"];
            string major = Request.Params["major"];
            string classs = Request.Params["class"];
            string qq = Request.Params["qq"];
            string email = Request.Params["email"];
            string telNum = Request.Params["telNum"];
            string learningExperience = Request.Params["learningExperience"];
            string selfEvaluation = Request.Params["selfEvaluation"];
            MODEL.T_InterviewerInfo model = new MODEL.T_InterviewerInfo()
            {
                Num = num,
                Name = name,
                Gender = gender == 1 ? "男" : "女",
                Academy = academy,
                Major = major,
                Class = classs,
                QQ = qq,
                Email = email,
                TelNum = telNum,
                LearningExperience = learningExperience,
                SelfEvaluation = selfEvaluation,
                IsRequestTech = isRequestTech == 1 ? true : false,
                type=isRequestTech
            };
            OperateContext.Current.BLLSession.IInterviewerInfoBLL.Add(model);
           
            Session["interviewerInfo"] = model;
            Session["timer"] = 1 * 10;
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("m", "10");
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            System.Web.HttpCookie cookie1 = new System.Web.HttpCookie("s", "5");
            cookie1.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie1);
            return Redirect("/JoinUs/BeginTest/DoQuestion/" + isRequestTech);
        }
        #endregion

        #region 4.0 提交成功 +ActionResult AccessCommit()
        /// <summary>
        /// 4.0 提交成功
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult AccessCommit()
        {
            
            return View();
        }
        #endregion

        #region 5.0 开始答题 +ActionResult DoQuestion()
        /// <summary>
        /// 5.0 开始答题
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult DoQuestion(int id)
        {
            //Session["exam"] = 2;
            //Response.Cookies["m"].Value = "0";
            //Response.Cookies["s"].Value = "5";

            string user = OperateContext.Current.User;
            Session["CurrentUsrTime"] = "60";
            ViewData["CurrentUsrTime"] = Session["CurrentUsrTime"];
            if (id == 1)
            {
                //第一次访问
                ViewData["typeId"]=id;
                if (Session["questionList1"] == null)
                {
                    //找到正在发布中的试卷id
                    int paperId = OperateContext.Current.BLLSession
                        .IPaperBLL.GetListBy(p => p.IsPublished == true && p.typeId == id).Select(p => p.ID).First();
                    //根据试卷id找到对应的试题id集合
                    List<int> questionList = OperateContext.Current.BLLSession
                        .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                        .OrderBy(p => p.T_Question.QuestionTypeID)
                        .Select(p => p.QuestionID).ToList();
                  
                    //保存题目总数                
                    Session["questionCount1"] = questionList.Count;
                    ViewData["questionCount"] = questionList.Count;
                    ViewData["questionIndex"] = 1;
                    //保存列表中第一题的题号
                    ViewData["questionID"] = questionList[0];
                    //题号列表保存到Session
                    Session["questionList1"] = questionList;
                    //生成答卷
                    //从Session中取出面试者信息
                    MODEL.T_InterviewerInfo interviewerInfo = (MODEL.T_InterviewerInfo)Session["interviewerInfo"];
                    MODEL.T_AnswerSheet asModel = new MODEL.T_AnswerSheet()
                    {
                        InterviewerID = interviewerInfo.ID,
                        PaperID = paperId
                    };
                    OperateContext.Current.BLLSession.IAnswerSheetBLL.Add(asModel);
                }
                else
                {
                    //从Session中取出题号列表
                    List<int> questionList = (List<int>)Session["questionList1"];
                    if (questionList.Count > 0)
                    {
                        //取出列表中第一题的题号
                        ViewData["questionID"] = questionList[0];
                        ViewData["questionCount"] = (int)Session["questionCount1"];
                        ViewData["questionIndex"] = (int)Session["questionCount1"] - questionList.Count + 1;
                    }
                    else
                    {
                        Session["questionList1"] = null;
                        Session["questionCount1"] = null;
                        Session["interviewerInfo"] = null;
                        return Redirect("/JoinUs/BeginTest/AccessCommit");
                    }
                }
                return View();
            }
            else if (id == 2)
            {
                //第一次访问
                ViewData["typeId"] = id;
                if (Session["questionList2"] == null)
                {
                    //找到正在发布中的试卷id
                    int paperId = OperateContext.Current.BLLSession
                        .IPaperBLL.GetListBy(p => p.IsPublished == true && p.typeId == id).Select(p => p.ID).First();
                    //根据试卷id找到对应的试题id集合
                    List<int> questionList = OperateContext.Current.BLLSession
                        .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                        .OrderBy(p => p.T_Question.QuestionTypeID)
                        .Select(p => p.QuestionID).ToList();
                   
                    //保存题目总数                
                    Session["questionCount2"] = questionList.Count;
                    ViewData["questionCount"] = questionList.Count;
                    ViewData["questionIndex"] = 1;
                    //保存列表中第一题的题号
                    ViewData["questionID"] = questionList[0];
                    //题号列表保存到Session
                    Session["questionList2"] = questionList;
                    //生成答卷
                    //从Session中取出面试者信息
                    MODEL.T_InterviewerInfo interviewerInfo = (MODEL.T_InterviewerInfo)Session["interviewerInfo"];
                    MODEL.T_AnswerSheet asModel = new MODEL.T_AnswerSheet()
                    {
                        InterviewerID = interviewerInfo.ID,
                        PaperID = paperId
                    };
                    OperateContext.Current.BLLSession.IAnswerSheetBLL.Add(asModel);
                }
                else
                {
                    //从Session中取出题号列表
                    List<int> questionList = (List<int>)Session["questionList2"];
                    if (questionList.Count > 0)
                    {
                        //取出列表中第一题的题号
                        ViewData["questionID"] = questionList[0];
                        ViewData["questionCount"] = (int)Session["questionCount2"];
                        ViewData["questionIndex"] = (int)Session["questionCount2"] - questionList.Count + 1;
                    }
                    else
                    {
                        Session["questionList2"] = null;
                        Session["questionCount2"] = null;
                        Session["interviewerInfo"] = null;
                        return Redirect("/JoinUs/BeginTest/AccessCommit");
                    }
                }
                return View();
            }
            else
            {
                //第一次访问
                ViewData["typeId"] = id;
                if (Session["questionList3"] == null)
                {
                    //找到正在发布中的试卷id
                    int paperId = OperateContext.Current.BLLSession
                        .IPaperBLL.GetListBy(p => p.IsPublished == true && p.typeId == id).Select(p => p.ID).First();
                    //根据试卷id找到对应的试题id集合
                    List<int> questionList = OperateContext.Current.BLLSession
                        .IPaperQuestionBLL.GetListBy(p => p.PaperID == paperId)
                        .OrderBy(p => p.T_Question.QuestionTypeID)
                        .Select(p => p.QuestionID).ToList();
                   
                    //保存题目总数                
                    Session["questionCount3"] = questionList.Count;
                    ViewData["questionCount"] = questionList.Count;
                    ViewData["questionIndex"] = 1;
                    //保存列表中第一题的题号
                    ViewData["questionID"] = questionList[0];
                    //题号列表保存到Session
                    Session["questionList3"] = questionList;
                    //生成答卷
                    //从Session中取出面试者信息
                    MODEL.T_InterviewerInfo interviewerInfo = (MODEL.T_InterviewerInfo)Session["interviewerInfo"];
                    MODEL.T_AnswerSheet asModel = new MODEL.T_AnswerSheet()
                    {
                        InterviewerID = interviewerInfo.ID,
                        PaperID = paperId
                    };
                    OperateContext.Current.BLLSession.IAnswerSheetBLL.Add(asModel);
                    
                }
                else
                {
                    //从Session中取出题号列表
                    List<int> questionList = (List<int>)Session["questionList3"];
                    if (questionList.Count > 0)
                    {
                        //取出列表中第一题的题号
                        ViewData["questionID"] = questionList[0];
                        ViewData["questionCount"] = (int)Session["questionCount3"];
                        ViewData["questionIndex"] = (int)Session["questionCount3"] - questionList.Count + 1;
                        
                    }
                    else
                    {
                        Session["questionList3"] = null;
                        Session["questionCount3"] = null;
                        Session["interviewerInfo"] = null;
                        return Redirect("/JoinUs/BeginTest/AccessCommit");
                    }
                }
                return View();
            }
        }
        #endregion


        #region 6.0 根据id获取题目信息 +ActionResult GetQuestionById(int id)
        /// <summary>
        /// 6.0 根据id获取题目信息
        /// </summary>
        [Common.Attributes.Skip]
        public ActionResult GetQuestionById(int id)
        {
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

       
        #region 7.0 提交答案 +ActionResult SubmitAnswer()
        /// <summary>
        /// 7.0 提交答案
        /// </summary>
        [Common.Attributes.Skip]
        public ActionResult SubmitAnswer(int id)
        {
      
            //题目类型
            int questionTypeId = int.Parse(Request.Params["questionTypeId"]);
            //题目ID
            int questionId = int.Parse(Request.Params["questionId"]);
            //题目答案
            string answerContent = Request.Params["answerContent"];
            string time = Request.Params["timer1"];

            if (answerContent == null)
            {
                answerContent = "";
            }
            //根据答卷人ID找到对应的答卷
            //取出答卷人ID
            MODEL.T_InterviewerInfo interviewerInfo = (MODEL.T_InterviewerInfo)Session["interviewerInfo"];
            //根据答卷人ID查找对应的答卷ID
            int asId = OperateContext.Current.BLLSession
                .IAnswerSheetBLL.GetListBy(a => a.InterviewerID == interviewerInfo.ID).Select(a => a.ID).First();
            //将答案插入答卷
            //选择题
            if (questionTypeId == 1)
            {
                MODEL.T_ChoiceAnswerSheet cas = new MODEL.T_ChoiceAnswerSheet()
                {
                    AnswerSheetID = asId,
                    QuestionID = questionId,
                    Answer = answerContent
                };
                OperateContext.Current.BLLSession.IChoiceAnswerSheetBLL.Add(cas);
            }
            //简答题
            else if (questionTypeId == 2)
            {
                MODEL.T_BriefAnswerSheet bas = new MODEL.T_BriefAnswerSheet()
                {
                    AnswerSheetID = asId,
                    QuestionID = questionId,
                    Answer = answerContent
                };
                OperateContext.Current.BLLSession.IBriefAnswerSheetBLL.Add(bas);
            }
            //从题目列表中移除已提交题目
            if (id == 1)
            {
                List<int> questionList = (List<int>)Session["questionList1"];
                questionList.RemoveAt(0);
                //题号列表保存到Session
                Session["questionList1"] = questionList;
                if (questionList.Count == 1)
                {
                    ViewData["lastone"] = 1;
                }
                else
                {
                    ViewData["lastone"] = 0;
                }
                if (time == "intime")
                {
                    return Redirect("/JoinUs/BeginTest/DoQuestion/" + id);
                }
                else
                {

                    List<MODEL.T_Question> model = OperateContext.Current.BLLSession
                         .IQuestionBLL.GetListBy(q => questionList.Contains(q.ID)).ToList();
                    foreach (MODEL.T_Question q in model)
                    {
                        if (q.QuestionTypeID == 1)
                        {
                            MODEL.T_ChoiceAnswerSheet cas = new MODEL.T_ChoiceAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IChoiceAnswerSheetBLL.Add(cas);
                        }
                        //简答题
                        else if (q.QuestionTypeID == 2)
                        {
                            MODEL.T_BriefAnswerSheet bas = new MODEL.T_BriefAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IBriefAnswerSheetBLL.Add(bas);
                        }
                    }
                    Session["interviewerInfo"] = null;
                    Session["questionList1"] = null;
                    return Redirect("/JoinUs/BeginTest/AccessCommit");
                }
            }
            else if (id == 2)
            {
                List<int> questionList = (List<int>)Session["questionList2"];
                questionList.RemoveAt(0);
                //题号列表保存到Session
                Session["questionList2"] = questionList;
                if (time == "intimer")
                {
                    return Redirect("/JoinUs/BeginTest/DoQuestion/" + id);
                }
                else
                {
                    List<MODEL.T_Question> model = OperateContext.Current.BLLSession
                         .IQuestionBLL.GetListBy(q => questionList.Contains(q.ID)).ToList();
                    foreach (MODEL.T_Question q in model)
                    {
                        if (q.QuestionTypeID == 1)
                        {
                            MODEL.T_ChoiceAnswerSheet cas = new MODEL.T_ChoiceAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IChoiceAnswerSheetBLL.Add(cas);
                        }
                        //简答题
                        else if (q.QuestionTypeID == 2)
                        {
                            MODEL.T_BriefAnswerSheet bas = new MODEL.T_BriefAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IBriefAnswerSheetBLL.Add(bas);
                        }
                    }
                    Session["interviewerInfo"] = null;
                    Session["questionList2"] = null;
                    return Redirect("/JoinUs/BeginTest/AccessCommit");
                }
            }
            else
            {
                List<int> questionList = (List<int>)Session["questionList3"];
                questionList.RemoveAt(0);
                //题号列表保存到Session
                Session["questionList3"] = questionList;
                if (time == "intimer")
                {
                    return Redirect("/JoinUs/BeginTest/DoQuestion/" + id);
                }
                else
                {
                    List<MODEL.T_Question> model = OperateContext.Current.BLLSession
                         .IQuestionBLL.GetListBy(q => questionList.Contains(q.ID)).ToList();
                    foreach (MODEL.T_Question q in model)
                    {
                        if (q.QuestionTypeID == 1)
                        {
                            MODEL.T_ChoiceAnswerSheet cas = new MODEL.T_ChoiceAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IChoiceAnswerSheetBLL.Add(cas);
                        }
                        //简答题
                        else if (q.QuestionTypeID == 2)
                        {
                            MODEL.T_BriefAnswerSheet bas = new MODEL.T_BriefAnswerSheet()
                            {
                                AnswerSheetID = asId,
                                QuestionID = q.ID,
                                Answer = ""
                            };
                            OperateContext.Current.BLLSession.IBriefAnswerSheetBLL.Add(bas);
                        }
                    }
                    Session["interviewerInfo"] = null;
                    Session["questionList3"] = null;
                    return Redirect("/JoinUs/BeginTest/AccessCommit");
                }
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
