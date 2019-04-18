using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Workflow.Models;

namespace Workflow.Controllers
{
    public class BaseController : Controller
    {
        public static string ConnectionString = Startup.GetConnectionString();
        public const string SessionId = "_Id";
        public static User CurrentUser;
        public bool LoggedIn = false;

        private readonly WorkflowContext _context = new WorkflowContext();

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // do this every time

            // defines current controller and action to determine where you are in the app
            string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            string action = this.ControllerContext.RouteData.Values["action"].ToString();
            string id = "";
            if (this.ControllerContext.RouteData.Values["id"] != null) id = this.ControllerContext.RouteData.Values["id"].ToString();

            

            // if a session is not set AND you are not on the login-page
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionId)))
            {
                if ((controller != "Login" || action != "Index"))
                {
                    // redirect to the login page
                    Response.Redirect("/Login/Index");
                }

            }
            else
            {
                LoggedIn = true;
                ViewBag.CurrentUser = CurrentUser;
                if (!AccessController.hasAccess(CurrentUser, controller, action, id))
                {
                    Response.Redirect("/Access/NoAccess");
                }

                List<Notification> notes = EventController.GetNotes(CurrentUser.UserId);
                ViewBag.notes = notes;

                List<Notification> viewedNotes = EventController.GetViewedNotes(CurrentUser.UserId);
                ViewBag.viewedNotes = viewedNotes;
            }
            ViewBag.LoggedIn = LoggedIn;

            
        }
    }
}