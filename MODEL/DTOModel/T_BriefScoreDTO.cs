using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_BriefScoreDTO
    {
        public int ID { get; set; }
        public int BriefAnswerSheetID { get; set; }
        public int PaperRaterID { get; set; }
        public int Score { get; set; }
        public string UserB { get; set; } 
    }
}
