from unittest import TestCase
from .models import Cart, MerchantProccesorAdapterSpy, Cashier, AlwaysDatedCard, ValidCard, \
    MerchantProccesorFailingAdapterStub


class CartTests(TestCase):
    def setUp(self):
        self.defaultCatalog()
        global SALES_BOOKS
        SALES_BOOKS = []

    def defaultCatalog(self):
        global BOOK_CATALOG
        BOOK_CATALOG = {1: 200, 2: 100}

    def testCartStartsEmpty(self):
        aCart = Cart()
        self.assertEquals(aCart.itemsList, [])

    def testCanAddItemsToCart(self):

        aCart = Cart()
        anISBN = 1

        aCart.add(anISBN, 1)

        self.assertEquals(aCart.itemsList, [anISBN])

    def testCanAddMultipleItemsToCart(self):
        aCart = Cart()

        anISBN = 1
        anotherISBN = 2

        aCart.add(anISBN, 1)
        aCart.add(anotherISBN, 1)

        self.assertEquals(aCart.itemsList, [anISBN, anotherISBN])

    def testCanAddMoreThanOneBooks(self):
        aCart = Cart()

        aCart.add(1, 3)

        self.assertTrue(1 in aCart.itemsList)
        self.assertEquals(len(aCart.itemsList), 3)

    def testCantAddlessThanOneBooks(self):
        aCart = Cart()

        anISBN = 3

        try:
            aCart.add(anISBN, 0)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._quantityErrorMessage)

    def testCantAddInvalidBookISBN(self):
        aCart = Cart()
        anISBN = 3

        try:
            aCart.add(anISBN, 1)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._invalidISBNErrorMessage)
            self.assertTrue(aCart.isEmpty())

    def testCartListCorrectly(self):
        aCart = Cart()
        anISBN = 1
        anotherISBN = 2
        aCart.add(anISBN, 2)
        aCart.add(anotherISBN, 4)
        self.assertEqual(aCart.listCart(), [(anISBN, 2), (anotherISBN, 4)])  # ??????????????????????

    def testCashierCantCheckoutEmptyCart(self):
        aCart = Cart()
        aCashier = Cashier(MerchantProccesorAdapterSpy())
        try:
            aCashier.checkout(aCart, ValidCard())
            self.fail()
        except Exception as e:
            self.assertTrue(e.message, "Empty Cart")
            self.assertEquals(SALES_BOOKS, [])

    def createCartWithSomeBooks(self):
        aCart = Cart()
        aCart.add(1, 1)
        aCart.add(2, 1)
        return aCart

    def testCantCheckOutDatedCard(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()
        aCashier = Cashier(merchantProccesor)

        try:
            aCashier.checkout(aCart, AlwaysDatedCard())
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, "Expired Card")
            self.assertEquals(SALES_BOOKS, [])
            self.assertEquals(merchantProccesor.hasCharge, [])

    def testCashierCanCalculateCartTotal(self):
        aCart = self.createCartWithSomeBooks()
        aCashier = Cashier(MerchantProccesorAdapterSpy())

        self.assertEquals(aCashier.getTotal(aCart), 300)

    def testCashierCanDebitTotal(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()
        aCashier = Cashier(merchantProccesor)

        card = ValidCard()

        aCashier.checkout(aCart, card)

        self.assertEquals(merchantProccesor.hasCharge, [(aCashier.getTotal(aCart), card.ccn, card.cco, card.cced)])
        self.assertEquals(SALES_BOOKS, [aCart.listCart()])

    # class MerchantProccesorFailingAdapterStub(MerchantProccesorAdapter):

    def testCashierCantCheckoutWithStolenCard(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorFailingAdapterStub(
            "Stolen Card")  # Tira excepcion con el mensaje que debe responder .
        aCashier = Cashier(merchantProccesor)
        card = ValidCard()

        try:
            aCashier.checkout(aCart, card)
            self.fail()
        except:
            self.assertEquals(SALES_BOOKS, [])
            self.assertEquals(merchantProccesor.hasCharge, [])
