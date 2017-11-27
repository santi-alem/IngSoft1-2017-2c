from unittest import TestCase

from Cart import Cart


class TusLibrosTest(TestCase):
    def aClient(self):
        return "Some Client"

    def defaultCatalog(self):
        return {self.productSellByCompany(): self.productPrice(), self.otherProductSellByCompany(): self.otherProductPrice()}

    def otherProductPrice(self):
        return 100.5

    def productPrice(self):
        return 202

    def otherProductSellByCompany(self):
        return "4321"

    def productSellByCompany(self):
        return "1234"

    def invalidProduct(self):
        return 3

    def createCartWithSomeBooks(self):
        aCart = Cart(self.defaultCatalog())
        aCart.add(self.productSellByCompany(), 1)
        aCart.add(self.otherProductSellByCompany(), 1)
        return aCart