using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IDAL
{
    public partial interface IMemberInformationDAL:IBaseDAL<MODEL.T_MemberInformation>
    {
        //导出部分数据时
        DataTable GetPartData(List<String> listData, int depart);
    }
}
