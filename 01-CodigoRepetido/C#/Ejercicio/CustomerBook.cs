using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdiomExercise
{
    public class CustomerBook
    {
        public static String CUSTOMER_NAME_EMPTY = "Customer name can not be empty";
        public static String CUSTOMER_ALREADY_EXISTS = "Customer already exists";
        public static string INVALID_CUSTOMER_NAME = "Invalid Customer name";

        private IList<String> customerNames = new List<String>();

        public void addCustomerNamed(String name)
        {
            if (name.Length == 0) throw new Exception(CUSTOMER_NAME_EMPTY);
            if (containsCustomerNamed(name)) throw new Exception(CUSTOMER_ALREADY_EXISTS);

            customerNames.Add(name);
        }

        public bool isEmpty()
        {
            return customerNames.Count == 0;
        }

        public int numberOfCustomers()
        {
            return customerNames.Count;
        }

        public bool containsCustomerNamed(String name)
        {
            return customerNames.Contains(name);
        }

        public void removeCustomerNamed(String name)
        {
            if (!customerNames.Remove(name))
                throw new InvalidOperationException(INVALID_CUSTOMER_NAME);
        }
    }

}
