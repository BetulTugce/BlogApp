using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;
        public CategoryController(ICategoryRepository _repository)
        {
            repository = _repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View(repository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.AddCategory(entity);
                return RedirectToAction("List");
            }
            else
            {
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(repository.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.UpdateCategory(entity);
                TempData["message"] = $"{entity.Name} güncellendi.";
                return RedirectToAction("List");
            }
            else
            {
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            if (id == null)
            {
                //Yeni kayıt
                //Boş bir nesne gönderildiği zaman formdaki blogId kısmına 0 değeri atanır, dolayısıyla posta gittiği zaman isValide takılmaz.
                return View(new Category());
            }
            else
            {
                //Güncelleme
                return View(repository.GetById((int)id));
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBlog(entity);
                TempData["message"] = $"{entity.Name} kayıt edildi.";
                return RedirectToAction("List");
            }
            else
            {
                return View(entity);
            }
        }
    }
}
