import datetime


class RestSession:
    SESSION_TIME_OUT = "Session timed out"

    def __init__(self, clientId, cart, clock):
        self.clock = clock
        self.cart = cart
        self.clientId = clientId
        self.lastTimeUsed = clock.now()

    def addToCart(self, anISNB, quantity):
        self.cart.add(anISNB, quantity)
        return anISNB

    def cartProductCart(self):
        return self.cart.productCount()

    def execute(self, closure):
        return closure()

    def assertSessionHasExpired(self):
        if self.hasSessionExpired():
            raise Exception(RestSession.SESSION_TIME_OUT)

    def hasSessionExpired(self):
        return self.clock.now() - self.lastTimeUsed > datetime.timedelta(minutes=30)

    def __enter__(self):
        self.assertSessionHasExpired()

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.lastTimeUsed = self.clock.now()
