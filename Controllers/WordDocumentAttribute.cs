using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cascadingdropdownlist.Controllers
{
    public class WordDocumentAttribute : ActionFilterAttribute
    {
        public string DefaultFileName { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;
            if (result != null)
                result.MasterName = "~/Views/Shared/Layout2.cshtml";

            filterContext.Controller.ViewBag.WordDocumentMode = true;

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var filename = filterContext.Controller.ViewBag.WordDocumentFilename;
            filename = filename ?? DefaultFileName ?? "Document";
            filterContext.HttpContext.Response.ContentType = "application/msword";
            base.OnResultExecuted(filterContext);
        }


    }
}