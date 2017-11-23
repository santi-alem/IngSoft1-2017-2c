using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.tenpines.advancetdd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C17_.Net_CustomerImport
{
    [TestClass]
    public class SupplierTest
    {

        [TestInitialize]
        public void setUp()
        {
            system = new PersistentSupplierSystem();
            system.Start();
        }

        [TestCleanup]
        public void tearDown()
        {
            system.Close();
        }
        public void BeginImport(TextReader lineReader)
        {
            new SupplierImporter(system, lineReader).Import();
        }

        private static TextReader ValidLineReader()
        {
            var data = "S,Supplier1,D,123\n" +
            "NC,Pepe,Sanchez,D,22333444\n" +
            "EC, D,5456774\n" +
            "A, San Martin,3322, Olivos,1636, BsAs\n" +
            "A, Maipu,888,Florida,1122,Buenos Aires\n";

            return new LineReaderStub(data);
        }

        private ISupplierSystem system;

        [TestMethod]
        public void ImportingSupplierAddsSupplier()
        {
            BeginImport(ValidLineReader());

            var suppliers = system.GetAll<Supplier>();
            Assert.AreEqual(1, suppliers.Count());
        }
        
    }
}
