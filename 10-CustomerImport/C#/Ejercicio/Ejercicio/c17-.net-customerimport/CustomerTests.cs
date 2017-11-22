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
            system = new PersistentCustomerSystem();

            system.Start();
        }

        [TestCleanup]
        public void tearDown()
        {
            system.Close(); 
        }


        [TestMethod]
        public void test01ImportCostumersImportsCostumers()
        {
            system.BeginImport();

            var customers = system.GetAll<Customer>();
            //var customers = session.Query<Customer>().ToList();

            Assert.AreEqual( customers.Count(), 2);
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "22333444" && x.IdentificationType == "D"));
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "23-25666777-9" && x.IdentificationType == "C"));
        }
        [TestMethod]
        public void test02ImportCostumersImportsAddress()
        {
            system.BeginImport();
            
            var addresses = system.GetAll<Address>();

            Assert.AreEqual( addresses.Count(), 3);
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1122 && x.Province == "Buenos Aires"));
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1001 && x.Province == "CABA"));
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1636 && x.Province == "BsAs"));
        }

        //verificar cantidad de parametros,mas y menos en customer
        //verificar cantidad de parametros,mas y menos en address
        //verificar lineas vacias(espacios)
        //verificar que no se puede importar address sin customer
    }
}
