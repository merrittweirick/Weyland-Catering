using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// This class contains all the "work" for catering system management
    /// </summary>
    public class CateringSystem
    {
        public CateringSystem()
        {

        }
        private Dictionary<string, CateringItem> items = new Dictionary<string, CateringItem>();
        List<string> purchaseReportLines = new List<string>();
        double amountSpent = 0;
        List<string> auditEntries = new List<string>();

        public double Balance { get; set; } = 0;

        public void AddCateringItem(CateringItem item)
        {
            items.Add(item.ID, item);
        }

        public List<string> GetAuditEntries()
        {
            return auditEntries;
        }

        public List<string> BuildCateringMenu()                                                                                     //****
        {
            List<string> menu = new List<string>();
            foreach (KeyValuePair<string, CateringItem> kvp in items)
            {
                if (kvp.Value.Quantity == 0)
                {
                     menu.Add($"{kvp.Value.Name} ~~ {kvp.Value.ID} ~~ ${kvp.Value.Price} ~~ SOLD OUT ");
                }
                else
                {
                    menu.Add($"{kvp.Value.Name} ~~ {kvp.Value.ID} ~~ ${kvp.Value.Price} ~~ {kvp.Value.Quantity} ");
                }
            }
            return menu; 
        }

        public string AddMoney(int deposit)
        {

            if (deposit < 0)
            {
                return $"Invalid input, please try again.";
            }
            else if ((Balance + deposit) <= 1000 && Balance + deposit >= 0)
            { 
                Balance += deposit;
                auditEntries.Add($"{DateTime.Now} ADD MONEY: ${deposit} ${Balance} ");
                return $"Your new Balance is: ${Math.Round(Balance, 2)}";
            }
            else if (Balance + deposit > 1000)
            {
                return "Invalid input (balance cannot exceed $1000), please try again.";
            }
            else
            {
                return "Invalid input, please try again";
            }

        }

        public string SelectProduct(string choice, int amountToPurchase, FileAccess file)
        {
            bool choiceIsInDictionary = items.ContainsKey(choice);
            
            if (choiceIsInDictionary) //if user selected a valid item
            {
                int amountAvailable = items[choice].Quantity;
                double costOfPurchase = items[choice].Price * amountToPurchase;

                if ((amountAvailable - amountToPurchase) >= 0 && (Balance >= costOfPurchase) && amountToPurchase > 0)  //if valid purchase
                {
                    items[choice].Quantity -= amountToPurchase;
                    Balance -= (costOfPurchase);

                    purchaseReportLines.Add($"{amountToPurchase} {items[choice].Name} at ${items[choice].Price}/item (${Math.Round(amountToPurchase * items[choice].Price, 2)} total)");

                    amountSpent += Math.Round(amountToPurchase * items[choice].Price, 2);


                    auditEntries.Add($"{DateTime.Now} {amountToPurchase} {items[choice].Name} {items[choice].ID} {costOfPurchase} {Balance} ");

                    items[choice].TotalAmountPurchased += amountToPurchase;
                    items[choice].TotalRevenue += Math.Round(amountToPurchase * items[choice].Price, 2);

                    return $"Your purchase of {choice} was successful, your new balance is ${Math.Round(Balance, 2)}";
                }
                else if (amountAvailable <= 0) //if item sold out
                {
                    return $"We are currently out of {choice}, please select different item";

                }
                else if ((amountAvailable - amountToPurchase) < 0) //if insufficient stock amount
                {
                    return "Insufficient stock for amount requested.";

                }
                else if (Balance < costOfPurchase) //if insufficient funds
                {
                    return "Insufficient funds. Please add enough money before attempting this purchase.";
                }
                else if (amountToPurchase <= 0)
                {
                    return "Invalid input, please try again";
                }
            }
            else //if user selected an invalid item
            {
                    return $"Sorry your selected choice of {choice} does not exist, please select product listed above.";
            }
            return "Invalid input, please try again";
        }

        /* Project required the customer's money to be returned using nickels, dimes, quarters, ones, fives, 
           tens, and twenties. (using the smallest amount of bills and coins possible) */
        public string ReturnMoney()
        { 
            string change;
            int numberOf20s = 0;
            int numberOf10s = 0;
            int numberOf5s = 0;
            int numberOf1s = 0;
            int numberOfQuarters = 0;
            int numberOfDimes = 0;
            int numberOfNickels = 0;
            int totalChange = numberOf20s + numberOf10s + numberOf5s + numberOf1s + numberOfQuarters + numberOfDimes + numberOfNickels;
            while (Balance >= 0.05)
            {
                if (Balance / 20 >= 1)
                {
                    numberOf20s = Convert.ToInt32(Math.Floor(Balance / 20));
                    Balance -= (20 * numberOf20s);
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / 10 >= 1)
                {
                    numberOf10s = Convert.ToInt32(Math.Floor(Balance / 10));
                    Balance -= 10 * numberOf10s;
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / 5 >= 1)
                {
                    numberOf5s = Convert.ToInt32(Math.Floor(Balance / 5));
                    Balance -= 5 * numberOf5s;
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / 1 >= 1)
                {
                    numberOf1s = Convert.ToInt32(Math.Floor(Balance / 1));
                    Balance -= numberOf1s;
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / .25 >= 1)
                {
                    numberOfQuarters = Convert.ToInt32(Math.Floor(Balance / .25));
                    Balance -= numberOfQuarters * .25;
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / .1 >= 1)
                {
                    numberOfDimes = Convert.ToInt32(Math.Floor(Balance / .1));
                    Balance -= numberOfDimes * .1;
                    Balance = Math.Round(Balance, 2);
                }
                else if (Balance / .05 >= 1)
                {
                    numberOfNickels = Convert.ToInt32(Math.Floor(Balance / .05));
                    Balance -= numberOfNickels * .05;
                    Balance = Math.Round(Balance, 2);
                }
            }

            change = $"Your change will be returned as follows: {numberOf20s} Twenty Dollar Bill(s), {numberOf10s} Ten Dollar Bill(s)," +
                $"{numberOf5s} Five Dollar Bill(s), {numberOf1s} One Dollar Bill(s), {numberOfQuarters} Quarter(s), {numberOfDimes} Dime(s), & {numberOfNickels} Nickel(s).";
            Balance = 0;
            auditEntries.Add($"{DateTime.Now} GIVE CHANGE: ${totalChange} ${Balance} ");
            return change;
        }
       
        public List<string> BuildScreenReport()
        {
            return purchaseReportLines;
        }

        public double PrintAmountSpent()
        {
            return amountSpent;
        }
    }  
}
