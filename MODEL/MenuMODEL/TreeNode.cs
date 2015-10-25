using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL.MenuMODEL
{
    /*  树的节点类
        id：节点id，对载入远程数据很重要。
        text：显示在节点的文本。
        state：节点状态，'open' or 'closed'，默认为'open'。当设置为'closed'时，拥有子节点的节点将会从远程站点载入它们。
        checked：表明节点是否被选择。
        url：结点的url。
        children：子节点，必须用数组定义。
     */
    public class TreeNode
    {
        public int menuId { get; set; }
        public string menuName { get; set; }
        public string state { get; set; }
        public bool Checked { get; set; }
        public string url { get; set; }
        public Icon icon { get; set; }
        public List<TreeNode> childMenus { get; set; }
    }
}
