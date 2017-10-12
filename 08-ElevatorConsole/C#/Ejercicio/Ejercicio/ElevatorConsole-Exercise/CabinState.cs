using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface CabinState
    {
        bool isStopped();
        bool isMoving();
        void cabinDoorClosedWhenWorking();
        void cabinDoorOpenedWhenWorking();
        void openCabinDoorWhenWorking();
        bool isWaitingForPeople();
        void waitForPeopleTimedOutWhenWorking();
        void closeCabinDoorWhenWorking();
        void accept(CabinStateVisitor visitor);
    }
}
