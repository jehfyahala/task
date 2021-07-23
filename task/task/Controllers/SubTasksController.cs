using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task.common;
using task.Data;
using task.Models;

namespace task.Controllers
{
    public class SubTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        //new cod secund partial
        //var
        private readonly int RecordsPerPage = 10;
        private Pagination<SubTask> PaginationSubTasks;
        public SubTasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        //route 2da parte
        [Route("/SubTask")]
        [Route("/SubTask/{search}")]
        [Route("/SubTask/{search}/{page}")]
        //fin 
        // GET: SubTasks
        public async Task<IActionResult> Index(string search, int page = 1)
        {

            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }

            //obtener registros totales
            totalRecords = await _context.SubTask.CountAsync(
                s => s.SubTaskDescription.Contains(search));
            //obtener datos

            var subTasks = await _context.SubTask
                .Where(s => s.SubTaskDescription.Contains(search))
                .Include(j => j.Job)
                .ToListAsync();
            //paginar
            var subTasksResult = subTasks.OrderBy(o => o.SubTaskDescription)
                .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            PaginationSubTasks = new Pagination<SubTask>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = subTasksResult,
                //State = (List<Job>)states
                //Task= (List<Job>)tasks
            };



            return View(PaginationSubTasks);


            //var applicationDbContext = _context.SubTask.Include(s => s.Job);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTask
                .Include(s => s.Job)
                .FirstOrDefaultAsync(m => m.SubTaskId == id);
            if (subTask == null)
            {
                return NotFound();
            }

            return View(subTask);
        }

        // GET: SubTasks/Create
        public IActionResult Create()
        {
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobDescription");
            return View();
        }

        // POST: SubTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubTaskId,SubTaskDescription,JobId")] SubTask subTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobDescription", subTask.JobId);
            return View(subTask);
        }

        // GET: SubTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTask.FindAsync(id);
            if (subTask == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobDescription", subTask.JobId);
            return View(subTask);
        }

        // POST: SubTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubTaskId,SubTaskDescription,JobId")] SubTask subTask)
        {
            if (id != subTask.SubTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubTaskExists(subTask.SubTaskId))
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
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobDescription", subTask.JobId);
            return View(subTask);
        }

        // GET: SubTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTask
                .Include(s => s.Job)
                .FirstOrDefaultAsync(m => m.SubTaskId == id);
            if (subTask == null)
            {
                return NotFound();
            }

            return View(subTask);
        }

        // POST: SubTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subTask = await _context.SubTask.FindAsync(id);
            _context.SubTask.Remove(subTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubTaskExists(int id)
        {
            return _context.SubTask.Any(e => e.SubTaskId == id);
        }
    }
}
