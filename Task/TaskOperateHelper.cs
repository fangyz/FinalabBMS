using IBLL;
using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    /// <summary>
    /// 任务模块帮助类
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public class TaskOperateHelper
    {
        IBLLSession iBLLSession = new OperateContext().BLLSession as IBLLSession;

        //获取所有拥有权限的任务类型
        public List<MODEL.T_TaskType> GetAccessRightTaskTypeList()
        {
            List<MODEL.T_TaskType> listTaskType = new List<MODEL.T_TaskType>();

            MODEL.T_MemberInformation user = new OperateContext().Usr;
            string stuNum = user.StuNum;

            if (iBLLSession.IProjectInformationBLL.GetListBy(proj => proj.ProjLeader == stuNum).Count > 0)
            {
                //添加项目任务
                listTaskType.Add(iBLLSession.ITaskTypeBLL.GetListBy(tp => tp.TaskTypeId == 10003)[0]);
            }

            List<MODEL.T_RoleAct> listRoleAct = iBLLSession.IRoleActBLL.GetListBy(ra => ra.RoleActor == stuNum);

            foreach(MODEL.T_RoleAct role in listRoleAct)
            {
                if (role.RoleId == 10001)//总裁
                {
                    listTaskType.Add(iBLLSession.ITaskTypeBLL.GetListBy(tp => tp.TaskTypeId == 10004)[0]);//学生讲座
                }
                if (role.RoleId == 10002)//部长
                {
                    listTaskType.Add(iBLLSession.ITaskTypeBLL.GetListBy(tp => tp.TaskTypeId == 10002)[0]);//开发任务
                }
                if (role.RoleId == 10003)//团长
                {
                    listTaskType.Add(iBLLSession.ITaskTypeBLL.GetListBy(tp => tp.TaskTypeId == 10001)[0]);//学习任务
                }
            }

            return listTaskType;
        }
    }
}
