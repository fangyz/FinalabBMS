using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_BriefAnswerSheet
    {
        public MODEL.DTOModel.T_BriefAnswerSheetDTO ToDTO()
        {
            return new MODEL.DTOModel.T_BriefAnswerSheetDTO()
            {
                Answer = this.Answer,
                AnswerSheetID = this.AnswerSheetID,
                ID = this.ID,
                QuestionID=this.QuestionID,
                Question = new MODEL.DTOModel.T_QuestionDTO()
            };
        }
    }
}
