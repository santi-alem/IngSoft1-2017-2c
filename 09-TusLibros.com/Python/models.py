import datetime
import random
from unittest.case import TestCase

## Todo: Cambiar esto y pasarlo por catalogo
BOOK_CATALOG = {}
SALES_BOOKS = []


## Cambiar Date
class CreditCard:
    def __init__(self, cco, ccn, cced):
        self.cced = cced
        self.ccn = ccn
        self.cco = cco

    def hasExpiredAt(self, date):
        return self.cced < date


class CreditCardError(Exception):
    pass


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


class MerchantProccesorAdapter:
    def debit(self, total, creditCardNumber, creditCardExpiration, creditCardOwner):
        pass


class MerchantProccesorAdapterSpy(MerchantProccesorAdapter):
    def __init__(self):
        self.hasCharge = []

    def debit(self, total, creditCardNumber, creditCardExpiration, creditCardOwner):
        self.hasCharge.append((total, creditCardNumber, creditCardExpiration, creditCardOwner))


class MerchantProccesorAdapterStub(MerchantProccesorAdapter):
    def __init__(self, toExcexute):
        self.toExcexute = toExcexute

    def debit(self, total, creditCardNumber, creditCardExpiration, creditCardOwner):
        return self.toExcexute()


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
    def __init__(self, merchantProccesor, aCart, aCreditCard):
        self.creditCard = aCreditCard
        self.cart = aCart
        self.merchantProccesor = merchantProccesor

    def checkout(self):
        if self.cart.isEmpty():
            raise Exception("Empty Cart")

        if self.creditCard.hasExpiredAt(datetime.date.today()):
            raise Exception("Expired Card")

        total = self.getTotal(self.cart)
        self.merchantProccesor.debit(total, self.creditCard.ccn, self.creditCard.cco, self.creditCard.cced)
        self.registerPurchase(self.cart)

    def getTotal(self, aCart):
        return reduce(lambda sum, item: sum + self.getPrice(item[0]) * item[1], aCart.listCart(), 0)

    def getPrice(self, ISBN):
        return BOOK_CATALOG[ISBN]

    def registerPurchase(self, aCart):
        SALES_BOOKS.append(aCart)


## TODO: Separar y Hacer una SuperClase en Comun . Usar Factories
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
        aCashier = Cashier(MerchantProccesorAdapterSpy(), aCart, ValidCard())
        try:
            aCashier.checkout()
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
        aCashier = Cashier(merchantProccesor, aCart, AlwaysDatedCard())

        try:
            aCashier.checkout()
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, "Expired Card")
            self.assertEquals(SALES_BOOKS, [])
            self.assertEquals(merchantProccesor.hasCharge, [])

    def testCashierCanDebitTotal(self):
        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterSpy()
        card = ValidCard()

        aCashier = Cashier(merchantProccesor, aCart, card)

        aCashier.checkout()

        self.assertEquals(merchantProccesor.hasCharge, [(aCashier.getTotal(aCart), card.ccn, card.cco, card.cced)])
        self.assertEquals(SALES_BOOKS, [aCart])

    def testCashierCantCheckoutWithStolenCard(self):

        def merchantProccesorStolenCardException():
            raise CreditCardError("Stolen Card")

        aCart = self.createCartWithSomeBooks()
        merchantProccesor = MerchantProccesorAdapterStub(merchantProccesorStolenCardException)
        card = ValidCard()
        aCashier = Cashier(merchantProccesor, aCart, card)

        try:
            aCashier.checkout()
            self.fail()
        except CreditCardError as e:
            self.assertEquals(e.message, "Stolen Card")
            self.assertEquals(SALES_BOOKS, [])
