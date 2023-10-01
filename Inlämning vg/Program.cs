using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämning_vg
{
    internal class Program
    {
        public static Member CurrentMember = null;


        static void Main(string[] args)
        {
            
            Member.ReadTextFileAndGenerateMembers();
            Product.InitializeProducts();
            Member.CustomerCartsStartup();
            StartUpMenu();
            Console.ReadKey();
        }
       
        public static void StartUpMenu()
        {
            Console.WriteLine("Hello and welcome to Walmart");
            Console.WriteLine("Would you like to signup or login?");
            Console.WriteLine("[1] Login.");
            Console.WriteLine("[2] Signup.");
            
            while (true)
            {
                int input = -1;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch 
                {
                    Console.WriteLine("Invalid input try again");
                    continue;
                }
                switch (input)
                {
                    case 1:
                        Console.Clear();
                        LogInMenu();
                        break;
                    case 2:
                        Console.Clear();
                        SignUpMenu();
                        break;
                    default: Console.WriteLine("Invalid input please try again");
                        StartUpMenu();
                        break;
                }
            }
        }
        public static void LogInMenu()
        {
            
            Console.WriteLine("Please write your username");
            string username = Console.ReadLine();
            for (int i = 0; i < Member.AllMembers.Count; i++)
            {
                var customer = Member.AllMembers[i];
                if (customer.UserName == username)
                {
                    while (true)
                    {
                        Console.WriteLine("Please enter your password");
                        var passwordInput = Console.ReadLine();
                        if (customer.Password == passwordInput)
                        {
                            Console.WriteLine("Welcome to Walmart!");
                            CurrentMember = customer;
                            CurrentMember.LoadCustomerCart();
                            InStoreMenu();
                        }
                        else
                        {
                            Console.WriteLine("Password invaild please try again");
                            continue;
                        }
                    }
                }
            }
            if (CurrentMember == null)
            {
                Console.WriteLine("The user does not exist would you like to sign up?");
                Console.WriteLine("[1] Sign up as nu member.");
                Console.WriteLine("[2] Try to log in again. ");
                int result = 0;
                bool sucess = false;
                sucess = int.TryParse(Console.ReadLine(), out result);
                if (sucess)
                { 
                    switch (result)
                    {
                        case 1:
                            SignUpMenu();
                            break;
                        case 2:
                            LogInMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid input please try again.");
                            LogInMenu();
                            break;
                    }
                }
                else
                {
                    LogInMenu();
                }
            }
        }
        public static void LogOutMenu()
        {
            CurrentMember.SaveCustomerCart();
            CurrentMember = null;
            StartUpMenu();
        }
        public static void SignUpMenu()
        {
            Console.WriteLine("Hello please choose a Username");
            string username = Console.ReadLine();
            if (Member.CheckMemberListForduplicates(username)) 
            {
                Console.WriteLine("Username alreaddy exixsts, please try again.");
               
                SignUpMenu();
            }
            Console.WriteLine("Please choose your password");
            string password = Console.ReadLine();
            var newMember = new Member(username, password, Member.MemberType.None);
            CurrentMember = newMember;
            Console.WriteLine("Please chose your memberlevel press:");
            Console.WriteLine("[1] for Bronze.");
            Console.WriteLine("[2] for Silver.");
            Console.WriteLine("[3] for Gold");
            newMember.SetMemberType();
            Member.AllMembers.Add(newMember);
            Member.SaveNewMemberTofile();
            Member.GenerateNewCustomerCartFile(newMember);
            Console.WriteLine("Account created!");
            Console.WriteLine("Press any key to continute to store");
            Console.ReadKey();
            InStoreMenu();
        }
        public static void InStoreMenu()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Walmart {CurrentMember.UserName}");
            Console.WriteLine($"[1] Product list. [2] Account information & Shopping Cart. [3] Checkout. [4] Logout.");
            
            
            while (true)
            {
                int input = -1;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input try again");
                    continue;
                }
                switch (input)
                {
                    case 1:
                        AddProductToCartMenu();
                        break;
                    case 2:
                        Console.WriteLine(CurrentMember.ToString());
                        Console.WriteLine("Press any key to return to store");
                        Console.Read();
                        InStoreMenu();
                        break;
                    case 3:
                        CheckOutMenu();
                        break;
                    case 4:
                        LogOutMenu();
                        break;
                }
            }
        }
        public static void AddProductToCartMenu()
        {
            Console.Clear();
            Console.WriteLine("We have many prodcuts in our store!");
            for (int i = 0; i < Product.AllProducts.Count; i++) 
            {
                Console.WriteLine($"[{i + 1}]{Product.AllProducts[i].Name} cost {Product.AllProducts[i].Price} each.");
            }
            Console.WriteLine("Chose your purchase according to its productnumber or press [4] to see cart and return to Main Meny");

            while (true) 
            {
                int input = -1;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input try again");
                    continue;
                }
                if (input == 1 || input == 2 || input == 3)
                {
                    CurrentMember.AddProduct(Product.AllProducts[input-1]);
                    Console.WriteLine($"An {Product.AllProducts[input - 1].Name} was added ro your cart");
                }
                else if (input == 4)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Prodcut does not exist try again");
                }
            }
            InStoreMenu();
        }
        public static void CheckOutMenu()
        {

            Console.Clear();
            Console.WriteLine($"The total cost of the items added to your cart is {CurrentMember.CalculateCartCost()} Sek");
            Console.WriteLine($"Your current member level as {CurrentMember.Type} offers you a discount today!");
            Console.WriteLine($"Your total will therefore be {CurrentMember.CostAfterDiscount()} Sek");
            Console.WriteLine($"Please press [1] to confirm purchase and chose payment or [2] to continue shopping");


            while (true)
            {
                int input = -1;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input try again");
                    continue;
                }
                switch (input) 
                {
                    case 1:
                        PaymentChoiceAndCheckoutMenu();
                        break;
                    case 2:
                        InStoreMenu();
                        break;
                }
                
            }
        }
        public static void PaymentChoiceAndCheckoutMenu()
        {
            Console.WriteLine("We at walmart offers several payment optinos for your convenience.");
            Console.WriteLine($"Press [1] to pay in SEK. Total: {CurrentMember.CostAfterDiscount()} Sek");
            Console.WriteLine($"Press [2] to pay in Dollar. Total: {CurrentMember.ConvertCartCostToDollar()} Dollar");
            Console.WriteLine($"Press [3] to pay in Euro. Total: {CurrentMember.ConvertCartCostToEuro()} Euro");
            Console.WriteLine($"Press [4] to return continue shopping");
            while (true)
            {
                int input = -1;
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input try again");
                    continue;

                }
                switch (input)
                {
                    case 1:
                        Console.WriteLine("Thank you for your purchase! Welcome back soon");
                        Console.WriteLine($"{CurrentMember.CheckOutSek()} have been debited from your account");
                        CurrentMember.SaveCustomerCart();
                        Console.ReadKey();
                        InStoreMenu();
                        break;
                    case 2:
                        Console.WriteLine("Thank you for your purchase! Welcome back soon");
                        Console.WriteLine($"{CurrentMember.CheckOutDollar()} dollars have been debited from your account");
                        CurrentMember.SaveCustomerCart();
                        Console.ReadKey();
                        InStoreMenu();
                        break;
                    case 3:
                        Console.WriteLine("Thank you for your purchase! Welcome back soon");
                        Console.WriteLine($"{CurrentMember.CheckOutEuro()} Euro have been debited from your account.");
                        CurrentMember.SaveCustomerCart();
                        Console.ReadKey();
                        InStoreMenu();
                        break;
                    case 4:
                        InStoreMenu();
                        break;
                }
            }

        }
      
    }
   
}
