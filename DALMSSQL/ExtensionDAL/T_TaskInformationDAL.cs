using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MODEL.ViewModel.Task;
using MODEL;
using System.Data.SqlClient;
using System.Data.Entity;

namespace DALMSSQL
{
    /// <summary>
    /// ExtensionDAL---T_TaskInformationDAL
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public partial class T_TaskInformationDAL : BaseDAL<MODEL.T_TaskInformation>, IDAL.ITaskInformationDAL
    {
        //#region 01.获取我的任务分页数据
        ///// <summary>
        ///// 获取我的任务分页数据
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">页大小</param>
        ///// <param name="stuNum">学号</param>
        ///// <param name="taskTypeId">任务类型ID</param>
        ///// <returns></returns>
        //public List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, int taskTypeId)
        //{

        //    DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
        //    DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
        //    DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
        //    DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

        //    List<MyTask> myTaskList = (from taskParticipation in taskParticipations
        //                               join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
        //                               join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
        //                               join memberInformation in memberInformations on taskInformation.TaskSender equals memberInformation.StuNum
        //                               where taskParticipation.TaskReceiver == stuNum
        //                               && taskInformation.TaskTypeId == taskTypeId
        //                               select new MyTask()
        //                               {
        //                                   TaskId = taskParticipation.TaskId,
        //                                   TaskSender = taskInformation.TaskSender,
        //                                   TaskName = taskInformation.TaskName,
        //                                   TaskTypeId = taskInformation.TaskTypeId,
        //                                   TaskContent = taskInformation.TaskContent,
        //                                   TaskBegTime = taskInformation.TaskBegTime,
        //                                   TaskEndTime = taskInformation.TaskEndTime,
        //                                   TaskReceiver = taskParticipation.TaskReceiver,
        //                                   TaskGrade = taskParticipation.TaskGrade,
        //                                   IsRead = taskParticipation.IsRead,
        //                                   IsComplete = taskParticipation.IsComplete,
        //                                   TaskSenderName = memberInformation.StuName,
        //                                   TaskTypeName = taskType.TaskTypeName
        //                               }).OrderBy(taskParticipation => taskParticipation.IsRead).ThenByDescending(taskParticipation => taskParticipation.TaskId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        //    return myTaskList;

        //}
        
        //#endregion

        //#region 02.获取我的任务总数
        ///// <summary>
        ///// 获取我的任务总数
        ///// </summary>
        ///// <param name="stuNum"></param>
        ///// <param name="taskTypeId"></param>
        ///// <returns></returns>
        //public int GetMyTaskCount(string stuNum, int taskTypeId)
        //{
        //    DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
        //    DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();

        //    var query = from taskParticipation in taskParticipations
        //                where taskParticipation.TaskReceiver == stuNum
        //                join taskInformation in taskInformations
        //                on taskParticipation.TaskId equals taskInformation.TaskId into collection
        //                from list in collection.DefaultIfEmpty()
        //                where list.TaskTypeId == taskTypeId
        //                select list;
        //    return query.Count();
        //}
        
        //#endregion

        #region 03.获取我的发布记录总数
        /// <summary>
        /// 获取我的发布记录总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public int GetReleaseHistoryCount(string stuNum)
        {
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();

            int count = taskInformations.Where(taskInformation => taskInformation.TaskSender == stuNum).Count();

            return count;
        } 
        #endregion

        #region 04.保存任务信息
        /// <summary>
        /// 保存任务信息
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool SaveTask(MODEL.T_TaskInformation taskInfo,string[] members)
        {
            db.Set<T_TaskInformation>().Add(taskInfo);

            if (db.SaveChanges() > 0)
            {
                foreach(string member  in members)
                {
                    MODEL.T_TaskParticipation tp = new  MODEL.T_TaskParticipation();
                    tp.TaskReceiver = member;
                    tp.TaskId = taskInfo.TaskId;
                    tp.TaskGrade = 0;
                    tp.TaskResponse = "";
                    tp.IsRead = false;
                    tp.IsComplete = false;
                    db.Set<T_TaskParticipation>().Add(tp);
                }
                return db.SaveChanges() > 0;
            }
            else
            {
                return false;
            }            
        }
        #endregion

        //#region 05.获取任务详情
        ///// <summary>
        ///// 获取任务详情
        ///// </summary>
        ///// <param name="taskId"></param>
        ///// <returns></returns>
        //public MODEL.ViewModel.Task.TaskDetail GetTaskDetailById(int taskId)
        //{
        //    DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
        //    DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
        //    DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();
        //    DbSet<T_ProjectInformation> projectInformations = db.Set<T_ProjectInformation>();
        //    DbSet<T_ProjPhase> projPhases = db.Set<T_ProjPhase>();
        //    DbSet<T_Role> roles = db.Set<T_Role>();
        //    DbSet<T_RoleAct> roleActs = db.Set<T_RoleAct>();
        //    DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();

        //    List<MODEL.ViewModel.Task.TaskDetail> listTaskDetail = (from taskInformation in taskInformations 
        //                                                            join taskType in taskTypes on taskInformation.TaskTypeId 
        //                                                            equals taskType.TaskTypeId
        //                                                            join memberInformation in memberInformations on taskInformation.TaskSender 
        //                                                            equals memberInformation.StuNum
        //                                                            join projectInformation in projectInformations on taskInformation.ProjId 
        //                                                            equals projectInformation.ProjId
        //                                                            join projPhase in projPhases on taskInformation.ProjPhaseId
        //                                                            equals projPhase.ProjPhaseId
        //                                                            where taskInformation.TaskId == taskId
        //                                                            select new MODEL.ViewModel.Task.TaskDetail()
        //                               {
        //                                    TaskName = taskInformation.TaskName,
        //                                    TaskId = taskInformation.TaskId,
        //                                    TaskTypeId = taskType.TaskTypeId,
        //                                    TaskTypeName = taskType.TaskTypeName,
        //                                    TaskSender = taskInformation.TaskSender,
        //                                    TaskSenderName = memberInformation.StuName,
        //                                    TaskContent = taskInformation.TaskContent,
        //                                    TaskBegTime = taskInformation.TaskBegTime,
        //                                    TaskEndTime = taskInformation.TaskEndTime, 
        //                                    ProjId = taskInformation.ProjId,
        //                                    ProjName = projectInformation.ProjName,
        //                                    ProjPhaseId = taskInformation.ProjPhaseId,
        //                                    ProjPhaseName = projPhase.ProjPhaseName,
        //                                    TaskAttachmentPath = taskInformation.TaskAttachmentPath,
                                           
        //                               }).OrderBy(taskInformation => taskInformation.TaskId).ToList();
        //    if(listTaskDetail.Count > 0){
        //        string taskSender =  listTaskDetail[0].TaskSender;
        //        List<T_Role> listRole = (from roleAct in roleActs
        //                              join role in roles
        //                              on roleAct.RoleId equals role.RoleId
        //                                 where roleAct.RoleActor == taskSender
        //                                 select role
        //                                  ).OrderBy(r => r.RoleId).ToList();
        //        if (listRole.Count > 0)
        //        {
        //            listTaskDetail[0].TaskSenderRoles = listRole[0].RoleName.Trim();
        //            for (int i = 1; i < listRole.Count; i++)
        //            {
        //                listTaskDetail[0].TaskSenderRoles += "," + listRole[i].RoleName.Trim();
        //            }
        //        }
        //        else
        //        {
        //            listTaskDetail[0].TaskSenderRoles = "";
        //        }
                
        //        return listTaskDetail[0];
        //    }else{
        //        return null;
        //    }             
        //}
        //#endregion

        #region 06.获取分页的任务评价数据
        /// <summary>
        /// 获取分页的任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public List<MODEL.ViewModel.Task.TaskEvaluateModel> GetPagedTaskEvaluateList(int pageIndex, int pageSize, string stuNum,int taskId)
        {
            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

            List<MODEL.ViewModel.Task.TaskEvaluateModel> listTaskEvaluate = (from taskParticipation in taskParticipations
                                                                             join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
                                                                             join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                                                             join memberInformation1 in memberInformations on taskInformation.TaskSender equals memberInformation1.StuNum
                                                                             join memberInformation2 in memberInformations on taskParticipation.TaskReceiver equals memberInformation2.StuNum
                                                                             where (taskInformation.TaskSender == stuNum
                                                                             || memberInformation2.StudyGuideNumber == stuNum)
                                                                             && taskParticipation.TaskId == taskId
                                                                             select new MODEL.ViewModel.Task.TaskEvaluateModel()
                                                                             {
                                                                                 TaskId = taskInformation.TaskId,
                                                                                 TaskName = taskInformation.TaskName,
                                                                                 TaskSender = taskInformation.TaskSender,
                                                                                 TaskSenderName = memberInformation1.StuName,
                                                                                 TaskTypeId = taskInformation.TaskTypeId,
                                                                                 TaskTypeName = taskType.TaskTypeName,
                                                                                 TaskContent = taskInformation.TaskContent,
                                                                                 TaskEndTime = taskInformation.TaskEndTime,
                                                                                 TaskReceiver = taskParticipation.TaskReceiver,
                                                                                 TaskReceiverName = memberInformation2.StuName,
                                                                                 TaskGrade = taskParticipation.TaskGrade,
                                                                                 IsRead = taskParticipation.IsRead,
                                                                                 IsComplete = taskParticipation.IsComplete,
                                                                                 TaskResponse = taskParticipation.TaskResponse,
                                                                                 TaskFinishTime = taskParticipation.TaskFinishTime,
                                                                                 TaskAttachmentPath = taskParticipation.TaskAttachmentPath
                                                                             }).OrderByDescending(taskParticipation => taskParticipation.TaskId).ThenByDescending(taskParticipation => taskParticipation.TaskSender).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return listTaskEvaluate;
        } 
        #endregion

        #region 06.1.获取任务评价数据
        /// <summary>
        /// 获取任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public List<MODEL.ViewModel.Task.TaskEvaluateModel> GetTaskEvaluateList(string stuNum, int taskId)
        {
            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

            List<MODEL.ViewModel.Task.TaskEvaluateModel> listTaskEvaluate = (from taskParticipation in taskParticipations
                                                                             join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
                                                                             join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                                                             join memberInformation1 in memberInformations on taskInformation.TaskSender equals memberInformation1.StuNum
                                                                             join memberInformation2 in memberInformations on taskParticipation.TaskReceiver equals memberInformation2.StuNum
                                                                             where (taskParticipation.TaskReceiver == stuNum)
                                                                             && taskParticipation.TaskId == taskId
                                                                             select new MODEL.ViewModel.Task.TaskEvaluateModel()
                                                                             {
                                                                                 TaskId = taskInformation.TaskId,
                                                                                 TaskName = taskInformation.TaskName,
                                                                                 TaskSender = taskInformation.TaskSender,
                                                                                 TaskSenderName = memberInformation1.StuName,
                                                                                 TaskTypeId = taskInformation.TaskTypeId,
                                                                                 TaskTypeName = taskType.TaskTypeName,
                                                                                 TaskContent = taskInformation.TaskContent,
                                                                                 TaskEndTime = taskInformation.TaskEndTime,
                                                                                 TaskReceiver = taskParticipation.TaskReceiver,
                                                                                 TaskReceiverName = memberInformation2.StuName,
                                                                                 TaskGrade = taskParticipation.TaskGrade,
                                                                                 IsRead = taskParticipation.IsRead,
                                                                                 IsComplete = taskParticipation.IsComplete,
                                                                                 TaskResponse = taskParticipation.TaskResponse,
                                                                                 TaskAttachmentPath = taskParticipation.TaskAttachmentPath
                                                                             }).OrderByDescending(taskParticipation => taskParticipation.TaskId).ThenByDescending(taskParticipation => taskParticipation.TaskSender).ToList();
            return listTaskEvaluate;
        }
        #endregion

        #region 07.获取任务评价总数
        /// <summary>
        /// 获取任务评价总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public int GetTaskEvaluateCount(string stuNum,int taskId)
        {
            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

            List<MODEL.ViewModel.Task.TaskEvaluateModel> listTaskEvaluate = (from taskParticipation in taskParticipations
                                                                             join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
                                                                             join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                                                             join memberInformation1 in memberInformations on taskInformation.TaskSender equals memberInformation1.StuNum
                                                                             join memberInformation2 in memberInformations on taskParticipation.TaskReceiver equals memberInformation2.StuNum
                                                                             where (taskInformation.TaskSender == stuNum
                                                                             || memberInformation2.StudyGuideNumber == stuNum)
                                                                             && taskInformation.TaskId == taskId
                                                                             select new MODEL.ViewModel.Task.TaskEvaluateModel()
                                                                             {
                                                                                 TaskId = taskInformation.TaskId,
                                                                                 TaskName = taskInformation.TaskName,
                                                                                 TaskSender = taskInformation.TaskSender,
                                                                                 TaskSenderName = memberInformation1.StuName,
                                                                                 TaskTypeId = taskInformation.TaskTypeId,
                                                                                 TaskTypeName = taskType.TaskTypeName,
                                                                                 TaskContent = taskInformation.TaskContent,
                                                                                 TaskEndTime = taskInformation.TaskEndTime,
                                                                                 TaskReceiver = taskParticipation.TaskReceiver,
                                                                                 TaskReceiverName = memberInformation2.StuName,
                                                                                 TaskGrade = taskParticipation.TaskGrade,
                                                                                 IsRead = taskParticipation.IsRead,
                                                                                 IsComplete = taskParticipation.IsComplete,
                                                                                 TaskResponse = taskParticipation.TaskResponse
                                                                             }).OrderByDescending(taskParticipation => taskParticipation.TaskId).ToList();
            return listTaskEvaluate.Count;
        }
        #endregion

        #region 08. 分页查询(逆序&&顺序) +  GetPagedList
        /// <summary>
        ///  分页查询 +  GetPagedList
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns></returns>
        public List<MODEL.T_TaskInformation> TaskGetPagedList<t>(int pageIndex, int pageSize, Expression<Func<MODEL.T_TaskInformation, bool>> whereLambda, Expression<Func<MODEL.T_TaskInformation, t>> orderBy, bool isDesc)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if(isDesc)
            {
                return db.Set<MODEL.T_TaskInformation>().Where(whereLambda).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }else
            {
                return db.Set<MODEL.T_TaskInformation>().Where(whereLambda).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            
        }
        #endregion

 //------------------------------------------------2015/7/9----------------------------------------------------------------------------


        #region 9.1 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        /// <summary>
        /// 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        /// </summary>
        /// <param name="taskBegTime"></param>
        /// <returns></returns>
        public List<T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        {
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();

            List<MODEL.T_TaskInformation> taskCountOfBegTime = (from taskInformation in taskInformations where taskInformation.TaskBegTime == taskBegTime select taskInformation).ToList();

            return taskCountOfBegTime;
        } 
        #endregion

        #region 9.2 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// <summary>
        /// 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// </summary>
        /// <param name="_taskType"></param>
        /// <returns></returns>
        public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        {
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            List<MODEL.T_TaskInformation> taskCountOfType = (from taskInformation in taskInformations
                                                             join taskType in taskTypes
                                                             on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                                             where taskType.TaskTypeName == _taskType
                                                             select taskInformation).ToList();
            return taskCountOfType;
        } 
        #endregion

        #region 05.获取任务详情
        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public MODEL.ViewModel.Task.TaskDetail GetTaskDetailById(int taskId)
        {
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();
            DbSet<T_ProjectInformation> projectInformations = db.Set<T_ProjectInformation>();
            DbSet<T_ProjPhase> projPhases = db.Set<T_ProjPhase>();
            DbSet<T_Role> roles = db.Set<T_Role>();
            DbSet<T_RoleAct> roleActs = db.Set<T_RoleAct>();
            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();

            List<MODEL.ViewModel.Task.TaskDetail> listTaskDetail = (from taskInformation in taskInformations
                                                                    join taskType in taskTypes on taskInformation.TaskTypeId
                                                                    equals taskType.TaskTypeId
                                                                    join taskParticipation in taskParticipations on taskInformation.TaskId
                                                                    equals taskParticipation.TaskId
                                                                    join memberInformation in memberInformations on taskInformation.TaskSender
                                                                    equals memberInformation.StuNum
                                                                    join projectInformation in projectInformations on taskInformation.ProjId
                                                                    equals projectInformation.ProjId
                                                                    join projPhase in projPhases on taskInformation.ProjPhaseId
                                                                    equals projPhase.ProjPhaseId
                                                                    where taskInformation.TaskId == taskId
                                                                    select new MODEL.ViewModel.Task.TaskDetail()
                                                                    {
                                                                        TaskName = taskInformation.TaskName,
                                                                        TaskId = taskInformation.TaskId,
                                                                        TaskTypeId = taskType.TaskTypeId,
                                                                        TaskTypeName = taskType.TaskTypeName,
                                                                        TaskSender = taskInformation.TaskSender,
                                                                        TaskSenderName = memberInformation.StuName,
                                                                        TaskContent = taskInformation.TaskContent,
                                                                        TaskBegTime = taskInformation.TaskBegTime,
                                                                        TaskEndTime = taskInformation.TaskEndTime,
                                                                        ProjId = taskInformation.ProjId,
                                                                        ProjName = projectInformation.ProjName,
                                                                        ProjPhaseId = taskInformation.ProjPhaseId,
                                                                        ProjPhaseName = projPhase.ProjPhaseName,
                                                                        TaskAttachmentPath = taskInformation.TaskAttachmentPath,
                                                                        TaskGrade = taskParticipation.TaskGrade,
                                                                        TaskResponse = taskParticipation.TaskResponse              
                                                                    }).OrderBy(taskInformation => taskInformation.TaskId).ToList();
            if (listTaskDetail.Count > 0)
            {
                string taskSender = listTaskDetail[0].TaskSender;
                List<T_Role> listRole = (from roleAct in roleActs
                                         join role in roles
                                         on roleAct.RoleId equals role.RoleId
                                         where roleAct.RoleActor == taskSender
                                         select role
                                          ).OrderBy(r => r.RoleId).ToList();
                if (listRole.Count > 0)
                {
                    listTaskDetail[0].TaskSenderRoles = listRole[0].RoleName.Trim();
                    for (int i = 1; i < listRole.Count; i++)
                    {
                        listTaskDetail[0].TaskSenderRoles += "," + listRole[i].RoleName.Trim();
                    }
                }
                else
                {
                    listTaskDetail[0].TaskSenderRoles = "";
                }

                return listTaskDetail[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 01.获取我的未完成任务分页数据
        /// <summary>
        /// 获取我的任务分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="stuNum">学号</param>
        /// <param name="taskTypeId">任务类型ID</param>
        /// <returns></returns>
        public List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, bool complete)
        {

            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

            List<MyTask> myTaskList = (from taskParticipation in taskParticipations
                                       join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
                                       join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                       join memberInformation in memberInformations on taskInformation.TaskSender equals memberInformation.StuNum
                                       where taskParticipation.TaskReceiver == stuNum
                                       && taskParticipation.IsComplete == complete
                                       select new MyTask()
                                       {
                                           TaskId = taskParticipation.TaskId,
                                           TaskSender = taskInformation.TaskSender,
                                           TaskName = taskInformation.TaskName,
                                           TaskTypeId = taskInformation.TaskTypeId,
                                           TaskContent = taskInformation.TaskContent,
                                           TaskBegTime = taskInformation.TaskBegTime,
                                           TaskEndTime = taskInformation.TaskEndTime,
                                           TaskReceiver = taskParticipation.TaskReceiver,
                                           TaskGrade = taskParticipation.TaskGrade,
                                           IsRead = taskParticipation.IsRead,
                                           IsComplete = taskParticipation.IsComplete,
                                           TaskSenderName = memberInformation.StuName,
                                           TaskTypeName = taskType.TaskTypeName
                                       }).OrderByDescending(taskInformation => taskInformation.TaskEndTime).ThenByDescending(taskParticipation => taskParticipation.TaskEndTime).ThenByDescending(taskParticipation => taskParticipation.TaskId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return myTaskList;

        }

        #endregion

        #region 01.获取我的已完成任务分页数据
        /// <summary>
        /// 获取我的任务分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="stuNum">学号</param>
        /// <param name="taskTypeId">任务类型ID</param>
        /// <returns></returns>
        public List<MyTask> GetPagedCompletedTaskList(int pageIndex, int pageSize, string stuNum, bool complete)
        {

            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            DbSet<T_TaskType> taskTypes = db.Set<T_TaskType>();
            DbSet<T_MemberInformation> memberInformations = db.Set<T_MemberInformation>();

            List<MyTask> myTaskList = (from taskParticipation in taskParticipations
                                       join taskInformation in taskInformations on taskParticipation.TaskId equals taskInformation.TaskId
                                       join taskType in taskTypes on taskInformation.TaskTypeId equals taskType.TaskTypeId
                                       join memberInformation in memberInformations on taskInformation.TaskSender equals memberInformation.StuNum
                                       where taskParticipation.TaskReceiver == stuNum
                                       && taskParticipation.IsComplete == complete
                                       select new MyTask()
                                       {
                                           TaskId = taskParticipation.TaskId,
                                           TaskSender = taskInformation.TaskSender,
                                           TaskName = taskInformation.TaskName,
                                           TaskTypeId = taskInformation.TaskTypeId,
                                           TaskContent = taskInformation.TaskContent,
                                           TaskBegTime = taskInformation.TaskBegTime,
                                           TaskEndTime = taskInformation.TaskEndTime,
                                           TaskReceiver = taskParticipation.TaskReceiver,
                                           TaskGrade = taskParticipation.TaskGrade,
                                           IsRead = taskParticipation.IsRead,
                                           IsComplete = taskParticipation.IsComplete,
                                           TaskSenderName = memberInformation.StuName,
                                           TaskTypeName = taskType.TaskTypeName,
                                           TaskFinishTime = taskParticipation.TaskFinishTime
                                       }).OrderByDescending(taskInformation => taskInformation.TaskEndTime).ThenByDescending(taskParticipation => taskParticipation.TaskFinishTime).ThenByDescending(taskParticipation => taskParticipation.TaskId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return myTaskList;

        }

        #endregion

        #region 02.获取我的任务总数
        /// <summary>
        /// 获取我的任务总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public int GetMyTaskCount(string stuNum, bool complete)
        {
            DbSet<T_TaskParticipation> taskParticipations = db.Set<T_TaskParticipation>();
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();

            var query = from taskParticipation in taskParticipations
                        where taskParticipation.TaskReceiver == stuNum && taskParticipation.IsComplete == complete
                        join taskInformation in taskInformations
                        on taskParticipation.TaskId equals taskInformation.TaskId into collection
                        from list in collection.DefaultIfEmpty()
                        select list;
            return query.Count();
        }

        #endregion

        #region 根据任务Id获得任务信息
        /// <summary>
        /// 根据任务Id获得任务信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public MODEL.T_TaskInformation GetTaskInfoByTaskId(int taskId)
        {
            DbSet<T_TaskInformation> taskInformations = db.Set<T_TaskInformation>();
            List<MODEL.T_TaskInformation> listTaskInfo = (from taskInformation in taskInformations
                                                          where taskInformation.TaskId == taskId
                                                          select taskInformation).ToList();
            return listTaskInfo[0];
        } 
        #endregion

        #region 保存任务提交信息 + public bool SaveTaskSubmit(MODEL.T_TaskInformation taskInfo)
        /// <summary>
        /// 保存任务提交信息 + public bool SaveTaskSubmit(MODEL.T_TaskInformation taskInfo)
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public bool SaveTaskSubmit(MODEL.T_TaskInformation taskInfo)
        {
            db.Set<MODEL.T_TaskInformation>().Add(taskInfo);
            return db.SaveChanges() > 0;
        } 
        #endregion
    }
}
