using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.DataModel.Repo;
using ToDoList;
using ToDoList.DataModel;


namespace todoapp.Services
{
    public class ReminderService : BackgroundService
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public ReminderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
          
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(GetRemider, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));
            return Task.CompletedTask;
        }

        private void GetRemider(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var todoservise = scope.ServiceProvider.GetRequiredService<ToDoList.Interfaces.IToDo>();
            //ToDo t = todoservise.GetById(38);
            //t.createdate = DateTime.Now;
            //todoservise.Update(t);
            //todoservise.Save();

            IEnumerable<ToDo> td= todoservise.GetNearestToDoCount();
            foreach (ToDo tt in td)
            {
                tt.todostatusid = 2;
                todoservise.Update(tt);
                
            }
            todoservise.Save();


        }












    }
}
