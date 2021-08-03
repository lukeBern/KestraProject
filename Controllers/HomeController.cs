using KestraCodingTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KestraCodingTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration Configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Index(FormModel formModel)
        {
            try
            {
                AddRecord(formModel);
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        public void AddRecord(FormModel formModel)
        {
            string connectionString = this.Configuration.GetConnectionString("Database");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("InsertRecord", conn);
            command.Parameters.AddWithValue("@first", formModel.FirstName);
            command.Parameters.AddWithValue("@last", formModel.LastName);
            command.Parameters.AddWithValue("@reason", formModel.Reason);
            command.CommandText = @"INSERT INTO [dbo].[Form_Input] VALUES (@first, @last, @reason)";

            conn.Open();
            int rowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }
    }
}
