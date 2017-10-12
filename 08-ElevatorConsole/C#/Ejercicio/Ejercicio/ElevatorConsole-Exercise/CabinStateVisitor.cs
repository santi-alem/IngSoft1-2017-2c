using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface CabinStateVisitor
    {
        void visitCabinMoving(CabinMovingState cabinMovingState);
        void visitCabinStopped(CabinStoppedState cabinStoppedState);
        void visitCabinWaitingPeople(CabinWaitingForPeopleState cabinWaitingForPeopleState);
    }
}
