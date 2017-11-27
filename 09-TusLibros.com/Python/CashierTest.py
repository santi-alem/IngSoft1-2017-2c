import datetime
import random

from Cart import Cart
from Cashier import Cashier
from CreditCard import CreditCard
from MerchantProcessor import MerchantProccesorAdapter
from TusLibrosTest import TusLibrosTest


class CashierTest(TusLibrosTest):
    def testCashierCantCheckoutEmptyCart(self):
        aCart = Cart(self.defaultCatalog())
        try:
            salesBook = []
            aCashier = Cashier(MerchantProccesorAdapterSpy(), aCart, ValidCard(), self.aClient(), salesBook)
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertTrue(e.message, Cashier.CAN_NOT_CHECKOUT_EMPTY_CART)

    def testCashierCantCheckOutDatedCard(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()

        try:
            salesBook = []
            aCashier = Cashier(merchantProccesor, aCart, AlwaysDatedCard(), self.aClient(), salesBook)
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cashier.CREDIT_CARD_EXPIRED)
            self.assertEquals(merchantProccesor.hasCharge, [])

    def testCashierCalculatesSalesTotalCorectly(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = ValidCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()

        self.assertEquals(sale.getTotal, expectedTotal)

    def testMerchantProccesorReceivesTherRightCreditCardAndAmount(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = ValidCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()

        self.assertEquals(sale.getTotal, expectedTotal)
        self.assertEquals(merchantProccesor.hasCharge, [(expectedTotal, card)])

    def testCashierAddesSaleToSalesBook(self):
        aCart = self.createCartWithSomeBooks()

        expectedTotal = self.productPrice() + self.otherProductPrice()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = ValidCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        sale = aCashier.checkout()
        self.assertEquals(len(salesBook), 1)
        self.assertTrue(sale in salesBook)

    def testCashierCantCheckoutWithStolenCard(self):

        def merchantProccesorStolenCardException():
            raise Exception(MerchantProccesorAdapter.CREDIT_CARD_STOLEN)

        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterStub(merchantProccesorStolenCardException)
        card = ValidCard()
        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        try:
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, MerchantProccesorAdapter.CREDIT_CARD_STOLEN)
            self.assertEquals(salesBook, [])

    def testCashierCantCheckoutWithCardWithNegativeBalance(self):

        def merchantProccesorStolenCardException():
            raise Exception(MerchantProccesorAdapter.CREDIT_CARD_WITHOUT_CREDIT)

        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterStub(merchantProccesorStolenCardException)
        card = ValidCard()
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
        card = ValidCard()
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


def ValidCard():
    someName = random.choice("aabbcccdde  effgghhii")
    cardNumber = random.randint(1111111111111111, 9999999999999999)
    expirationDate = datetime.date.today() + datetime.timedelta(days=365)
    return CreditCard(someName, cardNumber, expirationDate)


def AlwaysDatedCard():
    someName = random.choice("aabbcccdde  effgghhii")
    cardNumber = random.randint(1111111111111111, 9999999999999999)
    today = datetime.date.today()
    expirationDate = today - datetime.timedelta(days=today.day + 1)

    return CreditCard(someName, cardNumber, expirationDate)


class MerchantProccesorAdapterSpy(MerchantProccesorAdapter):
    def __init__(self):
        self.hasCharge = []

    def debit(self, total, creditCard):
        self.hasCharge.append((total, creditCard))


class MerchantProccesorAdapterStub(MerchantProccesorAdapter):
    def __init__(self, toExcexute):
        self.toExcexute = toExcexute

    def debit(self, total, creditCard):
        return self.toExcexute()
