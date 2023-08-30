using Blog.Application.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.Logging
{
    public class ConsoleExceptionLogger : IExceptionLogger
    {
        public void Log(Exception ex)
        {
            // ispis nastale greske i vremena greske
            Console.WriteLine("Occured at: " + DateTime.UtcNow);
            Console.WriteLine(ex.Message);
        }
    }
}
