using BLL;
using IBLL;
using MODEL;
using MODEL.ViewModel.Task;
using MVC.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Task.Controllers
{
    /// <summary>
    /// 任务-控制器
    /// author：潘帅
    /// 2015年2月 
    /// </summary>
    public class TaskController : Controller
    {
        IBLLSession iBLLSession = new OperateContext().BLLSession as IBLLSession;
        ////测试用跳过登录
        //public TaskController()
        //{
        //    new OperateContext().Usr = iBLLSession.IMemberInformationBLL.GetListBy(p => p.StuNum == "201202080222")[0];
        //}
         /////
        #region 01.我的任务
        /// <summary>
        /// 我的任务
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult MyTask()
        {
            int taskPageIndex = 1;
            int taskPageSize = 4;

            //从Session获取登录成员
            MODEL.T_MemberInformation user = OperateContext.Current.Usr;
            string stuNum = user.StuNum;//Lambda里边不能直接传Usr.StuNum不然报错

            //获取任务类型以便动态生成任务表
            //List<MODEL.T_TaskType> taskTypeList = iBLLSession.ITaskTypeBLL.GetListBy(t => t.TaskTypeId > 0);

            List<MODEL.ViewModel.Task.MyTask> myTaskListComplete = new List<MODEL.ViewModel.Task.MyTask>();

            List<MODEL.ViewModel.Task.MyTask> myTaskListNotComplete = new List<MODEL.ViewModel.Task.MyTask>();

            //List<MyTask>[] myTaskLists = new List<MyTask>[taskTypeList.Count];

            //int[] taskCount = new int[taskTypeList.Count];

            //按任务类型查找任务以及任务总数
            //for (int i = 0; i < taskTypeList.Count; i++)
            //{
            //    myTaskLists[i] = iBLLSession.ITaskInformationBLL.GetPagedTaskList(taskPageIndex, taskPageSize, stuNum, taskTypeList[i].TaskTypeId);
            //    taskCount[i] = iBLLSession.ITaskInformationBLL.GetMyTaskCount(stuNum, taskTypeList[i].TaskTypeId);
            //}

            myTaskListComplete = iBLLSession.ITaskInformationBLL.GetPagedCompletedTaskList(taskPageIndex, taskPageSize, stuNum, true);
            int  myTaskListCompleteCount = iBLLSession.ITaskInformationBLL.GetMyTaskCount(stuNum,true);
            myTaskListNotComplete = iBLLSession.ITaskInformationBLL.GetPagedTaskList(taskPageIndex, taskPageSize, stuNum, false);
            int myTaskListNotCompleteCount = iBLLSession.ITaskInformationBLL.GetMyTaskCount(stuNum,false);

            ViewData["myTaskListComplete"] = myTaskListComplete;

            ViewData["myTaskListNotComplete"] = myTaskListNotComplete;

            ViewData["myTaskListCompleteCount"] = myTaskListCompleteCount;

            ViewData["myTaskListNotCompleteCount"] = myTaskListNotCompleteCount;

            return View();
        } 
        #endregion
        /////
        #region 我的任务详情 +  public ActionResult MyTaskDetail(int taskId, string taskReciver)
        /// <summary>
        /// 我的任务详情 +  public ActionResult MyTaskDetail(int taskId, string taskReciver)
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskReciver"></param>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult MyTaskDetail(int taskId, string taskReciver)
        {
            MODEL.T_TaskInformation info = new MODEL.T_TaskInformation();
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;



            if ((iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskSender == stuNum
                && t.TaskId == taskId).Count > 0)
                || iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskReceiver == stuNum
                && t.TaskId == taskId).Count > 0)//验证是否有查看此任务的权限
            {
                MODEL.ViewModel.Task.TaskDetail taskDetail = iBLLSession.ITaskInformationBLL.GetTaskDetailById(taskId);
                MODEL.ViewModel.Task.TaskEvaluateModel taskEvaluate = iBLLSession.ITaskInformationBLL.GetTaskEvaluateList(taskReciver, taskId)[0];
                MODEL.T_TaskParticipation taskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskId == taskId & t.TaskReceiver == taskReciver).ToList()[0];


                ViewData["taskDetail"] = taskDetail;
                ViewData["taskEvaluate"] = taskEvaluate;
                ViewData["taskParticipation"] = taskParticipation;

                return View();
            }
            else
            {
                return null;
            }

        } 
        #endregion
        /////
        #region 02.获取任务分页数据+GetMyTaskJsonData?strPageIndex=1&strPageSize=4&strTaskTypeId=10001
        /// <summary>
        /// 获取任务分页数据
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult GetMyTaskJsonData(string strPageIndex, string strPageSize, bool complete)
        {
            int pageIndex = 1;
            int pageSize = 4;
            int taskTypeId = 10001;//初始化

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            List<MyTask> myTaskList = new List<MyTask>();
            

            if (int.TryParse(strPageIndex, out pageIndex)
                && int.TryParse(strPageSize, out pageSize))
            {

                myTaskList = iBLLSession.ITaskInformationBLL.GetPagedTaskList(pageIndex, pageSize, stuNum, complete);

                return Json(myTaskList, JsonRequestBehavior.AllowGet);//允许Get请求
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        /////
        #region 03.发布历史
        /// <summary>
        /// 发布历史
        /// </summary>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult ReleaseHistory()
        {
            int pageIndex = 1;
            int pageSize = 10;

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;
            
            //获取我的任务列表
            List<MODEL.T_TaskInformation> taskList = iBLLSession.ITaskInformationBLL.TaskGetPagedList<DateTime>(pageIndex, pageSize, task => task.TaskSender == stuNum,task => task.TaskEndTime, true);
            
            //任务我发布的任务条数
            int taskCount = iBLLSession.ITaskInformationBLL.GetReleaseHistoryCount(stuNum);

            List<MODEL.T_TaskType> taskTypeList = iBLLSession.ITaskTypeBLL.GetListBy(t => true);

            //以"taskType"+id：name的形式传到视图，以便@ViewData["taskType"+TaskTypeId]直接获取任务类型名称
            foreach(MODEL.T_TaskType taskType in taskTypeList)
            {
                ViewData["taskType" + taskType.TaskTypeId] = taskType.TaskTypeName;
            }

            List<int> taskIds = taskList.Select(task => task.TaskId).ToList();

            ViewData["canBeModify"] = iBLLSession.ITaskInformationBLL.CanByModify(taskIds);//查看每一个任务是否能被修改

            ViewData["taskList"] = taskList;

            ViewData["taskCount"] = taskCount;

            //列表大小
            ViewData["listSize"] = 10;

            return View();
        }
        #endregion
        /////
        #region 04.获取发布历史分页数据
        /// <summary>
        /// 获取发布历史分页数据，与发布历史相似，只不过传的是Json格式的数据
        /// </summary>
        /// <param name="strPageIndex"></param>
        /// <param name="strPageSize"></param>
        /// <returns></returns>
         [Common.Attributes.Skip]
        public ActionResult GetReleaseHistoryJsonData(string strPageIndex, string strPageSize)
        {
            int pageIndex = 1;
            int pageSize = 10;

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            List<MODEL.T_TaskInformation> taskList = new List<MODEL.T_TaskInformation>();
            List<MODEL.T_TaskType> taskTypeList = new List<MODEL.T_TaskType>();
            

            if (int.TryParse(strPageIndex, out pageIndex)
                && int.TryParse(strPageSize, out pageSize))
            {

                taskList = iBLLSession.ITaskInformationBLL.TaskGetPagedList<int>(pageIndex, pageSize, task => task.TaskSender == stuNum, task => task.TaskId, true);

                taskTypeList = iBLLSession.ITaskTypeBLL.GetListBy(t => true);
                List<int> taskIds = taskList.Select(task => task.TaskId).ToList();
                bool[] canBeModify = iBLLSession.ITaskInformationBLL.CanByModify(taskIds);

                var list = from tl in taskList
                           join ttl in taskTypeList on tl.TaskTypeId equals ttl.TaskTypeId
                           select new
                           {
                               TaskId = tl.TaskId,
                               TaskName = tl.TaskName,
                               TaskTypeId = ttl.TaskTypeId,
                               TaskTypeName = ttl.TaskTypeName,
                               TaskBegTime = tl.TaskBegTime.ToString("yyyy年MM月dd日"),
                               TaskEndTime = tl.TaskEndTime.ToString("yyyy年MM月dd日"),
                           };
                var jsonData = new { TaskList = list, CanBeModify = canBeModify };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        /////
        #region 05.发布任务
        /// <summary>
        ///  发布任务
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskRelease()
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            //获取有权限发布的任务类型
            List<MODEL.T_TaskType> listTaskType = new TaskOperateHelper().GetAccessRightTaskTypeList();
            
            //获取项目阶段
            List<MODEL.T_ProjPhase> listProjPhase = iBLLSession.IProjPhaseBLL.GetListBy(p => true);

            //获取用户所管理的项目
            List<MODEL.T_ProjectInformation> listProjectInfo = iBLLSession.IProjectInformationBLL.GetListBy(p => p.ProjLeader==stuNum);
            
            //获取实验室成员信息
            List<MODEL.T_MemberInformation> listMemberInfo = iBLLSession.IMemberInformationBLL.GetListBy(m => true);



            ViewData["listTaskType"] = listTaskType;
            ViewData["listProjPhase"] = listProjPhase;
            ViewData["listProjectInfo"] = listProjectInfo;
            ViewData["listMemberInfo"] = listMemberInfo;

            return View();
        }
        #endregion

                                                                       #region 06.获取成员信息
        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="membersType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GetMembersJsonData?membersType=&id=
        public ActionResult GetMembersJsonData(int membersType, int id)
        {
            #region 根据前台传来的membersType返回相应的成员信息
            //value="10001"所有成员
            //value="10002"实习生
            //value="10003"正式成员
            //value="10004".NET部门
            //value="10005"JAVA开发部门
            //value="10006"物联网开发部门
            //value="10007"系统编程开发部门
            //value="10008"项目组成员
            #endregion

            List<MODEL.ViewModel.Task.TaskReleaseMember> listMember = new List<MODEL.ViewModel.Task.TaskReleaseMember>();

            List<MODEL.T_TaskType> listTaskType = new TaskOperateHelper().GetAccessRightTaskTypeList();

            if (membersType == 10001 
                && (listTaskType.Select(tt => tt.TaskTypeId ==10004).ToList().Count  > 0))
            {
                listMember = (from m in iBLLSession.IMemberInformationBLL.GetListBy(m => true)
                              select new MODEL.ViewModel.Task.TaskReleaseMember()
                              {
                                  StuName = m.StuName,
                                  StuNum = m.StuNum
                              }).ToList();
            }
            else if ((membersType == 10002 || membersType == 10003)
                && (listTaskType.Select(tt => tt.TaskTypeId == 10004).ToList().Count > 0))
            {
                listMember = (from m in iBLLSession.IMemberInformationBLL.GetListBy(m => m.TechnicalLevel == id)
                              select new MODEL.ViewModel.Task.TaskReleaseMember()
                              {
                                  StuName = m.StuName,
                                  StuNum = m.StuNum
                              }).ToList();
            }
            else if ((membersType == 10004 || membersType == 10005 || membersType == 10006 || membersType == 10007)
                && (listTaskType.Select(tt => tt.TaskTypeId == 10004
                    || tt.TaskTypeId == 10002 ).ToList().Count > 0))
            {
                listMember = (from m in iBLLSession.IMemberInformationBLL.GetListBy(m => m.Department == id)
                              select new MODEL.ViewModel.Task.TaskReleaseMember()
                              {
                                  StuName = m.StuName,
                                  StuNum = m.StuNum
                              }).ToList();
            }
            else if (membersType == 10008
                && (listTaskType.Select(tt => tt.TaskTypeId == 10003).ToList().Count > 0))
            {
                listMember = (from m in iBLLSession.IMemberInformationBLL.GetListBy(m => true)
                              join p in iBLLSession.IProjectParticipationBLL.GetListBy(p => p.ProjId == id)
                              on m.StuNum equals p.ProjReceiver
                              select new MODEL.ViewModel.Task.TaskReleaseMember()
                              {
                                  StuName = m.StuName,
                                  StuNum = m.StuNum
                              }).ToList();
            }
            return Json(listMember,JsonRequestBehavior.AllowGet);
        }
        #endregion
        /////
        #region 07.保存任务信息（包括保存任务修改）
        /// <summary>
        /// 保存任务信息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public string SaveTaskInfo(FormCollection form)
        {
            MODEL.T_TaskInformation taskInfo = new MODEL.T_TaskInformation();
            int taskTypeId = 0, projId = 10001, projPhasesId = 10001;
            System.DateTime taskBegTime, taskEndTime;

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            string[] members = null;
            //TaskName,TaskBegTime,TaskEndTime,TaskContent,TaskTypeId
            //ProjId,ProjPhasesId,Members
            if (form["TaskName"] != null && form["TaskContent"] != null && form["Members"] != null)
            {
                if (!(int.TryParse(form["TaskTypeId"], out taskTypeId)
                    && System.DateTime.TryParse(form["TaskBegTime"], out taskBegTime)
                    && System.DateTime.TryParse(form["TaskEndTime"], out taskEndTime)))
                {
                    return "nook";
                }
                else
                {
                    if (taskTypeId == 10003)
                    {
                        if (!int.TryParse(form["ProjId"], out projId) || !int.TryParse(form["ProjPhasesId"], out projPhasesId))
                        {
                            return "nook";
                        }
                    }
                    taskInfo.TaskSender = stuNum;
                    taskInfo.TaskTypeId = taskTypeId;
                    taskInfo.TaskName = form["TaskName"];
                    taskInfo.TaskBegTime = taskBegTime;
                    taskInfo.TaskEndTime = taskEndTime;
                    taskInfo.TaskContent = form["TaskContent"];
                    taskInfo.ProjId = projId;
                    taskInfo.ProjPhaseId = projPhasesId;
                    taskInfo.TaskAttachmentPath = form["TaskFile"] + taskInfo.TaskAttachmentPath;
                    members = form["Members"].Split(new char[] { ',' });

                    #region ##任务修改代码
                    int oldTaskId;

                    if (form["TaskId"] != null)//如果TaskId不为空则说明是修改任务，需要先删除久的任务
                    {
                        if (int.TryParse(form["TaskId"], out oldTaskId))
                        {
                            if (DeleteTask(oldTaskId) != "ok")
                            {
                                return "nook";
                            }
                        }
                    } 
                    #endregion                    

                    if (iBLLSession.ITaskInformationBLL.SaveTask(taskInfo, members))
                    {
                        return "ok";
                    }
                    else
                    {
                        return "nook";
                    }
                }
            }
            return "nook";
        }
        #endregion
        /////
        #region 08.查看任务详情
        /// <summary>
        /// 查看任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ActionResult TaskDetail(int taskId)
        {
            MODEL.T_TaskInformation info = new MODEL.T_TaskInformation();
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if ((iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskSender == stuNum
                && t.TaskId == taskId).Count > 0)
                || iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskReceiver == stuNum
                && t.TaskId == taskId).Count > 0)//验证是否有查看此任务的权限
            {
                MODEL.ViewModel.Task.TaskDetail taskDetail = iBLLSession.ITaskInformationBLL.GetTaskDetailById(taskId);

                MODEL.T_TaskParticipation taskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskId == taskId & t.TaskReceiver == stuNum).ToList()[0];

                ViewData["taskDetail"] = taskDetail;
                ViewData["taskParticipation"] = taskParticipation;

                return View();
            }
            else
            {
                return null;
            }

            
        }
        #endregion

        #region 09.确认任务已完成
        /// <summary>
        /// 确认任务已完成
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string TaskCompleteConfirmation(int taskId)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            MODEL.T_TaskParticipation taskParticipation = new MODEL.T_TaskParticipation();
            taskParticipation.TaskId = taskId;
            taskParticipation.TaskReceiver = stuNum;
            taskParticipation.IsComplete = true;
            taskParticipation.IsRead = true;

            if (iBLLSession.ITaskParticipationBLL.ModifyBy(taskParticipation, tp => tp.TaskId == taskParticipation.TaskId
                && tp.TaskReceiver == taskParticipation.TaskReceiver, "IsComplete", "IsRead") > 0)
            {
                return "ok";
            }
            return "nook";
        } 
        #endregion
        /////
        #region 10.删除任务
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string DeleteTask(int taskId)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            //删除操作的验证在BLL层唷~~
            if (iBLLSession.ITaskInformationBLL.DeleteTask(taskId, stuNum))
            {
                return "ok";
            }
            return "nook";
        } 
        #endregion
        /////
        #region 11.修改任务
        /// <summary>
        /// 修改任务,主要是查询任务信息，让后传到视图上去
        /// 比较懒，修改和发布任务的视图差不多，所以在修改视图上就用了发布视图的JS和CSS并做了扩展
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ActionResult ModifyTask(int taskId)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            //权限验证
            if (!iBLLSession.ITaskInformationBLL.CanByModify(taskId) 
                || iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskId==taskId 
                    && t.TaskSender == stuNum).Count <= 0)
            {
                return null;
            }
            //任务信息
            MODEL.T_TaskInformation taskInfo = iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskId == taskId && t.TaskSender == stuNum)[0];
            //任务类型List            
            List<MODEL.T_TaskType> listTaskType = iBLLSession.ITaskTypeBLL.GetListBy(t => true);
            //项目阶段List
            List<MODEL.T_ProjPhase> listProjPhase = iBLLSession.IProjPhaseBLL.GetListBy(p => true);
            //项目信息List
            List<MODEL.T_ProjectInformation> listProjectInfo = iBLLSession.IProjectInformationBLL.GetListBy(p => p.ProjLeader == stuNum);
            //成员信息List
            List<MODEL.T_MemberInformation> listMemberInfo = iBLLSession.IMemberInformationBLL.GetListBy(m => true);
            
            ViewData["taskInfo"] = taskInfo;
            //当前任务的参与人List
            List<MODEL.T_TaskParticipation> listTaskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(tp => tp.TaskId == taskId);

            string jsonMembers = "";
            jsonMembers+="[";
            foreach(MODEL.T_TaskParticipation taskParticipation in listTaskParticipation){
                jsonMembers+="{";
                jsonMembers+="StuNum:"+taskParticipation.TaskReceiver.ToString();
                jsonMembers+="},";
            }            
            jsonMembers=jsonMembers.Substring(0,jsonMembers.Length-1)+"]";

            ViewData["listTaskType"] = listTaskType;
            ViewData["listProjPhase"] = listProjPhase;
            ViewData["listProjectInfo"] = listProjectInfo;
            ViewData["listMemberInfo"] = listMemberInfo;
            ViewData["jsonMembers"] = jsonMembers;
            return View();
        }
        #endregion


        /////
        #region 13.任务评价
        /// <summary>
        /// 任务评价
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskEvaluate(int taskId)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            List<MODEL.ViewModel.Task.TaskEvaluateModel> listTaskEvaluate = iBLLSession.ITaskInformationBLL.GetPagedTaskEvaluateList(1, 10, stuNum,taskId);

            ViewData["taskEvaluateCount"] = iBLLSession.ITaskInformationBLL.GetTaskEvaluateCount(stuNum,taskId);

            ViewData["listTaskEvaluate"] = listTaskEvaluate;

            return View();
        }
        #endregion        
        /////
        #region 14.保存任务评价[HttpPost]
        /// <summary>
        /// 保存任务评价
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveTaskGrade(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if (form["StuNum"] != null && form["TaskId"] != null && form["TaskGrade"] != null)
            {
                string taskReceiver = form["StuNum"];
                string strTaskId = form["TaskId"];
                string strGrade = form["TaskGrade"];
                int taskId;
                int grade;
                //格式验证
                if (int.TryParse(strTaskId, out taskId) && int.TryParse(strGrade,out grade) && (grade.Equals("优") || grade.Equals("良") || grade.Equals("不合格")))
                {
                    //保存评价，评价权限验证放在BLL层惹
                    if (iBLLSession.ITaskInformationBLL.SaveTaskGrade(stuNum, taskReceiver, taskId, grade)) 
                    {
                        return "ok";
                    }
                }
            }

            return "nook";
        } 
        #endregion
         /////
        #region 15.保存任务评语[HttpPost]
        /// <summary>
        /// 保存任务评语
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveTaskResponse(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if (form["StuNum"] != null && form["TaskId"] != null && form["TaskResponse"] != null)
            {
                string taskReceiver = form["StuNum"];
                string strTaskId = form["TaskId"];
                string taskResponse = form["TaskResponse"];
                int taskId;
                if (int.TryParse(strTaskId, out taskId))
                {
                    //保存任务评价，权限的验证放在了BLL层中喔~~
                    if (iBLLSession.ITaskInformationBLL.SaveTaskResponse(stuNum, taskReceiver, taskId, taskResponse)) 
                    {
                        return "ok";
                    }
                }
            }

            return "nook";
        } 
        #endregion
        /////
        #region 16.获取任务评价分页数据
        /// <summary>
        /// 获取任务评价分页数据,就是查询数据库，转Json格式数据没什么的，权限的验证
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetPagedTaskEvaluateList(int pageIndex, int pageSize,int taskId)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            List<MODEL.ViewModel.Task.TaskEvaluateModel> listTaskEvaluate = iBLLSession.ITaskInformationBLL.GetPagedTaskEvaluateList(pageIndex, pageSize, stuNum,taskId);

            return Json(listTaskEvaluate, JsonRequestBehavior.AllowGet);
        }
        #endregion


//----------------------------------------------------------2015/7/9-----------------------------------------------------
        
                                                            #region 任务统计 + public ActionResult TaskCount(DateTime time)
        /// <summary>
        /// 任务统计
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskCount(DateTime time)
        {
            List<MODEL.T_TaskInformation> taskOfBegTime = iBLLSession.ITaskInformationBLL.GetTaskCountOfBegTime(time);
            ViewData["taskOfBegTime"] = taskOfBegTime;
            return View();
        }
        #endregion
        /////
        #region 任务提交 + public ActionResult TaskSubmit(int taskId)
        /// <summary>
        /// 任务提交 + public ActionResult TaskSubmit(int taskId)
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ActionResult TaskSubmit(int taskId)
        {

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if ((iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskSender == stuNum
                && t.TaskId == taskId).Count > 0)
                || iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskReceiver == stuNum
                && t.TaskId == taskId).Count > 0)//验证是否有查看此任务的权限
            {
                MODEL.ViewModel.Task.TaskDetail taskDetail = iBLLSession.ITaskInformationBLL.GetTaskDetailById(taskId);

                ViewData["taskDetail"] = taskDetail;

                return View();
            }
            else
            {
                return null;
            }
        } 
        #endregion
        /////
        #region 提交任务并保存 +  public string SaveTaskSubmit(FormCollection form)
        /// <summary>
        /// 提交任务并保存 +  public string SaveTaskSubmit(FormCollection form)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public string SaveTaskSubmit(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;
            int taskId = int.Parse(form["taskId"]);
            string taskSubmition = form["TaskSubmition"];
            DateTime taskFinishTime = DateTime.Parse(form["taskFinishTime"]);

            MODEL.T_TaskParticipation taskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskId == taskId && t.TaskReceiver == stuNum).FirstOrDefault();
            taskParticipation.TaskSubmition = taskSubmition;
            taskParticipation.IsComplete = true;
            taskParticipation.TaskFinishTime = taskFinishTime;
            taskParticipation.IsRead = true;
            if (iBLLSession.ITaskParticipationBLL.Modify(taskParticipation, "TaskSubmition", "IsComplete", "TaskFinishTime", "IsRead") > 0)
            {
                return "ok";
            }
            else
            {
                return "nook";
            }
        } 
        #endregion
        /////
        #region 评阅任务 + public ActionResult TaskCorrect(int taskId,string taskReciver)
        /// <summary>
        /// 评阅任务 + public ActionResult TaskCorrect(int taskId,string taskReciver)
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskReciver"></param>
        /// <returns></returns>
        public ActionResult TaskCorrect(int taskId, string taskReciver)
        {
            MODEL.T_TaskInformation info = new MODEL.T_TaskInformation();
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if ((iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskSender == stuNum
                && t.TaskId == taskId).Count > 0)
                || iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskReceiver == stuNum
                && t.TaskId == taskId).Count > 0)//验证是否有查看此任务的权限
            {
                MODEL.ViewModel.Task.TaskDetail taskDetail = iBLLSession.ITaskInformationBLL.GetTaskDetailById(taskId);

                MODEL.ViewModel.Task.TaskEvaluateModel taskEvaluate = iBLLSession.ITaskInformationBLL.GetTaskEvaluateList(taskReciver, taskId)[0];

                MODEL.T_TaskParticipation taskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskReceiver == taskReciver && t.TaskId == taskId)[0];

                ViewData["taskParticipation"] = taskParticipation;

                ViewData["taskDetail"] = taskDetail;

                ViewData["taskEvaluate"] = taskEvaluate;

                ViewData["LoginUser"] = user.StuName;

                return View();
            }
            else
            {
                return null;
            }

        }
        #endregion
        /////
        #region 提交评价并保存 + public string SaveTaskCorret()
        /// <summary>
        /// 提交评价并保存 + public string SaveTaskCorret()
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SaveTaskCorret(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;
            if (form["TaskId"]!=null&&form["TaskReceiver"]!=null&&form["TaskScore"]!=null&&form["TaskRemark"]!=null)
            {
                string taskStrId = form["TaskId"];
                string taskReceiver = form["TaskReceiver"];
                int taskScore = Convert.ToInt32( form["TaskScore"]);
                string taskRemark = form["TaskRemark"];
                int taskId;
                if (int.TryParse(taskStrId,out taskId))
                {
                    if (iBLLSession.ITaskInformationBLL.SaveTaskCorrect(stuNum,taskReceiver,taskId,taskRemark,taskScore))
                    {
                        return "ok";
                    }
                }
            }

            return "nook";
        } 
        #endregion
        /////
        #region 保存上传文件 + public string SaveTaskFile(FormCollection form)
        /// <summary>
        /// 保存上传文件 + public string SaveTaskFile(FormCollection form)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SaveTaskFile(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;
            int taskId = int.Parse(form["strTaskId"]);
            MODEL.T_TaskParticipation taskParticipation = iBLLSession.ITaskParticipationBLL.GetListBy(t => t.TaskId == taskId && t.TaskReceiver == stuNum).FirstOrDefault();
            taskParticipation.TaskAttachmentPath = form["filename"]+","+taskParticipation.TaskAttachmentPath;
            if (iBLLSession.ITaskParticipationBLL.Modify(taskParticipation, "TaskAttachmentPath") > 0)
            {
                return "ok";
            }
            else
            {
                return "nook";
            }
        } 
        #endregion
        /////
        #region 修改任务-文件上传 +  public string SaveModifyFile(FormCollection form)
        /// <summary>
        /// 修改任务-文件上传 +  public string SaveModifyFile(FormCollection form)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SaveModifyFile(FormCollection form)
        {
            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;
            int taskId = int.Parse(form["strTaskId"]);
            MODEL.T_TaskInformation taskInformation = iBLLSession.ITaskInformationBLL.GetListBy(t => t.TaskId == taskId).FirstOrDefault();
            taskInformation.TaskAttachmentPath = form["filename"] + "," + taskInformation.TaskAttachmentPath;
            if (iBLLSession.ITaskInformationBLL.Modify(taskInformation, "TaskAttachmentPath") > 0)
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
