using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_ChoiceAnswerSheetDTO
    {
        public int ID { get; set; }
        public int  AnswerSheetID { get; set; }
        public int  QuestionID { get; set; }
        public string Answer { get; set; }
    }
}
