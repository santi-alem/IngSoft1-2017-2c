using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElevatorConsole_Exercise
{
    [TestClass]
    public class ElevatorControllerViewTest
    {
        [TestMethod]
        public void test01ElevatorControllerConsoleTracksDoorClosingState()
        {
            ElevatorController elevatorController = new ElevatorController();
            ElevatorControllerConsole elevatorControllerConsole = new ElevatorControllerConsole(elevatorController);

            elevatorController.goUpPushedFromFloor(1);

            IEnumerator<String> reader = elevatorControllerConsole.consoleReader();

            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrandose", reader.Current);
            Assert.IsFalse(reader.MoveNext());

        }

        [TestMethod]
        public void test02ElevatorControllerConsoleTracksCabinState()
        {
            ElevatorController elevatorController = new ElevatorController();
            ElevatorControllerConsole elevatorControllerConsole = new ElevatorControllerConsole(elevatorController);

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();

            IEnumerator<String> reader = elevatorControllerConsole.consoleReader();

            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrandose", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrada", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Cabina Moviendose", reader.Current);
            Assert.IsFalse(reader.MoveNext());

        }

        [TestMethod]
        public void test03ElevatorControllerConsoleTracksCabinAndDoorStateChanges()
        {
            ElevatorController elevatorController = new ElevatorController();
            ElevatorControllerConsole elevatorControllerConsole = new ElevatorControllerConsole(elevatorController);

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);

            IEnumerator<String> reader = elevatorControllerConsole.consoleReader();

            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrandose", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrada", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Cabina Moviendose", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Cabina Detenida", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Puerta Abriendose", reader.Current);
            Assert.IsFalse(reader.MoveNext());

        }

        [TestMethod]
        public void test04ElevatorControllerCanHaveMoreThanOneView()
        {
            ElevatorController elevatorController = new ElevatorController();
            ElevatorControllerConsole elevatorControllerConsole = new ElevatorControllerConsole(elevatorController);
            ElevatorControllerStatusView elevatorControllerStatusView = new ElevatorControllerStatusView(elevatorController);

            elevatorController.goUpPushedFromFloor(1);
            elevatorController.cabinDoorClosed();
            elevatorController.cabinOnFloor(1);

            IEnumerator<String> reader = elevatorControllerConsole.consoleReader();

            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrandose", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Puerta Cerrada", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Cabina Moviendose", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Cabina Detenida", reader.Current);
            reader.MoveNext();
            Assert.AreEqual("Puerta Abriendose", reader.Current);
            Assert.IsFalse(reader.MoveNext());

            Assert.AreEqual("Stopped", elevatorControllerStatusView.cabinFieldModel());
            Assert.AreEqual("Opening", elevatorControllerStatusView.cabinDoorFieldModel());
        }
    }
}
