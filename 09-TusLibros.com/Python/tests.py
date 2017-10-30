from unittest.case import TestCase

import datetime

BOOK_CATALOG = []


class CreditCard:
    def isValid(self):
        pass


class InvalidCard(CreditCard):
    def isValid(self):
        return False

    @property
    def cced(self):
        oldMonth = datetime.date.today() - datetime.timedelta(days=40)
        return oldMonth


class ValidCard(CreditCard):
    def isValid(self):
        return False

    @property
    def cced(self):
        oldMonth = datetime.date.today() - datetime.timedelta(days=50)
        return oldMonth


class AlwaysDatedCard(CreditCard):
    def isValid(self):
        return True

    @property
    def cced(self):
        oldMonth = datetime.date.today() - datetime.timedelta(days=40)
        return oldMonth


class Cart:
    _quantityErrorMessage = "Book quantity is less than 1"
    _invalidISBNErrorMessage = "Book has invalid ISBN"

    def __init__(self):
        self.itemsList = []

    def add(self, anISBN, quantity):
        if quantity < 1:
            raise Exception(self.__class__._quantityErrorMessage)
        if not self.isValid(anISBN):
            raise Exception(self._invalidISBNErrorMessage)
        self.itemsList += [anISBN for i in range(quantity)]

    def isValid(self, anISBN):
        return anISBN in BOOK_CATALOG

    def isEmpty(self):
        return not self.itemsList

    def listCart(self):
        books = set(self.itemsList)
        bookList = []
        for i in books:
            bookList.append((i, self.itemsList.count(i)))
        return bookList


class Cashier(object):
    def checkout(self, aCart, aCreditCard):
        if aCart.isEmpty():
            raise Exception("Empty Cart")
        if not aCreditCard.isValid():
            raise Exception("Invalid Card")
        if self.hasExpired(aCreditCard):
            raise Exception("Dated Card")
        self.debit(aCart, aCreditCard)

    def hasExpired(self, aCreditCard):
        today = datetime.date.today()
        thisMonth = datetime.date(today.year, today.month, 1)
        return aCreditCard.cced < thisMonth

    def debit(self, aCart, aCreditCard):
        pass

    def register(self, aCart, aCreditCard):
        pass


class CartTests(TestCase):
    def setUp(self):
        global BOOK_CATALOG
        BOOK_CATALOG = [1, 2]

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
        aCashier = Cashier()
        try:
            aCashier.checkout(aCart, ValidCard())
            self.fail()
        except Exception as e:
            self.assertTrue(e.message, "Empty Cart")

    def notEmptyCart(self):
        aCart = Cart()
        aCart.add(1, 1)
        return aCart

    def test08(self):
        aCart = self.notEmptyCart()
        aCashier = Cashier()

        try:
            aCashier.checkout(aCart, InvalidCard())
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, "Invalid Card")

    def testCantCheckOutDatedCard(self):
        aCart = self.notEmptyCart()
        aCashier = Cashier()
        try:
            aCashier.checkout(aCart, AlwaysDatedCard())
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, "Dated Card")