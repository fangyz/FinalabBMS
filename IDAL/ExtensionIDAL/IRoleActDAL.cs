using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IRoleActDAL : IBaseDAL<MODEL.T_RoleAct>
    {
        #region int AddList(List<MODEL.T_RoleAct> list)+批量增加
        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int AddList(List<MODEL.T_RoleAct> list);
        #endregion
    }
}
