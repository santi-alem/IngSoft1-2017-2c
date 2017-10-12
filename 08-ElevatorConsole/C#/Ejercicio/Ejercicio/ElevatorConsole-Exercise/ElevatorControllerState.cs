using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface ElevatorControllerState
    {
        bool isIdle();
        void goUpPushedFromFloor(int aFloorNumber);
        bool isWorking();
        void cabindDoorClosed();
        void cabinOnFloor(int aFloorNumber);
        void cabinDoorOpened();
        void openCabinDoor();
        void waitForPeopleTimedOut();
        void closeCabinDoor();
    }
}
