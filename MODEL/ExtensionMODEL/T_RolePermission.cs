using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public partial class T_RolePermission
    {
        #region 获取角色权限里的权限部分 GetPermissionPart
        /// <summary>
        /// 获取角色权限里的权限部分
        /// </summary>
        /// <returns></returns>
        public T_Permission GetPermissionPart()
        {
            return new T_Permission()
            {
                PerId = this.T_Permission.PerId,
                PerName = this.T_Permission.PerName,
                PerParent = this.T_Permission.PerParent,
                PerAreaName = this.T_Permission.PerAreaName,
                PerController = this.T_Permission.PerController,
                PerActionName = this.T_Permission.PerActionName,
                PerFormMethod = this.T_Permission.PerFormMethod,
                PerIsShow = this.T_Permission.PerIsShow,
                PerAddTime = this.T_Permission.PerAddTime,
                PerIco = this.T_Permission.PerIco,
                IsCommon = this.T_Permission.IsCommon,
                IsDelete = this.T_Permission.IsDelete,
                bpx = this.T_Permission.bpx,
                bpy = this.T_Permission.bpy,
            };
        }
        #endregion


    }
}
