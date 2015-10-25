using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.ViewModel.Task
{
    /// <summary>
    /// 任务评价视图模型
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public class TaskEvaluateModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskSender { get; set; }
        public string TaskSenderName { get; set; }         
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }
        public string TaskContent { get; set; }
        public System.DateTime TaskEndTime { get; set; }
        public System.DateTime? TaskFinishTime { get; set; }
        public string TaskAttachmentPath { get; set; }

        public string TaskReceiver { get; set; }
        public string TaskReceiverName { get; set; }
        public int TaskGrade { get; set; }
        public bool IsRead { get; set; }
        public bool IsComplete { get; set; }
        public string TaskResponse { get; set; }
    }
}
