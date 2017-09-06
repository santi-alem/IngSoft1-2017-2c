#
# Developed by 10Pines SRL
# License: 
# This work is licensed under the 
# Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. 
# To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/ 
# or send a letter to Creative Commons, 444 Castro Street, Suite 900, Mountain View, 
# California, 94041, USA.
#  
import unittest
import time


class CustomerBook:
    CUSTOMER_NAME_CAN_NOT_BE_EMPTY = 'Customer name can not be empty'
    CUSTOMER_ALREADY_EXIST = 'Customer already exists'
    INVALID_CUSTOMER_NAME = 'Invalid customer name'

    def __init__(self):
        self.customerNames = set()

    def addCustomerNamed(self, name):
        # El motivo por el cual se hacen estas verificaciones y se levanta esta excepcion es por motivos del
        # ejercicio - Hernan.
        if not name:
            raise ValueError(self.__class__.CUSTOMER_NAME_CAN_NOT_BE_EMPTY)
        if self.includesCustomerNamed(name):
            raise ValueError(self.__class__.CUSTOMER_ALREADY_EXIST)

        self.customerNames.add(name)

    def isEmpty(self):
        return self.numberOfCustomers() == 0

    def numberOfCustomers(self):
        return len(self.customerNames)

    def includesCustomerNamed(self, name):
        return name in self.customerNames

    def removeCustomerNamed(self, name):
        # Esta validacion mucho sentido no tiene, pero esta puesta por motivos del ejericion - Hernan
        if not self.includesCustomerNamed(name):
            raise KeyError(self.__class__.INVALID_CUSTOMER_NAME)

        self.customerNames.remove(name)




def functionTakeLessTime(function_to_time, time_in_ms):
    time_elapsed = timeFunction(function_to_time)
    return lessThanInMs(time_elapsed, time_in_ms)


def lessThanInMs(time_elapsed, time_in_ms):
    return (time_elapsed) * 1000 < time_in_ms


def timeFunction(function_to_time):
    start_time = time.time()
    function_to_time()
    end_time = time.time()
    return end_time - start_time


class IdionTest(unittest.TestCase):
    def testAddingCustomerShouldNotTakeMoreThan50Milliseconds(self):
        customerBook = CustomerBook()

        self.assertTrue(functionTakeLessTime(lambda: customerBook.addCustomerNamed('John Lennon'), 50))

    def testRemovingCustomerShouldNotTakeMoreThan100Milliseconds(self):
        customerBook = CustomerBook()

        customerBook.addCustomerNamed('Paul McCartney')

        self.assertTrue(functionTakeLessTime(lambda: customerBook.removeCustomerNamed('Paul McCartney'), 100))

    def failUnlessExceptionRaises(self, function_to_test, exception_class, exception_handler):
        try:
            function_to_test()
            self.Fail()
        except exception_class as exception:
            exception_handler(exception)

    def assertExceptionMessageAndExpression(self, exception, expected_message, assert_expression):
        self.assertEquals(exception.message, expected_message)
        assert_expression()

    def testCanNotAddACustomerWithEmptyName(self):
        customerBook = CustomerBook()

        assertCustomerBookIsEmpty = lambda: self.assertTrue(customerBook.isEmpty())

        self.failUnlessExceptionRaises(function_to_test=lambda: customerBook.addCustomerNamed(''),
                                       exception_class=ValueError,
                                       exception_handler=lambda exception: self.assertExceptionMessageAndExpression(
                                           exception, CustomerBook.CUSTOMER_NAME_CAN_NOT_BE_EMPTY,
                                           assertCustomerBookIsEmpty))

    def testCanNotRemoveNotAddedCustomer(self):
        customerBook = CustomerBook()
        customerBook.addCustomerNamed('Paul McCartney')

        def assertCostumerBookHasCostumerNamed(self=self, customerBook=customerBook, customer_name='Paul McCartney'):
            self.assertTrue(customerBook.numberOfCustomers() == 1)
            self.assertTrue(customerBook.includesCustomerNamed(customer_name))

        self.failUnlessExceptionRaises(function_to_test=lambda: customerBook.removeCustomerNamed('John Lennon'),
                                       exception_class=KeyError,
                                       exception_handler=lambda exception: self.assertExceptionMessageAndExpression(
                                           exception, CustomerBook.INVALID_CUSTOMER_NAME,
                                           assertCostumerBookHasCostumerNamed))

    if __name__ == "__main__":
        unittest.main()
