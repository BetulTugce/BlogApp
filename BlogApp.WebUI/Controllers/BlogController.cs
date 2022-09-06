using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository blogRepository;
        private ICategoryRepository categoryRepository;
        public BlogController(IBlogRepository _blogRepository, ICategoryRepository _categoryRepository)
        {
            blogRepository = _blogRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Details(int id)
        {
            return View(blogRepository.GetById(id));
        }

        public IActionResult Index(int? id, string q)
        {
            var query = blogRepository.GetAll().Where(i => i.isApproved);
            if (id != null)
            {
                query = query.Where(i => i.CategoryId == id);
            }
            if (!string.IsNullOrEmpty(q))
            {
                //query = query.Where(i => i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q));
                query = query.Where(i => EF.Functions.Like(i.Title, "%" + q + "%") || EF.Functions.Like(i.Description, "%" + q + "%") || EF.Functions.Like(i.Body, "%" + q + "%"));
            }

            return View(query.OrderByDescending(i => i.Date));
        }

        public IActionResult List()
        {
            return View(blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            entity.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                blogRepository.AddBlog(entity);
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View(blogRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Blog entity)
        {
            if (ModelState.IsValid)
            {
                blogRepository.UpdateBlog(entity);
                TempData["message"] = $"{entity.Title} güncellendi.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            if (id == null)
            {
                //Yeni kayıt
                //Boş bir nesne gönderildiği zaman formdaki blogId kısmına 0 değeri atanır, dolayısıyla posta gittiği zaman isValide takılmaz.
                return View(new Blog());
            }
            else
            {
                //Güncelleme
                return View(blogRepository.GetById((int)id));
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {
            if (ModelState.IsValid)
            {
                blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult Create2(int? id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");

            return View(new Blog());
        }

        [HttpPost]
        public IActionResult Create2(BlogCreateVM entity)
        {
            if (ModelState.IsValid)
            {
                Blog blog = new Blog()
                {
                    Title = entity.Title,
                    Description = entity.Description,
                    CategoryId = entity.CategoryId
                };
                blogRepository.SaveBlog(blog);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult Edit2(int id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");

            return View(blogRepository.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit2(Blog entity, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file!=null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); //Dosya fiziksel olarak kaydediliyor
                    }
                    //Dosyanın ismini entitynin içine almamız lazım
                    entity.Image = file.FileName;
                }

                blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
                return View(entity);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(blogRepository.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int BlogId)
        {
            blogRepository.DeleteBlog(BlogId);
            TempData["message"] = $"{BlogId} numaralı kayıt silindi.";
            return RedirectToAction("List");
        }
    }
}
