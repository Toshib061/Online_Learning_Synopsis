using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OLS.Models;
using System.Web.Mvc;

namespace OLS.Controllers
{
    public class CompletedQuizController : Controller
    {
        // GET: CompletedQuiz
        public ActionResult Index(string msg)
        {
            // CompletedQuiz completedQuiz = new CompletedQuiz();
            // List<CompletedQuiz> completedQuizzes = completedQuiz.GetCompletedQuizDB();

            ViewData["message"] = msg;

            // return View(completedQuizzes);
            return View();
        }
    }
}