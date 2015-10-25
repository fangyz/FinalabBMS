using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_AnswerSheetDTO
    {
        public int ID { get; set; }
        public int InterviewerID { get; set; }
        public int PaperID { get; set; }
        public int ChoiceScore { get; set; }
        public int BriefScore { get; set; }
        public int TotalScore { get; set; }
        public int Peoplecount { get; set; }
        public bool IsRated { get; set; }
        public MODEL.DTOModel.T_InterviewerInfoDTO InterviewerInfo { get; set; }
       
    }
}
