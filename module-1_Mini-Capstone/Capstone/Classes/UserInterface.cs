using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class UserInterface
    {
        private CateringSystem catering = new CateringSystem();
        private FileAccess file = new FileAccess();
        bool quitMainMenu = false;
        bool quitOrderMenu = false;

        public void RunMainMenu()
        {
            file.ReadFiles(catering);

            while (!quitMainMenu)
            {
                Console.WriteLine();
                Console.WriteLine("(1) Display Catering Items");

                Console.WriteLine("(2) Order");

                Console.WriteLine("(3) Quit");
                Console.WriteLine();

                string mainMenuChoice = Console.ReadLine();

                // Catering items are read from a file and populated into CateringItem subclasses to be displayed for the user here
                if(mainMenuChoice == "(1)" || mainMenuChoice == "1" || mainMenuChoice == "one" || mainMenuChoice == "One")
                {
                    
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    List<string> menu = catering.BuildCateringMenu(); 
                     
                    foreach(string menuItem in menu)
                    {
                        Console.WriteLine(menuItem);
                    }
                }

                // OrderMenu allows user to add funds, queue items to be purchased, and complete transaction
                else if(mainMenuChoice == "(2)" || mainMenuChoice == "2" || mainMenuChoice == "two" || mainMenuChoice == "Two")
                {
                    Console.Clear();
                    RunOrderMenu();
                }

                // Quitting writes all transactions to an audit log
                else if (mainMenuChoice == "(3)" || mainMenuChoice == "3" || mainMenuChoice == "three" || mainMenuChoice == "Three")
                {
                    file.WriteAuditLog(catering);
                    quitMainMenu = true;
                }
                else
                {
                    Console.WriteLine("Invalid selection, please try again.");
                }
            }
        }

        public void RunOrderMenu()
        {
            while (!quitOrderMenu)
            {
                Console.WriteLine();
                Console.WriteLine("(1) Add Money");

                Console.WriteLine("(2) Select Products");

                Console.WriteLine("(3) Complete Transaction");
                Console.WriteLine();
                Console.WriteLine($"Your Current Account Balance is: ${Math.Round(catering.Balance, 2)}");
                Console.WriteLine();
                string orderMenuChoice = Console.ReadLine();

                // User can add money to their account to later spend on purchasing items
                if (orderMenuChoice == "(1)" || orderMenuChoice == "1" || orderMenuChoice == "one" || orderMenuChoice == "One")
                {
                    AddMoney();
                }
       
                // User can spend money from their account to purchase a specified quantity of available items
                else if (orderMenuChoice == "(2)" || orderMenuChoice == "2" || orderMenuChoice == "two" || orderMenuChoice == "Two")
                {
                    SelectProducts();
                }

                // User leaves OrderMenu. Purchase Report is printed if applicable. Money is returned to user
                else if (orderMenuChoice == "(3)" || orderMenuChoice == "3" || orderMenuChoice == "three" || orderMenuChoice == "Three")
                {
                    CompleteTransaction();
                }
                else
                {
                    Console.WriteLine("Invalid selection, please try again.");
                }
            }
            return;
        }

        private void AddMoney() 
        {
            Console.WriteLine("How much money would you like to add? ");
            Console.WriteLine("Deposit amount must be in whole dollars (1/5/25/50?)");
            Console.WriteLine("Note: Balance may not exceed $1000");
            int deposit = int.Parse(Console.ReadLine());
            Console.WriteLine(catering.AddMoney(deposit)); 
        }

        private void SelectProducts()
        {
            Console.WriteLine("Please enter the product ID of the item you'd like to purchase");
            string selectProductChoice = Console.ReadLine();
            string upProductID = selectProductChoice.ToUpper();
            Console.WriteLine("Please enter the quantity you would like to purchase.");
            int productAmount = int.Parse(Console.ReadLine());
            Console.WriteLine(catering.SelectProduct(upProductID, productAmount, file));
        }

        private void CompleteTransaction() 
        {
            List<string> purchaseReport = catering.BuildScreenReport();
            if (purchaseReport.Count > 0)
            {
                Console.WriteLine("Successfully purchased:");
                foreach (string purchase in purchaseReport)
                {
                    Console.WriteLine(purchase);
                }
                Console.WriteLine($"Total: ${Math.Round(catering.PrintAmountSpent(), 2)}");
            }
            else
            {
                Console.WriteLine("No purchases made.");
            }

            Console.WriteLine(catering.ReturnMoney());

            quitOrderMenu = true;
        }
    }
}
