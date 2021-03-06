﻿using System;
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
                .Where(n => (n.UserId == UserId || n.Event.UserId == UserId) && n.Viewed == 0)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Task)
                .Include(n => n.Event.Creator)
                .Include(n => n.Event.Task)
                .Include(n => n.Event.Project)
                .OrderByDescending(n => n.Event.EventDate)
                .ToList();

            return notes;
        }

        public static List<Notification> GetViewedNotes(int UserId)
        {
            List<Notification> notes = _context.Notification
                .Where(n => (n.UserId == UserId || n.Event.UserId == UserId) && n.Viewed == 1)
                .Include(n => n.Event)
                    .ThenInclude(e => e.Task)
                .Include(n => n.Event.Creator)
                .Include(n => n.Event.Task)
                .Include(n => n.Event.Project)
                .OrderByDescending(n => n.Event.EventDate)
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
            

            if (createNotification == true && userId != creatorId)
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
        
        public IActionResult SetViewed(int userId)
        {
            userId = CurrentUser.UserId;
            List<Notification> notes = _context.Notification
                .Include(n => n.Event)
                .Where(n => n.UserId == userId || n.Event.UserId == userId)
                .ToList();


            foreach (Notification note in notes)
            {

                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine(note);
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine("");

                note.Viewed = 1;
                note.Email = 1;
                _context.Update(note);
            }

            _context.SaveChanges();
            return View();
        }

        public IActionResult RemoveNote(int noteId)
        {
            Notification n = _context.Notification.Find(noteId);
            _context.Remove(n);
            _context.SaveChanges();
            

            return View();
        }
    }
}