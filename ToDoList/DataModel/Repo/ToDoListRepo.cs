using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Interfaces;


namespace ToDoList.DataModel.Repo
{
    public class ToDoListRepo:IToDoList
    {
        private readonly ToDoContext _context;

        public ToDoListRepo(ToDoContext context)
        {
            _context = context;
        }
        public IEnumerable<Todolist> GetAll()
        {
            return _context.todolist.ToList();
        }

   
        public IEnumerable<ToDo> GetAlltodo()
        {
            return _context.todo.ToList();
        }
        public Todolist GetById(int? id)
        {
            return _context.todolist.Find(id);
        }
        public void Insert(Todolist list)
        {
            _context.todolist.Add(list);
        }
        public void Update(Todolist list)
        {
            _context.Entry(list).State = EntityState.Modified;
        }
        public void Delete(int? id)
        {
            Todolist list = _context.todolist.Find(id);
            _context.todolist.Remove(list);
        }

        public  int GetCountByStatus(int? ListIds, int? StatusId)
        {
            return _context.todo.Where(x=>x.todostatusid == StatusId && x.todolistid== ListIds).Count();
        }


     

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
