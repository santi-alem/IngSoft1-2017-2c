from AuthenticationSystem import AuthenticationSystem
from CashierTest import MerchantProccesorAdapterStub
from Clock import ManualClock
from RestInterface import Interface
from RestSession import RestSession
from TusLibrosTest import TusLibrosTest


class AthenticationSystemSimulator(AuthenticationSystem):
    def __init__(self, validUsers):
        self.validUsers = validUsers

    def authenticate(self, username, password):
        if not (username in self.validUsers and self.validUsers[username] == password):
            raise Exception(AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)


class InterfaceTest(TusLibrosTest):
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

    def invalidPassword(self):
        return "INVALID_PASSWORD"

    def invalidUser(self):
        return "INVALID_USER"

    def defaultInterface(self):
        def merchantProccesorDoesNothing(**kwargs):
            pass

        salesBook = self.defaultSalesBook()
        catalogue = self.defaultCatalog()
        cartList = self.defaultCarts()
        authenticationSystem = self.defaultAuthenticationSystem()
        clock = ManualClock()
        return Interface(authenticationSystem, salesBook, cartList, catalogue,
                         MerchantProccesorAdapterStub(merchantProccesorDoesNothing), clock)

    def testcannotCreateCartWithInvalidClientId(self):
        interface = self.defaultInterface()
        fakeUsername = "aaaaaaa"
        somePassword = "aasssdd"

        try:
            interface.createCart(fakeUsername, somePassword)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)

    def testcannotCreateCartWithInvalidPassword(self):
        interface = self.defaultInterface()
        username = self.validUsername()
        somePassword = "aasssdd"

        try:
            interface.createCart(username, somePassword)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)

    def testCreatesEmptyCart(self):
        interface = self.defaultInterface()

        cart_ID = interface.createCart(self.validUsername(), self.validPassword())

        self.assertTrue(cart_ID)
        self.assertEquals(len(interface.listCart(cart_ID)), 0)

    def testCantAddBooksWithInvalidId(self):
        interface = self.defaultInterface()
        cartID = interface.createCart(self.validUsername(), self.validPassword())
        anISBN = "1234"
        try:
            interface.addToCart(self.invalidID(), anISBN, quantity=1)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Interface.INVALID_CART_ID)
            self.assertEqual(len(interface.listCart(cartID)), 0)

    def testAddedBooksAreListed(self):
        interface = self.defaultInterface()
        cartID = interface.createCart(self.validUsername(), self.validPassword())
        anISBN = "1234"

        interface.addToCart(cartID, anISBN, quantity=1)

        self.assertTrue(anISBN in interface.listCart(cartID))

    def testTwoCartsWithDifferentIDSAreDifferent(self):
        interface = self.defaultInterface()

        username = self.validUsername()
        password = self.validPassword()
        aCart = interface.createCart(username, password)
        anotherCart = interface.createCart(username, password)
        anISBN = "1234"

        interface.addToCart(aCart, anISBN, quantity=2)

        self.assertNotEquals(aCart, anotherCart)
        self.assertEqual(len(interface.listCart(aCart)), 1)
        self.assertEqual(len(interface.listCart(anotherCart)), 0)

    def testCantCheckOutCartWithInvalidCartID(self):
        interface = self.defaultInterface()
        user = self.validUsername()

        try:
            interface.checkout(self.invalidID(), user, self.validCardOwner(), self.validCardNumber(),
                               self.validCardExpirationDate())
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Interface.INVALID_CART_ID)
            client_summary = interface.listUserPurchases(user, self.validPassword())
            self.assertEquals(client_summary.getProductsCounts(), {})

    def testCantListPurchasesWithInvalidClientID(self):
        interface = self.defaultInterface()
        try:
            interface.listUserPurchases(self.invalidUser(), self.validPassword())
        except Exception as e:
            self.assertEquals(e.message, AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)

    def testCantListPurchasesWithInvalidPassword(self):
        interface = self.defaultInterface()
        try:
            interface.listUserPurchases(self.validUsername(), self.invalidPassword())
        except Exception as e:
            self.assertEquals(e.message, AuthenticationSystem.INVALID_CLIENTID_OR_PASSWORD)

    def testPurchasesAreEmptyIfClientHasntPurchaseYet(self):
        interface = self.defaultInterface()
        purchase = interface.listUserPurchases(self.validUsername(), self.validPassword())
        purchase = interface.listUserPurchases(self.validUsername(), self.validPassword())
        self.assertAlmostEqual(purchase.getTotal(), 0, 0.1)
        self.assertTrue(purchase.productCountsAreEmpty())

    def testcheckOutAffectsClientPurchases(self):
        interface = self.defaultInterface()

        an_user = self.validUsername()
        aCart = interface.createCart(an_user, self.validPassword())
        anISBN = self.productSellByCompany()

        quantity = 3
        cart = interface.addToCart(aCart, anISBN, quantity)
        interface.checkout(aCart, an_user, self.validCardOwner(), self.validCardNumber(),
                           self.validCardExpirationDate())

        clientSummary = interface.listUserPurchases(an_user, self.validPassword())
        self.assertEquals(clientSummary.getProductsCountsSize(), 1)
        self.assertEquals(clientSummary.numberOf(anISBN), quantity)
        self.assertEquals(clientSummary.getTotal(), quantity * self.productPrice())

    def testCantAddToCartWithInvalidClientID(self):
        interface = self.defaultInterface()
        anISBN = "1234"

        try:
            interface.addToCart(self.invalidID(), anISBN, quantity=3)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Interface.INVALID_CART_ID)
            client_summary = interface.listUserPurchases(self.validUsername(), self.validPassword())
            self.assertEquals(client_summary.getProductsCounts(), {})

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

    def testInterfaceCantListExpiredCart(self):
        interface = self.defaultInterface()

        aCart = interface.createCart(self.validUsername(), self.validPassword())
        anISBN = "1234"
        interface.addToCart(aCart, anISBN, quantity=2)
        interface.clock.advance(minutes=31)
        try:
            interface.listCart(aCart)
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
            interface.checkout(aCart, an_user, self.validCardOwner(), self.validCardNumber(),
                               self.validCardExpirationDate())
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, RestSession.SESSION_TIME_OUT)

    def testMerchantProccesorReceivesTheRightCreditCardAndTotal(self):
        merchantProccesor = MerchantProccesorAdapterStub(self.assertValidCreditCardAndExpectedTotal)
        interface = Interface(self.defaultAuthenticationSystem(), self.defaultSalesBook(), {}, self.defaultCatalog(),
                              merchantProccesor, ManualClock())
        an_user = self.validUsername()
        aCart = interface.createCart(an_user, self.validPassword())
        anISBN = "1234"
        interface.addToCart(aCart, anISBN, quantity=2)

        interface.checkout(aCart, an_user, self.validCardOwner(), self.validCardNumber(),
                           self.validCardExpirationDate())

    def assertValidCreditCardAndExpectedTotal(self, creditCard, total):
        self.assertEquals(creditCard.owner, self.validCardOwner())
        self.assertEquals(creditCard.expirationDate, self.validCardExpirationDate())
        self.assertEquals(creditCard.number, self.validCardNumber())
        self.assertEquals(self.productPrice() * 2, total)

    def invalidID(self):
        return "1111"
