import datetime
import random
from unittest import TestCase

from Cart import Cart
from Cashier import Cashier
from CreditCard import CreditCard
from MerchantProcessor import MerchantProccesorAdapter


class CashierTest(TestCase):
    def defaultCatalog(self):
        return {1: 200, 2: 100}

    def aClient(self):
        return "Some Client"

    def createCartWithSomeBooks(self):
        aCart = Cart(self.defaultCatalog())
        aCart.add(1, 1)
        aCart.add(2, 1)
        return aCart

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

    def testCashierCanDebitTotal(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = ValidCard()

        salesBook = []
        aCashier = Cashier(merchantProccesor, aCart, card, self.aClient(), salesBook)

        aCashier.checkout()

        self.assertEquals(merchantProccesor.hasCharge,
                          [(aCart.total(), card)])

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
