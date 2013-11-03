using System.Web;
using System.Web.Mvc;

namespace publicar.electronia.com.mx
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}