using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial class T_RoleAct : BaseBLL<MODEL.T_RoleAct>, IBLL.IRoleActBLL
    {
        #region 批量增加+int AddList(List<MODEL.T_RoleAct> list)
        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddList(List<MODEL.T_RoleAct> list)
        {
            IDAL.IRoleActDAL idalRole = idal as IDAL.IRoleActDAL;
            return idalRole.AddList(list);
        }
        #endregion
    }
}
