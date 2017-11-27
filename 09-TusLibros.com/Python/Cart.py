class Cart:
    _quantityErrorMessage = "Product quantity must be strictly positive"
    _invalidProductErrorMessage = "Product is not sell by supermarket"

    def __init__(self, catalogue):
        self.catalogue = catalogue
        self.itemsList = []

    def add(self, product, quantity):
        self.assertQuantityIsPositiveAndInt(quantity)
        if not self.isValid(product):
            raise Exception(self._invalidProductErrorMessage)
        for _ in range(quantity):
            self.itemsList.append(product)

    def assertQuantityIsPositiveAndInt(self, quantity):
        if quantity < 1 and isinstance(quantity, int):
            raise Exception(self.__class__._quantityErrorMessage)

    def isValid(self, product):
        return product in self.catalogue

    def isEmpty(self):
        return not self.itemsList

    def listCart(self):
        books = set(self.itemsList)
        bookList = []
        for i in books:
            bookList.append((i, self.numberOf(i)))
        return bookList

    def total(self):
        return reduce(lambda sum, item: sum + self.getPriceOf(item[0]) * item[1], self.listCart(), 0)

    def getPriceOf(self, product):
        return self.catalogue[product]

    def productCount(self):
        return {i[0]: i[1] for i in self.listCart()}

    def contains(self, anISBN):
        return anISBN in self.itemsList

    def numberOf(self, aProduct):
        return self.itemsList.count(aProduct)