using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf 
{
    public class Fraccion: Numero
    {
	    protected Entero numerador;
	    protected Entero denominador;
	
	    public static Numero dividir(Entero dividendo, Entero divisor ) {
		
		    if(divisor.esCero()) throw new Exception (Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
		    if(dividendo.esCero()) return dividendo;
		
		    Entero maximoComunDivisor = dividendo.maximoComunDivisorCon(divisor);
		    Entero numerador = dividendo.divisionEntera(maximoComunDivisor);
		    Entero denominador = divisor.divisionEntera(maximoComunDivisor);
		
		    if (denominador.esUno()) return numerador;
		
		    return new Fraccion(numerador,denominador);

	    }
	
	    private Fraccion(Entero numerador, Entero denominador){
		    this.numerador = numerador;
		    this.denominador = denominador;
	    }
	
	    public Entero getNumerador (){
		    return numerador;
	    }
	
	    public Entero getDenominador(){
		    return denominador;
	    }

	    public override bool esCero() {
		    return false;
	    }

	    public override bool esUno() {
		    return false;
	    }
	    
	    public override bool Equals(Object anObject){
		    if (typeof(Fraccion)==anObject.GetType()) 
			    return equals((Fraccion) anObject);
		    else
			    return false;
	    }
	
	    public bool equals(Fraccion aFraccion){
		    return numerador.por(aFraccion.getDenominador()).Equals( denominador.por(aFraccion.getNumerador()));
	    }

        public override int GetHashCode() {
		    return numerador.GetHashCode() / denominador.GetHashCode();
	    }

        private Numero sumarNumero(Entero sumando)
        {
            Entero nuevoNumerador = (Entero)denominador.por(sumando).mas(numerador);
            return dividir(nuevoNumerador, denominador);
        }

        private Numero sumarNumero(Fraccion sumando)
        {
            Numero nuevoDenominador = denominador.por(sumando.getDenominador());
            Numero nuevoNumerador1 = numerador.por(sumando.getDenominador());
            Numero nuevoNumerador2 = denominador.por(sumando.getNumerador());
            Numero nuevoNumerador = nuevoNumerador1.mas(nuevoNumerador2);
            return nuevoNumerador.dividido(nuevoDenominador);
        }
        public override Numero mas(Numero sumando) {
            dynamic a = Convert.ChangeType(sumando, sumando.GetType());
            return sumarNumero(a);
        }
        private Numero porNumero(Fraccion multiplicador)
        {
            return numerador.por(multiplicador.getNumerador())
                .dividido(denominador.por(multiplicador.getDenominador()));
        }
        private Numero porNumero(Entero multiplicador)
        {
            return numerador.por(multiplicador).dividido(denominador);
        }

        public override Numero por(Numero multiplicador)
        {
            dynamic a = Convert.ChangeType(multiplicador, multiplicador.GetType());
            return porNumero(a);
        }

        private Numero divididoNumero(Fraccion divisor)
        {
            return numerador.por(divisor.getDenominador()).
                dividido(denominador.por(divisor.getNumerador()));
        }
        private Numero divididoNumero(Entero divisor)
        {
            return numerador.dividido(denominador.por(divisor));
        }


        public override Numero dividido(Numero divisor)
        {
            dynamic a = Convert.ChangeType(divisor, divisor.GetType());
            return divididoNumero(a);
        }

    }
}
