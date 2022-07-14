using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapp.Views.ViewModels
{
    public class ToDoListWithCount
    {
       public ToDoList.DataModel.Todolist tl;
       public  int NotStarted;
       public int InProgress;
       public  int Completed;

    }
}
