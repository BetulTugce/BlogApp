using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Models.ViewModels
{
    public class BlogCreateVM
    {
        //ViewModelde sadece taşınacak verinin propertyleri temsil edilir.
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
