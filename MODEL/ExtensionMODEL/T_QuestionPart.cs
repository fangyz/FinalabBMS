using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_Question
    {
        /// <summary>
        /// 将EF实体转成DTO实体
        /// </summary>
        /// <returns></returns>
        public DTOModel.T_QuestionDTO ToDTO()
        {
            return new DTOModel.T_QuestionDTO()
            {
                ID = this.ID,
                QuestionTypeID = this.QuestionTypeID,
                QuestionContent = this.QuestionContent,
                IsDel = this.IsDel,
                QuestionGrade = Convert.ToInt32(this.QuestionGrade),
                QuestionTag = this.QuestionTag,
                T_QuestionOption = new List<DTOModel.T_QuestionOptionDTO>() { },
                BriefAnswerSheet = new MODEL.DTOModel.T_BriefAnswerSheetDTO(),
                BriefScore = new MODEL.DTOModel.T_BriefScoreDTO(),
                ChoiceAnswerSheet = new MODEL.DTOModel.T_ChoiceAnswerSheetDTO(),
                AnswerSheetComment = new MODEL.DTOModel.T_AnswerSheetCommentDTO(),
                ScoreF =0,
                ScoreS = 0,
                ScoreT=0,
                TeacherF="",
                TeacherS="",
                TeacherT=""

            };
        }
    }
}
