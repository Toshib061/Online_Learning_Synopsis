using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OLS.Models
{
    public class UserAnswers
    {
        [Required]
        public int quiz_id { set; get; }
        [Required]
        public int user_id { set; get; }
        [Required]
        public int question_num { set; get; }
        public string user_answer { set; get; }


        public void SetUserAnswersDB(List<UserAnswers> userAnswers)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                for(int i=0;i< userAnswers.Count;i++)
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [user_answers] VALUES (@quiz_id, @user_id, @question_number, @user_ans)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@quiz_id", userAnswers[i].quiz_id);
                    sqlCommand.Parameters.AddWithValue("@user_id", userAnswers[i].user_id);
                    sqlCommand.Parameters.AddWithValue("@question_number", userAnswers[i].question_num);
                    sqlCommand.Parameters.AddWithValue("@user_ans", userAnswers[i].user_answer);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}