using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IBLL
{
    public partial interface IMemberInformationBLL : IBaseBLL<MODEL.T_MemberInformation>
    {
        //导出部分数据
        DataTable GetPartData(List<String> listData,int depart);
    }
}
