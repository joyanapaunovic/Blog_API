using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class FileDTO : BaseDTO
    {
        public string Path { get; set; }
    }
}
