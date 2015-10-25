using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
   public partial class T_ChoiceAnswerSheet
    {
       public DTOModel.T_ChoiceAnswerSheetDTO ToDTO()
       {
           return new DTOModel.T_ChoiceAnswerSheetDTO() 
           {
               ID=this.ID,
               AnswerSheetID = this.AnswerSheetID,
               QuestionID = this.QuestionID,
               Answer = this.Answer

           };
       }
    }
}
