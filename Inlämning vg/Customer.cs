using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämning_vg
{
   internal abstract class Customer
    {
        
        // Fields
        protected string _username;
        private string _password;
        protected List<Products> _usercart;

        //Properties
        public string UserName
        { 
            get { return _username; } 
             
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; } 
        }
        public List<Products> UserCart
        {
            get { return _usercart; }
            set { _usercart = value; }
        }
    }
}
