using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOModel
{
    public class T_PaperDTO
    {
        public int ID { get; set; }
        public string PaperName { get; set; }
        public System.DateTime AddDate { get; set; }
        public string PaperProducerID { get; set; }
        public bool IsDel { get; set; }
        public string PaperProducerName { get; set; }
        public bool IsPublished { get; set; }
        public int typeId { get; set; }
    }
}
