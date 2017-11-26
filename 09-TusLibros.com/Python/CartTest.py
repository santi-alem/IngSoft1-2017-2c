from unittest.case import TestCase

from Cart import Cart


class CartTests(TestCase):
    def defaultCatalog(self):
        return {1: 200, 2: 100}

    def testCartStartsEmpty(self):
        aCart = Cart(self.defaultCatalog())
        self.assertEquals(aCart.itemsList, [])

    def testCanAddItemsToCart(self):

        aCart = Cart(self.defaultCatalog())
        anISBN = 1

        aCart.add(anISBN, 1)

        self.assertEquals(aCart.itemsList, [anISBN])

    def testCanAddMultipleItemsToCart(self):
        aCart = Cart(self.defaultCatalog())

        anISBN = 1
        anotherISBN = 2

        aCart.add(anISBN, 1)
        aCart.add(anotherISBN, 1)

        self.assertEquals(aCart.itemsList, [anISBN, anotherISBN])

    def testCanAddMoreThanOneBooks(self):
        aCart = Cart(self.defaultCatalog())

        aCart.add(1, 3)

        self.assertTrue(1 in aCart.itemsList)
        self.assertEquals(len(aCart.itemsList), 3)

    def testCantAddlessThanOneBooks(self):
        aCart = Cart(self.defaultCatalog())

        anISBN = 3

        try:
            aCart.add(anISBN, 0)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._quantityErrorMessage)

    def testCantAddInvalidBookISBN(self):
        aCart = Cart(self.defaultCatalog())
        anISBN = 3

        try:
            aCart.add(anISBN, 1)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._invalidProductErrorMessage)
            self.assertTrue(aCart.isEmpty())

    def testCartListCorrectly(self):
        aCart = Cart(self.defaultCatalog())
        anISBN = 1
        anotherISBN = 2
        aCart.add(anISBN, 2)
        aCart.add(anotherISBN, 4)
        self.assertEqual(aCart.listCart(), [(anISBN, 2), (anotherISBN, 4)])  # ??????????????????????
