using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.FormatModel
{
    /// <summary>
    /// 统一的 Ajax分页格式类
    /// </summary>
    public class AjaxPagedModel
    {
        public string Msg { get; set; }
        public string Statu { get; set; }//ok,err,nologin,
        public string BackUrl { get; set; }
        public object Data { get; set; }//数据对象
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
    }
}
