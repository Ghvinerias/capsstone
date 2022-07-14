using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Interfaces;



namespace ToDoList.DataModel.Repo
{
    public class ToDoRepo:IToDo
    {
        private readonly ToDoContext _context;

        public ToDoRepo(ToDoContext context)
        {
            _context = context;
        }
        public IEnumerable<ToDo> GetAll()
        {
            return _context.todo.ToList();
        }

        public IEnumerable<ToDoStatuses> GetAllStatuses()
        {
            return _context.todoestatuses.ToList();
        }
        public ToDo GetById(int? id)
        {
            return _context.todo.Find(id);
        }
        public void Insert(ToDo todo)
        {
            _context.todo.Add(todo);
        }
        public void Update(ToDo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
        }
        public void Delete(int? id)
        {
            ToDo todo = _context.todo.Find(id);
            _context.todo.Remove(todo);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ToDo> GetAllByListID(int? listid)
        {
            return _context.todo.Where(x=>x.todolistid==listid).ToList();
        }
        public IEnumerable<ToDo> GetNearestToDoCount()
           
        {
            var today = DateTime.Now;
            var res = from  ToDo in _context.todo
                      
                      where ToDo.duedate.Date == today.Date && ToDo.duedate>today.AddMinutes(-10) && ToDo.duedate<today.AddMinutes(10) && ToDo.todostatusid==1
                      select ToDo;
            string st = "";
            foreach (ToDo td in res)
            {
             
                st=string.Concat(st,td.title, ',');
    

            }
            string stres = "";
            if (st.Length > 0)
            {
                 stres = st.Remove(st.Length - 1);
            }

            int c = res.Count();
            string r = "You have " + c.ToString() + " TODO for " + stres;
            Debug.WriteLine(r);

           


            return res;
        }
    }
}
