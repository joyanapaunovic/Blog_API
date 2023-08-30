using Blog.Application.Exceptions;
using Blog.Application.Logging;
using Blog.Application.UseCases;
using Blog.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Blog.Implementation
{
    public class UseCaseHandler
    {
        private IExceptionLogger _logger;
        private IApplicationUser _user;
        private IUseCaseLogger _useCaseLogger;
        public UseCaseHandler(IExceptionLogger logger, 
                              IApplicationUser user,
                              IUseCaseLogger useCaseLogger)
        {
            _logger = logger;
            _user = user;
            _useCaseLogger = useCaseLogger;
        }

        // COMMANDS HANDLER
        public void HandleCommand<TRequest>(ICommand<TRequest> command, TRequest data)
        {
            try
            {
                var UserId = HandleLoggingAndAuthorization(command, data);

                var stopwatch = new Stopwatch();

                stopwatch.Start();

                command.Execute(data);

                stopwatch.Stop();

                Console.WriteLine(command.Name + " Duration: " + stopwatch.ElapsedMilliseconds + " ms.");
            }
            catch(Exception ex)
            {
                _logger.Log(ex);
                throw;
            }
        }

        // COMMANDS HANDLER KADA IMAMO 2 PARAMETRA
        public void HandleCommandWithSecondParameter<TRequest, TRequest2>(ICommandWithSecondParameter<TRequest, TRequest2> 
                                                                          command, TRequest data, TRequest2 data2)
        {
            try
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();

                command.Execute(data, data2);

                stopwatch.Stop();

                Console.WriteLine(command.Name + " Duration: " + stopwatch.ElapsedMilliseconds + " ms.");
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw;
            }
        }
        // QUERY HANDLER
        public TResponse HandleQuery<TRequest, TResponse>(IQuery<TRequest, TResponse> query, TRequest data)
        {
            try
            {
               HandleLoggingAndAuthorization(query, data);

                var stopwatch = new Stopwatch();

                stopwatch.Start();

                var response = query.Execute(data);

                stopwatch.Stop();

                Console.WriteLine(query.Name + " Duration: " + stopwatch.ElapsedMilliseconds + " ms.");

                return response; // razlika je sto ovaj metod nije void, ima povratni tip
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw;
            }
        }
        // provera da li je korisnik ulogovan i ako jeste ovaj metod vraca njegov id za dalje svrhe (dobijanje trenutnog korisnika)
        public int HandleLoggingAndAuthorization<TRequest>(IUseCase useCase, TRequest data)
        {
            var useCaseLogger = new UseCaseLogger
            {
                User = _user.Identity,
                ExecutionDateTime = DateTime.UtcNow,
                UseCaseName = useCase.Name,
                UserId = _user.Id,
                Data = JsonConvert.SerializeObject(data),
                IsTheUserAuthorized = _user.UseCaseAllowedIds.Contains(useCase.Id)
            };

            _useCaseLogger.Log(useCaseLogger);

            if (!useCaseLogger.IsTheUserAuthorized)
            {
                throw new ForbiddenUseCaseExecutionException(_user.Identity, useCase.Name);
            }
            else
            {
                return useCaseLogger.UserId;
            }
        }
    }
}
