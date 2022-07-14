using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Interfaces;
namespace ToDoList.DataModel
{
    public static class StatusCount
    {
        private static readonly ToDoContext _context;


        public static int GetCountByStatus(this HtmlHelper html ,int? ListIds, int? StatusId)
        {
            return _context.todo.Where(x => x.todostatusid == StatusId && x.todolistid == ListIds).Count();
        }

    }
}
