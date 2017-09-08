using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf
{
    public class Entero: Numero
    {
    	protected int value;
	
	    public Entero(int value){
		    this.value = value;
	    }
	
	    public int getValue(){
		    return value;
	    }
	
	    override public Numero mas(Numero sumando) {
            return new Entero(value + ((Entero) sumando).getValue());
        }

	    override public Numero por(Numero multiplicador) {
            return new Entero(value * ((Entero) multiplicador).getValue());
        }
	
        override public Numero dividido(Numero divisor) {
            return Fraccion.dividir(this, (Entero) divisor);
        }
	
        public Entero maximoComunDivisorCon(Entero otroEntero)
        {
            if (otroEntero.esCero())
                return this;
            else
                return otroEntero.maximoComunDivisorCon(this.restoCon(otroEntero));
        }

        public Entero restoCon(Entero divisor)
        {
            return new Entero(value % divisor.getValue());
        }

        public Entero divisionEntera(Entero divisor)
        {
            return new Entero(value / divisor.getValue());
        }
	    
	    override public bool esCero () {
		    return value == 0;
	    }
	    
	    override public bool esUno() {
		    return value == 1;
	    }
	    
	    override public bool Equals(Object anObject){
		    if (typeof(Entero)==anObject.GetType())
			    return value==((Entero) anObject).getValue();
		    else
			    return false;
	    }

        override public int GetHashCode()
        {    
		    return value;
	    }
    }
}
