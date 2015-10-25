using System.Web.Mvc;

namespace Duty
{
    public class DutyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Duty";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Duty_default",
                "Duty/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[1] { "Duty.Controllers" }
            );
        }
    }
}
