using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Controllers
{
    public abstract class FonourControllerBase : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            byte[] result;
            filterContext.HttpContext.Session.TryGetValue("UserId", out result);
            if (result == null)
            {
                filterContext.Result = new RedirectResult("http://localhost:50425/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
