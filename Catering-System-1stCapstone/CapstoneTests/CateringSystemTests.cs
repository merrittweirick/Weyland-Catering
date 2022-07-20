using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class CateringSystemTests
    {
        Dictionary<string, CateringItem> testItems = new Dictionary<string, CateringItem>();

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(1000)]
        [DataRow(0)]
        public void AddMoneyShouldWork(int a)
        {
            // Arrange
            CateringSystem sut = new CateringSystem();

            // Act
            sut.AddMoney(a);

            // Assert
            Assert.AreEqual(a, sut.Balance);
        }

        [TestMethod]
        public void AddMoneyShouldNotChangeBalance()
        {
            // Arrange
            CateringSystem sut = new CateringSystem();
            int deposit = -5;

            // Act
            sut.AddMoney(deposit);

            // Assert
            Assert.AreEqual(0, sut.Balance);
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //                                         MAKE CHANGE METHOD
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        [TestMethod]
        public void IfBalanceIsNineteenNinetyFiveShouldReturnCorrectChange()
        {
            CateringSystem op = new CateringSystem();
            op.Balance = 19.95;

            string result = op.ReturnMoney();
            string expected = $"Your change will be returned as follows: {0} Twenty Dollar Bill(s), {1} Ten Dollar Bill(s)," +
                $"{1} Five Dollar Bill(s), {4} One Dollar Bill(s), {3} Quarter(s), {2} Dime(s), & {0} Nickel(s).";

            Assert.AreEqual(expected, result);
            Assert.AreEqual(op.Balance, 0);
        }
        [TestMethod]
        public void IfBalanceIsFiveCentsShouldReturn1OneNickelString()
        {
            CateringSystem op = new CateringSystem();
            op.Balance = .05;

            string result = op.ReturnMoney();
            string expected = $"Your change will be returned as follows: {0} Twenty Dollar Bill(s), {0} Ten Dollar Bill(s)," +
                $"{0} Five Dollar Bill(s), {0} One Dollar Bill(s), {0} Quarter(s), {0} Dime(s), & {1} Nickel(s).";

            Assert.AreEqual(expected, result);
            Assert.AreEqual(op.Balance, 0);
        }
        [TestMethod]
        public void IfBalanceIsOneThousandDollarsShouldReturn50Twenties()
        {
            CateringSystem op = new CateringSystem();
            op.Balance = 1000;

            string result = op.ReturnMoney();
            string expected = $"Your change will be returned as follows: {50} Twenty Dollar Bill(s), {0} Ten Dollar Bill(s)," +
                $"{0} Five Dollar Bill(s), {0} One Dollar Bill(s), {0} Quarter(s), {0} Dime(s), & {0} Nickel(s).";

            Assert.AreEqual(expected, result);
            Assert.AreEqual(op.Balance, 0);
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //                            BUILD CATERING MENU && ADD MONEY
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        [TestMethod]

        public void BuildCateringMenuBuildsStringsMatchingDictionaryValues ()
        {
            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();
           
            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            sut.AddCateringItem(bev);
            sut.AddCateringItem(app);
            sut.AddCateringItem(ent);
            sut.AddCateringItem(des);
            sut.AddCateringItem(zeroQuantity);

            //Act
            actual = sut.BuildCateringMenu();

            //Assert
            Assert.AreEqual(5, actual.Count);
            Assert.IsTrue(actual[0].Contains("Soda"));
            Assert.IsTrue(actual[1].Contains("3.5"));
            Assert.IsTrue(actual[2].Contains("E1"));
            Assert.IsTrue(actual[3].Contains("7"));
            Assert.IsTrue(actual[4].Contains("SOLD OUT"));



        }

        [TestMethod]
        public void ZeroedOutQuantityReturnsSoldOutString()
        {
            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();
            CateringItem app = new Appetizers("cheese", 4.00, "A6", 0);
            sut.AddCateringItem(app);

            actual = sut.BuildCateringMenu();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual[0].Contains("SOLD OUT"));

        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //                                                  SELECT PRODUCT
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        [DataTestMethod]
        [DataRow("A1", 1)]
        
        
        
        

        public void TheHappyPathOnSelectPathReturnsString(string a, int b)
        {
            CateringSystem bubba = new CateringSystem();
            string result;
            bubba.Balance = 10;
            double oldBalance = bubba.Balance;

            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();

            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            bubba.AddCateringItem(bev);
            bubba.AddCateringItem(app);
            bubba.AddCateringItem(ent);
            bubba.AddCateringItem(des);



            result = bubba.SelectProduct("A1", 1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("successful"));
            Assert.AreEqual(oldBalance-3.50, bubba.Balance);

        }
        [DataTestMethod]
        [DataRow("D5", 0)]
        public void CustomerIsCheapReturnsString(string a, int b)
        {
            CateringSystem bubba = new CateringSystem();
            string result;
            bubba.Balance = 10;
            double oldBalance = bubba.Balance;

            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();

            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            bubba.AddCateringItem(bev);
            bubba.AddCateringItem(app);
            bubba.AddCateringItem(ent);
            bubba.AddCateringItem(des);



            result = bubba.SelectProduct(a, b);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("troll"));
            Assert.AreEqual(10, bubba.Balance);

        }
        [DataTestMethod]
        [DataRow("A1", 11)]
        public void CustomerTriesToBuyTooManyReturnsString(string a, int b)
        {
            CateringSystem bubba = new CateringSystem();
            string result;
            bubba.Balance = 10;
            double oldBalance = bubba.Balance;

            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();

            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            bubba.AddCateringItem(bev);
            bubba.AddCateringItem(app);
            bubba.AddCateringItem(ent);
            bubba.AddCateringItem(des);



            result = bubba.SelectProduct(a, b);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("stock"));
            Assert.AreEqual(10, bubba.Balance);

        }
        [DataTestMethod]
        [DataRow("D7", 1)]
        public void CustomerTriesToBuySomethingThatDoesntExistReturnNonexistentString(string a, int b)
        {
            CateringSystem bubba = new CateringSystem();
            string result;
            bubba.Balance = 10;
            double oldBalance = bubba.Balance;

            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();

            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            bubba.AddCateringItem(bev);
            bubba.AddCateringItem(app);
            bubba.AddCateringItem(ent);
            bubba.AddCateringItem(des);



            result = bubba.SelectProduct(a, b);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("exist"));
            Assert.AreEqual(10, bubba.Balance);

        }
        [DataTestMethod]
        [DataRow("A4", 1)]
        public void CustomerTriesToBuySomethingOutOfStockReturnOutOfString(string a, int b)
        {
            CateringSystem bubba = new CateringSystem();
            string result;
            bubba.Balance = 10;
            double oldBalance = bubba.Balance;

            List<string> actual = new List<string>();
            CateringSystem sut = new CateringSystem();

            CateringItem bev = new Beverages("Soda", 1.50, "B1", 9);
            CateringItem app = new Appetizers("Pretzels and Mustard", 3.50, "A1", 3);
            CateringItem ent = new Entrees("Burnt Ends", 8.85, "E1", 2);
            CateringItem des = new Dessert("French Silk Pie", 2.50, "D5", 7);
            CateringItem zeroQuantity = new Appetizers("Bruschetta", 2.65, "A4", 0);
            bubba.AddCateringItem(bev);
            bubba.AddCateringItem(app);
            bubba.AddCateringItem(ent);
            bubba.AddCateringItem(des);
            bubba.AddCateringItem(zeroQuantity);


            result = bubba.SelectProduct(a, b);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("currently out of"));
            Assert.AreEqual(10, bubba.Balance);

        }
        
    }





    //Actual:<Your change will be returned as follows: 0 Twenty Dollar Bill(s), 1 Ten Dollar Bill(s),1 Five Dollar Bill(s), 4 One Dollar Bill(s), 3 Quarter(s), 1 Dime(s), & 1 Nickel(s).>
  //Expected:<Your change will be returned as follows: 0 Twenty Dollar Bill(s), 1 Ten Dollar Bill(s),1 Five Dollar Bill(s), 4 One Dollar Bill(s), 3 Quarter(s), 2 Dime(s), & 0 Nickel(s).>
}
