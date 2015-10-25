using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_BriefAnswerSheetDTO
    {
        public int ID { get; set; }
        public int AnswerSheetID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public T_QuestionDTO Question { get; set; }
    }
}
