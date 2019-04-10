using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workflow.Models;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace Workflow.Controllers
{
    public class EventController : BaseController
    {
        private static readonly WorkflowContext _context = new WorkflowContext();

        public static List<Notification> GetNotes(int UserId)
        {
            List<Notification> notes = _context.Notification
                .Where(n => n.Event.UserId == UserId)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Creator)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Task)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Project)
                .ToList();

            return notes;
        }

        public static void NewEvent(int projectId, int creatorId, string type, int userId, int? taskId, int? taskListId, bool? createNotification, bool? email, bool? app)
        {
            Event e = new Event();
            e.ProjectId = projectId;
            e.CreatorId = creatorId;
            e.Type = type;
            e.UserId = userId;
            if (taskId != null) e.TaskId = taskId;
            if (taskListId != null) e.TaskListId = taskListId;

            _context.Add(e);

            if (createNotification == true)
            {
                Notification n = new Notification();
                n.EventId = e.EventId;
                if (email == true) n.Email = 1;
                if (app == true) n.InApp = 1;

                _context.Add(n);
            }

            _context.SaveChanges();
        }
        

        public static bool SetViewed(int userId)
        {

            return true;
        }
    }
}