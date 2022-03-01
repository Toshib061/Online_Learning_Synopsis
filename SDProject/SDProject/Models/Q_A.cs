using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OLS.Models
{
    public class Q_A
    {
        [Required]
        public int quiz_id { set; get; }
        [Required]
        public int question_num { set; get; }
        [Required]
        public string question { set; get; }
        [Required]
        public string options_cs { set; get; }
        [Required]
        public string[] options_arr { set; get; }
        [Required]
        [Display(Name = "Correct Answer:")]
        public string correct_ans { set; get; }

        [Required]
        public string user_answer { set; get; } // handle the case where user inputs nothing as answer

        public List<Q_A> GetQuizDB(int quiz_id) // for when a user will participate in a quiz
        {
            List<Q_A> q_As = new List<Q_A>();

            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [question_answer] WHERE [quiz_id]= @quiz_id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@quiz_id", quiz_id);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Q_A tempq_A = new Q_A();

                    tempq_A.quiz_id = Convert.ToInt32(sqlDataReader["quiz_id"]);
                    tempq_A.question_num = Convert.ToInt32(sqlDataReader["question_num"]);
                    tempq_A.question = sqlDataReader["question"].ToString();
                    tempq_A.options_cs = sqlDataReader["options"].ToString();

                    tempq_A.options_arr = new string[4];
                    int start = 0, j = 0;
                    for (int i = 0; i < tempq_A.options_cs.Length; i++)
                    {
                        if (tempq_A.options_cs[i] == ',')
                        {
                            tempq_A.options_arr[j] = tempq_A.options_cs.Substring(start, i - start);
                            j++;
                            start = i + 1;
                        }
                    }

                    tempq_A.correct_ans = sqlDataReader["correct_ans"].ToString();

                    q_As.Add(tempq_A);
                }

                sqlConnection.Close();
            }

            return q_As;
        }

        public List<Q_A> GetQuizDB(int quiz_id, int user_id)    // for when a user wants to view his answers int an already participated quiz
        {
            List<Q_A> q_As = new List<Q_A>();

            string connString = ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;    // present in Web.config file

            SqlConnection sqlConnection = new SqlConnection(connString);

            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [question_answer] WHERE [quiz_id]= @quiz_id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@quiz_id", quiz_id);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Q_A tempq_A = new Q_A();

                    tempq_A.quiz_id = Convert.ToInt32(sqlDataReader["quiz_id"]);
                    tempq_A.question_num = Convert.ToInt32(sqlDataReader["question_num"]);
                    tempq_A.question = sqlDataReader["question"].ToString();
                    tempq_A.options_cs = sqlDataReader["options"].ToString();

                    tempq_A.options_arr = new string[4];
                    int start = 0, j = 0;
                    for (int i = 0; i < tempq_A.options_cs.Length; i++)
                    {
                        if (tempq_A.options_cs[i] == ',')
                        {
                            tempq_A.options_arr[j] = tempq_A.options_cs.Substring(start, i - start);
                            j++;
                            start = i + 1;
                        }
                    }

                    tempq_A.correct_ans = sqlDataReader["correct_ans"].ToString();

                    q_As.Add(tempq_A);
                }

                sqlConnection.Close();
            }

            SqlConnection sqlConnection2 = new SqlConnection(connString);

            using (sqlConnection2)
            {
                sqlConnection2.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT [user_ans] FROM [user_answers] WHERE [quiz_id]=@quiz_id AND [user_id]=@user_id", sqlConnection2);

                sqlCommand.Parameters.AddWithValue("@quiz_id", quiz_id);
                sqlCommand.Parameters.AddWithValue("@user_id", user_id);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                for (int i = 0; sqlDataReader.Read(); i++)
                {
                    q_As[i].user_answer = sqlDataReader["user_ans"].ToString();
                }
            }

            return q_As;
        }
    }
}