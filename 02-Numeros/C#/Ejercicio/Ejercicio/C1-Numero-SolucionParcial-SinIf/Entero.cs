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

        private Numero sumarNumero(Entero sumando)
        {
            return new Entero(value + sumando.getValue());
        }

        private Numero sumarNumero(Fraccion sumando)
        {
            Numero nuevoNumerador = this.por(sumando.getDenominador().mas(sumando.getNumerador()));
            return Fraccion.dividir((Entero)nuevoNumerador, sumando.getDenominador());
        }
        public override Numero mas(Numero sumando)
        {
            dynamic a = Convert.ChangeType(sumando, sumando.GetType());
            return sumarNumero(a);
        }

        private Numero porNumero(Fraccion multiplicador)
        {
            return Fraccion.dividir((Entero)multiplicador.getNumerador().por(this), multiplicador.getDenominador());
        }
        private Numero porNumero(Entero multiplicador)
        {
            return new Entero(value * (multiplicador).getValue());
        }
        public override Numero por(Numero multiplicador)
        {
            dynamic a = Convert.ChangeType(multiplicador, multiplicador.GetType());
            return porNumero(a);
        }

        private Numero divididoNumero(Fraccion divisor)
        {
            return Fraccion.dividir(new Entero(value * divisor.getDenominador().value), divisor.getNumerador());
        }
        private Numero divididoNumero(Entero divisor)
        {
            return Fraccion.dividir(this,divisor);
        }

        public override Numero dividido(Numero divisor)
        {
            dynamic a = Convert.ChangeType(divisor, divisor.GetType());
            return divididoNumero(a);
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
