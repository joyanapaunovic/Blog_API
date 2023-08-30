using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class UseCaseLogs
    {
        public int Id { get; set; }
        public string UseCaseName { get; set; }
        public int UserId { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string Data { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
