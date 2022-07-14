using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DataModel;

namespace todoapp.Views.ViewModels
{
    public class IndexVM
    {
        public List<ToDo> lists { get; set; }

        public List<Todolist> todoeslist { get; set; }
        public string title { get; set; }



    }
}
