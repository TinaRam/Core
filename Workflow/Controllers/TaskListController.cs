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
    public class TaskListController : BaseController
    {
        private readonly WorkflowContext _context;

        public TaskListController(WorkflowContext context)
        {
            _context = context;
        }

        // GET: TaskList
        public async Task<IActionResult> Index()
        {
            var workflowContext = _context.TaskList.Include(t => t.Project);
            return View(await workflowContext.ToListAsync());
        }

        // GET: TaskList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskList
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskListId == id);
            if (taskList == null)
            {
                return NotFound();
            }

            return View(taskList);
        }

        // GET: TaskList/Create
        //public IActionResult Create()
        //{
        //    ViewData["ProjectId"] = new SelectList(_context.Project.Where(t => t.ProjectManager == CurrentUser.UserId), "ProjectId", "ProjectName");
        //    return View();
        //}

        // POST: TaskList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public void Create(int ProjectId, string ListName)
        {
            TaskList t = new TaskList();
            t.ProjectId = ProjectId;
            t.ListName = ListName;
            _context.Add(t);
            _context.SaveChanges();

            EventController.NewEvent(ProjectId, CurrentUser.UserId, "new tasklist", null, null, t.TaskListId, null, null, null);

            Response.Redirect("/Project/Details/" + ProjectId);
        }

        // GET: TaskList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskList = await _context.TaskList.FindAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", taskList.ProjectId);
            return View(taskList);
        }

        // POST: TaskList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskListId,ProjectId,ListName")] TaskList taskList)
        {
            if (id != taskList.TaskListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(taskList.TaskListId))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", taskList.ProjectId);
            return View(taskList);
        }

        // GET: TaskList/Delete/5
        public async void Delete(int? id)
        {
            var taskList = await _context.TaskList
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskListId == id);
         
            var i = taskList.ProjectId;

            taskList.Deleted = 1;
            EventController.NewEvent(i, CurrentUser.UserId, "remove tasklist", null, null, taskList.TaskListId, false, null, null);


            _context.SaveChanges();

            Response.Redirect("/Project/Details/" + i);
        }

        // POST: TaskList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var taskList = await _context.TaskList.FindAsync(id);
        //    var i = taskList.ProjectId;
        //    _context.TaskList.Remove(taskList);
        //    await _context.SaveChangesAsync();
        //    return Redirect("/Project/Details/" + i);
        //}

        private bool TaskListExists(int id)
        {
            return _context.TaskList.Any(e => e.TaskListId == id);
        }
    }
}
