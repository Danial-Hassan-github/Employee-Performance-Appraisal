using System.Web;
using System.Web.Mvc;

namespace Biit_Employee_Performance_Apraisal_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
