using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL.ViewModel
{
    public class PermissionMsg
    {
        /// <summary>
        /// 分页时这一页的数据
        /// </summary>
        public List<MODEL.T_Permission> ListPer{get;set;}
        /// <summary>
        /// 所有数据的条数
        /// </summary>
        public int TotalRecord{get;set;}
    }
}
