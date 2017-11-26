import datetime
import uuid

from Sale import Sale


class Cashier(object):
    CAN_NOT_CHECKOUT_EMPTY_CART = "Can not checkout an empty cart"
    CREDIT_CARD_EXPIRED = "Credit card is expired"
    CAN_CHECKOUT_ONLY_ONCE = "Can checkout only once"

    def __init__(self, merchantProccesor, aCart, aCreditCard, aClient, salesBook):
        self.salesBook = salesBook
        self.client = aClient

        self.assertCartIsNotEmpty(aCart)
        self.assertCreditCardIsNotExpired(aCreditCard)

        self.creditCard = aCreditCard
        self.cart = aCart
        self.merchantProccesor = merchantProccesor
        self.debited = False
        self.hasCheckOut = False

    def checkout(self):
        total = self.total()
        self.merchantProccesor.debit(total, self.creditCard)
        self.hasCheckOut = True
        sale = Sale(uuid.uuid4(), self, total)
        self.salesBook.append(sale)
        return sale

    def total(self):
        return self.cart.total()

    def productsCount(self):
        return self.cart.productCount()

    def assertCreditCardIsNotExpired(self, aCreditCard):
        if aCreditCard.hasExpiredAt(datetime.date.today()):
            raise Exception(Cashier.CREDIT_CARD_EXPIRED)

    def assertCartIsNotEmpty(self, aCart):
        if aCart.isEmpty():
            raise Exception(Cashier.CAN_NOT_CHECKOUT_EMPTY_CART)