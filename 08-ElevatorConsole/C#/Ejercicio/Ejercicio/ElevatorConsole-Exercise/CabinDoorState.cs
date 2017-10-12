using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface CabinDoorState
    {
        bool isOpened();
        bool isOpening();
        bool isClosing();
        bool isClosed();
        void cabinDoorClosedWhenWorkingAndCabinStopped();
        void openCabinDoorWhenWorkingAndCabinStopped();
        void closeCabinDoorWhenWorkingAndCabinStopped();
        void accept(CabinDoorStateVisitor visitor);
    }
}
