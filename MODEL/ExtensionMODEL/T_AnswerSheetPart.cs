using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_AnswerSheet
    {
        public MODEL.DTOModel.T_AnswerSheetDTO ToDTO()
        {
            return new MODEL.DTOModel.T_AnswerSheetDTO()
            {
                ID = this.ID,
                InterviewerID = this.InterviewerID,
                PaperID = this.PaperID,
                ChoiceScore = 0,
                BriefScore=0,
                TotalScore=0,
                Peoplecount=0,
                IsRated=false,
                InterviewerInfo = new DTOModel.T_InterviewerInfoDTO() { }
            };
        }
    }
}
