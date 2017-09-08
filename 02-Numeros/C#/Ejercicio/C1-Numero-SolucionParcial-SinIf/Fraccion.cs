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

	    override public bool esCero() {
		    return false;
	    }

	    override public bool esUno() {
		    return false;
	    }
	    
	    override public bool Equals(Object anObject){
		    if (typeof(Fraccion)==anObject.GetType()) 
			    return equals((Fraccion) anObject);
		    else
			    return false;
	    }
	
	    public bool equals(Fraccion aFraccion){
		    return numerador.por(aFraccion.getDenominador()).Equals( denominador.por(aFraccion.getNumerador()));
	    }
	    
	    override public int GetHashCode() {
		    return numerador.GetHashCode() / denominador.GetHashCode();
	    }
	    
	    override public Numero mas(Numero sumando) {
            Fraccion sumandoComoFraccion = (Fraccion)sumando;
            Numero nuevoDenominador = denominador.por(sumandoComoFraccion.getDenominador());
            Numero nuevoNumerador1 = numerador.por(sumandoComoFraccion.getDenominador());
            Numero nuevoNumerador2 = denominador.por(sumandoComoFraccion.getNumerador());
            Numero nuevoNumerador = nuevoNumerador1.mas(nuevoNumerador2);

            return nuevoNumerador.dividido(nuevoDenominador);
        }
	
	    override public Numero por(Numero multiplicador) {
            Fraccion multiplicadorComoFraccion = (Fraccion)multiplicador;
            return numerador.por(multiplicadorComoFraccion.getNumerador()).
                dividido(denominador.por(multiplicadorComoFraccion.getDenominador()));
        }
	
	    override public Numero dividido(Numero divisor) {
            Fraccion divisorComoFraccion = (Fraccion)divisor;
            return numerador.por(divisorComoFraccion.getDenominador()).
                dividido(denominador.por(divisorComoFraccion.getNumerador()));
        }

    }
}
