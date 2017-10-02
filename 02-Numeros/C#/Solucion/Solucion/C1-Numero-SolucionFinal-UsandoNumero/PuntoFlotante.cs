using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionFinal_UsandoNumero
{
    public class PuntoFlotante: Numero
    {
        public override bool esCero()
        {
            throw new NotImplementedException();
        }

        public override bool esUno()
        {
            throw new NotImplementedException();
        }

        public override Numero mas(Numero sumando)
        {
            return sumando.masPuntoFlotante(this);
        }

        public override Numero masEntero(Entero sumando)
        {
            throw new NotImplementedException();
        }

        public override Numero masFraccion(Fraccion sumando)
        {
            throw new NotImplementedException();
        }

        public override Numero por(Numero multiplicador)
        {
            throw new NotImplementedException();
        }

        public override Numero porEntero(Entero multiplicador)
        {
            throw new NotImplementedException();
        }

        public override Numero porFraccion(Fraccion multiplicador)
        {
            throw new NotImplementedException();
        }

        public override Numero dividido(Numero divisor)
        {
            throw new NotImplementedException();
        }

        public override Numero dividirEntero(Entero dividendo)
        {
            throw new NotImplementedException();
        }

        public override Numero dividirFraccion(Fraccion dividendo)
        {
            throw new NotImplementedException();
        }
    }
}
