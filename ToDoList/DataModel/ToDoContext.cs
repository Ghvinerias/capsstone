using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.DataModel
{
    public class ToDoContext : DbContext
    {


        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {

        }
        public DbSet<Todolist> todolist { get; set; }
        public DbSet<ToDo> todo { get; set; }
        public DbSet<ToDoStatuses> todoestatuses { get; set; }

    }
}
