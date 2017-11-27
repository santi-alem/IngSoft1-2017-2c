class ClientSummary:
    def __init__(self, aClientSales):
        self.clientSales = aClientSales
        self.productsCounts = {}
        self.total = None

    def getProductsCounts(self):
        if self.productsCounts == {}:
            self.initializeProductCount()
        return self.productsCounts

    def initializeProductCount(self):
        self.productsCounts = self.combineProductsCounts([sale.productsCount() for sale in self.clientSales])

    def combineProductsCounts(self, productsCountBySales):
        productsCounts = {}
        for productCount in productsCountBySales:
            self.mergeProductCounts(productsCounts, productCount)

        return productsCounts

    def mergeProductCounts(self, productCount, productCountToAdd):
        for product, value in productCountToAdd.items():
            if product in productCount:
                productCount[product] += value
            else:
                productCount[product] = value

    def getTotal(self):
        if self.total is None:
            self.total = reduce(lambda total, sale: total + sale.total, self.clientSales, 0)
        return self.total

    def getProductsCountsSize(self):
        return len(self.getProductsCounts())
    def numberOf(self, product):
        return self.getProductsCounts()[product]
    def productCountsAreEmpty(self):
        return len(self.productsCounts) == 0
