using OLS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLS.Controllers
{
    public class QuizController : Controller
    {
        [HttpGet]
        public ActionResult Selection()
        {
            if (Session["CurrentUserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { id = "nonexistent" });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Selection(Selection selection)
        {
        
            return RedirectToAction("CreateQuiz", selection);
        }

        [HttpGet]
        public ActionResult CreateQuiz(Selection selection)
        {
            if (selection.question_count == 0)
            {
                return RedirectToAction("Index", "Home", new { id = "nonexistent" });
            }

            Quiz quiz = new Quiz();
            quiz.admin_id = Convert.ToInt32(Session["CurrentUserID"]);

            quiz.title = selection.title;
            //quiz.duration = selection.duration;
            quiz.topic_id = selection.topic_id;
            ViewData["QuestionCount"] = selection.question_count;

            return View(quiz);
        }

        [HttpPost]
        public ActionResult CreateQuiz(Quiz quiz)
        {
            // if (TryUpdateModel<Quiz>(quiz))
            // {
                if (quiz.SetQuizDB(quiz))
                    return RedirectToAction("Index", "Home",new { id = "Success" });
            //ViewData["message"] = "Quiz Created Successfully";
                else
                    return RedirectToAction("Index", "Home", new { id = "Failure" });
            //ViewData["message"] = "Quiz Creation Failed";
            // }


            return RedirectToAction("Index", "Home");
            return View();
        }        

        public ActionResult AllQuizzes()
        {
            if (Session["CurrentUserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { id = "nonexistent" });
            }
            Quiz quiz = new Quiz();
            List<Quiz> quizzes = quiz.GetQuizzesDB(Convert.ToInt32(Session["CurrentUserID"])); // will take from session later on OR somehow from q_As

            return View(quizzes);
        }

        public ActionResult AllQuizzesAdmin()
        {
            if (Session["CurrentUserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { id = "nonexistent" });
            }
            Quiz quiz = new Quiz();
            List<Quiz> quizzes = quiz.GetQuizzesAdminDB();

            return View(quizzes);
        }

        public ActionResult AllParticipatedQuizzes(int id)
        {
            if (Session["CurrentUserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { id = "nonexistent" });
            }
            Quiz quiz = new Quiz();
            List<Quiz> quizzes = quiz.GetQuizzesDB(id);

            return View(quizzes);
        }

        
    }
}