using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    /// <summary>
    /// 给角色权限表加的方法
    /// </summary>
    public partial interface IRolePermissionBLL
    {
        //根据角色id拿到这个id所有的权限
        List<MODEL.T_Permission> GetPermissionByRoled(int roleId);
    }
}
