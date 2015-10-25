using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL.FormatModel
{
    /// <summary>
    /// 统一的 Ajax格式类
    /// </summary>
    public class AjaxMsgModel
    {
        public string Msg { get; set; }
        public string Statu { get; set; }//ok,err
        public string BackUrl { get; set; }
        public object Data { get; set; }//数据对象
    }
}
