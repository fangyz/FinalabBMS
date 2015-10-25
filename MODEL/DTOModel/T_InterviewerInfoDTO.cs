using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_InterviewerInfoDTO
    {
        public int ID { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Academy { get; set; }
        public string Major { get; set; }
        public string Class { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string TelNum { get; set; }
        public string LearningExperience { get; set; }
        public string SelfEvaluation { get; set; }
        public bool IsRequestTech { get; set; }
    }
}
