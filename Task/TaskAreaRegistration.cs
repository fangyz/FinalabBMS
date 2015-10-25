using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Task
{
    public class TaskAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Task";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Task_default",
                "Task/{controller}/{action}/{id}",
                new { action = "MyTask", id = UrlParameter.Optional },
                //控制器所在命名空间
                new string[1] { "Task.Controllers" }
            );
        }
   
    }
}
