using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_AnswerSheetComment
    {
        public MODEL.DTOModel.T_AnswerSheetCommentDTO ToDTO()
        {
            return new MODEL.DTOModel.T_AnswerSheetCommentDTO()
            {
                ID = this.ID,
                AnswerSheetID = this.AnswerSheetID,
                RaterID = this.RaterID,
                CommentContent = this.CommentContent,
                UserCom=this.UserCom
            };
        }
    }
}
