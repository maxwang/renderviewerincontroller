using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DownLoadHtml.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;

namespace DownLoadHtml.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public HomeController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<FileContentResult> Html()
        {
            var view = _viewEngine.FindView(ControllerContext, "JobSummary", false).View;

            var writer = new StringWriter();

            ViewData.Model = new JobSummary
            {
                Name = "Zhang",
                Email = "test@gmail.com"
            };
            var viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, writer, new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            Response.Headers.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Headers.Add("Content-Disposition", "inline; filename=job.html");

            return new FileContentResult(Encoding.UTF8.GetBytes(writer.ToString()), "application/octet-stream");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
