using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.DataModel;
using todoapp.Views.ViewModels;
using System.Dynamic;
using ToDoList.DataModel.Repo;
using ToDoList.Interfaces;

namespace todoapp.Controllers
{
 
    public class TodolistsController : Controller
    {
        private readonly IToDoList _context;
        private readonly IToDo _context_todo;



        public TodolistsController(IToDoList context, IToDo context_todo)
        {
            _context = context;
            _context_todo = context_todo;


        }


        // GET: Todolists
        public IActionResult Index()
        {
            
            return View( _context.GetAll());
        }

        public IActionResult Index2()
        {
            var s1 = GetallToDoLists();
            var s2 = GetallToDoes();
            IndexVM vm = new IndexVM();
            vm.lists = (List<ToDoList.DataModel.ToDo>)s2;
            vm.todoeslist = (List<Todolist>)s1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult GetCountByStatus()
        {
            List<ToDoListWithCount> todocount= new List<ToDoListWithCount>();
            List<Todolist> tlist = (List<Todolist>)_context.GetAll();
            foreach (Todolist l in tlist)
            {
                ToDoListWithCount tdc = new ToDoListWithCount();
                tdc.tl = _context.GetById(l.id);

                tdc.NotStarted = _context.GetCountByStatus(l.id, 1);
                tdc.InProgress = _context.GetCountByStatus(l.id, 2);
                tdc.Completed = _context.GetCountByStatus(l.id, 3);
                todocount.Add(tdc);
            }


            return View(todocount);
        }


        public IEnumerable<ToDo> GetallToDoes()
        {
            return (List<ToDo>)_context.GetAlltodo();
        }
        public IEnumerable<Todolist> GetallToDoLists()
        {
            return (List<Todolist>)_context.GetAll();
        }

        // GET: Todolists/Details/5
        public IActionResult Details(int? id)
        {
            TempData["ListID"] = id;
            if (id == null)
            {
                return NotFound();
            }
            
            var todolist =  _context.GetById(id);
            if (todolist == null)
            {
                return NotFound();
            }


            return View(todolist);

        }




        // GET: Todolists/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Todolists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,Name,createdate")] Todolist todolist)
        {
            TempData["ListID"] = todolist.id;
            if (ModelState.IsValid)
            {
                _context.Insert(todolist);
                 _context.Save();
                TempData["success"] = "Operation was Succeed";
                return RedirectToAction(nameof(GetCountByStatus));

            }
            
            return View(todolist);
        }

        [HttpGet]
       
        public IActionResult CopyList(int? id)
        {
            Todolist copylist = _context.GetById(id);
            copylist.createdate = DateTime.Now;
          

            if (ModelState.IsValid)
            {
                
                copylist.id = 0;
                _context.Insert(copylist);

                _context.Save();
                int lastlistid = copylist.id;

                IEnumerable<ToDo> todo_of_list = _context_todo.GetAllByListID(id);

                foreach (ToDo td  in todo_of_list)
                {
                    td.Id = 0;
                    td.todolistid = lastlistid;
                    td.createdate = DateTime.Now;
                    _context_todo.Insert(td);
                    
                }
                _context_todo.Save();
                TempData["success"] = "Operation was Succeed";
                return RedirectToAction(nameof(GetCountByStatus));

            }

            return View(copylist);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CopyList([Bind("id,Name,createdate")] Todolist todolist)
        {
           
           
           
            if (ModelState.IsValid)
            {
                _context.Insert(todolist);
              
                _context.Save();
                int lastlistid = todolist.id;

                TempData["success"] = "Operation was Succeed";
                return RedirectToAction(nameof(GetCountByStatus));

            }

            return View(todolist);
        }


        // GET: Todolists/Edit/5
        public IActionResult Edit(int? id)
        {
            TempData["ListID"] = id;
            if (id == null)
            {
                return NotFound();
            }

            var todolist =  _context.GetById(id);
            if (todolist == null)
            {
                return NotFound();
            }
            return View(todolist);
        }

        // POST: Todolists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,createdate")] Todolist todolist)
        {
            TempData["ListID"] = id;
            if (id != todolist.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todolist);
                     _context.Save();
                    TempData["success"] = "Operation was Succeed";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodolistExists(todolist.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetCountByStatus));
            }
            return View(todolist);
        }

        // GET: Todolists/Delete/5
        public  IActionResult Delete(int? id)
        {
            TempData["ListID"] = id;
            if (id == null)
            {
                return NotFound();
            }

            var todolist =  _context.GetById(id);
               
            if (todolist == null)
            {
                return NotFound();
            }

            return View(todolist);
        }

        // POST: Todolists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["ListID"] = id;
            var todolist =  _context.GetById(id);
            _context.Delete(id);
             _context.Save();
            TempData["success"] = "Operation was Succeed";
            return RedirectToAction(nameof(GetCountByStatus));
        }

        private bool TodolistExists(int id)
        {
            return _context.GetAll().Any(e => e.id == id);
        }
    }
}

