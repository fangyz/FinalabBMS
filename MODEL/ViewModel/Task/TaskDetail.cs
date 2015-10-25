using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.ViewModel.Task
{
    /// <summary>
    /// 任务详情视图模型
    /// author：潘帅
    /// 2015年2月
    /// </summary>
    public class TaskDetail
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }
        public string TaskSender { get; set; }
        public string TaskSenderName { get; set; }
        public string TaskSenderRoles { get; set; }
        public string TaskContent { get; set; }
        public System.DateTime TaskBegTime { get; set; }
        public System.DateTime TaskEndTime { get; set; }
        public int ProjId { get; set; }
        public string ProjName { get; set; }
        public int ProjPhaseId { get; set; }
        public string ProjPhaseName { get; set; }
        public string TaskAttachmentPath { get; set; }
        public int TaskGrade { get; set; }
        public string TaskResponse { get; set; }
    }
}
