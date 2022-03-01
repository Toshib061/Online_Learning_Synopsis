using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDProject.ServiceLayer;
using SDProject.ViewModels;

namespace SDProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        IQuestionsService qs;
        ICategoriesService cs;

        public HomeController(IQuestionsService qs, ICategoriesService cs)
        {
            this.qs = qs;
            this.cs = cs;
        }
        public ActionResult Index(String id)
        {


            if(id != null)
            {
                if (id == "nonexistent")
                    ViewData["message"] = "The requested page doesn't exist";
                else if (id == "Success")
                    ViewData["message"] = "Quiz created successfully";
                else if (id == "Failure")
                    ViewData["message"] = "Quiz couldn't be created";
                else
                    ViewData["message"] = "You scored: " + id + " points";
            }

            List<QuestionViewModel> questions = this.qs.GetQuestions().Take(10).ToList();
            return View(questions);
            //return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }


        public ActionResult Categories()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            return View(categories);
        }


        [Route("allquestions")]
        public ActionResult Questions()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions();
            return View(questions);
        }


        public ActionResult Search(string str)
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Where(temp => temp.QuestionName.ToLower().Contains(str.ToLower()) || temp.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }



    }
}