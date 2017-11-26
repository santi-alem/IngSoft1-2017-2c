class CreditCard:
    def __init__(self, owner, number, expirationDate):
        self.expirationDate = expirationDate
        self.number = number
        self.owner = owner

    def hasExpiredAt(self, date):
        return self.expirationDate < date

