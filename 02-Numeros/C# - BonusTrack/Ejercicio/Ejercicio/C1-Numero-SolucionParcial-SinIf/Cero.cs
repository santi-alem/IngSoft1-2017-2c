using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf
{
    public class Cero : Numero
    {
        public override bool esCero()
        {
            return true;
        }

        public override bool esUno()
        {
            return false;
        }

        private Numero sumarNumero(Entero sumando)
        {
            return new Entero(sumando.getValue());
        }

        private Numero sumarNumero(Fraccion sumando)
        {
            dynamic numeradorSumandoNoTipado = Convert.ChangeType(sumando.getNumerador(), sumando.getNumerador().GetType());
            return Fraccion.dividir(numeradorSumandoNoTipado, sumando.getDenominador());
        }

        private Numero sumarNumero(Unidad sumando)
        {
            return new Unidad();
        }

        private Numero sumarNumero(Cero sumando)
        {
            return new Cero();
        }
        public override Numero mas(Numero sumando)
        {
            dynamic a = Convert.ChangeType(sumando, sumando.GetType());
            return sumarNumero(a);
        }

        public override Numero por(Numero multiplicador)
        {
            return new Cero();
        }
        private Numero divididoNumero(Entero divisor)
        {
            return new Cero();
        }
        private Numero divididoNumero(Fraccion divisor)
        {
            return new Cero();
        }
        private Numero divididoNumero(Unidad divisor)
        {
            return new Cero();
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

        public override bool Equals(object cero)
        {
            return cero is Cero;
        }

    }
}
