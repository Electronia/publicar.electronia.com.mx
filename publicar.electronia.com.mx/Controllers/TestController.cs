using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace publicar.electronia.com.mx.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public void Index()
        {
            Response.Write("Sitio funcionado al 100%");

            Response.Write( Path.GetFullPath("/images"));
            Response.Write("<br> -->" + Server.MapPath("/Images").ToString());

        }

    }
}
