using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository blogRepository;
        public HomeController(IBlogRepository repository)
        {
            blogRepository = repository;
        }
        public IActionResult Index()
        {
            var model = new HomeBlogModel();
            model.HomeBlogs = blogRepository.GetAll().Where(i => i.isApproved && i.isHome).ToList();
            model.SliderBlogs = blogRepository.GetAll().Where(i => i.isApproved && i.isSlider).ToList();
            //Model içerisinden ayrı ayrı alınan bilgiler paketlenir ve viewa gönderilir.
            return View(model);
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
