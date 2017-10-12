using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class ElevatorControllerIsWorkingState: ElevatorControllerState
    {
	    private ElevatorController elevatorController;
	    public ElevatorControllerIsWorkingState(
			    ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public void goUpPushedFromFloor(int aFloorNumber) {
		    elevatorController.goUpPushedFromFloorWhenWorking(aFloorNumber);
	    }

	    
	    public bool isIdle() {
		    return false;
	    }

	    
	    public bool isWorking() {
		    return true;
	    }

	    
	    public void cabindDoorClosed() {
		    elevatorController.cabinDoorClosedWhenWorking();
	    }

	    
	    public void cabinOnFloor(int aFloorNumber) {
		    elevatorController.cabinOnFloorWhenWorking(aFloorNumber);
	    }

	    
	    public void cabinDoorOpened() {
		    elevatorController.cabinDoorOpenendWhenWorking();
	    }

	    
	    public void openCabinDoor() {
		    elevatorController.openCabinDoorWhenWorking();
	    }

	    
	    public void waitForPeopleTimedOut() {
		    elevatorController.waitForPeopleTimedOutWhenWorking();
	    }

	    
	    public void closeCabinDoor() {
		    elevatorController.closeCabinDoorWhenWorking();
	    }
    }
}
