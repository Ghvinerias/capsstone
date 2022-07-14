using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DataModel;

namespace ToDoList.Interfaces
{
    public interface IToDoList
    {
        IEnumerable<Todolist> GetAll();

         IEnumerable<ToDo> GetAlltodo();
        Todolist GetById(int? id);
        void Insert(Todolist list);
        void Update(Todolist list);
        void Delete(int? id);
        int GetCountByStatus(int? ListIds, int? StatusId);
  
        void Save();
     }
}
