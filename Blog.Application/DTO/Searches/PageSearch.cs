using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO.Searches
{
    public class PageSearch
    {
        public string Keyword { get; set; }
        public int? PerPage { get; set; }
        public int? CurrentPage { get; set;}
        public int? CurrentUser { get; set; }
    }
}
