using MODEL.MenuMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public partial class T_Permission
    {
        #region 1.0 生成  +T_Permission ToPOCO()
        /// <summary>
        /// 生成 很纯洁的 实体对象
        /// </summary>
        /// <returns></returns>
        public T_Permission ToPOCO()
        {
            T_Permission poco = new T_Permission()
            {
                PerId = this.PerId,
                PerName = this.PerName,
                PerParent = this.PerParent,
                PerActionName=this.PerActionName,
                PerAreaName = this.PerAreaName,
                PerController = this.PerController,
                PerFormMethod = this.PerFormMethod,
                PerIsShow = this.PerIsShow,
                PerIco=this.PerIco
            };
            return poco;
        }
        #endregion

        #region 2.0 将当前权限 对象 转成 树节点对象 +TreeNode ToNode()
        /// <summary>
        /// 将当前权限 对象 转成 树节点对象                                           
        /// </summary>
        /// <returns></returns>
        public TreeNode ToNode()
        {
            TreeNode node = new TreeNode()
            {
                menuId = this.PerId,
                menuName = this.PerName,
                state = "open",
                Checked = false,
                url = this.GetUrl(),
                icon = new Icon() { imgPath = this.PerIco},
                childMenus = new List<TreeNode>()
            };
            return node;
        }
        #endregion

        #region 2.1 将 当前权限的 区域名+控制器名+Action方法名 合并成一个url返回 +string GetUrl()
        /// <summary>
        /// 将 当前权限的 区域名+控制器名+Action方法名 合并成一个url返回
        /// </summary>s
        protected string GetUrl()
        {
            return GetUrlPart(this.PerAreaName)
                + GetUrlPart(this.PerController)
                + GetUrlPart(this.PerActionName);
        }

        protected string GetUrlPart(string urlPart)
        {
            return string.IsNullOrEmpty(urlPart) ? "" : "/" + urlPart;
        }
        #endregion

        #region 2.2 将 权限集合 转成 树节点集合 +List<MODEL.EasyUIModel.TreeNode> ToTreeNodes(List<Ou_Permission> listPer)
        /// <summary>
        /// 将 权限集合 转成 树节点集合
        /// </summary>
        /// <param name="listPer"></param>
        /// <returns></returns>
        public static List<MODEL.MenuMODEL.TreeNode> ToTreeNodes(List<T_Permission> listPer)
        {
            List<MODEL.MenuMODEL.TreeNode> listNodes = new List<MenuMODEL.TreeNode>();
            //生成 树节点时，根据 pid=1的根节点 来生成
            LoadTreeNode(listPer, listNodes, 0);
            return listNodes;
        }
        #endregion

        #region 2.3 递归权限集合 创建 树节点集合
        /// <summary>
        /// 递归权限集合 创建 树节点集合
        /// </summary>
        /// <param name="listPer">权限集合</param>
        /// <param name="listNodes">节点集合</param>
        /// <param name="pid">节点父id</param>
        public static void LoadTreeNode(List<T_Permission> listPer, List<TreeNode> listNodes, int pid)
        {
            foreach (var permission in listPer)
            {
                //如果权限父id=参数
                if (permission.PerParent == pid)
                {
                    //将 权限转成 树节点
                    TreeNode node = permission.ToNode();
                    //将节点 加入到 树节点集合
                    listNodes.Add(node);
                    //递归 为这个新创建的 树节点找 子节点
                    LoadTreeNode(listPer, node.childMenus, node.menuId);
                }
            }
        }
        #endregion

        public T_Permission GetPermissionPart()
        {
            return new T_Permission()
            {

            };
        }
    }
}

