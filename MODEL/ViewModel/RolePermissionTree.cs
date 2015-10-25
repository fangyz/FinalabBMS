using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL.ViewModel
{
    public class RolePermissionTree
    {
        // <summary>
        /// 某角色的权限
        /// </summary>
        public List<MODEL.T_Permission> UserPer { get; set; }
        /// <summary>
        /// 系统中所有权限
        /// </summary>
        public List<MODEL.T_Permission> AllPer { get; set; }
        /// <summary>
        /// 父权限集合
        /// </summary>
        public List<MODEL.T_Permission> ParentPer { get; set; }
    }
}
