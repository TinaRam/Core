using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workflow.Models;

namespace Workflow.Controllers
{
    public class PTaskController : Controller
    {
        private readonly WorkflowContext _context;

        public PTaskController(WorkflowContext context)
        {
            _context = context;
        }

        // GET: PTask
        public async Task<IActionResult> Index()
        {
            var workflowContext = _context.Ptask.Include(p => p.TaskProject);
            return View(await workflowContext.ToListAsync());
        }

        // GET: PTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pTask = await _context.Ptask
                .Include(p => p.TaskProject)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (pTask == null)
            {
                return NotFound();
            }

            return View(pTask);
        }

        // GET: PTask/Create
        public IActionResult Create()
        {
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName");
            return View();
        }

        // POST: PTask/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,TaskName,Description,Priority,TaskCreationDate,TaskDeadline,CompletionDate,TaskProjectId")] PTask pTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", pTask.TaskProjectId);
            return View(pTask);
        }

        // GET: PTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pTask = await _context.Ptask.FindAsync(id);
            if (pTask == null)
            {
                return NotFound();
            }
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", pTask.TaskProjectId);
            return View(pTask);
        }

        // POST: PTask/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,TaskName,Description,Priority,TaskCreationDate,TaskDeadline,CompletionDate,TaskProjectId")] PTask pTask)
        {
            if (id != pTask.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PTaskExists(pTask.TaskId))
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
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", pTask.TaskProjectId);
            return View(pTask);
        }

        // GET: PTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pTask = await _context.Ptask
                .Include(p => p.TaskProject)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (pTask == null)
            {
                return NotFound();
            }

            return View(pTask);
        }

        // POST: PTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pTask = await _context.Ptask.FindAsync(id);
            _context.Ptask.Remove(pTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PTaskExists(int id)
        {
            return _context.Ptask.Any(e => e.TaskId == id);
        }
    }
}
