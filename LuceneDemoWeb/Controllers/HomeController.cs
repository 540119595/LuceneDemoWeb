using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuceneDemoWeb.Models;
using LuceneDemoWeb.AppCode;
using Microsoft.AspNetCore.Hosting;

namespace LuceneDemoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)

        {
            _hostingEnvironment = hostingEnvironment;
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

            LuceneHelper lHelper = new LuceneHelper(_hostingEnvironment.WebRootPath);
            var eA = new EditViewArticle()
            {
                Id = 1007,
                Title = "吃神农放心肉 享健康人生——2018“神农杯”高尔夫邀请赛圆满结束",
                Summary = "5月6日，清风和畅 天高云阔 在绿草如茵、风景如画的 昆明玉龙湾高尔夫球场 2018神农杯高尔夫邀请赛华丽开杆 一百多位神农合作伙伴参加了比赛",
                CreateTime = DateTime.Now
            };
            lHelper.AddIndex(eA);
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Search()
        {
            LuceneHelper lHelper = new LuceneHelper(_hostingEnvironment.WebRootPath);
            lHelper.Search("神农");
            lHelper.Search("六月份");
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
