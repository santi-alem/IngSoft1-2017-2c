import datetime
import uuid

from Cart import Cart
from Cashier import Cashier
from ClientSummary import ClientSummary
from CreditCard import CreditCard
from RestSession import RestSession


class UserException(Exception):
    message = "Wrong username or password"


class CartDoesNotExist(Exception):
    message = "Cart does not exist"


class Interface:
    INVALID_CART_ID = "Invalid cart id"
    SESSION_TIMED_OUT = "Session timed out"

    def __init__(self, authenticationSystem, salesBook, sessions, bookCatalogue, merchantProcessor, clock):
        self.clock = clock
        self.merchantProcessor = merchantProcessor
        self.bookCatalogue = bookCatalogue
        self.sessions = sessions
        self.salesBook = salesBook
        self.authenticationSystem = authenticationSystem

    def getSession(self, cartID):
        if cartID not in self.sessions:
            raise Exception(Interface.INVALID_CART_ID)

        session = self.sessions[cartID]

        return session

    def getCartUser(self, cartID):
        return self.sessions[cartID][2]

    def now(self):
        return datetime.datetime.now()

    def createCart(self, username, password):
        self.authenticate(username, password)
        cartID = self.createID()
        self.sessions[cartID] = RestSession(username, Cart(self.bookCatalogue), self.clock)
        return cartID

    def listCart(self, cartID):
        session = self.getSession(cartID)
        with session:
            return session.cart.productCount()

    def authenticate(self, username, password):
        self.authenticationSystem.authenticate(username, password)

    def createID(self):
        return uuid.uuid4()

    def addToCart(self, cartID, anISBN, quantity):
        session = self.getSession(cartID)
        with session:
            session.cart.add(anISBN, quantity)

    def checkout(self, cartID, user, aCardOwner, aCardNumber, aCardExpirationDate):
        session = self.getSession(cartID)
        with session:
            cart = session.cart
            aCreditCard = CreditCard(aCardOwner, aCardNumber, aCardExpirationDate)
            cashier = Cashier(self.merchantProcessor, cart, aCreditCard, user, self.salesBook)
            cashier.checkout()

    def listUserPurchases(self, username, password):
        self.authenticate(username, password)
        purchases = filter(lambda sale: sale.isOf(username), self.salesBook)

        return ClientSummary(purchases)


class ExpiredCartError(Exception):
    message = "This Cart Has Expired"
