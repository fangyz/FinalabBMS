using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_QuestionOption
    {
        /// <summary>
        /// 将EF实体转为DTO实体
        /// </summary>
        /// <returns></returns>
        public DTOModel.T_QuestionOptionDTO ToDTO()
        {
            return new DTOModel.T_QuestionOptionDTO()
            {
                ID = this.ID,
                QuestionID = this.QuestionID,
                OptionID = this.OptionID,
                OptionContent = this.OptionContent,
                OptionWeight = this.OptionWeight
            };
        }
    }
}
