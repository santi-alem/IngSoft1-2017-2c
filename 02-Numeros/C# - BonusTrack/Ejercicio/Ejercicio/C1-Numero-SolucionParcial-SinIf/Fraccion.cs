using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C1_Numero_SolucionParcial_SinIf 
{
    public class Fraccion: Numero
    {
	    protected Numero numerador;
	    protected Entero denominador;
	
	    public static Numero dividir(Entero dividendo, Entero divisor )
        {
            Numero maximoComunDivisor = dividendo.maximoComunDivisorCon(divisor);
		    Numero numerador = dividendo.divisionEntera(maximoComunDivisor);
		    Numero denominador = divisor.divisionEntera(maximoComunDivisor);

            dynamic numeradorNoTipado = Convert.ChangeType(numerador, numerador.GetType());
            dynamic denominadorNoTipado = Convert.ChangeType(denominador, denominador.GetType());

            Assert.IsFalse(numerador is Entero && numerador.Equals(new Entero(1)));
            Assert.IsFalse(numerador is Entero && numerador.Equals(new Entero(0)));

            return dividirCoprimos(numeradorNoTipado, denominadorNoTipado);   
	    }

        public static Numero dividir<T>(T dividendo, Cero divisor) where T : Numero
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
        }
       

        public static Numero dividir(Cero dividendo, Unidad divisor)
        {
            return new Cero();
        }
        public static Numero dividir(Unidad dividendo, Unidad divisor)
        {
            return new Unidad();

        }
        public static Numero dividir(Entero dividendo, Unidad divisor)
        {
            return new Entero(dividendo.getValue());

        }
        public static Numero dividir(Cero dividendo, Entero divisor)
        {
            return new Cero();
        }

        public static Numero dividir(Unidad dividendo, Entero divisor)
        {
            return new Fraccion(new Entero(1), divisor);
        }

        private static Entero dividirCoprimos(Entero dividendo, Unidad divisor)
        {
            return new Entero(dividendo.getValue());
        }

        private static Fraccion dividirCoprimos(Entero dividendo, Entero divisor)
        {
            return new Fraccion(dividendo, divisor);
        }
        private static Unidad dividirCoprimos(Unidad dividendo, Unidad divisor)
        {
            return new Unidad();
        }

        private static Fraccion dividirCoprimos(Unidad dividendo, Entero divisor)
        {
            return new Fraccion(new Entero(1), divisor);
        }








        private Fraccion(Entero numerador, Entero denominador){
		    this.numerador = numerador;
		    this.denominador = denominador;
	    }

	
	    public Numero getNumerador (){
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
	        var o = anObject as Fraccion;
	        return o != null && this.equals(o);
	    }
	
	    public bool equals(Fraccion aFraccion){
		    return numerador.por(aFraccion.getDenominador()).Equals( denominador.por(aFraccion.getNumerador()));
	    }

        public override int GetHashCode() {
		    return numerador.GetHashCode() / denominador.GetHashCode();
	    }
        private Numero sumarNumero(Unidad sumando)
        {
            Entero nuevoNumerador = (Entero)denominador.mas(numerador);
            return dividir(nuevoNumerador, denominador);
        }
        private Numero sumarNumero(Cero sumando)
        {
            dynamic numeradorNoTipado = Convert.ChangeType(numerador, numerador.GetType());
            return dividir(numeradorNoTipado, denominador);
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
        private Numero porNumero(Unidad multiplicador)
        {
            dynamic numeradorNoTipado = Convert.ChangeType(numerador, numerador.GetType());
            return dividir(numeradorNoTipado, denominador);
        }
        private Numero porNumero(Cero multiplicador)
        {
            return new Cero();
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
        private Numero divididoNumero(Unidad divisor)
        {
            dynamic numeradorNoTipado = Convert.ChangeType(numerador, numerador.GetType());
            return dividir(numeradorNoTipado, denominador);
        }
        private Numero divididoNumero(Cero divisor)
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
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
