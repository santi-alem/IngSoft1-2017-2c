using System;
using System.Collections.Generic;
using System.IO;
using com.tenpines.advancetdd;

namespace C17_.Net_CustomerImport
{
    public class Supplier : IDataObject
    {
        public int Id { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string IdentificationType { get; set; }
        public virtual string IdentificationNumber { get; set; }
        public virtual IList<Customer> Customers { get; set; }
        public virtual IList<Address> Addresses { get; set; }

        public void AddAddress(Address newAddress)
        {
            Addresses.Add(newAddress);
        }

        public void AddCustomer(Customer newCustomer)
        {
            Customers.Add(newCustomer);
        }
    }
    public class SupplierImporter : IObjectImporter
    {
        private static ISupplierSystem supplierSystem;
        private static TextReader lineReader;
        private static string _line;
        private static Supplier _newSupplier;
        private static string[] _lineData;


        public SupplierImporter(ISupplierSystem systemFrom, TextReader lineReaderFrom)
        {
            supplierSystem = systemFrom;
            lineReader = lineReaderFrom;
        }

        public void Import()
        {
            while (hasNextLine())
            {
                ExtractRecord();
                ImportRecord();
            }

        }

        private static void ImportRecord()
        {
            if (nextLineIsSupplier())
            {
                addSupplierFromLine();
            }
            else if (nextLineIsAddress())
            {
                addAddressToSupplier();
            }
            else if(nextLineIsNewCustomer())
            {
                addCustomerToSupplier();
            }
            else if (nextLineIsExistingCustomer())
            {
                //addCostumerFromLine();
            }
        }

        private static void addCustomerToSupplier()
        {
            var newCustomer = new Customer();

            _newSupplier.AddCustomer(newCustomer);
            newCustomer.FirstName = _lineData[1];
            newCustomer.LastName = _lineData[2];
            newCustomer.IdentificationType = _lineData[3];
            newCustomer.IdentificationNumber = _lineData[4];
            
        }

        private static void addAddressToSupplier()
        {
            var newAddress = new Address();

            _newSupplier.AddAddress(newAddress);
            newAddress.StreetName = _lineData[1];
            newAddress.StreetNumber = Int32.Parse(_lineData[2]);
            newAddress.Town = _lineData[3];
            newAddress.ZipCode = Int32.Parse(_lineData[4]);
            newAddress.Province = _lineData[5];
        }

        private static void addSupplierFromLine()
        {
            _newSupplier = new Supplier();
            _newSupplier.SupplierName = _lineData[1];
            _newSupplier.IdentificationType = _lineData[2];
            _newSupplier.IdentificationNumber = _lineData[3];
            addSupplier(); ;
        }

        private static void addSupplier()
        {
            supplierSystem.AddSupplier(_newSupplier);
        }

        private static bool nextLineIsSupplier()
        {
            return _line.StartsWith("E");
        }
        private static bool nextLineIsAddress()
        {
            return _line.StartsWith("A");
        }
        private static bool nextLineIsNewCustomer()
        {
            return _line.StartsWith("NC");
        }
        private static bool nextLineIsExistingCustomer()
        {
            return _line.StartsWith("EC");
        }
        private static void ExtractRecord()
        {
            _lineData = _line.Split(',');

        }

        private static bool hasNextLine()
        {
            _line = lineReader.ReadLine();
            return _line != null;
        }
    }
}