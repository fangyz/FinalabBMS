using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DALMSSQL
{
    public partial class T_MemberInformationDAL:BaseDAL<MODEL.T_MemberInformation>,IDAL.IMemberInformationDAL
    {
        #region 查询部分数据 +DataTable  GetPartData(List<String> listData)
        /// <summary>
        /// 查询部分数据,这部分数据时不确定的，暂时没有发现用lambda怎么写
        /// </summary>
        public DataTable GetPartData(List<String> listData, int depart)
        {
            string where = "select ";
            for (int i = 0; i < listData.Count; i++)
            {
                if (i == listData.Count - 1)
                {
                    where = where + listData[i];
                }
                else
                {
                    where = where + listData[i] + ",";
                }
            }
            where = where + " from T_MemberInformation where IsDelete=0";
            DataTable dt = new DataTable();
            if (depart != 0)
            {
                where += " and Department=" + depart+" and StuNum!='FinalAdmin'";
                dt = SqlHelper.ExecuteDataTable(where);
            }
            else
            {
                where += " and StuNum!='FinalAdmin'";
                dt = SqlHelper.ExecuteDataTable(where);
            }
            return dt;
            //Func<int, int, int> fu = (int x, int y) => x * y;
        }
        #endregion
    }
}
