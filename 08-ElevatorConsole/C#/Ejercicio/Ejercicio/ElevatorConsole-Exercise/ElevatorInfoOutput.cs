using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    interface ElevatorInfoSubscirber
    {
        void cabinDoorStateChangedTo(CabinDoorState cabinDoorState);

        void cabinStateChangedTo(CabinState cabinState);

    }
}
