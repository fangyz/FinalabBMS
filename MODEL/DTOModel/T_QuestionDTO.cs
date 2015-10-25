using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_QuestionDTO
    {
        public int ID { get; set; }
        public string QuestionContent { get; set; }
        public int QuestionTypeID { get; set; }
        public bool IsDel { get; set; }
        public bool IsAdded { get; set; }
        public int QuestionGrade { get; set; }
        public string QuestionTag { get; set; }
        public List<T_QuestionOptionDTO> T_QuestionOption { get; set; }
        public T_BriefAnswerSheetDTO BriefAnswerSheet { get; set; }
        public T_BriefScoreDTO BriefScore { get; set; }
        public T_ChoiceAnswerSheetDTO ChoiceAnswerSheet { get; set; }
        public T_AnswerSheetCommentDTO AnswerSheetComment { get; set; }
        public int ScoreF { get; set; }
        public int ScoreS { get; set; }
        public int ScoreT { get; set; }
        public string TeacherF { get; set; }
        public string TeacherS { get; set; }
        public string TeacherT { get; set; }
       
    }
}
