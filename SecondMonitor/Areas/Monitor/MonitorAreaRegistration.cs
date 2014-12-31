using System.Web.Mvc;

namespace SecondMonitor.Areas.Monitor
{
    public class MonitorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BaseMonitoredObj";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Monitor_default",
                "BaseMonitoredObj/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}