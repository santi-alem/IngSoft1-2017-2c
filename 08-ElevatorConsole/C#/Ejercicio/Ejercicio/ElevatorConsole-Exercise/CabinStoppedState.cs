using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class CabinStoppedState: CabinState 
    {

	    private ElevatorController elevatorController;
	
	    public CabinStoppedState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController; 
	    }

	    
	    public bool isStopped() {
		    return true;
	    }

	    
	    public bool isMoving() {
		    return false;
	    }

	    
	    public void cabinDoorClosedWhenWorking() {
		    elevatorController.cabinDoorClosedWhenWorkingAndCabinStopped();
	    }

	    
	    public void cabinDoorOpenedWhenWorking() {
		    elevatorController.cabinDoorOpenedWhenWorkingAndCabinStopped();
	    }

	    
	    public void openCabinDoorWhenWorking() {
		    elevatorController.openCabinDoorWhenWorkingAndCabinStopped();
	    }

	    
	    public bool isWaitingForPeople() {
		    return false;
	    }

	    
	    public void closeCabinDoorWhenWorking() {
		    elevatorController.closeCabinDoorWhenWorkingAndCabinStopped();
	    }

	    
	    public void waitForPeopleTimedOutWhenWorking() {
		    throw new Exception();
	    }

	    
	    public void accept(CabinStateVisitor visitor) {
		    visitor.visitCabinStopped(this);
	    }
    }
}
