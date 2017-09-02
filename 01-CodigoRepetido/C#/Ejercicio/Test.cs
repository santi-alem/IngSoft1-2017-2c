using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdiomExercise
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void AddingCustomerShouldNotTakeMoreThan50Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();

            DateTime timeBeforeRunning = DateTime.Now;
            customerBook.addCustomerNamed("John Lennon");
            DateTime timeAfterRunning = DateTime.Now;

            Assert.IsTrue(timeAfterRunning.Subtract(timeBeforeRunning).TotalMilliseconds < 50);
        }

        [TestMethod]
        public void RemovingCustomerShouldNotTakeMoreThan100Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();
            String paulMcCartney = "Paul McCartney";

            customerBook.addCustomerNamed(paulMcCartney);

            DateTime timeBeforeRunning = DateTime.Now;
            customerBook.removeCustomerNamed(paulMcCartney);
            DateTime timeAfterRunning = DateTime.Now;

            Assert.IsTrue(timeAfterRunning.Subtract(timeBeforeRunning).TotalMilliseconds < 100);
        }

        [TestMethod]
        public void CanNotAddACustomerWithEmptyName()
        {

            CustomerBook customerBook = new CustomerBook();

            try
            {
                customerBook.addCustomerNamed("");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, CustomerBook.CUSTOMER_NAME_EMPTY);
                Assert.IsTrue(customerBook.isEmpty());
            }
        }

        [TestMethod]
        public void CanNotRemoveNotAddedCustomer()
        {
            CustomerBook customerBook = new CustomerBook();
            
            try
            {
                customerBook.removeCustomerNamed("John Lennon");
                Assert.Fail();
            }
            // Se utiliza otro tipo de exception por motivos del ejercicio
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(e.Message, CustomerBook.INVALID_CUSTOMER_NAME);
                Assert.AreEqual(0, customerBook.numberOfCustomers());
            }

        }
    }
}
