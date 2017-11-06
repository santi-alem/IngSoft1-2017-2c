import datetime
import uuid
from unittest import TestCase

import models
from models import Cart, Cashier, MerchantProccesorAdapter, ValidCard, AlwaysDatedCard


class UserException(Exception):
    message = "Wrong username or password"


class InterfaceAdapter:
    def __init__(self, users, salesBook, carts, bookCatalogue):
        self.bookCatalogue = bookCatalogue
        self.carts = carts
        self.salesBook = salesBook
        self.users = users

    def getCart(self, cartID):
        cart = self.carts[cartID]

        if self.now() - cart[1] > datetime.timedelta(minutes=30):
            raise ExpiredCartError()
        return cart[0]

    def getCartUser(self, cartID):
        return self.carts[cartID][2]

    def updateCart(self, cartID):
        self.carts[cartID] = (self.carts[cartID][0], self.now(), self.carts[cartID][2])

    def now(self):
        return datetime.datetime.now()

    def createCart(self, username, password):
        if self.authenticate(username, password):
            cartID = self.createID()
            self.carts[cartID] = (Cart(self.bookCatalogue), self.now(), username)
            return cartID

    def listCart(self, cartID):
        return self.getCart(cartID).listCart()

    def authenticate(self, username, password):
        if username not in self.users or self.users[username] != password:
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
        self.salesBook[username] = self.salesBook[username].append(cashier) if username in self.salesBook else [
            cashier]

    def listPurchases(self, username, password):
        if self.authenticate(username, password):
            return [purchase.cart.listCart() for purchase in
                    self.salesBook[username]] if username in self.salesBook else []


class ExpiredCartError(Exception):
    message = "This Cart Has Expired"


class InterfaceTest(TestCase):
    def defaultlCatalogue(self):
        return {"1234": 100}

    def defaultSalesBook(self):
        return {}

    def defaultCarts(self):
        return {}

    def defaultUsers(self):
        return {"anUser": "123"}

    def defaultInterface(self):
        salesBook = self.defaultSalesBook()
        catalogue = self.defaultlCatalogue()
        cartList = self.defaultCarts()
        users = self.defaultUsers()
        return InterfaceAdapter(users, salesBook, cartList, catalogue)

    def testInterfaceFailsToCreateCartWhenUserDoesntExist(self):
        interface = self.defaultInterface()
        fakeUsername = "aaaaaaa"
        somePassword = "aasssdd"

        try:
            interface.createCart(fakeUsername, somePassword)
            self.fail()
        except UserException:
            self.assertEquals(interface.carts, {})

    def testInterfaceCreatesEmptyCart(self):
        interface = self.defaultInterface()

        cart_ID = interface.createCart("anUser", "123")

        self.assertTrue(cart_ID)
        self.assertEquals(interface.listCart(cart_ID), [])

    def testCanAddBooks(self):
        interface = self.defaultInterface()
        cartID = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(cartID, anISBN, quantity=1)

        self.assertEqual(interface.listCart(cartID), [(anISBN, 1)])

    def testTwoCartsWithDifferentIDSAreDifferent(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anotherCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=2)

        self.assertNotEquals(aCart, anotherCart)
        self.assertEqual(interface.listCart(aCart), [(anISBN, 2)])
        self.assertEqual(interface.listCart(anotherCart), [])

    def testInterfaceCantIntercatWithExpiredCart(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.now = lambda: datetime.datetime.now() + datetime.timedelta(minutes=30)
        try:
            interface.addToCart(aCart, anISBN, quantity=2)
            self.fail()
        except ExpiredCartError as e:
            self.assertEquals(e.message, "This Cart Has Expired")

    def testInterfaceAddsPurchaseAfterCheckOut(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)

        interface.checkout(aCart, ValidCard())

        self.assertEquals(interface.listPurchases("anUser", "123"), [[(anISBN, 3)]])

    def testInterfaceDoenstDoAnythingIfCheckoutFails(self):
        interface = self.defaultInterface()
        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        try:
            interface.checkout(aCart,
                               AlwaysDatedCard())  ##Una de las maneras que tengo de hacer que falle desde afuera el checkout
        except Exception as error:
            self.assertEquals(interface.listPurchases("anUser", "123"), [])
