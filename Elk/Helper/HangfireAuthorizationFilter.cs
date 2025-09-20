using Hangfire.Dashboard;

namespace Elk.Helper
{
   
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {

            return true;
        }
    }
}
