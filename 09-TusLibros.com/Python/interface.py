import datetime
import uuid
from unittest import TestCase

from models import Cart, Cashier, ValidCard, AlwaysDatedCard, CreditCardError, \
    MerchantProccesorAdapterStub


class UserException(Exception):
    message = "Wrong username or password"


class CartDoesNotExist(Exception):
    message = "Cart does not exist"


class Interface:
    def __init__(self, users, salesBook, carts, bookCatalogue, merchantProcessor):
        self.merchantProcessor = merchantProcessor
        self.bookCatalogue = bookCatalogue
        self.carts = carts
        self.salesBook = salesBook
        self.users = users

    def getCart(self, cartID):
        # Levanto esta excepcion por que es un poco mas declarativa que el key error de python
        if cartID not in self.carts:
            raise CartDoesNotExist()

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
        cashier = Cashier(self.merchantProcessor, cart, aCreditCard)
        cashier.checkout()
        username = self.getCartUser(cartID)
        self.salesBook[username] = self.salesBook[username] + [cashier] if username in self.salesBook else [cashier]

    def listPurchases(self, username, password):
        purchases = {}
        if self.authenticate(username, password) and username in self.salesBook:
            for purchase in self.salesBook[username]:
                for item in purchase.cart.listCart():
                    purchases[item[0]] = purchases[item[0]] + item[1] if item[0] in purchases else item[1]
        return purchases


class ExpiredCartError(Exception):
    message = "This Cart Has Expired"


class InterfaceTest(TestCase):
    def defaultlCatalogue(self):
        return {"1234": 100, "4321": 200}

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
        return Interface(users, salesBook, cartList, catalogue, MerchantProccesorAdapterStub(lambda: None))

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

    def testInterfaceCantAddToExpiredCart(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"
        interface.now = lambda: datetime.datetime.now() + datetime.timedelta(minutes=30)
        try:
            interface.addToCart(aCart, anISBN, quantity=2)
            self.fail()
        except ExpiredCartError as e:
            self.assertEquals(e.message, "This Cart Has Expired")

    def testInterfaceCantCheckoutExpiredCart(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"
        interface.addToCart(aCart, anISBN, quantity=2)
        interface.now = lambda: datetime.datetime.now() + datetime.timedelta(minutes=30)
        try:
            interface.checkout(aCart, ValidCard())
            self.fail()
        except ExpiredCartError as e:
            self.assertEquals(e.message, "This Cart Has Expired")

    def testInterfaceAddsPurchaseAfterCheckOut(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        interface.checkout(aCart, ValidCard())

        self.assertEquals(interface.listPurchases("anUser", "123"), {anISBN: 3})

    def testInterfaceCanListUsersMultiplePurchases(self):
        interface = self.defaultInterface()

        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"
        anotherISBN = "4321"

        interface.addToCart(aCart, anISBN, quantity=3)
        interface.checkout(aCart, ValidCard())

        aCart = interface.createCart("anUser", "123")
        interface.addToCart(aCart, anISBN, quantity=3)
        interface.addToCart(aCart, anotherISBN, quantity=2)
        interface.checkout(aCart, ValidCard())

        self.assertEquals(interface.listPurchases("anUser", "123"), {anISBN: 6, anotherISBN: 2})

    def testInterfaceDoenstDoAnythingIfCheckoutFails(self):
        interface = self.defaultInterface()
        aCart = interface.createCart("anUser", "123")
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        try:
            ##Una de las maneras que tengo de hacer que falle desde afuera el checkout
            interface.checkout(aCart, AlwaysDatedCard())
        except CreditCardError:
            self.assertEquals(interface.listPurchases("anUser", "123"), {})

    def testInterfaceFailsWhenAddingToCartThatDoenstExists(self):
        interface = self.defaultInterface()
        aFakeCart = "fakeCart"
        anISBN = "1234"

        try:
            interface.addToCart(aFakeCart, anISBN, quantity=3)
            self.fail()
        except CartDoesNotExist:
            self.assertEquals(interface.listPurchases("anUser", "123"), {})

    def testInterfaceFailsWhenCheckingOutCartThatDoenstExists(self):
        interface = self.defaultInterface()
        aFakeCart = "fakeCart"
        try:
            interface.checkout(aFakeCart, ValidCard())
            self.fail()
        except CartDoesNotExist:
            self.assertEquals(interface.listPurchases("anUser", "123"), {})
