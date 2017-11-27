from Cart import Cart
from Cashier import Cashier
from MerchantProcessor import MerchantProccesorAdapter
from TusLibrosTest import TusLibrosTest


class CashierTest(TusLibrosTest):
    def testCashierCantCheckoutEmptyCart(self):
        aCart = Cart(self.defaultCatalog())
        try:
            salesBook = []
            aCashier = Cashier(MerchantProccesorAdapterSpy(), aCart, self.validCard(), self.aClient(), salesBook)
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertTrue(e.message, Cashier.CAN_NOT_CHECKOUT_EMPTY_CART)

    def testCashierCantCheckOutDatedCard(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()

        try:
            salesBook = []
            aCreditCard = self.alwaysDatedCard()
            aCashier = Cashier(merchantProccesor, aCart, aCreditCard, self.aClient(), salesBook)
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cashier.CREDIT_CARD_EXPIRED)
            self.assertFalse(merchantProccesor.hasDebitCard(aCreditCard))

    def testCashierCalculatesSalesTotalCorectly(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = self.validCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()

        self.assertEquals(sale.total, expectedTotal)

    def testMerchantProccesorReceivesTherRightCreditCardAndAmount(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = self.validCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()

        self.assertEquals(sale.total, expectedTotal)
        self.assertTrue(merchantProccesor.hasDebitCard(card))
        self.assertEquals(merchantProccesor.debitAmount(card), expectedTotal)

    def testCashierAddesSaleToSalesBook(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = self.validCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()
        self.assertEquals(len(salesBook), 1)
        self.assertTrue(sale in salesBook)

    def testCashierCantCheckoutWithStolenCard(self):

        def merchantProccesorStolenCardException(**kwargs):
            raise Exception(MerchantProccesorAdapter.CREDIT_CARD_STOLEN)

        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterStub(merchantProccesorStolenCardException)
        card = self.validCard()
        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        try:
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, MerchantProccesorAdapter.CREDIT_CARD_STOLEN)
            self.assertEquals(salesBook, [])

    def testCashierCantCheckoutWithCardWithNegativeBalance(self):

        def merchantProccesorStolenCardException(**kwargs):
            raise Exception(MerchantProccesorAdapter.CREDIT_CARD_WITHOUT_CREDIT)

        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterStub(merchantProccesorStolenCardException)
        card = self.validCard()
        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        try:
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, MerchantProccesorAdapter.CREDIT_CARD_WITHOUT_CREDIT)
            self.assertEquals(salesBook, [])

    def testCashierCanCheckoutOnlyOnce(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = self.validCard()
        salesBook = []

        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()

        try:
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cashier.CAN_CHECKOUT_ONLY_ONCE)
            self.assertEquals(len(salesBook), 1)
            self.assertTrue(sale in salesBook)


class MerchantProccesorAdapterSpy(MerchantProccesorAdapter):
    def __init__(self):
        self.debitCards = {}

    def debit(self, total, creditCard):
        self.debitCards[creditCard] = total

    def hasDebitCard(self, aCard):
        return aCard in self.debitCards

    def debitAmount(self, aCard):
        return self.debitCards[aCard]


class MerchantProccesorAdapterStub(MerchantProccesorAdapter):
    def __init__(self, toExcexute):
        self.toExcexute = toExcexute

    def debit(self, total, creditCard):
        return self.toExcexute(total=total, creditCard=creditCard)

