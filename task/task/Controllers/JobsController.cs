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
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //new cod secund partial
        //var
        private readonly int RecordsPerPage = 10;
        private Pagination<Job> PaginationJobs;
        //fin parte 1
        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }
        //route 2da parte
        [Route("/Job")]
        [Route("/Job/{search}")]
        [Route("/Job/{search}/{page}")]
        //fin 
        // GET: Jobs
        
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            
            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }

            //var states = _context.Job.Include(j => j.State);
            //obtener registros totales
            totalRecords = await _context.Job.CountAsync(
                s => s.JobDescription.Contains(search));
            //obtener datos
            
            var jobs = await _context.Job
                .Where(s => s.JobDescription.Contains(search))
                .Include(s=>s.State)
                .ToListAsync();
            //paginar
            var jobsResult = jobs.OrderBy(o => o.JobDescription)
                .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            PaginationJobs = new Pagination<Job>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = jobsResult,
                //State = (List<Job>)states
                //Task= (List<Job>)tasks
            };

            
            
            return View(PaginationJobs);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.State)
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateDescription");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,JobDescription,JobStartDate,JobFinalDate,StateId")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateDescription", job.StateId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateDescription", job.StateId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,JobDescription,JobStartDate,JobFinalDate,StateId")] Job job)
        {
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.JobId))
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
            //cambio
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateDescription", job.StateId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.State)
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Job.FindAsync(id);
            _context.Job.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.JobId == id);
        }
    }
}
