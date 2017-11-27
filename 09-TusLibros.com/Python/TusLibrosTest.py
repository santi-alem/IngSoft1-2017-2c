import datetime
import random
from unittest import TestCase

from Cart import Cart
from CreditCard import CreditCard, MonthYear


class TusLibrosTest(TestCase):
    def aClient(self):
        return "Some Client"

    def defaultCatalog(self):
        return {self.productSellByCompany(): self.productPrice(),
                self.otherProductSellByCompany(): self.otherProductPrice()}

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

    def validCard(self):
        someName = self.validCardOwner()
        cardNumber = self.validCardNumber()
        month_year = self.validCardExpirationDate()

        return CreditCard(someName, cardNumber, month_year)

    def validCardExpirationDate(self):
        return MonthYear((datetime.date.today() + datetime.timedelta(days=365)).year, (
            datetime.date.today() + datetime.timedelta(days=365)).month)

    def validCardNumber(self):
        return "1111222233334444"

    def validCardOwner(self):
        return "Valid Owner Name"

    def alwaysDatedCard(self):
        someName = self.validCardOwner()
        cardNumber = self.validCardNumber()
        today = datetime.date.today()
        month_year = self.datedCardExpirationDate(today)
        return CreditCard(someName, cardNumber, month_year)

    def datedCardExpirationDate(self, today):
        return MonthYear((today - datetime.timedelta(days=today.day + 1)).month, (
            today - datetime.timedelta(days=today.day + 1)).year)


def ValidCard():
    someName = random.choice("aabbcccdde  effgghhii")
    cardNumber = random.randint(1111111111111111, 9999999999999999)
    expirationDate = datetime.date.today() + datetime.timedelta(days=365)
    return CreditCard(someName, cardNumber, MonthYear(expirationDate.year, expirationDate.month))


def AlwaysDatedCard():
    someName = random.choice("aabbcccdde  effgghhii")
    cardNumber = random.randint(1111111111111111, 9999999999999999)
    today = datetime.date.today()
    expirationDate = today - datetime.timedelta(days=today.day + 1)

    return CreditCard(someName, cardNumber, MonthYear(expirationDate.month, expirationDate.year))
