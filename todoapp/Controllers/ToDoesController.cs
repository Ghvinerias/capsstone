using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.DataModel;
using ToDoList.Interfaces;


namespace todoapp.Controllers
{


    public class ToDoesController : Controller
    {
        private readonly IToDo _context;
        private readonly IToDoList _context_list;

      
        public ToDoesController(IToDo context, IToDoList context_list)
        {
            _context = context;
            _context_list = context_list;
        }

        // GET: ToDoes
        public IActionResult Index()
        {
            return View(_context.GetAll());
          

        }

        public IEnumerable<ToDo> GetMyToDo()
        {
            var todos = _context.GetAll();



        


            return _context.GetAll();
        }

      
        public IActionResult GetToDoTable()
        {
            return PartialView("_ToDoTable", GetMyToDo());
           

        }

        [HttpGet]
        public IActionResult GetTodoByLIstId(int? id)
        {

            TempData["LIstId"] = id;
            TempData.Keep("LIstId");
            if ((TempData["LISTNAME"] = _context_list.GetById(id).Name )== null)
            {
                return NotFound();
            }
            TempData["LISTNAME"] = _context_list.GetById(id).Name;
            TempData.Keep("LISTNAME");
            List<ToDo> td = (List<ToDo>)_context.GetAllByListID(id);
            int CompletedCount = 0;
            foreach (ToDo t in td)
            {


                if (t.todostatusid == 3)
                {
                    CompletedCount++;
                }
                ViewBag.Percent = Math.Round(100f * (float)CompletedCount / (float)(td.Count));
                t.ToDoStatusCollection = _context.GetAllStatuses().ToList();
            }
            return View(td);
       


        }



        // GET: ToDoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoes = _context.GetById(id);
            if (toDoes == null)
            {
                return NotFound();
            }

            return View(toDoes);
        }

        // GET: ToDoes/Create
        public IActionResult Create()
        {

            ToDo mytodo = new ToDo();
            if (TempData["LIstId"]!=null)
            {
                mytodo.todolistid = (int)TempData["LIstId"];
                TempData.Keep("LIstId");
                mytodo.ToDoStatusCollection = _context.GetAllStatuses().ToList();

                return View(mytodo);
            
            }
            else
            {
                return NotFound();
            }


        }

        // POST: ToDoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,title,description,duedate,createdate,list_id,status_id")]*/ ToDo toDoes)
        {
            if (ModelState.IsValid)
            {


                toDoes.todolistid = (int)TempData["LIstId"];

                TempData.Keep("LIstId");
                toDoes.ToDoStatusCollection = _context.GetAllStatuses().ToList();
               
                _context.Insert(toDoes);
                _context.Save();
               
                TempData["success"] = "Operation was Succeed";
                return  RedirectToAction("GetTodoByLIstId", "ToDoes", new { id = toDoes.todolistid });
            }
            return View(toDoes);
          
        }

        // GET: ToDoes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoes = _context.GetById(id);
            toDoes.ToDoStatusCollection = _context.GetAllStatuses().ToList();
            TempData["LIstId"] = toDoes.todolistid;
            TempData.Keep("LIstId");
            TempData["StatusId"] = toDoes.todostatusid;
            if (toDoes == null)
            {
                return NotFound();
            }

            return View(toDoes);
        }

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,title,description,duedate,createdate,todolistid,todostatusId")] ToDo toDoes)
        {
            if (id != toDoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDoes.ToDoStatusCollection = _context.GetAllStatuses().ToList();
                    if (TempData["LIstId"]!= null)
                    {
                        toDoes.todolistid = (int)TempData["LIstId"];
                        TempData.Keep("LIstId");

                        toDoes.todostatusid = 1;
                        _context.Update(toDoes);
                        _context.Save();
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoesExists(toDoes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = "Operation was Succeed";
            return View(toDoes);
        }



        [HttpPost]

        public async Task<IActionResult> AjaxEdit(int id, int statusid)
        {
            if (id == null)
            {
                return NotFound();
            }
            var toDoes = _context.GetById(id);
            if (ModelState.IsValid)
            {

                toDoes.ToDoStatusCollection = _context.GetAllStatuses().ToList();
                toDoes.todolistid = (int)TempData["LIstId"];
                TempData.Keep("LIstId");
                toDoes.todostatusid = statusid;
                toDoes.createdate = DateTime.Now;
                _context.Update(toDoes);
                _context.Save();

                TempData["success"] = "Operation was Succeed";


                //return View(toDoes);
                // return View();
                 //return RedirectToAction("GetTodoByLIstId", "ToDoes",  toDoes.todolistid );
                return RedirectToAction("GetTodoByLIstId", "ToDoes", new { id = toDoes.todolistid });
            }


            //TempData.Keep("LIstId");
            return View();
                
        }

        [HttpGet]
        public IActionResult AddEdit(int id = 0)
        {
            ToDo stokmodel = new ToDo();

            if (id != 0)
            {
                stokmodel = _context.GetById(id);
            }
            stokmodel.todolistid = (int)TempData["LIstId"];
            TempData.Keep("LIstId");
            stokmodel.ToDoStatusCollection = _context.GetAllStatuses().ToList();
          
            return View(stokmodel);
        }

        [HttpPost]
        public IActionResult AddEdit(ToDo st)
        {
            ToDo stokmodel = st;
            st.todolistid = (int)TempData["LIstId"];
           
            TempData.Keep("LIstId");
            if (ModelState.IsValid)
            {

                stokmodel.ToDoStatusCollection = _context.GetAllStatuses().ToList();
                _context.Update(st);
                _context.Save();
                TempData["success"] = "Operation was Succeed";

                return RedirectToAction("GetTodoByLIstId", "ToDoes", new { id = stokmodel.todolistid });
            }

            return View(stokmodel);
        }


        // GET: ToDoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoes =  _context.GetById(id);
            if (toDoes == null)
            {
                return NotFound();
            }
           
            return View(toDoes);
        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoes =  _context.GetById(id);
            _context.Delete(id);
            TempData["success"] = "Operation was Succeed";
            _context.Save();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("GetTodoByLIstId", "ToDoes", new { id = toDoes.todolistid });
        }

        private bool ToDoesExists(int id)
        {
            return _context.GetAll().Any(e => e.Id == id);
        }
    }
}
