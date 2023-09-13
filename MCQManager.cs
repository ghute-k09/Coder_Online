using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoderOnline
{
    public class MCQManager
    {
        private static readonly string connectionString = "Server=localhost;Database=net;Uid=root;Pwd=cdac;";

        public static void InitializeDatabase(string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create table for MCQ questions
                string createMCQQuestionTableQuery = "CREATE TABLE IF NOT EXISTS mcq_questions (" +
                    "id INT AUTO_INCREMENT PRIMARY KEY," +
                    "question VARCHAR(200) NOT NULL," +
                    "option_1 VARCHAR(100) NOT NULL," +
                    "option_2 VARCHAR(100) NOT NULL," +
                    "option_3 VARCHAR(100) NOT NULL," +
                    "option_4 VARCHAR(100) NOT NULL," +
                    "correct_option_index INT NOT NULL" +
                    ");";

                using (MySqlCommand command = new MySqlCommand(createMCQQuestionTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public MCQManager()
        {
           // this.connectionString = connectionString;
            InitializeDatabase(connectionString);
        }

        public static List<MCQQuestion> GetAllMCQQuestions()
        {
            List<MCQQuestion> questions = new List<MCQQuestion>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM mcq_questions;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MCQQuestion question = new MCQQuestion();
                            question.Id = Convert.ToInt32(reader["id"]);
                            question.Question = Convert.ToString(reader["question"]);
                            question.Options = new List<string>
                            {
                                Convert.ToString(reader["option_1"]),
                                Convert.ToString(reader["option_2"]),
                                Convert.ToString(reader["option_3"]),
                                Convert.ToString(reader["option_4"])
                            };
                            question.CorrectOptionIndex = Convert.ToInt32(reader["correct_option_index"]);

                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }

        public static  void AddMCQQuestion(MCQQuestion question)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO mcq_questions (question, option_1, option_2, option_3, option_4, correct_option_index) " +
                               "VALUES (@question, @option1, @option2, @option3, @option4, @correctOptionIndex);";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@question", question.Question);
                    command.Parameters.AddWithValue("@option1", question.Options[0]);
                    command.Parameters.AddWithValue("@option2", question.Options[1]);
                    command.Parameters.AddWithValue("@option3", question.Options[2]);
                    command.Parameters.AddWithValue("@option4", question.Options[3]);
                    command.Parameters.AddWithValue("@correctOptionIndex", question.CorrectOptionIndex);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddQuestion(MCQManager mcqManager)
        {
            Console.Clear();
            Console.WriteLine("----- Add Question -----");

            Console.Write("Enter the question text: ");
            string questionText = Console.ReadLine();

            MCQQuestion newQuestion = new MCQQuestion();
            newQuestion.Question = questionText;

            Console.Write("Enter option 1: ");
            newQuestion.Options.Add(Console.ReadLine());

            Console.Write("Enter option 2: ");
            newQuestion.Options.Add(Console.ReadLine());

            Console.Write("Enter option 3: ");
            newQuestion.Options.Add(Console.ReadLine());

            Console.Write("Enter option 4: ");
            newQuestion.Options.Add(Console.ReadLine());

            int correctOptionIndex;
            do
            {
                Console.Write("Enter the correct option index (1-4): ");
            } while (!int.TryParse(Console.ReadLine(), out correctOptionIndex) || correctOptionIndex < 1 || correctOptionIndex > 4);

            newQuestion.CorrectOptionIndex = correctOptionIndex - 1; // Convert to 0-based index

            MCQManager.AddMCQQuestion(newQuestion);
            Console.WriteLine("Question added successfully!");
        }

        public static void TakeMCQQuiz(MCQManager mcqManager)
        {
            Console.Clear();
            Console.WriteLine("----- Welcome to Coders Eduction System -  MCQ Quiz -----");

            List<MCQQuestion> questions = MCQManager.GetAllMCQQuestions();

            if (questions.Count == 0)
            {
                Console.WriteLine("No MCQ questions available.");
                return;
            }

            int score = 0;

            foreach (MCQQuestion question in questions)
            {
                Console.WriteLine("Question:");
                Console.WriteLine(question.Question);

                for (int i = 0; i < question.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");
                }

                int selectedOption;
                do
                {
                    Console.Write("Enter your answer (1-4): ");
                } while (!int.TryParse(Console.ReadLine(), out selectedOption) || selectedOption < 1 || selectedOption > 4);

                if (selectedOption - 1 == question.CorrectOptionIndex)
                {
                    Console.WriteLine("Correct answer!");
                    score++;
                }
                else
                {
                    Console.WriteLine("Wrong answer!");
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Quiz completed. Your score: {score}/{questions.Count}");
        }


    }

}
