using MODEL.ViewModel.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// ExtensionBll---T_TaskInformationBll
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public partial class T_TaskInformation : BaseBLL<MODEL.T_TaskInformation>,IBLL.ITaskInformationBLL
    {
        //#region 01.获取我的任务分页数据
        ///// <summary>
        ///// 获取我的任务分页数据
        ///// </summary>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="stuNum"></param>
        ///// <param name="taskTypeId"></param>
        ///// <returns></returns>
        //public List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, int taskTypeId)
        //{
        //    return DBSession.ITaskInformationDAL.GetPagedTaskList(pageIndex, pageSize, stuNum, taskTypeId);
        //} 
        //#endregion

        //#region 02.获取我的任务条数
        ///// <summary>
        ///// 获取我的任务条数
        ///// </summary>
        ///// <param name="stuNum"></param>
        ///// <param name="taskTypeId"></param>
        ///// <returns></returns>
        //public int GetMyTaskCount(string stuNum, int taskTypeId)
        //{
        //    return DBSession.ITaskInformationDAL.GetMyTaskCount(stuNum, taskTypeId);
        //} 
        //#endregion

        #region 03.获取发布的记录条数
        /// <summary>
        /// 获取发布的记录条数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public int GetReleaseHistoryCount(string stuNum)
        {
            return DBSession.ITaskInformationDAL.GetReleaseHistoryCount(stuNum);
        } 
        #endregion

        #region 04.保存任务
        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool SaveTask(MODEL.T_TaskInformation taskInfo, string[] members)
        {
            return DBSession.ITaskInformationDAL.SaveTask(taskInfo, members);
        } 
        #endregion

        #region 05.获取任务详细信息
        /// <summary>
        /// 获取任务详细信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public MODEL.ViewModel.Task.TaskDetail GetTaskDetailById(int taskId)
        {
            return DBSession.ITaskInformationDAL.GetTaskDetailById(taskId);
        } 
        #endregion

        #region 06.删除任务
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskSender"></param>
        /// <returns></returns>
        public bool DeleteTask(int taskId, string taskSender)
        {
            if (DBSession.ITaskInformationDAL.GetListBy(t => t.TaskId == taskId && t.TaskSender == taskSender).Count > 0
                && DBSession.ITaskParticipationDAL.GetListBy(tp => tp.TaskId == taskId && tp.IsRead == true).Count <= 0)
            {
                if (DBSession.ITaskParticipationDAL.DelBy(tp => tp.TaskId == taskId) > 0)
                {
                    if (DBSession.ITaskInformationDAL.DelBy(t => t.TaskId==taskId ) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        } 
        #endregion

        #region 07.任务是否可修改
        /// <summary>
        /// 任务是否可修改
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public bool[] CanByModify(List<int> taskIds)
        {
            bool[] canBeModify = new bool[taskIds.Count];

            for (int i = 0; i < taskIds.Count; i++)
            {
                int taskId = taskIds[i];
                canBeModify[i] = !(DBSession.ITaskParticipationDAL.GetListBy(tp => tp.TaskId == taskId && tp.IsRead == true).Count > 0);
            }

            return canBeModify;
        }
        #endregion

        #region 08.查询任务是否可修改
        /// <summary>
        /// 查询任务是否可修改
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool CanByModify(int taskId)
        {
            return !(DBSession.ITaskParticipationDAL.GetListBy(tp => tp.TaskId == taskId && tp.IsRead == true).Count > 0);
        } 
        #endregion

        #region 09.获取分页的任务评价数据
        /// <summary>
        /// 获取分页的任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public List<MODEL.ViewModel.Task.TaskEvaluateModel> GetPagedTaskEvaluateList(int pageIndex, int pageSize, string stuNum,int taskId)
        {
            return DBSession.ITaskInformationDAL.GetPagedTaskEvaluateList(pageIndex, pageSize, stuNum, taskId);
        }
        #endregion


        #region 10.获取任务评价总数
        /// <summary>
        /// 获取任务评价总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public int GetTaskEvaluateCount(string stuNum,int taskId)
        {
            return DBSession.ITaskInformationDAL.GetTaskEvaluateCount(stuNum, taskId);
        } 
        #endregion

        #region 06.1 获取任务评价数据
        /// <summary>
        /// 获取任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        public List<MODEL.ViewModel.Task.TaskEvaluateModel> GetTaskEvaluateList(string stuNum, int taskId)
        {
            return DBSession.ITaskInformationDAL.GetTaskEvaluateList(stuNum, taskId);
        }
        #endregion

        #region 11.保存任务评价
        /// <summary>
        /// 保存任务评价
        /// </summary>
        /// <param name="operatorStuNum"></param>
        /// <param name="taskReceiver"></param>
        /// <param name="taskId"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public bool SaveTaskGrade(string operatorStuNum, string taskReceiver, int taskId, int grade)
        {
            //只有学习顾问或者任务发布人才有更改权限
            if (DBSession.IMemberInformationDAL.GetListBy(m => m.StuNum == taskReceiver
                && m.StudyGuideNumber == operatorStuNum).Count > 0
                || DBSession.ITaskInformationDAL.GetListBy(t => t.TaskSender == operatorStuNum
                    && t.TaskId == taskId).Count > 0)
            {
                MODEL.T_TaskParticipation tp = new MODEL.T_TaskParticipation();
                tp.TaskGrade = grade;
                tp.TaskId = taskId;
                tp.TaskReceiver = taskReceiver;
                if (DBSession.ITaskParticipationDAL.ModifyBy(tp, t => t.TaskId == taskId && t.TaskReceiver == taskReceiver, "TaskGrade") > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 12.保存任务评语
        /// <summary>
        /// 保存任务评价
        /// </summary>
        /// <param name="operatorStuNum"></param>
        /// <param name="taskReceiver"></param>
        /// <param name="taskId"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public bool SaveTaskResponse(string operatorStuNum, string taskReceiver, int taskId, string taskResponse)
        {
            //只有学习顾问或者任务发布人才有更改权限
            if (DBSession.IMemberInformationDAL.GetListBy(m => m.StuNum == taskReceiver
                && m.StudyGuideNumber == operatorStuNum).Count > 0
                || DBSession.ITaskInformationDAL.GetListBy(t => t.TaskSender == operatorStuNum
                    && t.TaskId == taskId).Count > 0)
            {
                MODEL.T_TaskParticipation tp = new MODEL.T_TaskParticipation();
                tp.TaskResponse = taskResponse;
                tp.TaskId = taskId;
                tp.TaskReceiver = taskReceiver;
                if (DBSession.ITaskParticipationDAL.ModifyBy(tp, t => t.TaskId == taskId && t.TaskReceiver == taskReceiver, "TaskResponse") > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region  保存任务 +  public bool SaveTaskCorrect(string operatorStuNum, string taskReceiver, int taskId, string taskResponse,string grade)
        /// <summary>
        /// 保存任务 +  public bool SaveTaskCorrect(string operatorStuNum, string taskReceiver, int taskId, string taskResponse,string grade)
        /// </summary>
        /// <param name="operatorStuNum"></param>
        /// <param name="taskReceiver"></param>
        /// <param name="taskId"></param>
        /// <param name="taskResponse"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public bool SaveTaskCorrect(string operatorStuNum, string taskReceiver, int taskId, string taskResponse, int grade)
        {
            if (DBSession.IMemberInformationDAL.GetListBy(m => m.StuNum == taskReceiver && m.StudyGuideNumber == operatorStuNum).Count > 0
                || DBSession.ITaskInformationDAL.GetListBy(t => t.TaskSender == operatorStuNum && t.TaskId == taskId).Count > 0)
            {
                MODEL.T_TaskParticipation tp = new MODEL.T_TaskParticipation();
                tp.TaskGrade = grade;
                tp.TaskId = taskId;
                tp.TaskReceiver = taskReceiver;
                tp.TaskResponse = taskResponse;
                if (DBSession.ITaskParticipationDAL.ModifyBy(tp, t => t.TaskId == taskId && t.TaskReceiver == taskReceiver, "TaskResponse", "TaskGrade") > 0)
                {
                    return true;
                }
            }
            return false;
        } 
        #endregion

        #region 13. 分页查询(逆序&&顺序) +  GetPagedList
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
            return DBSession.ITaskInformationDAL.TaskGetPagedList<t>(pageIndex, pageSize, whereLambda, orderBy, isDesc);
        }
        #endregion



//---------------------------------------------------------------------------2015/7/9-----------------------------------------------------
       
        #region 14.1 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        /// <summary>
        /// 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        /// </summary>
        /// <param name="taskBegTime"></param>
        /// <returns></returns>
        public List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        {
            return DBSession.ITaskInformationDAL.GetTaskCountOfBegTime(taskBegTime);
        } 
        #endregion

        #region 14.2 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// <summary>
        /// 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// </summary>
        /// <param name="_taskType"></param>
        /// <returns></returns>
        public List<MODEL.T_TaskInformation> GetTaskCountOfType(string _taskType)
        {
            return DBSession.ITaskInformationDAL.GetTaskCountOfType(_taskType);
        } 
        #endregion

        #region 01.获取我的未完成任务分页数据
        /// <summary>
        /// 获取我的任务分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, bool complete)
        {
            return DBSession.ITaskInformationDAL.GetPagedTaskList(pageIndex, pageSize, stuNum, complete);
        }
        #endregion

        #region 01.获取我的已完成任务分页数据
        /// <summary>
        /// 获取我的任务分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public List<MyTask> GetPagedCompletedTaskList(int pageIndex, int pageSize, string stuNum, bool complete)
        {
            return DBSession.ITaskInformationDAL.GetPagedCompletedTaskList(pageIndex, pageSize, stuNum, complete);
        }
        #endregion

        #region 02.获取我的任务条数
        /// <summary>
        /// 获取我的任务条数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public int GetMyTaskCount(string stuNum, bool complete)
        {
            return DBSession.ITaskInformationDAL.GetMyTaskCount(stuNum, complete);
        }
        #endregion

        #region 根据任务Id得到任务信息
        /// <summary>
        /// 根据任务Id得到任务信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public MODEL.T_TaskInformation GetTaskInfoByTaskId(int taskId)
        {
            return DBSession.ITaskInformationDAL.GetTaskInfoByTaskId(taskId);
        }
        #endregion

    }
}
