using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CoderOnline
{
    class Program
    {
        static void Main( )
        {
            MCQManager mcqManager = new MCQManager();
            UserManager userManager = new UserManager();
            User currentUser = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Coders Education System -----");
                Console.WriteLine("1. Register New User");
                Console.WriteLine("2. User Login");
                Console.WriteLine("3. Admin Login");
                Console.WriteLine("4. Exit");
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
                        RegisterNewUser(userManager);
                        break;
                    case 2:
                        currentUser = UserLogin(userManager);
                        Console.WriteLine($"Welcome, {currentUser.Username}!");
                        UserManager.ShowUserMenu();

                        break;
                    case 3:
                        AdminLogin(userManager);
                        break;
                    case 4:
                        Console.WriteLine("Exiting the program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void RegisterNewUser(UserManager userManager)
        {
            Console.Clear();
            Console.WriteLine("----- Register New User -----");

            while (true)
            {
                Console.Write("Enter new username: ");
                string newUsername = Console.ReadLine();
                if (!ValidateUsername(newUsername))
                {
                    Console.WriteLine("Invalid username. Username should contain only letters and digits.");
                    continue;
                }

                if (userManager.GetUserByUsername(newUsername) != null)
                {
                    Console.WriteLine("Username already exists. Please choose a different username.");
                }
                else
                {
                    Console.Write("Enter new password: ");
                    string newPassword = Console.ReadLine();

                    Console.Write("Enter email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter age: ");
                    if (!int.TryParse(Console.ReadLine(), out int age) || age < 1 || age > 150)
                    {
                        Console.WriteLine("Invalid age. Age should be a positive value between 1 and 150.");
                        continue;
                    }

                    userManager.CreateUser(newUsername, newPassword, email, age);
                    Console.WriteLine("User registration successful!");
                    break;
                }
            }
        }

        static User UserLogin(UserManager userManager)
        {
            Console.Clear();
            Console.WriteLine("----- User Login -----");
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            User user = userManager.GetUserByUsername(username);

            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }

        public static void AdminLogin(UserManager userManager)
        {
            Console.Clear();
            Console.WriteLine("----- Admin Login -----");
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            //User user = userManager.GetUserByUsername(username);

            if (password == "kailas123" && username.ToLower() == "kailas")
            {
                Console.WriteLine("Admin login successful!");
                AdminManager.AdminMenu(userManager);
                
            }
            else
            {
                Console.WriteLine("Invalid admin username or password.");
                
            }
        }
                
        public static bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            foreach (char c in username)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
        }
    }
}
