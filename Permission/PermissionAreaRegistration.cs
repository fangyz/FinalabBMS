using System.Web.Mvc;

namespace Permission
{
    public class PermissionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Permission";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Permission_default",
                "Permission/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                //控制器所在命名空间
                new string[1] { "Permission" }
            );
        }
    }
}
