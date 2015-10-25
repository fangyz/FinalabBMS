using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALMSSQL
{
    public partial class T_RoleActDAL : BaseDAL<MODEL.T_RoleAct>, IDAL.IRoleActDAL
    {
        #region 批量增加+int AddList(List<MODEL.T_RoleAct> list)
        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddList(List<MODEL.T_RoleAct> list)
        {
            foreach (MODEL.T_RoleAct role in list)
            {
                db.Set<MODEL.T_RoleAct>().Add(role);
            }
            return db.SaveChanges();
        }
        #endregion
    }
}
