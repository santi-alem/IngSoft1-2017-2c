using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class CabinDoorOpeningState: CabinDoorState 
    {
	    private ElevatorController elevatorController;
	    public CabinDoorOpeningState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public void cabinDoorClosedWhenWorkingAndCabinStopped() {
		    elevatorController.cabinDoorClosedWhenWorkingAndCabinStoppedAndCabinDoorOpening();
	    }

	    
	    public bool isClosed() {
		    return false;
	    }

	    
	    public bool isClosing() {
		    return false;
	    }

	    
	    public bool isOpened() {
		    return false;
	    }

	    
	    public bool isOpening() {
		    return true;
	    }

	    
	    public void openCabinDoorWhenWorkingAndCabinStopped() {
		    elevatorController.openCabinDoorWhenWorkingAndCabinStoppedAndCabinDoorOpening();
	    }

	    
	    public void closeCabinDoorWhenWorkingAndCabinStopped() {
		    elevatorController.closeCabinDoorWhenWorkingAndCabinStoppedAndCabinDoorOpening();
	    }

	    
	    public void accept(CabinDoorStateVisitor visitor) {
		    visitor.visitCabinDoorOpening(this);
	    }

    }
}
