using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_AnswerSheetCommentDTO
    {
        public int ID { get; set; }
        public int AnswerSheetID { get; set; }
        public int RaterID { get; set; }
        public string CommentContent { get; set; }
        public string UserCom { get; set; }
    }
}
