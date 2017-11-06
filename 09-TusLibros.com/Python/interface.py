import datetime
import random
from unittest import TestCase

import models
from models import Cart

USERS = {}
CARTS = {}


class UserException(Exception):
    message = "Wrong username or password"


##???
class InterfaceAdapter:
    def now(self):
        return datetime.datetime.now()

    def createCart(self, username, password):
        if self.authenticate(username, password):
            cartID = self.createID()
            CARTS[cartID] = (Cart(), self.now())
            return cartID

    def listCart(self, cartID):
        return CARTS[cartID][0].listCart()

    def authenticate(self, username, password):
        if username not in USERS or USERS[username] != password:
            raise UserException
        return True

    def createID(self):
        return random.randint(1, 999999999999999999999999999999999999999)

    def addToCart(self, cartID, anISBN, quantity):
        CARTS[cartID][0].add(anISBN, quantity)


class InterfaceTest(TestCase):
    def setUp(self):
        global USERS
        USERS = {"anUser": "123"}

        global CARTS
        CARTS = {}

        models.BOOK_CATALOG = {"1234": 100}

    def testInterfaceFailsToCreateCartWhenUserDoesntExist(self):
        interface = InterfaceAdapter()
        fakeUsername = "aaaaaaa"
        somePassword = "aasssdd"

        try:
            interface.createCart(fakeUsername, somePassword)
            self.fail()
        except UserException:
            self.assertEquals(CARTS, {})

    def testInterfaceCreatesEmptyCart(self):
        interface = InterfaceAdapter()

        cart_ID = interface.createCart("anUser", "123")

        self.assertTrue(cart_ID)
        self.assertEquals(interface.listCart(cart_ID), [])

    def testCanAddBooks(self):
        interface = InterfaceAdapter()
        cartID = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(cartID, anISBN, quantity=1)

        self.assertEqual(interface.listCart(cartID), [(anISBN, 1)])

    def testTwoCartsWithDifferentIDSAreDifferent(self):
        self.assertTrue(False)
        #
        # @unittest.skip
        # def testCantUseExpiredCart(self):
        #     self.assertTrue(False)
