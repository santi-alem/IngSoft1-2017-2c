using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.tenpines.advancetdd;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace C17_.Net_CustomerImport
{  
    
    [TestClass]
    public class CustomerTests
    {
        private ICustomerSystem system;

        [TestInitialize]
        public void setUp()
        {
            system = new TransientCustomerSystem();
            system.Start();
        }

        [TestCleanup]
        public void tearDown()
        {
            system.Close(); 
        }
        public void BeginImport(TextReader lineReader)
        {
            new CustomerImporter(system, lineReader).ImportCustomers();
        }


        [TestMethod]
        public void test01ImportCostumersImportsCostumers()
        {
            BeginImport(ValidLineReader());

            var customers = system.GetAllCustomers();
            //var customers = session.Query<Customer>().ToList();

            Assert.AreEqual(customers.Count(), 2);
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "22333444" && x.IdentificationType == "D"));
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "23-25666777-9" && x.IdentificationType == "C"));
        }

        private static TextReader ValidLineReader()
        {
            var data = "C,Pepe,Sanchez,D,22333444\n" +
                       "A,San Martin,3322,Olivos,1636,BsAs\n" +
                       "A,Maipu,888,Florida,1122,Buenos Aires\n" +
                       "C,Juan,Perez,C,23-25666777-9\n" +
                       "A,Alem,1122,CABA,1001,CABA";

            return new LineReaderStub(data);
        }

        [TestMethod]
        public void test02ImportCostumersImportsAddress()
        {
            BeginImport(ValidLineReader());
            
            var addresses = system.GetAllAddresses();

            Assert.AreEqual(addresses.Count(), 3);
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1122 && x.Province == "Buenos Aires"));
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1001 && x.Province == "CABA"));
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1636 && x.Province == "BsAs"));
        }

        [TestMethod]
        public void test03ImportShouldFailIfWrongCustomerParameterAmount()
        {
            try
            {
                BeginImport(LineReaderWithStreamWithLessParameters());
                Assert.IsTrue(false);
            }
            catch (Exception ex) //excepxion un toque generica , habria que hacer algo un toque mas especifico 
            {
                Assert.AreEqual(system.GetAllCustomers().Count(), 0);

            }
        }

        private static LineReaderStub LineReaderWithStreamWithLessParameters()
        {
            return new LineReaderStub("C,Pepe,Sanchez\n" + "A,San Martin,3322,Olivos,1636,BsAs\n");
        }

        private static LineReaderStub LineReaderWithStreamWithWrongFormatOrder()
        {
            return new LineReaderStub("A,San Martin,3322,Olivos,1636,BsAs\n" + "C,Pepe,Sanchez,D,22333444\n" );
        }
        private static LineReaderStub LineReaderWithEmptyLine()
        {
            return new LineReaderStub("A,San Martin,3322,Olivos,1636,BsAs\n" + "\n" + "C,Pepe,Sanchez,D,22333444\n" );
        }

        [TestMethod]
        public void test04ImportShouldFailIFWrongFormatOrder()
        {
            try
            {
                BeginImport(LineReaderWithStreamWithWrongFormatOrder());
                Assert.IsTrue(false);
            }
            catch (Exception ex) //excepxion un toque generica , habria que hacer algo un toque mas especifico 
            {
                Assert.AreEqual(system.GetAllCustomers().Count(), 0);
            }
        }
        [TestMethod]
        public void test05ImportShouldFailWithEmptyLine()
        {
            try
            {
                BeginImport(LineReaderWithEmptyLine());
                Assert.IsTrue(false);
            }
            catch (Exception ex) //excepxion un toque generica , habria que hacer algo un toque mas especifico 
            {
                Assert.AreEqual(system.GetAllCustomers().Count(), 0);
            }
        }
        //verificar cantidad de parametros,mas y menos en customer
        //verificar cantidad de parametros,mas y menos en address
        //verificar lineas vacias(espacios)
        //verificar que no se puede importar address sin customer
    }
}
