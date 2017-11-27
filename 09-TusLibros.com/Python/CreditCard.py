class CreditCard:
    def __init__(self, owner, number, expirationDate):
        self.expirationDate = expirationDate
        self.number = number
        self.owner = owner

    def hasExpiredAt(self, date):
        return self.expirationDate.isBefore(date)


class MonthYear:
    def __init__(self, month, year):
        self.year = year
        self.month = month

    def isBefore(self, date):
        return self.year <= date.year and self.month < date.month

    def __eq__(self, other):
        return self.year == other.year and self.month == other.month