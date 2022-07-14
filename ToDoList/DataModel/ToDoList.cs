using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.DataModel
{
    public class Todolist
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public DateTime createdate { get; set; } = DateTime.Now;
    }
}
