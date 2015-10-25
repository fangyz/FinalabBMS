using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public partial class T_MemberInformation:BaseBLL<MODEL.T_MemberInformation>,IBLL.IMemberInformationBLL
    {
        //导出部分数据
        public DataTable GetPartData(List<String> listData, int depart)
        {
            IDAL.IMemberInformationDAL imem = idal as IDAL.IMemberInformationDAL;
            return imem.GetPartData(listData,depart);
        }
    }
}
