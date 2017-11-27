from unittest.case import TestCase

from Cart import Cart
from TusLibrosTest import TusLibrosTest


class CartTests(TusLibrosTest):


    def testCartAreCreatedEmpty(self):
        aCart = Cart(self.defaultCatalog())
        self.assertTrue(aCart.isEmpty())

    def testCartIsNotEmptyAfterAddingProducts(self):
        aCart = Cart(self.defaultCatalog())
        aProduct = self.productSellByCompany()
        aCart.add(aProduct, self.productSellByCompany())
        self.assertFalse(aCart.isEmpty())

    def testCartContainsAddedIsInCart(self):
        aCart = Cart(self.defaultCatalog())
        aProduct = self.productSellByCompany()
        aCart.add(aProduct, self.productSellByCompany())
        self.assertTrue(aCart.contains(aProduct))

    def testCanAddMultipleItemsToCart(self):
        aCart = Cart(self.defaultCatalog())

        aProduct = self.productSellByCompany()
        anotherProduct = self.otherProductSellByCompany()

        aCart.add(aProduct, 1)
        aCart.add(anotherProduct, 1)

        self.assertTrue(aCart.contains(aProduct))
        self.assertTrue(aCart.contains(anotherProduct))

    def testCanAddMoreThanOneProduct(self):
        aCart = Cart(self.defaultCatalog())

        aProduct = self.productSellByCompany()
        aCart.add(aProduct, 3)

        self.assertTrue(1 in aCart.itemsList)
        self.assertEquals(aCart.numberOf(aProduct), 3)

    def testCanAddMoreThanOneProductMultipleTimes(self):
        aCart = Cart(self.defaultCatalog())

        aProduct = self.productSellByCompany()
        aCart.add(aProduct, 1)
        aCart.add(aProduct, 3)

        self.assertTrue(1 in aCart.itemsList)
        self.assertEquals(aCart.numberOf(aProduct), 4)

    def testCantAddlessThanOneProduct(self):
        aCart = Cart(self.defaultCatalog())

        someProduct = 2

        try:
            aCart.add(someProduct, 0)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._quantityErrorMessage)

    def testCantAddProductNotSellByCompany(self):
        aCart = Cart(self.defaultCatalog())
        aProduct = self.invalidProduct()

        try:
            aCart.add(aProduct, 1)
            self.fail()
        except Exception as e:
            self.assertEquals(e.message, Cart._invalidProductErrorMessage)
            self.assertTrue(aCart.isEmpty())
