using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorConsole_Exercise
{
    class ElevatorControllerConsole : ElevatorInfoSubscirber, CabinStateVisitor, CabinDoorStateVisitor
    {
	    private List<String> console;

	    public ElevatorControllerConsole(ElevatorController elevatorController)
	    {
	        elevatorController.SubscribeToControllerOutput(this);
            console = new List<String>();
	    }

	    public void cabinDoorStateChangedTo(CabinDoorState cabinDoorState) {
		    cabinDoorState.accept(this);
	    }

        public void cabinStateChangedTo(CabinState cabinState) {
		    cabinState.accept(this);
	    }

	    public IEnumerator<String> consoleReader() {
		    return console.GetEnumerator();
	    }

	    public void visitCabinMoving(CabinMovingState cabinMovingState) {
		    console.Add("Cabina Moviendose");
	    }

	    public void visitCabinStopped(CabinStoppedState cabinStoppedState) {
		    console.Add("Cabina Detenida");
	    }

	    public void visitCabinWaitingPeople(CabinWaitingForPeopleState cabinWaitingForPeopleState) {
		    console.Add("Cabina Esperando Gente");
	    }

	    public void visitCabinDoorClosing(CabinDoorClosingState cabinDoorClosingState) {
		    console.Add("Puerta Cerrandose");
	    }

	    public void visitCabinDoorClosed(CabinDoorClosedState cabinDoorClosedState) {
		    console.Add("Puerta Cerrada");
	    }

	    public void visitCabinDoorOpened(CabinDoorOpenedState cabinDoorOpenedState) {
		    console.Add("Puerta Abierta");
	    }

	    public void visitCabinDoorOpening(CabinDoorOpeningState cabinDoorOpeningState) {
		    console.Add("Puerta Abriendose");
	    }
    }
}
