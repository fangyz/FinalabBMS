using MODEL.ViewModel.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// ExtensionIDAL---ITaskInformationDAL
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public partial interface ITaskInformationDAL : IBaseDAL<MODEL.T_TaskInformation>
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
        //List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, int taskTypeId);
        //#endregion
        
        //#region 02.获取我的任务总数
        ///// <summary>
        ///// 获取我的任务总数
        ///// </summary>
        ///// <param name="stuNum"></param>
        ///// <param name="taskTypeId"></param>
        ///// <returns></returns>
        //int GetMyTaskCount(string stuNum, int taskTypeId);
        //#endregion

        #region 03.获取我的发布记录总数
        /// <summary>
        /// 获取我的发布记录总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        int GetReleaseHistoryCount(string stuNum);
        #endregion

        #region 04.保存任务信息
        /// <summary>
        /// 保存任务信息
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        bool SaveTask(MODEL.T_TaskInformation taskInfo, string[] members);
        #endregion

        #region 05.获取任务详情
        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        MODEL.ViewModel.Task.TaskDetail GetTaskDetailById(int taskId);
        #endregion

        #region 06.获取分页的任务评价数据
        /// <summary>
        /// 获取分页的任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        List<MODEL.ViewModel.Task.TaskEvaluateModel> GetPagedTaskEvaluateList(int pageIndex, int pageSize, string stuNum,int taskId);
        #endregion

         #region 06.1 获取任务评价数据
        /// <summary>
        /// 获取任务评价数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        List<MODEL.ViewModel.Task.TaskEvaluateModel> GetTaskEvaluateList(string stuNum, int taskId);
        #endregion

        #region 07.获取任务评价总数
        /// <summary>
        /// 获取任务评价总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <returns></returns>
        int GetTaskEvaluateCount(string stuNum,int taskId);
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
        List<MODEL.T_TaskInformation> TaskGetPagedList<t>(int pageIndex, int pageSize, Expression<Func<MODEL.T_TaskInformation, bool>> whereLambda, Expression<Func<MODEL.T_TaskInformation, t>> orderBy, bool isDesc);
        #endregion

//--------------------------------------------2015/7/9------------------------------------------------------------

        #region 9.1. 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime)
        /// <summary>
        /// 根据任务开始时间统计任务 +  List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime);
        /// </summary>
        /// <param name="taskBegTime">任务开始时间</param>
        /// <returns></returns>
        List<MODEL.T_TaskInformation> GetTaskCountOfBegTime(DateTime taskBegTime); 
        #endregion

        #region 9.2 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// <summary>
        /// 根据任务类型统计任务 +  public List<T_TaskInformation> GetTaskCountOfType(string _taskType)
        /// </summary>
        /// <param name="_taskType"></param>
        /// <returns></returns>
        List<MODEL.T_TaskInformation> GetTaskCountOfType(string _taskType); 
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
        List<MyTask> GetPagedTaskList(int pageIndex, int pageSize, string stuNum, bool complete);
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
        List<MyTask> GetPagedCompletedTaskList(int pageIndex, int pageSize, string stuNum, bool complete);
        #endregion
        

        #region 02.获取我的任务总数
        /// <summary>
        /// 获取我的任务总数
        /// </summary>
        /// <param name="stuNum"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        int GetMyTaskCount(string stuNum, bool complete);
        #endregion

        #region 根据任务Id得到任务信息
        /// <summary>
        /// 根据任务Id得到任务信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        MODEL.T_TaskInformation GetTaskInfoByTaskId(int taskId); 
        #endregion
    }
}
