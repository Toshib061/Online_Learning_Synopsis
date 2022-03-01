using OLS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLS.Controllers
{
    public class Q_AController : Controller
    {
        [HttpGet]
        public ActionResult ParticipateInQuiz(int id)

        {
             
            int user_id = Convert.ToInt32(Session["CurrentUserID"]);    // will take from session later on OR somehow from q_As

            CompletedQuiz completedQuiz = new CompletedQuiz();
            if (completedQuiz.AlreadyParticipated(id, user_id))
                return RedirectToAction("ViewQuiz",new { id = id });
            
            Q_A q_A = new Q_A();
            List<Q_A> q_As = q_A.GetQuizDB(id);

            return View(q_As);
        }

        public ActionResult ViewQuiz(int id)
        {
            int user_id = Convert.ToInt32(Session["CurrentUserID"]);    // will take from session later on OR somehow from q_As

            Q_A q_A = new Q_A();
            List<Q_A> q_As = q_A.GetQuizDB(id,user_id);

            return View(q_As);
        }

        [HttpPost]
        public ActionResult ParticipateInQuiz(List<Q_A> q_As)
        {
            // TODO: 1.Evaluation, 2.mark setting, 3.storing user answers in db
            // if(TryUpdateModel(q_A))

            // 1.Evaluation
            int obtained_mark = 0;
            List<UserAnswers> userAnswers = new List<UserAnswers>();
            for(int i=0;i<q_As.Count;i++)
            {
                // 3.storing user answers in db
                UserAnswers tempUserAnswers = new UserAnswers();

                tempUserAnswers.quiz_id = q_As[i].quiz_id;
                tempUserAnswers.user_id = Convert.ToInt32(Session["CurrentUserID"]); // will take from session later on OR somehow from q_As
                tempUserAnswers.question_num = q_As[i].question_num;
                tempUserAnswers.user_answer = q_As[i].user_answer;

                userAnswers.Add(tempUserAnswers);

                if (q_As[i].correct_ans == q_As[i].user_answer)
                    obtained_mark++;    // put some custom marks later
            }

            // 3.storing user answers in db continued
            UserAnswers userAnswer = new UserAnswers();
            userAnswer.SetUserAnswersDB(userAnswers);

            // 2.mark setting
            CompletedQuiz completedQuiz = new CompletedQuiz();
            completedQuiz.quiz_id = userAnswers[0].quiz_id;
            completedQuiz.user_id = Convert.ToInt32(Session["CurrentUserID"]);  // will take from session later on OR somehow from q_As
            completedQuiz.obtained_mark = obtained_mark;

            completedQuiz.SetCompletedQuizDB(completedQuiz);

            return RedirectToAction("Index","Home",new { id = obtained_mark });
        }
    }
}