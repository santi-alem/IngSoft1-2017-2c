from unittest.case import TestCase


class Cart:
    _carts_id = 0
    _quantityErrorMessage = "Book quantity is less than 1"

    def __init__(self):
        self.itemsList = []
        self.id = self.__class__._carts_id
        self.__class__._carts_id += 1

    def add(self, anISBN, quantity):
        if quantity < 1:
            raise Exception(self.__class__._quantityErrorMessage)

        self.itemsList.append((anISBN, quantity))


class CartTests(TestCase):
    def testCartHasID(self):
        aCart = Cart()

        self.assertTrue(aCart.id is not None)

    def testTwoCartsHaveDifferentIds(self):
        aCart = Cart()
        anotherCart = Cart()

        self.assertNotEquals(aCart.id, anotherCart.id)

    def testCanAddItemsToCart(self):
        aCart = Cart()
        anISBN = 1

        aCart.add(anISBN, 1)

        self.assertEquals(aCart.itemsList, [(anISBN, 1)])

    def testCanAddMultipleItemsToCart(self):
        aCart = Cart()
        anISBN = 1
        anotherISBN = 3

        aCart.add(anISBN, 3)
        aCart.add(anotherISBN, 1)

        self.assertEquals(aCart.itemsList, [(anISBN, 3), (anotherISBN, 1)])

    def testCantAddNoBooks(self):
        aCart = Cart()
        anISBN = 1

        try:
            aCart.add(anISBN, 0)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._quantityErrorMessage)

    def testCantAddInvalidBookISBN(self):
        aCart = Cart()
        anISBN = 1

        try:
            aCart.add(anISBN, 0)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message,)
