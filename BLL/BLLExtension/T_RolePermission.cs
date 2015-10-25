using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 给角色权限表加的方法
    /// </summary>
    public partial class T_RolePermission:IBLL.IRolePermissionBLL
    {
        //根据角色id拿到这个id所有的权限
        public List<MODEL.T_Permission> GetPermissionByRoled(int roleId)
        {
            return this.GetListBy(rp => rp.RoleId == roleId).Select(rp => rp.GetPermissionPart()).ToList();
        }


    }
}
