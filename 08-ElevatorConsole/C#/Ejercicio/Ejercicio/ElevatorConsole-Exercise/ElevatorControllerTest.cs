using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElevatorConsole_Exercise
{
    [TestClass]
    public class ElevatorControllerTest
    {
        [TestMethod]
        public void testElevatorStartsIdleWithDoorOpenOnFloorZero()
        {
            ElevatorController elevatorController = new ElevatorController();

            Assert.IsTrue(elevatorController.isIdle());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpened());
            Assert.AreEqual(0, elevatorController.cabinFloorNumber());
        }

        [TestMethod]
        public void testCabinDoorStartsClosingWhenElevatorGetsCalled()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);

            Assert.IsFalse(elevatorController.isIdle());
            Assert.IsTrue(elevatorController.isWorking());

            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsFalse(elevatorController.isCabinMoving());

            Assert.IsFalse(elevatorController.isCabinDoorOpened());
            Assert.IsFalse(elevatorController.isCabinDoorOpening());
            Assert.IsTrue(elevatorController.isCabinDoorClosing());
            Assert.IsFalse(elevatorController.isCabinDoorClosed());
        }

        [TestMethod]
        public void testCabinStartsMovingWhenDoorGetsClosed()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();

            Assert.IsFalse(elevatorController.isIdle());
            Assert.IsTrue(elevatorController.isWorking());

            Assert.IsFalse(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinMoving());

            Assert.IsFalse(elevatorController.isCabinDoorOpened());
            Assert.IsFalse(elevatorController.isCabinDoorOpening());
            Assert.IsFalse(elevatorController.isCabinDoorClosing());
            Assert.IsTrue(elevatorController.isCabinDoorClosed());
        }

        [TestMethod]
        public void testCabinStopsAndStartsOpeningDoorWhenGetsToDestination()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);

            Assert.IsFalse(elevatorController.isIdle());
            Assert.IsTrue(elevatorController.isWorking());

            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsFalse(elevatorController.isCabinMoving());

            Assert.IsFalse(elevatorController.isCabinDoorOpened());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
            Assert.IsFalse(elevatorController.isCabinDoorClosing());
            Assert.IsFalse(elevatorController.isCabinDoorClosed());

            Assert.AreEqual(1, elevatorController.cabinFloorNumber());
        }

        [TestMethod]
        public void testElevatorGetsIdleWhenDoorGetOpened()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            elevatorController.cabinDoorOpened();

            Assert.IsTrue(elevatorController.isIdle());
            Assert.IsFalse(elevatorController.isWorking());

            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsFalse(elevatorController.isCabinMoving());

            Assert.IsTrue(elevatorController.isCabinDoorOpened());
            Assert.IsFalse(elevatorController.isCabinDoorOpening());
            Assert.IsFalse(elevatorController.isCabinDoorClosing());
            Assert.IsFalse(elevatorController.isCabinDoorClosed());

            Assert.AreEqual(1, elevatorController.cabinFloorNumber());
        }

        // STOP HERE!

        [TestMethod]
        public void testDoorKeepsOpenedWhenOpeningIsRequested()
        {
            ElevatorController elevatorController = new ElevatorController();

            Assert.IsTrue(elevatorController.isCabinDoorOpened());

            elevatorController.openCabinDoor();

            Assert.IsTrue(elevatorController.isCabinDoorOpened());

        }

        [TestMethod]
        public void testDoorMustBeOpenedWhenCabinIsStoppedAndClosingDoors()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorClosing());

            elevatorController.openCabinDoor();
            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
        }

        [TestMethod]
        public void testCanNotOpenDoorWhenCabinIsMoving()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinMoving());
            Assert.IsTrue(elevatorController.isCabinDoorClosed());

            elevatorController.openCabinDoor();
            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinMoving());
            Assert.IsTrue(elevatorController.isCabinDoorClosed());
        }

        [TestMethod]
        public void testDoorKeepsOpeneingWhenItIsOpeneing()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());

            elevatorController.openCabinDoor();
            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
        }

        // STOP HERE!!

        [TestMethod]
        public void testRequestToGoUpAreEnqueueWhenRequestedWhenCabinIsMoving()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorOpened();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinWaitingForPeople());
            Assert.IsTrue(elevatorController.isCabinDoorOpened());
        }

        [TestMethod]
        public void testCabinDoorStartClosingAfterWaitingForPeople()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorOpened();
            elevatorController.waitForPeopleTimedOut();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorClosing());
        }

        [TestMethod]
        public void testStopsWaitingForPeopleIfCloseDoorIsPressed()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorOpened();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinWaitingForPeople());
            Assert.IsTrue(elevatorController.isCabinDoorOpened());

            elevatorController.closeCabinDoor();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorClosing());

        }

        [TestMethod]
        public void testCloseDoorDoesNothingIfIdle()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.closeCabinDoor();

            Assert.IsTrue(elevatorController.isIdle());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpened());

        }

        [TestMethod]
        public void testCloseDoorDoesNothingWhenCabinIsMoving()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinMoving());
            Assert.IsTrue(elevatorController.isCabinDoorClosed());

            elevatorController.closeCabinDoor();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinMoving());
            Assert.IsTrue(elevatorController.isCabinDoorClosed());
        }

        [TestMethod]
        public void testCloseDoorDoesNothingWhenOpeningTheDoorToWaitForPeople()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());

            elevatorController.closeCabinDoor();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());

        }

        // STOP HERE!!

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfStoppedAndOtherFloorSensorTurnsOn()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            try
            {
                elevatorController.cabinOnFloor(0);
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de cabina desincronizado");
            }
        }

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfFalling()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            try
            {
                elevatorController.cabinOnFloor(0);
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de cabina desincronizado");
            }
        }

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfJumpsFloors()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(3);
            elevatorController.cabinDoorClosed();
            try
            {
                elevatorController.cabinOnFloor(3);
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de cabina desincronizado");
            }
        }

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfDoorClosesAutomatically()
        {
            ElevatorController elevatorController = new ElevatorController();

            try
            {
                elevatorController.cabinDoorClosed();
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de puerta desincronizado");
            }
        }

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfDoorClosedSensorTurnsOnWhenClosed()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            try
            {
                elevatorController.cabinDoorClosed();
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de puerta desincronizado");
            }
        }

        [TestMethod]
        public void testElevatorHasToEnterEmergencyIfDoorClosesWhenOpening()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);
            try
            {
                elevatorController.cabinDoorClosed();
                Assert.Fail();
            }
            catch (Exception elevatorEmergency)
            {
                Assert.IsTrue(elevatorEmergency.Message == "Sensor de puerta desincronizado");
            }
        }

        // STOP HERE!!
        // More tests here to verify bad sensor function

        [TestMethod]
        public void testCabinHasToStopOnTheFloorsOnItsWay()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinOnFloor(1);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
        }

        [TestMethod]
        public void testElevatorCompletesAllTheRequests()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinOnFloor(1);
            elevatorController.cabinDoorOpened();
            elevatorController.waitForPeopleTimedOut();
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(2);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
        }

        [TestMethod]
        public void testCabinHasToStopOnFloorsOnItsWayNoMatterHowTheyWellCalled()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorClosed();
            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinOnFloor(1);

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorOpening());
        }

        [TestMethod]
        public void testCabinHasToStopAndWaitForPeopleOnFloorsOnItsWayNoMatterHowTheyWellCalled()
        {
            ElevatorController elevatorController = new ElevatorController();

            elevatorController.goUpPushedFromFloor(2);
            elevatorController.cabinDoorClosed();
            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinOnFloor(1);
            elevatorController.cabinDoorOpened();
            elevatorController.waitForPeopleTimedOut();

            Assert.IsTrue(elevatorController.isWorking());
            Assert.IsTrue(elevatorController.isCabinStopped());
            Assert.IsTrue(elevatorController.isCabinDoorClosing());
        }
    }
}
