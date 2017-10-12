using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class CabinMovingState: CabinState 
    {
        private ElevatorController elevatorController;
	
	    public CabinMovingState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public void cabinDoorClosedWhenWorking() {
		    elevatorController.cabinDoorClosedWhenWorkingAndCabinMoving();
	    }

	    
	    public bool isMoving() {
		    return true;
	    }

	    
	    public bool isStopped() {
		    return false;
	    }

	    
	    public void cabinDoorOpenedWhenWorking() {
		    throw new Exception();
	    }

	    
	    public void openCabinDoorWhenWorking() {
		    elevatorController.openCabinDoorWhenWorkingAndCabinMoving();
	    }

	    
	    public bool isWaitingForPeople() {
		    return false;
	    }

	    
	    public void closeCabinDoorWhenWorking() {
		    elevatorController.closeCabinDoorWhenWorkingAndCabinMoving();
	    }

	    
	    public void waitForPeopleTimedOutWhenWorking() {
		    throw new Exception();
	    }

	    
	    public void accept(CabinStateVisitor visitor) {
		    visitor.visitCabinMoving(this);
	    }
    }
}
