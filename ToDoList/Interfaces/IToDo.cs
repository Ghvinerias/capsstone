using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DataModel;

namespace ToDoList.Interfaces
{
    public interface IToDo
    {
        IEnumerable<ToDo> GetAll();
        IEnumerable<ToDoStatuses> GetAllStatuses();
        ToDo GetById(int? id);
        void Insert(ToDo todo);
        void Update(ToDo todo);
        void Delete(int? id);
        void Save();
        IEnumerable<ToDo> GetAllByListID(int? listid);
        IEnumerable<ToDo> GetNearestToDoCount();

    }
}
