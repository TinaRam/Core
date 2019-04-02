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
    public class ProjectController : BaseController
    {
        private readonly WorkflowContext _context;

        public ProjectController(WorkflowContext context)
        {
            _context = context;
        }


        // GET: Project
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Index()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            List<Project> Projects = new List<Project>();
            List<ProjectParticipant> Participants = _context.ProjectParticipant.ToList();
            foreach (ProjectParticipant p in Participants)
            {
                if (p.UserId == CurrentUser.UserId)
                {
                    Projects.Add(_context.Project.Include(t => t.ProjectManagerNavigation).FirstOrDefault(t => t.ProjectId == p.ProjectId));
                }
            }
            return View(Projects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.ProjectManagerNavigation)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            List<TaskList> TaskList = new List<TaskList>();

            var list = _context.TaskList
                .Include(tasklist => tasklist.Ptask)
                    .ThenInclude(task => task.AssignedTask)
                .ToList();

            foreach (TaskList t in list)
            {
                if (t.ProjectId == id)
                {
                    TaskList.Add(t);
                }
            }
            
            List<ProjectParticipant> Participants = new List<ProjectParticipant>();

            foreach (ProjectParticipant p in _context.ProjectParticipant.Include(s => s.User).ToList())
            {
                if (p.ProjectId == id)
                {
                    Participants.Add(p);
                }
            }

            List<User> availableUsers = _context.User.ToList();
            
            foreach (ProjectParticipant p in Participants)
            {
                foreach (User u in _context.User.ToList())
                {
                    if (p.UserId == u.UserId)
                    {
                        availableUsers.Remove(u);
                    }
                }
            }

            ViewBag.project = project;
            ViewBag.tasklist = TaskList;
            ViewBag.Participants = Participants;
            ViewBag.availableUsers = availableUsers;
            
            // må sende med context fordi assignedtask-tabellen ikke har noen connection til user-tabellen,
            // derfor må man finne den fra viewet. ikke så clean, men det funker.. 
            ViewBag.context = _context;

            return View();
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewData["ProjectManager"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectDescription,ProjectStart,ProjectDeadline,CompletionDate,ProjectManager,MarkedAsFinished")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectManager"] = new SelectList(_context.User, "UserId", "Email", project.ProjectManager);
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["ProjectManager"] = new SelectList(_context.User, "UserId", "Email", project.ProjectManager);
            ViewBag.project = project;
            return View();
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,ProjectDescription,ProjectStart,ProjectDeadline,CompletionDate,ProjectManager,MarkedAsFinished")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            ViewData["ProjectManager"] = new SelectList(_context.User, "UserId", "Email", project.ProjectManager);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.ProjectManagerNavigation)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

        public void AddParticipant(int projectId, int userId)
        {
            ProjectParticipant p = new ProjectParticipant();
            p.ProjectId = projectId;
            p.UserId = userId;
            _context.Add(p);
            _context.SaveChanges();

            // TODO: send notification

            Response.Redirect("/Project/Details/" + projectId);
        }

        public void RemoveParticipant(int projectId, int userId)
        {
            ProjectParticipant p = _context.ProjectParticipant.Find(projectId, userId);
            _context.Remove(p);
            _context.SaveChanges();

            // TODO: send notification

            Response.Redirect("/Project/Details/" + projectId);
        }
    }
}
