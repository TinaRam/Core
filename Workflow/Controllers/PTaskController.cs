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
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Index()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            List<Ptask> list = new List<Ptask>();

            foreach (AssignedTask a in _context.AssignedTask.Where(a => a.UserId == CurrentUser.UserId))
            {
                list.Add(_context.Ptask.Include(p => p.TaskList).Include(p => p.TaskProject).FirstOrDefault(m => m.TaskId == a.TaskId));
            }

            return View(list);
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
        public IActionResult Create(int id)
        {
            ViewData["TaskListId"] = new SelectList(_context.TaskList, "TaskListId", "ListName");
            ViewData["TaskProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName");

            ViewBag.tasklist = _context.TaskList.Where(t => t.ProjectId == id).ToList();
            ViewBag.project = _context.Project.Find(id);

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
            ViewBag.tasklist = _context.TaskList.Where(t => t.ProjectId == ptask.TaskProjectId).ToList();
            ViewBag.project = _context.Project.Find(ptask.TaskProjectId);
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

        public void Complete(int id)
        {
            Ptask task = _context.Ptask.Find(id);
            task.CompletionDate = DateTime.Now;
            _context.SaveChanges();

            Response.Redirect("/Project/Details/" + task.TaskProjectId);
        }

        public void RemoveComplete(int id)
        {
            Ptask task = _context.Ptask.Find(id);
            task.CompletionDate = null;
            _context.SaveChanges();

            Response.Redirect("/Project/Details/" + task.TaskProjectId);
        }

        public void Assign(int user_id, int task_id, int project_id)
        {
            Ptask task = _context.Ptask.Find(task_id);

            AssignedTask at = new AssignedTask();
            at.ProjectId = project_id;
            at.UserId = user_id;
            at.TaskId = task_id;
            _context.Add(at);
            _context.SaveChanges();

            Response.Redirect("/Project/Details/" + project_id);
        }

        public void Unassign(int id)
        {
            AssignedTask a = _context.AssignedTask.Find(id);
            var project_id = a.ProjectId;
            _context.Remove(a);
            _context.SaveChanges();
            Response.Redirect("/Project/Details/" + project_id);
        }
    }
}
