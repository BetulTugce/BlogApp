using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            //Uygulama içindeki contexte getrequiredservice ile ulaşıp contexti bekleyen bir migrate varsa önce onu yapmasını
            //yani, database updatei çalıştırmasını sağlar.
            BlogContext context = app.ApplicationServices.GetRequiredService<BlogContext>();

            context.Database.Migrate();

            //Daha önceden kategori ve bloga eklenen bir context yoksa test kayıtları ekleyecek.
            //Bloglar kategoriye bağımlı olduğu için önce kategorileri eklememiz gerek.
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category() { Name="Category 1"},
                    new Category() { Name="Category 2"},
                    new Category() { Name="Category 3"}
                );
                context.SaveChanges();
            }
            
            if (!context.Blogs.Any())
            {
                context.Blogs.AddRange(
                    new Blog() { Title = "Blog title 1", Description = "Blog Description 1", Body = "Blog Body 1", Image = "1.jpg", Date = DateTime.Now.AddDays(-5), isApproved = true, CategoryId = 1 },
                    new Blog() { Title = "Blog title 2", Description = "Blog Description 2", Body = "Blog Body 2", Image = "2.jpg", Date = DateTime.Now.AddDays(-6), isApproved = true, CategoryId = 1 },
                    new Blog() { Title = "Blog title 3", Description = "Blog Description 3", Body = "Blog Body 3", Image = "3.jpg", Date = DateTime.Now.AddDays(-7), isApproved = false, CategoryId = 3 },
                    new Blog() { Title = "Blog title 4", Description = "Blog Description 4", Body = "Blog Body 4", Image = "4.jpg", Date = DateTime.Now.AddDays(-7), isApproved = true, CategoryId = 2 }
                );

                context.SaveChanges();
            }
        }
    }
}
//Seed datayı uygulamaya tanıtmak için Startupa git.