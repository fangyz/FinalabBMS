using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_QuestionOptionDTO
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string OptionID { get; set; }
        public string OptionContent { get; set; }
        public int OptionWeight { get; set; }

        //public T_QuestionDTO T_Question { get; set; }
    }
}
