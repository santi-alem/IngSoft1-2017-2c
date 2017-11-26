from unittest import TestCase

from AuthenticationSystem import AuthenticationSystem
from CashierTest import MerchantProccesorAdapterStub, ValidCard, AlwaysDatedCard
from Clock import ManualClock
from RestInterface import Interface
from RestSession import RestSession


class AthenticationSystemSimulator(AuthenticationSystem):
    def __init__(self, validUsers):
        self.validUsers = validUsers

    def authenticate(self, username, password):
        if not (username in self.validUsers and self.validUsers[username] == password):
            raise Exception(AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)


class InterfaceTest(TestCase):
    def defaultlCatalogue(self):
        return {"1234": 100.0, "4321": 200.0}

    def defaultSalesBook(self):
        return []

    def defaultCarts(self):
        return {}

    def defaultAuthenticationSystem(self):
        validUsers = {self.validUsername(): self.validPassword()}
        return AthenticationSystemSimulator(validUsers)

    def validUsername(self):
        return "anUser"

    def validPassword(self):
        return "123"

    def defaultInterface(self):
        salesBook = self.defaultSalesBook()
        catalogue = self.defaultlCatalogue()
        cartList = self.defaultCarts()
        authenticationSystem = self.defaultAuthenticationSystem()
        clock = ManualClock()
        return Interface(authenticationSystem, salesBook, cartList, catalogue,
                         MerchantProccesorAdapterStub(lambda: None), clock)

    def testInterfaceFailsToCreateCartWhenUserDoesntExist(self):
        interface = self.defaultInterface()
        fakeUsername = "aaaaaaa"
        somePassword = "aasssdd"

        try:
            interface.createCart(fakeUsername, somePassword)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)
            self.assertEquals(interface.sessions, {})

    def testInterfaceCreatesEmptyCart(self):
        interface = self.defaultInterface()

        cart_ID = interface.createCart(self.validUsername(), self.validPassword())

        self.assertTrue(cart_ID)
        self.assertEquals(interface.listCart(cart_ID), [])

    def testCanAddBooks(self):
        interface = self.defaultInterface()
        cartID = interface.createCart(self.validUsername(), self.validPassword())
        anISBN = "1234"

        interface.addToCart(cartID, anISBN, quantity=1)

        self.assertEqual(interface.listCart(cartID), [(anISBN, 1)])

    def testTwoCartsWithDifferentIDSAreDifferent(self):
        interface = self.defaultInterface()

        username = self.validUsername()
        password = self.validPassword()
        aCart = interface.createCart(username, password)
        anotherCart = interface.createCart(username, password)
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=2)

        self.assertNotEquals(aCart, anotherCart)
        self.assertEqual(interface.listCart(aCart), [(anISBN, 2)])
        self.assertEqual(interface.listCart(anotherCart), [])

    def testInterfaceCantAddToExpiredCart(self):
        interface = self.defaultInterface()

        aCart = interface.createCart(self.validUsername(), self.validPassword())
        anISBN = "1234"
        interface.clock.advance(minutes=31)
        try:
            interface.addToCart(aCart, anISBN, quantity=2)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, RestSession.SESSION_TIME_OUT)

    def testInterfaceCantCheckoutExpiredCart(self):
        interface = self.defaultInterface()

        an_user = self.validUsername()
        aCart = interface.createCart(an_user, self.validPassword())
        anISBN = "1234"
        interface.addToCart(aCart, anISBN, quantity=2)
        interface.clock.advance(minutes=31)
        try:
            interface.checkout(aCart, ValidCard(), an_user)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, RestSession.SESSION_TIME_OUT)

    def testInterfaceAddsPurchaseAfterCheckOut(self):
        interface = self.defaultInterface()

        an_user = self.validUsername()
        aCart = interface.createCart(an_user, self.validPassword())
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        interface.checkout(aCart, ValidCard(), an_user)

        clientSummary = interface.listUserPurchases(an_user, self.validPassword())
        self.assertEquals(clientSummary.getProductsCounts(), {anISBN: 3})

    def testInterfaceCanListUsersMultiplePurchases(self):
        interface = self.defaultInterface()

        user = self.validUsername()
        aCart = interface.createCart(user, self.validPassword())
        anISBN = "1234"
        anotherISBN = "4321"

        interface.addToCart(aCart, anISBN, quantity=3)
        interface.checkout(aCart, ValidCard(), user)

        aCart = interface.createCart(user, self.validPassword())
        interface.addToCart(aCart, anISBN, quantity=3)
        interface.addToCart(aCart, anotherISBN, quantity=2)
        interface.checkout(aCart, ValidCard(), user)

        clientSummary = interface.listUserPurchases(user, self.validPassword())
        self.assertEquals(clientSummary.getProductsCounts(), {anISBN: 6, anotherISBN: 2})

    def testInterfaceDoenstDoAnythingIfCheckoutFails(self):
        interface = self.defaultInterface()
        user = self.validUsername()
        aCart = interface.createCart(user, self.validPassword())
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=3)
        try:
            interface.checkout(aCart, AlwaysDatedCard(), user)
            self.fail()
        except Exception:
            clientSummary = interface.listUserPurchases(user, self.validPassword())
            self.assertEquals(clientSummary.getProductsCounts(), {})

    def testInterfaceFailsWhenAddingToCartThatDoenstExists(self):
        interface = self.defaultInterface()
        aFakeCart = "fakeCart"
        anISBN = "1234"

        try:
            interface.addToCart(aFakeCart, anISBN, quantity=3)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Interface.INVALID_CART_ID)
            client_summary = interface.listUserPurchases(self.validUsername(), self.validPassword())
            self.assertEquals(client_summary.getProductsCounts(), {})

    def testInterfaceFailsWhenCheckingOutCartThatDoenstExists(self):
        interface = self.defaultInterface()
        aFakeCart = "fakeCart"
        user = self.validUsername()

        try:
            interface.checkout(aFakeCart, ValidCard(), user)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Interface.INVALID_CART_ID)
            client_summary = interface.listUserPurchases(user, self.validPassword())
            self.assertEquals(client_summary.getProductsCounts(), {})
