using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OLS.Models
{
    public class CompletedQuiz
    {
        public int quiz_id { set; get; }
        public int user_id { set; get; }
        public int obtained_mark { set; get; }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//


        public List<CompletedQuiz> GetCompletedQuizDB(int user_id)
        {
            List<CompletedQuiz> CompletedQuizzes = new List<CompletedQuiz>();

            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [completed_quiz] WHERE [user_id]=@user_id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    CompletedQuiz tempCompletedQuiz = new CompletedQuiz();

                    tempCompletedQuiz.quiz_id = Convert.ToInt32(sqlDataReader["quiz_id"]);
                    tempCompletedQuiz.user_id = Convert.ToInt32(sqlDataReader["user_id"]);
                    tempCompletedQuiz.obtained_mark = Convert.ToInt32(sqlDataReader["obtained_mark"]);

                    CompletedQuizzes.Add(tempCompletedQuiz);
                }

                sqlConnection.Close();
            }

            return CompletedQuizzes;
        }

        public void SetCompletedQuizDB(CompletedQuiz completedQuiz)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [completed_quiz] VALUES (@quiz_id, @user_id, @obtained_mark)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@quiz_id", completedQuiz.quiz_id);
                sqlCommand.Parameters.AddWithValue("@user_id", completedQuiz.user_id);
                sqlCommand.Parameters.AddWithValue("@obtained_mark", completedQuiz.obtained_mark);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }


        public bool AlreadyParticipated(int quiz_id, int user_id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file
            object count;

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {

                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [completed_quiz] WHERE [quiz_id]=@quiz_id AND [user_id]=@user_id", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@quiz_id", quiz_id);
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);

                sqlConnection.Open();
                count = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
            }

            if (Convert.ToInt32(count) == 0)
                return false;
            else
                return true;
        }
    }
}