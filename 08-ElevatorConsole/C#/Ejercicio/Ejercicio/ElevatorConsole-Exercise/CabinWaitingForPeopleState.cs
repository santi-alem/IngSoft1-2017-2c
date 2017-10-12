using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class CabinWaitingForPeopleState: CabinState 
    {
        private ElevatorController elevatorController;

	    public CabinWaitingForPeopleState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public void cabinDoorClosedWhenWorking() {
		    throw new Exception();
	    }

	    
	    public void cabinDoorOpenedWhenWorking() {
		    throw new Exception();
	    }

	    
	    public bool isMoving() {
		    throw new Exception();
	    }

	    
	    public bool isStopped() {
		    return false;
	    }

	    
	    public void openCabinDoorWhenWorking() {
		    throw new Exception();
	    }

	    
	    public bool isWaitingForPeople() {
		    return true;
	    }

	    
	    public void waitForPeopleTimedOutWhenWorking() {
		    elevatorController.waitForPeopleTimedOutWhenWorkingAndCabinWaitingForPeople();
	    }

	    
	    public void closeCabinDoorWhenWorking() {
		    elevatorController.closeCabinDoorWhenWorkingAndCabinWaitingForPeople();
	    }

	    
	    public void accept(CabinStateVisitor visitor) {
		    visitor.visitCabinWaitingPeople(this);
	    }
    }
}
