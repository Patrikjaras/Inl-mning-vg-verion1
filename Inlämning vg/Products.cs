using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämning_vg
{
    internal class Products
    {
        //fields
        private string _name;
        private double _price;
        private static List<Products> _allproducts = new List<Products>();

        //Constructor
        public Products(string name, double price)
        {
            Name = name;
            Price = price;
        }
        //Properties
        public string Name
        { 
            get { return _name; } 
            set { _name = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public static List<Products> AllProducts
        {
            get { return _allproducts; }
            set { _allproducts = value; }
        }
        //plocka bort
        public static void DisplayProdcuts()
      {
          Console.WriteLine("These are the prodcuts we have for sale");
          foreach (var product in AllProducts) 
          {
              Console.WriteLine($"{product.Name}, Price: {product.Price} sek each");
          }
      }
        public static void InitializeProducts()
        {
            AllProducts.Add(new Products("Apple", 9));
            AllProducts.Add(new Products("Fish", 69));
            AllProducts.Add(new Products("Coca Cola Zero", 10));
        }
      
        

    }
}
