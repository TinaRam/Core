using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Workflow.Models;

namespace Workflow.Controllers
{
    public class LoginController : BaseController
    {
        // GET: /<controller>/
        private readonly WorkflowContext _context;

        public LoginController(WorkflowContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Console.Write("test");
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                {

                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM user WHERE Username = '" + username + "' AND Password = '" + password + "';";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HttpContext.Session.SetString(SessionId, reader.GetValue(1).ToString());
                    }

                }
                return Redirect("~/Home/Index");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove(SessionId);
            return Redirect("~/Login/Index");
        }
    }
}