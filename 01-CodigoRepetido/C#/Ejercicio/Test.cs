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

        private double TimerClassFunction(Action classFunction)
        {
            DateTime timeBeforeRunning = DateTime.Now;
            classFunction();
            DateTime timeAfterRunning = DateTime.Now;

            return DurationInMiliseconds(timeBeforeRunning, timeAfterRunning);
        }
        [TestMethod]
        public void AddingCustomerShouldNotTakeMoreThan50Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();

            double testDuration = TimerClassFunction( () => customerBook.addCustomerNamed("paulMcCartney"));

            Assert.IsTrue(testDuration < 50);
        }

        [TestMethod]
        public void RemovingCustomerShouldNotTakeMoreThan100Milliseconds()
        {
            CustomerBook customerBook = new CustomerBook();
            String paulMcCartney = "Paul McCartney";

            customerBook.addCustomerNamed(paulMcCartney);

            double testDuration = TimerClassFunction(() => customerBook.removeCustomerNamed(paulMcCartney));

            Assert.IsTrue(testDuration < 100);
        }

        [TestMethod]
        public void CanNotAddACustomerWithEmptyName()
        {
            CustomerBook customerBook = new CustomerBook();
            GenerateExceptionWithCustomerBookMethod(() => customerBook.addCustomerNamed(""), customerBook);
        }

        [TestMethod]
        public void CanNotRemoveNotAddedCustomer()
        {
            CustomerBook customerBook = new CustomerBook();

            GenerateExceptionWithCustomerBookMethod(() => customerBook.removeCustomerNamed("John Lennon"), customerBook);

        }

        private void CheckErrorMessageAndEmptyCustomer(String message, String errorCode, CustomerBook customerBook)
        {
            Assert.AreEqual(message, errorCode);
            Assert.IsTrue(customerBook.isEmpty());
        }

        private void GenerateExceptionWithCustomerBookMethod(Action classFunction, CustomerBook customerBook)
        {
            try
            {
                classFunction();
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {
                CheckErrorMessageAndEmptyCustomer(e.Message, CustomerBook.INVALID_CUSTOMER_NAME, customerBook);
            }
            catch (Exception e)
            {
                CheckErrorMessageAndEmptyCustomer(e.Message, CustomerBook.CUSTOMER_NAME_EMPTY, customerBook);
            }
        }
    }
}
