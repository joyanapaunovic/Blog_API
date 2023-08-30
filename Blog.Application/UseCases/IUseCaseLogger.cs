using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases
{
    
    public interface IUseCaseLogger
    {
        void Log(UseCaseLogger log);
    }
    public class UseCaseLogger
    {
        public int Id { get; set; }
        public string UseCaseName { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        public string Data { get; set; }
        public bool IsTheUserAuthorized { get; set; }

    }

}
