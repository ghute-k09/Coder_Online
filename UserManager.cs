using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoderOnline
{
    public class UserManager
    {
        private const string DbConnectionString = "Server=localhost;Database=net;Uid=root;Pwd=cdac;";
        private static MCQManager mcqManager;
        public UserManager()
        {
            InitializeDatabase();
            InitializeAdminUser();
            InitializeMCQQuestionsTable();
            mcqManager = new MCQManager();
        }

        private void InitializeMCQQuestionsTable()
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                // Create the mcqquestions table if it doesn't exist
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS mcqquestions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        questionText VARCHAR(255) NOT NULL,
                        option1 VARCHAR(255) NOT NULL,
                        option2 VARCHAR(255) NOT NULL,
                        option3 VARCHAR(255) NOT NULL,
                        option4 VARCHAR(255) NOT NULL,
                        correctOptionIndex INT,
                        userId INT,
                        FOREIGN KEY (userId) REFERENCES users(id)
                    );";

                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void InitializeDatabase()
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                // Create the users table if it doesn't exist
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        username VARCHAR(255) NOT NULL,
                        password VARCHAR(255) NOT NULL,
                        email VARCHAR(255) NOT NULL,
                        age INT
                    );";
                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void InitializeAdminUser()
        {
            // Check if the admin user already exists
            User adminUser = GetUserByUsername("kailas");
            if (adminUser == null)
            {
                // If not, create the admin user with a default password
                CreateUser("kailas", "kailas123", "kailas@gmail.com", 27);
            }
        }

        public void CreateUser(string username, string password, string email, int age)
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO users (username, password, email, age) VALUES (@username, @password, @email, @age);";

                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@age", age);

                    command.ExecuteNonQuery();
                }
            }

           // Console.WriteLine("User created successfully!");
        }

        public User GetUserByUsername(string username)
        {
            User user = null;

            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM users WHERE username = @username;";
                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString(),
                                Age = Convert.ToInt32(reader["age"])
                            };
                        }
                    }
                }
            }

            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM users;";
                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString(),
                                Age = Convert.ToInt32(reader["age"])
                            });
                        }
                    }
                }
            }

            return users;
        }

        public static void ShowUserMenu()
        {
            Console.WriteLine("Welcome to the MCQ System!");
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Take MCQ Quiz");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        TakeQuiz();
                        break;
                    case 2:
                        Console.WriteLine("Thank you for using the MCQ System. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please choose a valid option from the menu.");
                        break;
                }
            }
        }

        public static void TakeQuiz()
        {
            Console.Clear();
            Console.WriteLine("----- MCQ Quiz -----");

            MCQManager.TakeMCQQuiz(mcqManager);

            Console.WriteLine();
        }



    }
}