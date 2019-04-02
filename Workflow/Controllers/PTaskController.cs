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
    public class PTaskController :BaseController
    {
        private readonly WorkflowContext _context;

        public PTaskController(WorkflowContext context)
        {
            _context = context;
        }

        // GET: PTask
        public async Task<IActionResult> Index()
        {
            var workflowContext = _context.Ptask.Include(p => p.TaskList).Include(p => p.TaskProject);
            return View(await workflowContext.ToListAsync());
        }

        // GET: PTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ptask = await _context.Ptask
                .Include(p => p.TaskList)
                .Include(p => p.TaskProject)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (ptask == null)
            {
                return NotFound();
            }

            return View(ptask);
        }

        // GET: PTask/Create
        public IActionResult Create()
        {
            ViewData["TaskListId"] = new SelectList(_context.TaskList, "TaskListId", "ListName");
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName");
            return View();
        }

        // POST: PTask/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,TaskName,Description,Priority,TaskCreationDate,TaskDeadline,CompletionDate,TaskProjectId,TaskListId")] Ptask ptask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ptask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskListId"] = new SelectList(_context.TaskList, "TaskListId", "TaskListId", ptask.TaskListId);
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", ptask.TaskProjectId);
            return View(ptask);
        }

        // GET: PTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ptask = await _context.Ptask.FindAsync(id);
            if (ptask == null)
            {
                return NotFound();
            }
            ViewData["TaskListId"] = new SelectList(_context.TaskList, "TaskListId", "ListName", ptask.TaskListId);
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", ptask.TaskProjectId);
            return View(ptask);
        }

        // POST: PTask/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,TaskName,Description,Priority,TaskCreationDate,TaskDeadline,CompletionDate,TaskProjectId,TaskListId")] Ptask ptask)
        {
            if (id != ptask.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ptask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PtaskExists(ptask.TaskId))
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
            ViewData["TaskListId"] = new SelectList(_context.TaskList, "TaskListId", "TaskListId", ptask.TaskListId);
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", ptask.TaskProjectId);
            return View(ptask);
        }

        // GET: PTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ptask = await _context.Ptask
                .Include(p => p.TaskList)
                .Include(p => p.TaskProject)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (ptask == null)
            {
                return NotFound();
            }

            return View(ptask);
        }

        // POST: PTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ptask = await _context.Ptask.FindAsync(id);
            _context.Ptask.Remove(ptask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PtaskExists(int id)
        {
            return _context.Ptask.Any(e => e.TaskId == id);
        }

        public async void Complete(int id)
        {
            Ptask task = _context.Ptask.Find(id);
            task.CompletionDate = DateTime.Now;
            _context.SaveChanges();
        }

        public async void RemoveComplete(int id)
        {
            Ptask task = _context.Ptask.Find(id);
            task.CompletionDate = null;
            _context.SaveChanges();
        }

        public async Task<IActionResult> Assign(int? id, int? project)
        {
            if (id == null || project == null)
            {
                return NotFound();
            }

            var ptask = await _context.Ptask
                .Include(p => p.TaskList)
                .Include(p => p.TaskProject)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (ptask == null)
            {
                return NotFound();
            }

            return View(ptask);
        }
    }
}
