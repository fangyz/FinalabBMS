using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_TeacherInfo
    {
        public DTOModel.T_TeacherInfoDTO ToDTO()
        {
            return new DTOModel.T_TeacherInfoDTO()
            {
                ID = this.ID,
                Name = this.Name
            };
        }
    }
}
