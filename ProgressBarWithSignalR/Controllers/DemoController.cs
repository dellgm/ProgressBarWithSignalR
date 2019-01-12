using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProgressBarWithSignalR.Controllers
{
    public class ViewModelBase
    {
        protected ViewModelBase(string title = "")
        {
            Title = title;
        }

        public static ViewModelBase Default(string title = "")
        {
            var model = new ViewModelBase(title);
            return model;
        }

        public string Title { get; set; }

    }

    public class DemoController : Controller
    {  
        public DemoController()
        {

        }

        public IActionResult Progress()
        {
            return View();
        }
    }
}