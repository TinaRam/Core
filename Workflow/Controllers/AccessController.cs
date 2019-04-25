using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workflow.Models;

namespace Workflow.Controllers
{
    public class AccessController : BaseController
    {

        private static readonly WorkflowContext _context = new WorkflowContext();
        

        public static bool hasAccess(User user, string controller, string action, string id)
        {
            if (user.Role == 1) return true; // er 1 admin? husker ikke.. insert admin here

            if (controller == "Login") return true;
            if (controller == "Home") return true;
            if (controller == "Access") return true;
            if (controller == "Event") return true;

            if (controller == "Project")
            {
                if (action == "Create") return false;
                if (action == "Delete") return false;
                if (action == "Index") return true;
                if (action == "Details")
                {
                    if (id == "") return false;
                    int i = Convert.ToInt32(id);
                    Project project = _context.Project.Find(i);
                    if (project == null) return false;

                    foreach (ProjectParticipant p in _context.ProjectParticipant.Where(p => p.ProjectId == i).ToList())
                    {
                        if (p.UserId == user.UserId)
                        {
                            return true;
                        }
                    }
                    return isManager(id, user);
                }
                if (action == "Edit")
                {
                    return (isManager(id, user));
                }
                if (action == "AddParticipant" || action == "RemoveParticipant") return true;
            }
            if (controller == "PTask")
            {
                if (action == "Index" || action == "Delete" || action == "DeleteConfirmed" || action == "Restore") return true;
                if (action == "Create")
                {
                    return true;
                }
                if (action == "Complete" || action == "RemoveComplete")
                {
                    if (id == "") return false;
                    int i = Convert.ToInt32(id);
                    Ptask t = _context.Ptask.Find(i);
                    if (t.TaskProject.ProjectManager == user.UserId) return true;
                    //AssignedTask a = _context.AssignedTask.Where(at => at.TaskId == i).First();
                    List<AssignedTask> a = _context.AssignedTask.Where(at => at.TaskId == i).ToList();
                    foreach (AssignedTask at in a)
                    {
                        if (user.UserId == at.UserId)
                        {
                            return true;
                        }
                    }
                }
                if (action == "Edit")
                {
                    if (id == "") return false;
                    int i = Convert.ToInt32(id);
                    Ptask t = _context.Ptask.Find(i);
                    if (t.TaskProject.ProjectManager == user.UserId) return true;
                }
                if (action == "Details")
                {
                    if (id == "") return false;
                    int i = Convert.ToInt32(id);
                    AssignedTask t = _context.AssignedTask.Where(a => a.TaskId == i).First();

                    if (t.UserId == user.UserId) return true;
                    return isManager(id, user);
                }
                if (action == "Assign" || action == "Unassign")
                {
                    return true;
                }
            }
            if (controller == "Report")
            {
                if (action == "Create" || action == "Details") return isManager(id, user);
            }
            if (controller == "User")
            {
                if (action == "Details" || action == "Profile")
                {

                    if (id == "") return false;

                    int i = Convert.ToInt32(id);
                    User u = _context.User.Find(i);
                    if (u == null) return false;
                    


                    if (u.UserId == user.UserId) return true;
                    


                    foreach (Project p in _context.Project.ToList())
                    {
                        if (p.ProjectManager == user.UserId) return true;
                    }
                }
                if (action == "Edit" || action == "RenderImg")
                {
                    return true;
                }
            }
            if (controller == "TaskList")
            {
                if (action == "Edit")
                {
                    var t = _context.TaskList.Find(Convert.ToInt32(id));
                    if (t.Project.ProjectManager == user.UserId) return true;
                }
                foreach (Project p in _context.Project.ToList())
                {
                    if (p.ProjectManager == user.UserId) return true;
                }
            }
            return false;
        }

        private static bool isManager(string id, User user)
        {
            if (id == "") return false;
            int i = Convert.ToInt32(id);
            Project project = _context.Project.Find(i);
            if (project == null) return false;

            if (project.ProjectManager == user.UserId) return true;
            return false;
        }

        public IActionResult NoAccess()
        {
            return View();
        }
    }
}