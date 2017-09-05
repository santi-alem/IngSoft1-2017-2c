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


# # la clase esta no se si es un toque demasiado
# class StopWatch:
#     def __init__(self):
#         self.start_time = 0
#         self.end_time = 0
#
#     def start(self):
#         self.start_time = time.time()
#
#     def stop(self):
#         self.end_time = time.time()
#
#     @property
#     def time_elapsed(self):
#         return self.end_time - self.start_time
#
#     ## A partir de aaca  Esto ya no se si es la responsabilidad del Objecto
#     def time_function(self, function_to_time):
#         self.start()
#         function_to_time()
#         self.stop()
#
#     def less_than_ms(self, time_in_ms):
#         return (self.time_elapsed) * 1000 < time_in_ms
#
#
# def functionTakesLessTimeThan(function, time_in_ms):
#     watch = StopWatch()
#     watch.time_function(function)
#     return watch.less_than_ms(time_in_ms)


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

    def failUnlessException(self, function_to_test, error, assert_expression):
        try:
            function_to_test()
            self.Fail()
        except error as exception:
            assert_expression(exception)
        


    def testAddingCustomerShouldNotTakeMoreThan50Milliseconds(self):
        customerBook = CustomerBook()

        self.assertTrue(functionTakeLessTime(lambda: customerBook.addCustomerNamed('John Lennon'), 50))

    def testRemovingCustomerShouldNotTakeMoreThan100Milliseconds(self):
        customerBook = CustomerBook()

        customerBook.addCustomerNamed('Paul McCartney')

        self.assertTrue(functionTakeLessTime(lambda: customerBook.removeCustomerNamed('Paul McCartney'), 100))

    def testCanNotAddACustomerWithEmptyName(self):
        customerBook = CustomerBook()

        try:
            customerBook.addCustomerNamed('')
            self.fail()
        except ValueError as exception:
            self.assertEquals(exception.message, CustomerBook.CUSTOMER_NAME_CAN_NOT_BE_EMPTY)
            self.assertTrue(customerBook.isEmpty())

    def testCanNotRemoveNotAddedCustomer(self):
        customerBook = CustomerBook()
        customerBook.addCustomerNamed('Paul McCartney')

        try:
            customerBook.removeCustomerNamed('John Lennon')
            self.fail()
        except KeyError as exception:
            self.assertEquals(exception.message, CustomerBook.INVALID_CUSTOMER_NAME)
            self.assertTrue(customerBook.numberOfCustomers() == 1)
            self.assertTrue(customerBook.includesCustomerNamed('Paul McCartney'))


if __name__ == "__main__":
    unittest.main()
