using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace br.com.mvc.lib.mngmt.Controllers
{
    public class Error : Controller
    {
        public IActionResult Index(int code)
        {
            ViewBag.Status = code;
            return View();
        }
    }
}
