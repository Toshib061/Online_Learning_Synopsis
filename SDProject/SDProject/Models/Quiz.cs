using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OLS.Models
{
    public class Quiz
    {
        [Required]
        public int quiz_id { set; get; }
        [Required]
        public int admin_id { set; get; }
        [Required]
        public int topic_id { set; get; }
        public string topic_name { set; get; }
        [Required]
        public string title { set; get; }
        [Required]
        public DateTime created_on { set; get; }
        //[Required]
        //public int duration { set; get; }
        //public string duration_string { set; get; }
        public bool participated { set; get; }
        public int obtained_mark { set; get; }
        public string created_by { set; get; }

        [Required]
        public List<Q_A> q_As { set; get; }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------


        public bool SetQuizDB(Quiz quiz)    // inserts the data of a quiz submitted via form in the view
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;
            object generatedId = null;

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = "INSERT INTO [quiz] OUTPUT inserted.quiz_id VALUES (@admin_id, @topic_id, @title, @created_on)"; // SQL identifies a parameter by the '@' at the beginning
                sqlCommand.Connection = sqlConnection;

                sqlCommand.Parameters.AddWithValue("@admin_id", quiz.admin_id);             // defining the "parameter" used in the sql query
                                                                                            // setting the value for that parameter (i.e finding "@admin_id" parameter and replacing it the desired value)
                sqlCommand.Parameters.AddWithValue("@topic_id", quiz.topic_id);
                sqlCommand.Parameters.AddWithValue("@title", quiz.title);
                sqlCommand.Parameters.AddWithValue("@created_on", DateTime.Now);
                // sqlCommand.Parameters.AddWithValue("@duration", quiz.duration);

                sqlConnection.Open();
                generatedId = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
            }

            if (generatedId != null)    // done with storing a quiz entry in the quiz table, now onto storing the questions
            {
                // int quiz_id_qatbl = Convert.ToInt32(generatedId);

                for (int i = 0; i < q_As.Count; i++)
                {
                    q_As[i].quiz_id = Convert.ToInt32(generatedId);
                    q_As[i].question_num = i + 1;
                }

                // if there are 5 questions, the following sqlcommand needs to execute 5 times

                foreach (Q_A q_A in q_As)
                {
                    SqlConnection sqlConnection2 = new SqlConnection(connString);

                    using (sqlConnection2)
                    {
                        SqlCommand sqlCommand2 = new SqlCommand();
                        sqlCommand2.CommandText = "INSERT INTO [question_answer] VALUES (@quiz_id, @question_num, @question, @options, @correct_ans)";
                        sqlCommand2.Connection = sqlConnection2;

                        sqlCommand2.Parameters.AddWithValue("@quiz_id", q_A.quiz_id);               // defining the "parameter" used in the sql query
                                                                                                    // setting the value for that parameter (i.e finding "@admin_id" parameter and replacing it the desired value)
                        sqlCommand2.Parameters.AddWithValue("@question_num", q_A.question_num);
                        sqlCommand2.Parameters.AddWithValue("@question", q_A.question);

                        q_A.options_cs = "";

                        for (int i = 0; i < 4; i++)
                            q_A.options_cs += q_A.options_arr[i] + ",";

                        sqlCommand2.Parameters.AddWithValue("@options", q_A.options_cs);
                        sqlCommand2.Parameters.AddWithValue("@correct_ans", q_A.correct_ans);

                        sqlConnection2.Open();
                        sqlCommand2.ExecuteNonQuery();
                        sqlConnection2.Close();
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Quiz> GetQuizzesDB(int user_id)    // for when all the quizzes available are to be shown
        {
            List<Quiz> quizzes = new List<Quiz>();

            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [quiz]", sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Quiz tempquiz = new Quiz();

                    tempquiz.quiz_id = Convert.ToInt32(sqlDataReader["quiz_id"]);
                    tempquiz.admin_id = Convert.ToInt32(sqlDataReader["admin_id"]);
                    tempquiz.topic_id = Convert.ToInt32(sqlDataReader["topic_id"]);
                    tempquiz.topic_name = GetTopicName(tempquiz.topic_id);
                    tempquiz.title = sqlDataReader["title"].ToString();
                    tempquiz.created_on = Convert.ToDateTime(sqlDataReader["created_on"]);
                    //tempquiz.duration = Convert.ToInt32(sqlDataReader["duration"]);
                    //tempquiz.duration_string = (tempquiz.duration / 60).ToString() + " : " + (tempquiz.duration % 60).ToString();

                    quizzes.Add(tempquiz);
                }

                sqlConnection.Close();
            }

            SqlConnection sqlConnection1 = new SqlConnection(connString);

            using (sqlConnection1)
            {
                for (int i = 0; i < quizzes.Count; i++)
                {
                    sqlConnection1.Open();

                    SqlCommand sqlCommand1 = new SqlCommand("SELECT COUNT(*) FROM [completed_quiz] WHERE [quiz_id]=" + quizzes[i].quiz_id + " AND [user_id]=" + user_id, sqlConnection1);
                    quizzes[i].participated = Convert.ToBoolean(sqlCommand1.ExecuteScalar());

                    sqlConnection1.Close();
                }
            }

            SqlConnection sqlConnection2 = new SqlConnection(connString);

            using (sqlConnection2)
            {
                for (int i = 0; i < quizzes.Count; i++)
                {
                    sqlConnection2.Open();

                    SqlCommand sqlCommand2 = new SqlCommand("SELECT [obtained_mark] FROM [completed_quiz] WHERE [quiz_id]=" + quizzes[i].quiz_id + " AND [user_id]=" + user_id, sqlConnection2);
                    quizzes[i].obtained_mark = Convert.ToInt32(sqlCommand2.ExecuteScalar());

                    sqlConnection2.Close();
                }
            }













            return quizzes;
        }

        public List<Quiz> GetQuizzesAdminDB()    // for when all the quizzes available are to be shown
        {
            List<Quiz> quizzes = new List<Quiz>();

            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [quiz]", sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Quiz tempquiz = new Quiz();

                    tempquiz.quiz_id = Convert.ToInt32(sqlDataReader["quiz_id"]);
                    tempquiz.admin_id = Convert.ToInt32(sqlDataReader["admin_id"]);
                    tempquiz.topic_id = Convert.ToInt32(sqlDataReader["topic_id"]);
                    tempquiz.topic_name = GetTopicName(tempquiz.topic_id);
                    tempquiz.title = sqlDataReader["title"].ToString();
                    tempquiz.created_on = Convert.ToDateTime(sqlDataReader["created_on"]);
                    //tempquiz.duration = Convert.ToInt32(sqlDataReader["duration"]);
                    //tempquiz.duration_string = (tempquiz.duration / 60).ToString() + " : " + (tempquiz.duration % 60).ToString();

                    quizzes.Add(tempquiz);
                }

                sqlConnection.Close();
            }

            SqlConnection sqlConnection2 = new SqlConnection(connString);

            using (sqlConnection2)
            {
                for (int i = 0; i < quizzes.Count; i++)
                {
                    sqlConnection2.Open();

                    SqlCommand sqlCommand2 = new SqlCommand("SELECT [Name] FROM [Users] WHERE [UserID]=" + quizzes[i].admin_id + " AND [IsAdmin]= 1", sqlConnection2);
                    quizzes[i].created_by = sqlCommand2.ExecuteScalar().ToString();

                    sqlConnection2.Close();
                }
            }

            return quizzes;
        }

        public static string GetTopicName(int topicID)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                string ret = "";
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT [topic_name] FROM [topics] WHERE [topic_id]=" + topicID.ToString(), sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                    ret = sqlDataReader["topic_name"].ToString();

                sqlConnection.Close();

                return ret;
            }
        }
    }
}