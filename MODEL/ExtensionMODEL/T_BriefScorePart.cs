using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_BriefScore
    {
        public DTOModel.T_BriefScoreDTO ToDTO()
        {
            return new DTOModel.T_BriefScoreDTO()
            {
                ID = this.ID,
                BriefAnswerSheetID = this.BriefAnswerSheetID,
                PaperRaterID = this.PaperRaterID,
                Score = this.Score,
                UserB=this.UserB
            };
        }
    }
}
