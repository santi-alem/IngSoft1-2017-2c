using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface CabinDoorStateVisitor
    {
        void visitCabinDoorClosing(CabinDoorClosingState cabindDoorClosingState);
        void visitCabinDoorClosed(CabinDoorClosedState cabinDoorClosedState);
        void visitCabinDoorOpened(CabinDoorOpenedState cabinDoorOpenedState);
        void visitCabinDoorOpening(CabinDoorOpeningState cabinDoorOpeningState);
    }
}
