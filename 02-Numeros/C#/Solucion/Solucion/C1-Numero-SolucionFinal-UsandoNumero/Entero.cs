using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionFinal_UsandoNumero
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
            return sumando.masEntero(this);
        }

	    override public Numero masEntero(Entero sumando) {
		    return new Entero (value+sumando.getValue());
	    } 

	    
	    override public Numero masFraccion(Fraccion sumando){
		    return sumando.masEntero(this);
	    }

	    
	    override public Numero por(Numero multiplicador) {
            return multiplicador.porEntero(this);
        }

	    override public Numero porEntero(Entero multiplicador) {
		    return new Entero (value*multiplicador.getValue());
	    }

	    
	    override public Numero porFraccion(Fraccion multiplicador) {
		    return this.por(multiplicador.getNumerador()).dividido(multiplicador.getDenominador());
	    }
	
        override public Numero dividido(Numero divisor) {
		    return divisor.dividirEntero(this);
	    }
	
	    override public Numero dividirEntero(Entero dividendo) {
		    return Fraccion.dividir(dividendo, this);
	    }

	    override public Numero dividirFraccion(Fraccion dividendo) {
		    return dividendo.getNumerador().dividido(dividendo.getDenominador().por(this));
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
