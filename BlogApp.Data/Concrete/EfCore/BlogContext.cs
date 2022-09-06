using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {
            //Bu yapıcı fonksiyon dışardan bir option alacak ve bana connectionstringi getirecek
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
