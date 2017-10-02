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
        private double DurationInMiliseconds(DateTime beforeRunning, DateTime afterRunning)
        {
            return afterRunning.Subtract(beforeRunning).TotalMilliseconds;
        }

        private void FunctionShouldTakeLessThan(Action customerFunctionToTime, int timeLimit)
        {
            DateTime timeBeforeRunning = DateTime.Now;
            customerFunctionToTime();
            DateTime timeAfterRunning = DateTime.Now;

            double duration = DurationInMiliseconds(timeBeforeRunning, timeAfterRunning);
            Assert.IsTrue(duration < timeLimit);
        }
        [TestMethod]
        public void AddingCustomerShouldNotTakeMoreThan50Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();

            FunctionShouldTakeLessThan( () => customerBook.addCustomerNamed("paulMcCartney"), 50);
        }

        [TestMethod]
        public void RemovingCustomerShouldNotTakeMoreThan100Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();
            String paulMcCartney = "Paul McCartney";

            customerBook.addCustomerNamed(paulMcCartney);

            FunctionShouldTakeLessThan(() => customerBook.removeCustomerNamed(paulMcCartney), 100);

        }

        [TestMethod]
        public void CanNotAddACustomerWithEmptyName()
        {
            CustomerBook customerBook = new CustomerBook();
            shouldThrowWithExceptionDo<Exception>(() => customerBook.addCustomerNamed(""),
                (exceptionMessage) => CheckErrorMessageAndEmptyCustomer(CustomerBook.CUSTOMER_NAME_EMPTY, customerBook, exceptionMessage));
        }

        [TestMethod]
        public void CanNotRemoveNotAddedCustomer()
        {
            CustomerBook customerBook = new CustomerBook();
            shouldThrowWithExceptionDo<InvalidOperationException>(() => customerBook.removeCustomerNamed("John Lennon"), 
                (exception) => CheckErrorMessageAndEmptyCustomer(CustomerBook.INVALID_CUSTOMER_NAME, customerBook, exception));
        }

        private void CheckErrorMessageAndEmptyCustomer(String expectedMessage, CustomerBook customerBook, Exception exception)
        {
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.IsTrue(customerBook.isEmpty());
        }


        private void shouldThrowWithExceptionDo<T>(Action customerFunctionShouldFail, Action<T> exceptionAssertionBlock) where T : Exception 
        {
            try
            {
                customerFunctionShouldFail();
                Assert.Fail();
            }
            catch (T e)
            {
                exceptionAssertionBlock(e);
            }
            
        }
        
    }
}
