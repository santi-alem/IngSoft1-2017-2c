using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class CabinDoorClosedState: CabinDoorState 
    {
        private ElevatorController elevatorController;
	    public CabinDoorClosedState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public void cabinDoorClosedWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public bool isClosed() {
		    return true;
	    }

	    
	    public bool isClosing() {
		    return false;
	    }

	    
	    public bool isOpened() {
		    return false;
	    }

	    
	    public bool isOpening() {
		    return false;
	    }

	    
	    public void closeCabinDoorWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public void openCabinDoorWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public void accept(CabinDoorStateVisitor visitor) {
		    visitor.visitCabinDoorClosed(this);
	    }

    }
}
