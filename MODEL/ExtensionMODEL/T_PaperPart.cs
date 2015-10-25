using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_Paper
    {
        public DTOModel.T_PaperDTO ToDTO()
        {
            return new DTOModel.T_PaperDTO()
            {
                ID = this.ID,
                PaperName = this.PaperName,
                AddDate = this.AddDate,
                PaperProducerID = this.PaperProducerID,
                IsDel = this.IsDel,
                PaperProducerName = "",
                IsPublished = this.IsPublished,
                typeId=this.typeId
            };
        }
    }
}
