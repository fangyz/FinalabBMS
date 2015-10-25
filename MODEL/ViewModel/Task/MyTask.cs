using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.ViewModel.Task
{
    /// <summary>
    /// 我的任务视图模型
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public class MyTask
    {
        //TaskInfo表数据
        public int TaskId { get; set; }
        public string TaskSender { get; set; }
        public string TaskSenderName { get; set; }//任务发布人姓名    由外键TaskSender获取
        public string TaskName { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }//任务类型名称    由外键TaskTypeId获取
        public string TaskContent { get; set; }
        public System.DateTime TaskBegTime { get; set; }
        public System.DateTime TaskEndTime { get; set; }

        //TaskParticipate表数据
        public string TaskReceiver { get; set; }
        public int TaskGrade { get; set; }
        public bool IsRead { get; set; }
        public bool IsComplete { get; set; }
        public System.DateTime? TaskFinishTime { get; set; }

    }
}
