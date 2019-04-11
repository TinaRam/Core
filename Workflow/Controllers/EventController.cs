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
                .Where(n => n.UserId == UserId || n.Event.UserId == UserId)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Creator)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Task)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Project)
                .ToList();

            return notes;
        }

        public static void NewEvent(int projectId, int creatorId, string type, int? userId, int? taskId, int? taskListId, bool? createNotification, bool? email, bool? app)
        {
            Event e = new Event();
            e.ProjectId = projectId;
            e.CreatorId = creatorId;
            e.Type = type;
            if (userId != null) e.UserId = userId;
            if (taskId != null) e.TaskId = taskId;
            if (taskListId != null) e.TaskListId = taskListId;

            _context.Add(e);
            

            if (createNotification == true)
            {
                if (userId == null)
                {
                    List<ProjectParticipant> participants = _context.ProjectParticipant.Where(p => p.ProjectId == projectId).ToList();
                    foreach (ProjectParticipant pp in participants)
                    {
                        NewNotification(e.EventId, email, app, pp.UserId);
                    }
                }
                else
                {
                    NewNotification(e.EventId, email, app, null);
                }
            }

            _context.SaveChanges();
        }

        public static void NewNotification(int eventId, bool? email, bool? app, int? userId)
        {
            Notification n = new Notification();
            n.EventId = eventId;
            if (email == true) n.Email = 1;
            if (app == true) n.InApp = 1;
            if (userId != null) n.UserId = userId;

            _context.Add(n);

            _context.SaveChanges();
        }
        

        public static bool SetViewed(int userId)
        {

            return true;
        }
    }
}