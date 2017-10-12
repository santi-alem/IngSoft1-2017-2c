using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class ElevatorControllerStatusView: ElevatorInfoSubscirber, CabinStateVisitor,CabinDoorStateVisitor 
    {
	    private String m_cabinFieldModel;
	    private String m_cabinDoorFieldModel;

	    public ElevatorControllerStatusView(ElevatorController elevatorController) {
            elevatorController.SubscribeToControllerOutput(this);
        }

	    public void cabinDoorStateChangedTo(CabinDoorState cabinDoorState) {
		    cabinDoorState.accept(this);
	    }

	    public void cabinStateChangedTo(CabinState cabinState) {
		    cabinState.accept(this);
	    }

	    
	    public void visitCabinDoorClosing(CabinDoorClosingState cabinDoorClosingState) {
		    m_cabinDoorFieldModel = "Closing";
	    }

	    
	    public void visitCabinDoorClosed(CabinDoorClosedState cabinDoorClosedState) {
		    m_cabinDoorFieldModel = "Closed";
	    }

	    
	    public void visitCabinDoorOpened(CabinDoorOpenedState cabinDoorOpenedState) {
		    m_cabinDoorFieldModel = "Open";
	    }

	    
	    public void visitCabinDoorOpening(CabinDoorOpeningState cabinDoorOpeningState) {
		    m_cabinDoorFieldModel = "Opening";
	    }

	    
	    public void visitCabinMoving(CabinMovingState cabinMovingState) {
		    m_cabinFieldModel = "Moving";
	    }

	    
	    public void visitCabinStopped(CabinStoppedState cabinStoppedState) {
		    m_cabinFieldModel = "Stopped";
	    }

	    
	    public void visitCabinWaitingPeople(CabinWaitingForPeopleState cabinWaitingForPeopleState) {
		    m_cabinFieldModel = "Waiting People";
	    }

	    public String cabinFieldModel() {
		    return m_cabinFieldModel;
	    }

	    public String cabinDoorFieldModel() {
		    return m_cabinDoorFieldModel;
	    }
    }
}
