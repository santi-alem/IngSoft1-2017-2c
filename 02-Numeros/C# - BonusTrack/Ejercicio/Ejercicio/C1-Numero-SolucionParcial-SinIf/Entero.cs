using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf
{
    public class Entero: Numero
    {
    	protected int value;
	
        public Entero() { }
	    public Entero(int value){
		    this.value = value;
	    }
	
	    public int getValue(){
		    return value;
	    }
        private Numero sumarNumero(Unidad sumando)
        {
            return new Entero(value + 1);
        }

        private Numero sumarNumero(Cero sumando)
        {
            return new Entero(value);
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
        private Numero porNumero(Unidad multiplicador)
        {
            return new Entero(value);
        }
        private Numero porNumero(Cero multiplicador)
        {
            return new Cero();
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
        private Numero divididoNumero(Unidad divisor)
        {
            return new Entero(value);
        }

        private Numero divididoNumero(Cero divisor)
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
        }
        private Numero divididoNumero(Fraccion divisor)
        {
            Entero nuevoNumerador = new Entero(((Entero) this.por(divisor.getDenominador())).value);
            dynamic denominadorNoTipado = Convert.ChangeType(divisor.getNumerador(), divisor.getNumerador().GetType());
            return Fraccion.dividir(nuevoNumerador, denominadorNoTipado);
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

        private Numero maximoComunDivisorAux(Entero otroEntero, Unidad divisor)
        {
            return new Unidad();
        }
        private Numero maximoComunDivisorAux(Entero otroEntero, Cero divisor)
        {
            return new Entero(otroEntero.getValue());
        }

        private Numero maximoComunDivisorAux(Entero otroEntero, Entero resto)
        {
            return otroEntero.maximoComunDivisorCon(resto);
        }

        public Numero maximoComunDivisorCon(Entero otroEntero)
        {
            Numero resto = this.restoCon(otroEntero);
            dynamic restoDinamico = Convert.ChangeType(resto, resto.GetType());
            return maximoComunDivisorAux(otroEntero, restoDinamico);
        }


        public Numero restoCon(Entero divisor)
        {
            if (value % divisor.getValue() == 0)
                return new Cero();

            if (value % divisor.getValue() == 1)
                return new Unidad();

            return new Entero(value % divisor.getValue());
        }

        private Numero divisionEnteraAux(Cero divisor)
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
        }

        private Entero divisionEnteraAux(Unidad divisor)
        {
            return new Entero(value);
        }

        private Numero divisionEnteraAux(Entero divisor)
        {
            if (this.value == divisor.getValue())
                return new Unidad();

            return new Entero(value / divisor.getValue());
        }

        public Numero divisionEntera(Numero otroEntero)
        { 
            dynamic otroEnteroNoTipado = Convert.ChangeType(otroEntero, otroEntero.GetType());
            return divisionEnteraAux(otroEnteroNoTipado);
        }


        override public bool esCero () {
		    return value == 0;
	    }
	    
	    override public bool esUno() {
		    return value == 1;
	    }
	    
	    override public bool Equals(Object anObject)
	    {
	        var o = anObject as Entero;
	        return o != null && value == o.getValue();
	    }

        override public int GetHashCode()
        {    
		    return value;
	    }
    }
}
