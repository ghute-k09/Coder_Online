using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoderOnline
{
    public class AdminManager
    {
        private const string DbConnectionString = "Server=localhost;Database=net;Uid=root;Pwd=cdac;";
        private static MCQManager mcqManager;

        public AdminManager() {
            mcqManager = new MCQManager();
        }

        public static void AdminMenu(UserManager userManager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Admin Menu -----");
                Console.WriteLine("1. View All Users");
                Console.WriteLine("2. Update User");
                Console.WriteLine("3. Delete User");
                Console.WriteLine("4. AddQuestion ");
                Console.WriteLine("5. Print all Questions ");
                Console.WriteLine("6. Delete Question ");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");
                string choiceStr = Console.ReadLine();

                if (!int.TryParse(choiceStr, out int choice))
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ViewAllUsers(userManager);
                        break;
                    case 7:
                        Console.WriteLine("Exiting admin menu.");
                        return;
                    case 2:
                        Console.Write("Enter the user ID to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("Invalid user ID. Please try again.");
                            continue;
                        }

                        Console.Write("Enter new username: ");
                        string newUsername = Console.ReadLine();

                        Console.Write("Enter new password: ");
                        string newPassword = Console.ReadLine();

                        Console.Write("Enter new email: ");
                        string newEmail = Console.ReadLine();

                        Console.Write("Enter new age: ");
                        if (!int.TryParse(Console.ReadLine(), out int newAge) || newAge < 1 || newAge > 150)
                        {
                            Console.WriteLine("Invalid age. Age should be a positive value between 1 and 150.");
                            continue;
                        }

                        UpdateUser(updateId, newUsername, newPassword, newEmail, newAge);
                        break;

                    case 3:
                        Console.Write("Enter the user ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            Console.WriteLine("Invalid user ID. Please try again.");
                            continue;
                        }

                        DeleteUser(deleteId);
                        break;
                    case 4:
                        Console.WriteLine("Add Question : .");
                        AddQuestion();
                        break;
                    case 5:
                        PrintAllQuestions();
                        break;
                    case 6:
                        Console.Write("Enter the question ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int deleteQId))
                        {
                            Console.WriteLine("Invalid user ID. Please try again.");
                            continue;
                        }
                        DeleteQuestion(deleteQId);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void UpdateUser(int id, string username, string password, string email, int age)
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE users
                SET username = @username, password = @password, email = @email, age = @age
                WHERE id = @id;";

                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@age", age);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("User updated successfully!");
        }

        public static void ViewAllUsers(UserManager userManager)
        {
            Console.Clear();
            Console.WriteLine("----- View All Users -----");

            List<User> allUsers = userManager.GetAllUsers();

            if (allUsers.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("+------+------------------+-----------------+-----+");
            Console.WriteLine("|  ID  |    Username      |       Email     | Age |");
            Console.WriteLine("+------+------------------+-----------------+-----+");
            foreach (User user in allUsers)
            {
                Console.WriteLine($"| {user.Id,-5}| {user.Username,-16} | {user.Email,-15} | {user.Age,-3} |");
            }
            Console.WriteLine("+------+------------------+-----------------+-----+");
        }

        public static void DeleteUser(int id)
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM users WHERE id = @id;";

                using (var command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("User deleted successfully!");
        }

        public static void AddQuestion()
        {
            Console.Clear();
            Console.WriteLine("----- Add Question -----");

            MCQManager.AddQuestion(mcqManager);

            Console.WriteLine();
        }

        public static void PrintAllQuestions()
        {
            Console.Clear();
            Console.WriteLine("----- All Questions -----");

            List<MCQQuestion> allQuestions = MCQManager.GetAllMCQQuestions();

            if (allQuestions.Count == 0)
            {
                Console.WriteLine("No questions found.");
                return;
            }

            Console.WriteLine("+------+-----------------------------------------------------------+");
            Console.WriteLine("|  ID  |                       Question                          |");
            Console.WriteLine("+------+-----------------------------------------------------------+");
            foreach (MCQQuestion question in allQuestions)
            {
                Console.WriteLine($"| {question.Id,-5}| {question.Question,-59} |");
                Console.WriteLine("+------+-----------------------------------------------------------+");

                foreach (string option in question.Options)
                {
                    Console.WriteLine($"       - {option}");
                }

                Console.WriteLine();
            }
        }


        public static void DeleteQuestion(int id)
        {
            using (var connection = new MySqlConnection(DbConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM mcq_questions WHERE id = @id;";

                using (var command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Question deleted successfully!");
        }


    }
}
