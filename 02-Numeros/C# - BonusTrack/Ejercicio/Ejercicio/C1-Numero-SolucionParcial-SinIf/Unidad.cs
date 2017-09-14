using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf
{
    public class Unidad : Numero
    {
        public override bool esCero()
        {
            return false;
        }

        public override bool esUno()
        {
            return true;
        }

        private Numero sumarNumero(Entero sumando)
        {
            return new Entero(1 + sumando.getValue());
        }

        private Numero sumarNumero(Fraccion sumando)
        {
            Numero nuevoNumerador = sumando.getDenominador().mas(sumando.getNumerador());
            return Fraccion.dividir((Entero)nuevoNumerador, sumando.getDenominador());
        }

        private Numero sumarNumero(Unidad sumando)
        {
            return new Entero(2);
        }

        private Numero sumarNumero(Cero sumando)
        {
            return new Unidad();
        }
        public override Numero mas(Numero sumando)
        {
            dynamic a = Convert.ChangeType(sumando, sumando.GetType());
            return sumarNumero(a);
        }

        private Numero porNumero(Unidad multiplicador)
        {
            return new Unidad();
        }
        private Numero porNumero(Cero multiplicador)
        {
            return new Cero();
        }
        private Numero porNumero(Fraccion multiplicador)
        {
            dynamic numeradorMultiplicadorNoTipado = Convert.ChangeType(multiplicador.getNumerador(), multiplicador.getNumerador().GetType());
            return Fraccion.dividir(numeradorMultiplicadorNoTipado, multiplicador.getDenominador());
        }
        private Numero porNumero(Entero multiplicador)
        {
            return new Entero(multiplicador.getValue());
        }

        public override Numero por(Numero multiplicador)
        {
            dynamic a = Convert.ChangeType(multiplicador, multiplicador.GetType());
            return sumarNumero(a);
        }

        private Numero divididoNumero(Entero divisor)
        {
            return Fraccion.dividir(new Unidad(), divisor);
        }
        private Numero divididoNumero(Fraccion divisor)
        {
            dynamic numeradorDivisorNoTipado = Convert.ChangeType(divisor.getNumerador(), divisor.getNumerador().GetType());
            return Fraccion.dividir(divisor.getDenominador(), numeradorDivisorNoTipado);
        }
        private Numero divididoNumero(Unidad divisor)
        {
            return new Unidad();
        }
        private Numero divididoNumero(Cero divisor)
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
        }
        public override Numero dividido(Numero divisor)
        {
            dynamic a = Convert.ChangeType(divisor, divisor.GetType());
            return divididoNumero(a);
        }

        private Numero divisionEnteraAux(Cero divisor)
        {
            throw new Exception(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO);
        }
        private Unidad divisionEnteraAux(Unidad divisor)
        {
            return new Unidad();
        }

        private Cero divisionEnteraAux(Entero divisor)
        {
            return new Cero();
        }

        public Numero divisionEntera(Numero otroEntero)
        {
            dynamic otroEnteroNoTipado = Convert.ChangeType(otroEntero, otroEntero.GetType());
            return divisionEnteraAux(otroEnteroNoTipado);
        }

        public override bool Equals(object unidad)
        {
            return unidad is Unidad;
        }
    }
}
