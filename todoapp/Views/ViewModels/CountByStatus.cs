using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Interfaces;
using ToDoList.DataModel.Repo;
using ToDoList.DataModel;

namespace todoapp.Views.ViewModels
{
    public static class CountByStatus
    {
        static ToDoContext dbcon;

        public static int GetCountByStatus(int? ListIds, int? StatusId)
        {
            return dbcon.todo.Where(x=>x.todolistid== ListIds && x.todostatusid== StatusId).Count();
        }
    }
}
