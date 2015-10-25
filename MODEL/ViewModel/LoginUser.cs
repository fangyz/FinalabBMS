using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MODEL.ViewModel
{
    /// <summary>
    /// 登陆视图 模型
    /// </summary>
    public class LoginUser
    {
        public string LoginName { get; set; }
        public string Pwd { get; set; }

        /// <summary>
        /// 为true时，是记住密码
        /// </summary>
        public bool IsAlways { get; set; }
    }
}
