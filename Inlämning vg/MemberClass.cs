using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Inlämning_vg
{
    internal class Member : Customer
    {
        public enum MemberType
        {
            None,
            Bronze,
            Silver,
            Gold,
        }
        //Fields
        private MemberType _type;
        private static List<Member> _allmembers = new List<Member>();
        //Constructor
        public Member(string name, string password, MemberType type)
        {
            _username = name;
            Password = password;
            Type = type;
            UserCart = new List<Product>();
        }
        public static List<Member> AllMembers
        {
            get { return _allmembers; }
            set { _allmembers = value; }
        }
        //Properties
        public MemberType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public MemberType SetMemberType()
        {
            Console.WriteLine("What kind om member would you like to be?");
            Console.WriteLine("[1] Bronze. [2] Silver. [3]Gold");
            int input = int.Parse(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Type = MemberType.Bronze;
                    break;
                case 2:
                    Type = MemberType.Silver;
                    break;
                case 3:
                    Type = MemberType.Gold;
                    break;
                default:
                    Console.WriteLine("Invalid input please try again");
                    Type = MemberType.None;
                    SetMemberType();
                    break;
            }
            return Type;
        }
        public void AddProduct(Product product)
        {
            UserCart.Add(product);
        }
        public double CalculateCartCost()
        {
            double totalCost = 0;
            foreach (var product in _usercart)
            {
                totalCost += product.Price;
            }
            return totalCost;

        }
        public double CalculateDiscount()
        {
            double discount = 1;
            if (Type == MemberType.Bronze)
            {
                discount = 0.95;
            }
            else if (Type == MemberType.Silver)
            {
                discount = 0.90;
            }
            else if (Type == MemberType.Gold)
            {
                discount = 0.85;
            }
            return discount;
        }
        public double CostAfterDiscount()
        {
            double discount = CalculateDiscount();
            double cartCost = CalculateCartCost();
            double cartCostAfterDiscount = cartCost * discount;
            return cartCostAfterDiscount;
        }
        public double ConvertCartCostToDollar()
        {
            double dollar = 0.091;
            double cartCost = CostAfterDiscount();
            double cartToDollar = cartCost * dollar;
            double roundedValue = Math.Round(cartToDollar, 2);
            return roundedValue;
        }
        public double ConvertCartCostToEuro()
        {
            double euro = 0.086;
            double cartCost = CostAfterDiscount();
            double cartToEuro = cartCost * euro;
            double roundedValue = Math.Round(cartToEuro, 2);
            return roundedValue;
        }
        public override string ToString()
        {
            Console.Clear();
            List<string> printed = new List<string>();

            string accountInformation = $"Username: {UserName}. Password: {Password} Items in cart: {UserCart.Count} Total cost: {CalculateCartCost()} ";

            foreach (var product in _usercart)
            {
                if (!printed.Contains(product.Name))
                {
                    accountInformation += "\r\n";
                    accountInformation += $"You have {GetProductInCartByName(product.Name)} {product.Name}s in your cart, they cost {product.Price} each. \r\n" +
                                          $"Total cost for {product.Name} {CalculateCostForSpecificItem(product.Name)} sek";
                    printed.Add(product.Name);
                }
            }
            return accountInformation;
        }
        public double CalculateCostForSpecificItem(string productName)
        {
            double totalCost = 0;
            foreach (var product in _usercart)
            {
                if (product.Name == productName)
                {
                    totalCost += product.Price;
                }
            }
            return totalCost;
        }
        public int GetProductInCartByName(string productName)
        {
            int count = 0;
            foreach (Product product in UserCart)
            {
                if (product.Name == productName)
                {
                    count++;
                }
            }
            return count;
        }
        public static void GenerateMembers()
        {

            AllMembers.Add(new Member("Knatte", "123", MemberType.Silver));
            AllMembers.Add(new Member("Fnatte", "321", MemberType.Bronze));
            AllMembers.Add(new Member("Tjatte", "213", MemberType.Gold));
        }
        public double CheckOutSek()
        {
            var cost = CostAfterDiscount();
            UserCart.Clear();
            return cost;
        }
        public double CheckOutDollar()
        {
            double cost = ConvertCartCostToDollar();
            UserCart.Clear();
            return cost;
        }
        public double CheckOutEuro()
        {
            double cost = ConvertCartCostToEuro();
            UserCart.Clear();
            return cost;
        }
        public static void ReadTextFileAndGenerateMembers()
        {
            
            try
            {
                if (File.Exists("Users.txt"))
                {
                    using (StreamReader sr = new StreamReader("Users.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {

                            string[] customerList = line.Split(',');
                            if (customerList.Length == 3)
                            {
                                string name = customerList[0];
                                string password = customerList[1];
                                if (Enum.TryParse(customerList[2], out MemberType type))
                                {
                                    AllMembers.Add(new Member(name, password, type));
                                }
                            }
                        }
                    }

                }
                else 
                {
                    GenerateMembers();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception" + ex.Message);
            }
        }
        public static void SaveNewMemberTofile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Users.txt")) 
                { 
                     foreach (var member in AllMembers)
                     {
                         string line = $"{member.UserName},{member.Password},{member.Type}";
                         sw.WriteLine(line);
                     }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
        }
        public static bool CheckMemberListForduplicates(string username)
        {
            return AllMembers.Any(Member => Member.UserName == username);
        }
        public void SaveCustomerCart()
        {
            string dir = @"MemberCarts";
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
                try
                {
                
                    using (StreamWriter sw = new StreamWriter($"MemberCarts\\{Program.CurrentMember.UserName}.txt"))
                    {
                        foreach (var product in Program.CurrentMember.UserCart)
                        {
                            sw.WriteLine($"{product.Name},{product.Price}");
                        }
                    }
                
                }
            catch (Exception ex)
            {
                Console.WriteLine("Cart couldnt be saved");
            }
        }
        public void LoadCustomerCart()
        {
            UserCart.Clear();
            string filePath = $"MemberCarts\\{UserName}.txt";
       
            if (File.Exists(filePath)) 
            {
                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            if (parts.Length == 2)
                            {
                                string productName = parts[0];
                                double productPrice;

                                if (double.TryParse(parts[1], out productPrice))

                                UserCart.Add(new Product(productName, productPrice)); 

                            }
                        }
                    }
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex); 
                }
       
            }
            else 
            {
                
            }
        }
        public static void CustomerCartsStartup() 
        { 
            for (int i = 0; i < AllMembers.Count; i++)
            {
                var member = AllMembers[i];
                member.LoadCustomerCart();

            }
        }
        public static void GenerateNewCustomerCartFile(Member newMember)
        {
            try
            {
                string fileName = $"MemberCarts\\{Program.CurrentMember.UserName}";
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                sw.WriteLine("Customer Cart");
                }
               
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exeption:" +ex.Message);
            }

        }
    }  
}
