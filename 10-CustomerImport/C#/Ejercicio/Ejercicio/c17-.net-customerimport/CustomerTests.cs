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
        private static ISession session;
        private static ITransaction transaction;
        private static StreamReader lineReader;
        private static FileStream fileStream;
        [TestInitialize]
        public void setUp()
        {
            var storeConfiguration = new StoreConfiguration();
            var configuration = Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard.ShowSql().ConnectionString("Data Source=CustomerImport.sdf"))
                .Mappings(m => m.AutoMappings.Add(AutoMap
                    .AssemblyOf<Customer>(storeConfiguration)
                    .Override<Customer>(map => map.HasMany(x => x.Addresses).Cascade.All())));

            var sessionFactory = configuration.BuildSessionFactory();
            new SchemaExport(configuration.BuildConfiguration()).Execute(true, true, false);
            session = sessionFactory.OpenSession();

            fileStream = new System.IO.FileStream("input.txt", FileMode.Open);
            lineReader = new StreamReader(fileStream);

            transaction = session.BeginTransaction();


        }

        [TestCleanup]
        public void tearDown()
        {
            transaction.Commit();
            fileStream.Close();
            session.Close();
            session.Dispose();
        }


        [TestMethod]
        public void test01ImportCostumersImportsCostumers()
        {
            ImportCustomers();
            
            var customers = session.Query<Customer>().ToList();

            Assert.AreEqual((int) customers.Count(), 2);
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "22333444" && x.IdentificationType == "D"));
            Assert.IsTrue(customers.Any(x => x.IdentificationNumber == "23-25666777-9" && x.IdentificationType == "C"));
        }
        [TestMethod]
        public void test02ImportCostumersImportsAddress()
        {
            ImportCustomers();
            
            var addresses = session.Query<Address>().ToList();

            Assert.AreEqual((int) addresses.Count(), 3);
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1122 && x.Province == "Buenos Aires"));
            Assert.IsTrue(addresses.Any(x => x.ZipCode == 1636 && x.Province == "BsAs"));
        }

        public static void ImportCustomers()
        {
            Customer newCustomer = null;
            var line = lineReader.ReadLine();
            while (line != null)
            {
                if (line.StartsWith("C"))
                {
                    var customerData = line.Split(',');
                    newCustomer = new Customer();
                    newCustomer.FirstName = customerData[1];
                    newCustomer.LastName = customerData[2];
                    newCustomer.IdentificationType = customerData[3];
                    newCustomer.IdentificationNumber = customerData[4];
                    session.Persist(newCustomer);
                }
                else if (line.StartsWith("A"))
                {
                    var addressData = line.Split(',');
                    var newAddress = new Address();

                    newCustomer.AddAddress(newAddress);
                    newAddress.StreetName = addressData[1];
                    newAddress.StreetNumber = Int32.Parse(addressData[2]);
                    newAddress.Town = addressData[3];
                    newAddress.ZipCode = Int32.Parse(addressData[4]);
                    newAddress.Province = addressData[5];
                }

                line = lineReader.ReadLine();
            }

        }
    }
}
