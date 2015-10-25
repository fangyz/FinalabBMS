using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.ViewModel
{
    public class SendBox
    {
        public string ReceiveName
        {
            get;
            set;
        }
        public string SendName
        {
            get;
            set;
        }
        public string ReceiveId
        {
            get;
            set;
        }
        public string SendId
        {
            get;
            set;
        }
        public string MessageTitle
        {
            get;
            set;
        }
        public string SendTime
        {
            get;
            set;
        }
       
        public int MessageId
        {
            get;
            set;
        }
        public int UserMessageId
        {
            get;
            set;
        }
        public bool IsRead
        {
            get;
            set;
        }
        public bool ReceiveIsDelete
        {
            get;
            set;
        }
        public bool IsDraft
        {
            get;
            set;
        }
        public bool SendIsDelete
        {
            get;
            set;
        }
    }
}
