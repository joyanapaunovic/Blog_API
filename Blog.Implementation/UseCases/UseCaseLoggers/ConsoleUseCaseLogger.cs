using Blog.Application.UseCases;
using Blog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.UseCaseLoggers
{
    public class ConsoleUseCaseLogger : IUseCaseLogger
    {
        private readonly BlogContext _context;

        public ConsoleUseCaseLogger(BlogContext context)
        {
            _context = context;
        }

        public void Log(UseCaseLogger log)
        {
            Console.WriteLine($"Use case: {log.UseCaseName}," +
                $"User: {log.User}, {log.ExecutionDateTime}," +
                $"Authorized: {log.IsTheUserAuthorized}");

            Console.WriteLine($"Use case data: {log.Data}");
            // UPIS U BAZU PODATAKA
            var useCaseLog = new Blog.Domain.UseCaseLogs
            {
                Id = log.Id,
                UseCaseName = log.UseCaseName,
                UserId = log.UserId,
                ExecutionTime = DateTime.UtcNow,
                Data = log.Data,
                IsAuthorized = log.IsTheUserAuthorized
            };

            _context.UseCaseLogs.Add(useCaseLog);
            _context.SaveChanges();
        }
    }
}
