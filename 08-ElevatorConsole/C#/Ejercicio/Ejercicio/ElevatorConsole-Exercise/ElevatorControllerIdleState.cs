using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class ElevatorControllerIdleState: ElevatorControllerState
    {
	    private ElevatorController elevatorController;
	
	    public ElevatorControllerIdleState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public bool isIdle() {
		    return true;
	    }

	    
	    public void goUpPushedFromFloor(int aFloorNumber) {
		    elevatorController.goUpPushedFromFloorWhenIdle(aFloorNumber);
	    }

	    
	    public bool isWorking() {
		    return false;
	    }

	    
	    public void cabindDoorClosed() {
		    elevatorController.cabinDoorClosedWhenIdle();
	    }

	    
	    public void cabinOnFloor(int aFloorNumber) {
		    elevatorController.cabinOnFloorWhenIdle(aFloorNumber);
	    }

	    
	    public void cabinDoorOpened() {
		    throw new Exception();
	    }

	    
	    public void openCabinDoor() {
		    elevatorController.openCabinDoorWhenIdle();
	    }

	    
	    public void closeCabinDoor() {
		    elevatorController.closeCabinDoorWhenIdle();
	    }

	    
	    public void waitForPeopleTimedOut() {
		    throw new Exception();
	    }
    }
}
