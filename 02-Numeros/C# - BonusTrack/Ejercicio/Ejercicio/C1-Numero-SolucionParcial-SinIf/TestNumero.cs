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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C1_Numero_SolucionParcial_SinIf
{
    [TestClass]
    public class TestNumero
    {
        protected Numero cero;
        protected Numero uno;
        protected Numero dos;
        protected Numero tres;
        protected Numero cuatro;
        protected Numero cinco;

        protected Numero unQuinto;
        protected Numero dosQuintos;
        protected Numero tresQuintos;
        protected Numero dosVeinticincoavos;
        protected Numero unMedio;
        protected Numero cincoMedios;
        protected Numero seisQuintos;
        protected Numero cuatroMedios;
        protected Numero dosCuartos;

        [TestInitialize]
        public void setUp()
        {
            //inicializar los numeros
            cero = new Cero();
            uno = new Unidad();
            dos = new Entero(2);
            tres = new Entero(3);
            cuatro = new Entero(4);
            cinco = new Entero(5);

            unQuinto = uno.dividido(cinco);
            dosQuintos = dos.dividido(cinco);
            tresQuintos = tres.dividido(cinco);
            dosVeinticincoavos = dos.dividido(new Entero(25));
            unMedio = uno.dividido(dos);
            cincoMedios = cinco.dividido(dos);
            seisQuintos = new Entero(6).dividido(cinco);
            cuatroMedios = cuatro.dividido(dos);
            dosCuartos = dos.dividido(cuatro);
        }

        [TestMethod]
        public void test01EsCeroDevuelveTrueSoloParaElCero()
        {
            Assert.IsTrue(cero.esCero());
            Assert.IsFalse(uno.esCero());
        }

        [TestMethod]
        public void test02EsUnoDevuelveTrueSoloParaElUno()
        {
            Assert.IsTrue(uno.esUno());
            Assert.IsFalse(cero.esUno());
        }

        [TestMethod]
        public void test03SumaDeEnteros()
        {
            Assert.AreEqual(dos, uno.mas(uno));
        }

        [TestMethod]
        public void test04MultiplicacionDeEnteros()
        {
            Assert.AreEqual(cuatro, dos.por(dos));
        }

        [TestMethod]
        public void test05DivisionDeEnteros()
        {
            Numero uno2 = dos.dividido(dos);
            Assert.AreEqual(uno, uno2);
        }

        [TestMethod]
        public void test06SumaDeFracciones()
        {
            Numero sieteDecimos = new Entero(7).dividido(new Entero(10));
            Assert.AreEqual(sieteDecimos, unQuinto.mas(unMedio));
            /* 
             * La suma de fracciones es:
             * 
             * a/b + c/d = (a.d + c.b) / (b.d)
             * 
             * SI ESTAN PENSANDO EN LA REDUCCION DE FRACCIONES NO SE PREOCUPEN!
             * TODAVIA NO SE ESTA TESTEANDO ESE CASO
             */
        }

        [TestMethod]
        public void test07MultiplicacionDeFracciones()
        {
            Assert.AreEqual(dosVeinticincoavos, unQuinto.por(dosQuintos));
            /* 
             * La multiplicación de fracciones es:
             * 
             * (a/b) * (c/d) = (a.c) / (b.d)
             * 
             * SI ESTAN PENSANDO EN LA REDUCCION DE FRACCIONES NO SE PREOCUPEN!
             * TODAVIA NO SE ESTA TESTEANDO ESE CASO
             */
        }

        [TestMethod]
        public void test08DivisionDeFracciones()
        {
            Assert.AreEqual(cincoMedios, unMedio.dividido(unQuinto));
            /* 
             * La división de fracciones es:
             * 
             * (a/b) / (c/d) = (a.d) / (b.c)
             * 
             * SI ESTAN PENSANDO EN LA REDUCCION DE FRACCIONES NO SE PREOCUPEN!
             * TODAVIA NO SE ESTA TESTEANDO ESE CASO
             */
        }

        /* 
         * Ahora empieza lo lindo! - Primero hacemos que se puedan sumar enteros con fracciones
         * y fracciones con enteros 
         */
        [TestMethod]
        public void test09SumaDeEnteroYFraccion()
        {
            Assert.AreEqual(seisQuintos, uno.mas(unQuinto));
        }

        [TestMethod]
        public void test10SumaDeFraccionYEntero()
        {
            Assert.AreEqual(seisQuintos, unQuinto.mas(uno));
        }

        /* 
         * Hacemos lo mismo para la multipliación
         */
        [TestMethod]
        public void test11MultiplicacionDeEnteroPorFraccion()
        {
            Assert.AreEqual(dosQuintos, dos.por(unQuinto));
        }

        [TestMethod]
        public void test12MultiplicacionDeFraccionPorEntero()
        {
            Assert.AreEqual(dosQuintos, unQuinto.por(dos));
        }

        /* 
         * Hacemos lo mismo para la division
         */
        [TestMethod]
        public void test13DivisionDeEnteroPorFraccion()
        {
            Assert.AreEqual(cincoMedios, uno.dividido(dosQuintos));
        }

        [TestMethod]
        public void test14DivisionDeFraccionPorEntero()
        {
            Assert.AreEqual(dosVeinticincoavos, dosQuintos.dividido(cinco));
        }

        /* 
         * Ahora si empezamos con problemas de reducción de fracciones
         */
        [TestMethod]
        public void test15UnaFraccionPuedeSerIgualAUnEntero()
        {
            Assert.AreEqual(dos, cuatroMedios);
        }

        [TestMethod]
        public void test16LasFraccionesAparentesSonIguales()
        {
            Assert.AreEqual(unMedio, dosCuartos);
            /*
             * Las fracciones se reducen utilizando el maximo comun divisor (mcd)
             * Por lo tanto, para a/b, sea c = mcd (a,b) => a/b reducida es:
             * (a/c) / (b/c).
             * 
             * Por ejemplo: a/b = 2/4 entonces c = 2. Por lo tanto 2/4 reducida es:
             * (2/2) / (4/2) = 1/2
             * 
             * Para obtener el mcd pueden usar el algoritmo de Euclides que es:
             * 
             * mcd (a,b) = 
             * 		si b = 0 --> a
             * 		si b != 0 -->mcd(b, restoDeDividir(a,b))
             * 	
             * Ejemplo:
             * mcd(2,4) ->
             * mcd(4,restoDeDividir(2,4)) ->
             * mcd(4,2) ->
             * mcd(2,restoDeDividir(4,2)) ->
             * mcd(2,0) ->
             * 2
             */
        }

        [TestMethod]
        public void test17LaSumaDeFraccionesPuedeDarEntero()
        {
            Numero unity = unMedio.mas(unMedio);
            Assert.AreEqual(uno, unMedio.mas(unMedio));
        }

        [TestMethod]
        public void test18LaMultiplicacionDeFraccionesPuedeDarEntero()
        {
            Assert.AreEqual(dos, cuatro.por(unMedio));
        }

        [TestMethod]
        public void test19LaDivisionDeEnterosPuedeDarFraccion()
        {
            Assert.AreEqual(unMedio, dos.dividido(cuatro));
        }


        [TestMethod]
        public void test20LaDivisionDeFraccionesPuedeDarEntero()
        {
            Assert.AreEqual(uno, unMedio.dividido(unMedio));
        }

        [TestMethod]
        public void test21NoSePuedeDividirEnteroPorCero()
        {
            try
            {
                uno.dividido(cero);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO, e.Message);
            }
        }

        [TestMethod]
        public void test22NoSePuedeDividirFraccionPorCero()
        {
            try
            {
                unQuinto.dividido(cero);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO, e.Message);
            }
        }

        // Este test puede ser redundante dependiendo de la implementación realizada 
        [TestMethod]
        public void test23NoSePuedeCrearFraccionConDenominadorCero()
        {
            try
            {
                crearFraccionCon(uno, cero);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(Numero.DESCRIPCION_DE_ERROR_NO_SE_PUEDE_DIVIDIR_POR_CERO, e.Message);
            }
        }

        public Numero crearFraccionCon(Numero numerador, Numero denominador)
        {
            return numerador.dividido(denominador);
        }
    }
}
