import datetime
import uuid
from unittest import TestCase

import models
from models import Cart, Cashier, MerchantProccesorAdapter, ValidCard, AlwaysDatedCard

USERS = {}
CARTS = {}
USERS_SALES_BOOK = {}


class UserException(Exception):
    message = "Wrong username or password"


class InterfaceAdapter:
    def getCart(self, cartID):
        cart = CARTS[cartID]

        if self.now() - cart[1] > datetime.timedelta(minutes=30):
            raise ExpiredCartError()
        return cart[0]

    def getCartUser(self, cartID):
        return CARTS[cartID][2]

    def updateCart(self, cartID):
        CARTS[cartID] = (CARTS[cartID][0], self.now(), CARTS[cartID][2])

    def now(self):
        return datetime.datetime.now()

    def createCart(self, username, password):
        if self.authenticate(username, password):
            cartID = self.createID()
            CARTS[cartID] = (Cart(), self.now(), username)
            return cartID

    def listCart(self, cartID):
        return self.getCart(cartID).listCart()

    def authenticate(self, username, password):
        if username not in USERS or USERS[username] != password:
            raise UserException
        return True

    def createID(self):
        return uuid.uuid4()

    def addToCart(self, cartID, anISBN, quantity):
        self.getCart(cartID).add(anISBN, quantity)
        self.updateCart(cartID)

    def checkout(self, cartID, aCreditCard):
        cart = self.getCart(cartID)
        cashier = Cashier(MerchantProccesorAdapter(), cart, aCreditCard)
        cashier.checkout()
        username = self.getCartUser(cartID)
        USERS_SALES_BOOK[username] = USERS_SALES_BOOK[username].append(cashier) if username in USERS_SALES_BOOK else [
            cashier]

    def listPurchases(self, username, password):
        if self.authenticate(username, password):
            return [purchase.cart.listCart() for purchase in
                    USERS_SALES_BOOK[username]] if username in USERS_SALES_BOOK else []


class ExpiredCartError(Exception):
    message = "This Cart Has Expired"


class InterfaceTest(TestCase):
    def setUp(self):
        global USERS
        USERS = {"anUser": "123"}

        global CARTS
        CARTS = {}

        global USERS_SALES_BOOK
        USERS_SALES_BOOK = {}

        models.BOOK_CATALOG = {"1234": 100}
        models.SALES_BOOKS = []

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
        interface = InterfaceAdapter()

        aCart = interface.createCart("anUser", "123")
        anotherCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=2)

        self.assertNotEquals(aCart, anotherCart)
        self.assertEqual(interface.listCart(aCart), [(anISBN, 2)])
        self.assertEqual(interface.listCart(anotherCart), [])

    def testInterfaceCantIntercatWithExpiredCart(self):
        interface = InterfaceAdapter()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.now = lambda: datetime.datetime.now() + datetime.timedelta(minutes=30)
        try:
            interface.addToCart(aCart, anISBN, quantity=2)
            self.fail()
        except ExpiredCartError as e:
            self.assertEquals(e.message, "This Cart Has Expired")

    def testInterfaceAddsPurchaseAfterCheckOut(self):
        interface = InterfaceAdapter()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)

        interface.checkout(aCart, ValidCard())

        self.assertEquals(interface.listPurchases("anUser", "123"), [[(anISBN, 3)]])

    def testInterfaceDoenstDoAnythingIfCheckoutFails(self):
        interface = InterfaceAdapter()
        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        try:
            interface.checkout(aCart, AlwaysDatedCard()) ##La manera que tengo de hacer que falle desde afuera el checkout
        except Exception as error:
            self.assertEquals(interface.listPurchases("anUser", "123"), [])