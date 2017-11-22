using System;
using System.Collections.Generic;
using System.IO;
using C17_.Net_CustomerImport;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;


namespace com.tenpines.advancetdd
{

    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type == typeof(Customer) || type == typeof(Address);
        }
    }

    public class CustomerImporter
    {
        private static string _line;
        private static string[] _lineData;
        private static ISession sessionTo;
        private static TextReader lineReaderFrom;
        private static Customer _newCustomer;

        public  void ImportCustomers()
        {
            while (hasNextLine())
            {
                ExtractRecord();
                ImportRecord();
            }

        }

        private void ImportRecord()
        {
            if (nextLineIsCostumer())
            {
                addCostumerFromLine();
            }
            else if (nextLineIsAddress())
            {
                addAddressToCostumer();
            }
        }

        private static void ExtractRecord()
        {
            _lineData = _line.Split(',');
            
        }

        public CustomerImporter(ISession session, TextReader lineReader)
        {
            sessionTo = session;
            lineReaderFrom = lineReader;
        }

        private static void addAddressToCostumer()
        {
            var newAddress = new Address();

            _newCustomer.AddAddress(newAddress);
            newAddress.StreetName = _lineData[1];
            newAddress.StreetNumber = Int32.Parse(_lineData[2]);
            newAddress.Town = _lineData[3];
            newAddress.ZipCode = Int32.Parse(_lineData[4]);
            newAddress.Province = _lineData[5];
        }

        private static void addCostumerFromLine()
        {
            _newCustomer = new Customer();
            _newCustomer.FirstName = _lineData[1];
            _newCustomer.LastName = _lineData[2];
            _newCustomer.IdentificationType = _lineData[3];
            _newCustomer.IdentificationNumber = _lineData[4];
            sessionTo.Persist(_newCustomer);
        }

        private static bool nextLineIsAddress()
        {
            return _line.StartsWith("A");
        }

        private static bool nextLineIsCostumer()
        {
            return _line.StartsWith("C");
        }

        private static bool hasNextLine()
        {
            _line = lineReaderFrom.ReadLine();
            return _line != null;
        }
    }
    public class Address : IDataObject
    {
        public virtual Guid Id { get; set; }
        public virtual string StreetName { get; set; }
        public virtual int StreetNumber { get; set; }
        public virtual string Town { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string Province { get; set; }
    }


    public class Customer : IDataObject
    {
        public virtual long Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string IdentificationType { get; set; }
        public virtual string IdentificationNumber { get; set; }
        public virtual IList<Address> Addresses { get; set; }

        public Customer()
        {
            Addresses = new List<Address>();
        }

        public virtual void AddAddress(Address anAddress)
        {
            Addresses.Add(anAddress);
        }

           public static void Main(string[] args)
        {
            try
            {
                //CustomerImporter.ImportCustomers(CustomerTests.createSession(),);
            }
            catch (Exception e)
            {
                Console.Out.Write(e.Message);
            }
        }
    }


}
