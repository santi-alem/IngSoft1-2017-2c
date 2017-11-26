import datetime


class Clock:
    def now(self):
        pass


class ManualClock(Clock):
    def __init__(self, now=None):
        self._now = now or datetime.datetime.now()

    def advance(self, minutes=0, hours=0, days=0):
        self._now = self._now + datetime.timedelta(minutes=minutes, hours=hours, days=days)

    def now(self):
        return self._now
