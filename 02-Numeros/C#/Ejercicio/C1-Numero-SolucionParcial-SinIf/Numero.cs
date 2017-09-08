/*
 * Developed by 10Pines SRL
 * License: 
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/ 
 * or send a letter to Creative Commons, 444 Castro Street, Suite 900, Mountain View, 
 * California, 94041, USA.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C1_Numero_SolucionParcial_SinIf
{
    public abstract class Numero
    {
	    public abstract bool esCero();
	    public abstract bool esUno();

	    public abstract Numero mas(Numero sumando);
        public abstract Numero por(Numero multiplicador);
        public abstract Numero dividido(Numero divisor);

        public static string DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO = "No se puede dividir por cero";
    }
}
