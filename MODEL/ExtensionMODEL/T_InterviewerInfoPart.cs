using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_InterviewerInfo
    {
        public MODEL.DTOModel.T_InterviewerInfoDTO ToDTO()
        {
            return new MODEL.DTOModel.T_InterviewerInfoDTO()
            {
                ID = this.ID,
                Num = this.Num,
                Name = this.Name,
                Gender = this.Gender,
                Academy = this.Academy,
                Major = this.Major,
                Class = this.Class,
                QQ = this.QQ,
                Email = this.Email,
                TelNum = this.TelNum,
                LearningExperience = this.LearningExperience,
                SelfEvaluation = this.SelfEvaluation
            };
        }
    }
}
