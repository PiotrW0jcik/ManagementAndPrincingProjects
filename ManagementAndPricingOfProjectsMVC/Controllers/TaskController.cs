using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementAndPricingOfProjectsMVC.Data;
using ManagementAndPricingOfProjectsMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Task = ManagementAndPricingOfProjectsMVC.Models.Task;

namespace ManagementAndPricingOfProjectsMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Task
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tasks.Include(t => t.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create(int? id)
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectId", "Description", id);
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,ProjectID,TaskName,Technology,Role,Hours,PricePerHour,sum")] Task task)
        {
            if (ModelState.IsValid)
            {
               task.sum = task.Hours * task.PricePerHour;
               var project = await _context.Projects.FindAsync(task.ProjectID);
               project.PriceForProject += task.sum;
                _context.Add(task);
                _context.Update(project);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Project", new { id = project.ProjectId });
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectId", "Description", task.ProjectID);
            return View(task);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            task.sum = task.Hours * task.PricePerHour;
           
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectId", "Description", task.ProjectID);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,ProjectID,Technology,TaskName,Role,Hours,PricePerHour,sum")] Task task)
        {
            if (id != task.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Project", new { id = task.Project.ProjectId });
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectId", "Description", task.ProjectID);
            var project = await _context.Projects.FindAsync(task.ProjectID);
            await _context.SaveChangesAsync();
            return View(task);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            
            var task = await _context.Tasks.FindAsync(id);

            var project = await _context.Projects.FindAsync(task.ProjectID);
            project.PriceForProject -= task.sum;
            _context.Update(project);

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Project", new { id = project.ProjectId });
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
