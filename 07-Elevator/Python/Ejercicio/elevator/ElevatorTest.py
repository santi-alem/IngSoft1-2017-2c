#
# Developed by 10Pines SRL
# License: 
# This work is licensed under the 
# Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. 
# To view a copy of this license, visit http://creativecommons.org/licen ses/by-nc-sa/3.0/
# or send a letter to Creative Commons, 444 Castro Street, Suite 900, Mountain View, 
# California, 94041, USA.
#  
import unittest


class ElevatorController:
    def __init__(self):
        self._floor_queue = []
        self._floor_number = 0
        self.state = ElevatorIdleState()

    def isIdle(self):
        return self.state.isIdle()

    def isCabinStopped(self):
        return self.state.isCabinStopped()

    def isCabinDoorOpened(self):
        return self.state.isCabinDoorOpened()

    def cabinFloorNumber(self):
        return self._floor_number

    def isElevatorDoorOpening(self):
        return self.state.isElevatorDoorOpening()

    def goUpPushedFromFloor(self, floor):
        self._floor_queue.append(floor)
        return self.state.goUpPushedFromFloor(floor)

    def isWorking(self):
        return self.state.isWorking()

    def isCabinMoving(self):
        return self.state.isCabinMoving()

    def isCabinDoorOpening(self):
        return self.state.isCabinDoorOpening()

    def isCabinDoorClosing(self):
        return self.state.isCabinDoorClosing()

    def cabinDoorClosed(self):
        return self.state.cabinDoorClosed()

    def isCabinDoorClosed(self):
        return self.state.isCabinDoorClosed()

    def cabinOnFloor(self, floor):
        self.state.cabinOnFloor(floor)
        if floor != self._floor_number + 1:
            raise ElevatorEmergency("Sensor de cabina desincronizado")
        self._floor_number = floor

    def cabinDoorOpened(self):
        self.state.cabinDoorOpened()

    def openCabinDoor(self):
        return self.state.openCabinDoor()

    def isCabinWaitingForPeople(self):
        return self.state.isCabinWaitingForPeople()

    def waitForPeopleTimedOut(self):
        self.state.waitForPeopleTimeOut()

    def closeCabinDoor(self):
        self.state.closeCabinDoor()


class ElevatorState:
    def __init__(self):
        self.nextStates = []

    def isIdle(self):
        pass

    def isCabinStopped(self):
        pass

    def isCabinDoorOpened(self):
        pass

    def isElevatorDoorOpening(self):
        pass

    def goUpPushedFromFloor(self, floor):
        self.nextStates.append(ElevatorWaitingPeopleWithOpenDoors)

    def isWorking(self):
        pass

    def isCabinMoving(self):
        pass

    def isCabinDoorOpening(self):
        pass

    def isCabinDoorClosing(self):
        pass

    def cabinDoorClosed(self):
        pass

    def isCabinDoorClosed(self):
        pass

    def cabinOnFloor(self, floor):
        pass

    def cabinDoorOpened(self):
        pass

    def openCabinDoor(self):
        pass

    def isCabinWaitingForPeople(self):
        pass

    def openCabinDoorAWaitPeople(self):
        pass

    def waitForPeopleTimeOut(self):
        pass

    def closeCabinDoor(self):
        pass

    def nextState(self):
        ## Hago esto por que en python2.7 no hay safe pop
        return self.nextStates.pop() if self.nextStates else ElevatorIdleState


class ElevatorIdleState(ElevatorState):
    def isIdle(self):
        return True

    def isCabinStopped(self):
        return True

    def isCabinDoorOpened(self):
        return True

    def goUpPushedFromFloor(self, floor):
        self.__class__ = ElevatorClosingDoorsState

    def cabinDoorClosed(self):
        raise ElevatorEmergency("Sensor de puerta desincronizado")


class ElevatorClosingDoorsState(ElevatorState):
    def isIdle(self):
        return False

    def isWorking(self):
        return True

    def isCabinDoorClosed(self):
        return False

    def isCabinDoorClosing(self):
        return True

    def isCabinDoorOpened(self):
        return False

    def cabinDoorClosed(self):
        self.__class__ = ElevatorMovingState

    def openCabinDoor(self):
        self.__class__ = ElevatorOpeningDoorState

    def isCabinMoving(self):
        return False

    def isCabinStopped(self):
        return True


class ElevatorMovingState(ElevatorState):
    def isCabinMoving(self):
        return True

    def isIdle(self):
        return False

    def isWorking(self):
        return True

    def isCabinDoorOpened(self):
        return False

    def isCabinDoorOpening(self):
        return False

    def isCabinDoorClosed(self):
        return True

    def isCabinDoorClosing(self):
        return False

    def isCabinStopped(self):
        return False

    def cabinOnFloor(self, floor):
        self._floor_number = floor
        self.__class__ = ElevatorOpeningDoorState

    def cabinDoorClosed(self):
        raise ElevatorEmergency("Sensor de puerta desincronizado")


class ElevatorOpeningDoorState(ElevatorState):
    def isCabinStopped(self):
        return True

    def isCabinMoving(self):
        return False

    def isIdle(self):
        return False

    def isWorking(self):
        return True

    def isCabinDoorOpening(self):
        return True

    def isCabinDoorOpened(self):
        return False

    def isCabinDoorClosing(self):
        return False

    def isCabinDoorClosed(self):
        return False

    def cabinDoorOpened(self):
        self.__class__ = self.nextState()

    def cabinOnFloor(self, floor):
        raise ElevatorEmergency("Sensor de cabina desincronizado")

    def cabinDoorClosed(self):
        raise ElevatorEmergency("Sensor de puerta desincronizado")


class ElevatorWaitingPeopleWithOpenDoors(ElevatorState):
    def isWorking(self):
        return True

    def isCabinDoorOpened(self):
        return True

    def isCabinWaitingForPeople(self):
        return True

    def waitForPeopleTimeOut(self):
        self.closeCabinDoor()

    def closeCabinDoor(self):
        self.__class__ = ElevatorClosingDoorsState


class ElevatorEmergency(Exception):
    pass


class ElevatorTest(unittest.TestCase):
    def test01ElevatorStartsIdleWithDoorOpenOnFloorZero(self):
        elevatorController = ElevatorController()

        self.assertTrue(elevatorController.isIdle())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpened())
        self.assertEqual(0, elevatorController.cabinFloorNumber())

    def test02CabinDoorStartsClosingWhenElevatorGetsCalled(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)

        self.assertFalse(elevatorController.isIdle())
        self.assertTrue(elevatorController.isWorking())

        self.assertTrue(elevatorController.isCabinStopped())
        self.assertFalse(elevatorController.isCabinMoving())

        self.assertFalse(elevatorController.isCabinDoorOpened())
        self.assertFalse(elevatorController.isCabinDoorOpening())
        self.assertTrue(elevatorController.isCabinDoorClosing())
        self.assertFalse(elevatorController.isCabinDoorClosed())

    def test03CabinStartsMovingWhenDoorGetsClosed(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()

        self.assertFalse(elevatorController.isIdle())
        self.assertTrue(elevatorController.isWorking())

        self.assertFalse(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinMoving())

        self.assertFalse(elevatorController.isCabinDoorOpened())
        self.assertFalse(elevatorController.isCabinDoorOpening())
        self.assertFalse(elevatorController.isCabinDoorClosing())
        self.assertTrue(elevatorController.isCabinDoorClosed())

    def test04CabinStopsAndStartsOpeningDoorWhenGetsToDestination(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)

        self.assertFalse(elevatorController.isIdle())
        self.assertTrue(elevatorController.isWorking())

        self.assertTrue(elevatorController.isCabinStopped())
        self.assertFalse(elevatorController.isCabinMoving())

        self.assertFalse(elevatorController.isCabinDoorOpened())
        self.assertTrue(elevatorController.isCabinDoorOpening())
        self.assertFalse(elevatorController.isCabinDoorClosing())
        self.assertFalse(elevatorController.isCabinDoorClosed())

        self.assertEquals(1, elevatorController.cabinFloorNumber())

    def test05ElevatorGetsIdleWhenDoorGetOpened(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        elevatorController.cabinDoorOpened()

        self.assertTrue(elevatorController.isIdle())
        self.assertFalse(elevatorController.isWorking())

        self.assertTrue(elevatorController.isCabinStopped())
        self.assertFalse(elevatorController.isCabinMoving())

        self.assertTrue(elevatorController.isCabinDoorOpened())
        self.assertFalse(elevatorController.isCabinDoorOpening())
        self.assertFalse(elevatorController.isCabinDoorClosing())
        self.assertFalse(elevatorController.isCabinDoorClosed())

        self.assertEquals(1, elevatorController.cabinFloorNumber())

    # STOP HERE!

    def test06DoorKeepsOpenedWhenOpeningIsRequested(self):
        elevatorController = ElevatorController()

        self.assertTrue(elevatorController.isCabinDoorOpened())

        elevatorController.openCabinDoor()

        self.assertTrue(elevatorController.isCabinDoorOpened())

    def test07DoorMustBeOpenedWhenCabinIsStoppedAndClosingDoors(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorClosing())

        elevatorController.openCabinDoor()
        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    def test08CanNotOpenDoorWhenCabinIsMoving(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinMoving())
        self.assertTrue(elevatorController.isCabinDoorClosed())

        elevatorController.openCabinDoor()
        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinMoving())
        self.assertTrue(elevatorController.isCabinDoorClosed())

    def test09DoorKeepsOpeneingWhenItIsOpeneing(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

        elevatorController.openCabinDoor()
        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    # STOP HERE!!

    def test10RequestToGoUpAreEnqueueWhenRequestedWhenCabinIsMoving(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorOpened()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinWaitingForPeople())
        self.assertTrue(elevatorController.isCabinDoorOpened())

    def test11CabinDoorStartClosingAfterWaitingForPeople(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorOpened()
        elevatorController.waitForPeopleTimedOut()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorClosing())

    def test12StopsWaitingForPeopleIfCloseDoorIsPressed(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorOpened()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinWaitingForPeople())
        self.assertTrue(elevatorController.isCabinDoorOpened())

        elevatorController.closeCabinDoor()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorClosing())

    def test13CloseDoorDoesNothingIfIdle(self):
        elevatorController = ElevatorController()

        elevatorController.closeCabinDoor()

        self.assertTrue(elevatorController.isIdle())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpened())

    def test14CloseDoorDoesNothingWhenCabinIsMoving(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinMoving())
        self.assertTrue(elevatorController.isCabinDoorClosed())

        elevatorController.closeCabinDoor()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinMoving())
        self.assertTrue(elevatorController.isCabinDoorClosed())

    def test15CloseDoorDoesNothingWhenOpeningTheDoorToWaitForPeople(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

        elevatorController.closeCabinDoor()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    # STOP HERE!!

    def test16ElevatorHasToEnterEmergencyIfStoppedAndOtherFloorSensorTurnsOn(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        try:
            elevatorController.cabinOnFloor(0)
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de cabina desincronizado")

    def test17ElevatorHasToEnterEmergencyIfFalling(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        try:
            elevatorController.cabinOnFloor(0)
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de cabina desincronizado")

    def test18ElevatorHasToEnterEmergencyIfJumpsFloors(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(3)
        elevatorController.cabinDoorClosed()
        try:
            elevatorController.cabinOnFloor(3)
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de cabina desincronizado")

    def test19ElevatorHasToEnterEmergencyIfDoorClosesAutomatically(self):
        elevatorController = ElevatorController()

        try:
            elevatorController.cabinDoorClosed()
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de puerta desincronizado")

    def test20ElevatorHasToEnterEmergencyIfDoorClosedSensorTurnsOnWhenClosed(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        try:
            elevatorController.cabinDoorClosed()
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de puerta desincronizado")

    def test21ElevatorHasToEnterEmergencyIfDoorClosesWhenOpening(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(1)
        try:
            elevatorController.cabinDoorClosed()
            self.fail()
        except ElevatorEmergency as elevatorEmergency:
            self.assertTrue(elevatorEmergency.message == "Sensor de puerta desincronizado")

    # STOP HERE!!
    # More tests here to verify bad sensor function

    def test22CabinHasToStopOnTheFloorsOnItsWay(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinOnFloor(1)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    def test23ElevatorCompletesAllTheRequests(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinDoorClosed()
        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinOnFloor(1)
        elevatorController.cabinDoorOpened()
        elevatorController.waitForPeopleTimedOut()
        elevatorController.cabinDoorClosed()
        elevatorController.cabinOnFloor(2)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    def test24CabinHasToStopOnFloorsOnItsWayNoMatterHowTheyWellCalled(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorClosed()
        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinOnFloor(1)

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorOpening())

    def test25CabinHasToStopAndWaitForPeopleOnFloorsOnItsWayNoMatterHowTheyWellCalled(self):
        elevatorController = ElevatorController()

        elevatorController.goUpPushedFromFloor(2)
        elevatorController.cabinDoorClosed()
        elevatorController.goUpPushedFromFloor(1)
        elevatorController.cabinOnFloor(1)
        elevatorController.cabinDoorOpened()
        elevatorController.waitForPeopleTimedOut()

        self.assertTrue(elevatorController.isWorking())
        self.assertTrue(elevatorController.isCabinStopped())
        self.assertTrue(elevatorController.isCabinDoorClosing())
