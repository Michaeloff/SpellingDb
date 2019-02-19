using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellingDb.Models
{
    public class WordDataAccessLayer
    {
        string connectionString = @"Data Source=DAVEPC\SQLEXPRESS;Initial Catalog=Spelling;Integrated Security=True";
   
        public IEnumerable<Word> GetAllWords()
        {
            List<Word> words = new List<Word>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllWords", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    Word word = new Word();

                    word.ID = Convert.ToInt32(sqlReader["WordID"]);
                    word.Name = sqlReader["Word"].ToString();
                    word.PartOfSpeech = sqlReader["PartOfSpeech"].ToString();
                    word.Definition = sqlReader["Definition"].ToString();
                    word.Example = sqlReader["Example"].ToString();
                    word.Synonyms = sqlReader["Synonyms"].ToString();
                    word.Grade = Convert.ToByte(sqlReader["Grade"]);
                    word.List = sqlReader["List"].ToString();

                    words.Add(word);
                }
                connection.Close();
            }
            return words;
        }

        public Word GetWordDetails(int? id)
        {
            Word word = new Word();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM dbo.Words WHERE WordID=" + id;
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                connection.Open();
                SqlDataReader sqlReader = cmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    word.ID = Convert.ToInt32(sqlReader["WordID"]);
                    word.Name = sqlReader["Word"].ToString();
                    word.PartOfSpeech = sqlReader["PartOfSpeech"].ToString();
                    word.Definition = sqlReader["Definition"].ToString();
                    word.Example = sqlReader["Example"].ToString();
                    word.Synonyms = sqlReader["Synonyms"].ToString();
                    word.Grade = Convert.ToByte(sqlReader["Grade"]);
                    word.List = sqlReader["List"].ToString();
                }
            }
            return word;
        }

        public void AddWord(Word word)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddWord", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Word", word.Name);
                cmd.Parameters.AddWithValue("@PartOfSpeech", word.PartOfSpeech);
                cmd.Parameters.AddWithValue("@Definition", word.Definition);
                cmd.Parameters.AddWithValue("@Example", word.Example);
                cmd.Parameters.AddWithValue("@Synonyms", word.Synonyms);
                cmd.Parameters.AddWithValue("@Grade", word.Grade);
                cmd.Parameters.AddWithValue("@List", word.List);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
   
        public void UpdateWord(Word word)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateWord", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@WordID", word.ID);
                cmd.Parameters.AddWithValue("@Word", word.Name);
                cmd.Parameters.AddWithValue("@PartOfSpeech", word.PartOfSpeech);
                cmd.Parameters.AddWithValue("@Definition", word.Definition);
                cmd.Parameters.AddWithValue("@Example", word.Example);
                cmd.Parameters.AddWithValue("@Synonyms", word.Synonyms);
                cmd.Parameters.AddWithValue("@Grade", word.Grade);
                cmd.Parameters.AddWithValue("@List", word.List);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    
        public void DeleteWord(int? id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteWord", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@WordID", id);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
