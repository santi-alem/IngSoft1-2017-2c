class Sale:
    def __init__(self, number, cashier, total):
        self.total = total
        self.cashier = cashier
        self.number = number

    def isOf(self, aClient):
        return self.cashier.client == aClient

    def productsCount(self):
        return self.cashier.productsCount()

