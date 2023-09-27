using System;
using System.Collections.Generic;
using System.Linq;
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
        public Member (string name, string password, MemberType type )
        {
            _username = name;    
            Password = password;
            Type = type;
            UserCart = new List <Products> ();
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
            int input = int.Parse( Console.ReadLine());     
           
            switch ( input ) 
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
        public void AddProduct(Products product)
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
     
        public override string ToString()
        {
            Console.Clear();
            List <string> printed = new List<string>();

            string accountInformation = $"Username: {UserName}. Password: {Password} Items in cart: {UserCart.Count} Total cost: {CalculateCartCost()} ";

            foreach (var product in _usercart)
            {
                if (!printed.Contains(product.Name)) 
                {
                    accountInformation += "\r\n";
                    accountInformation += $"You have {GetProductInCartByName(product.Name)}  in your cart, they cost {product.Price} each. \r\n" +
                                          $"Total cost for {product.Name}s {CalculateCostForSpecificItem(product.Name)} sek";
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
                if(product.Name == productName) 
                { 
                    totalCost += product.Price;
                }
            }
            return totalCost;
        }
        public int GetProductInCartByName(string productName)
        {
            int count = 0;
            foreach (Products product in UserCart)
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
        public double CheckOut()
        {
            var cost = CostAfterDiscount();
            UserCart.Clear();
            return cost;
        }
        
    }
}
