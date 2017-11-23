using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using com.tenpines.advancetdd;
using C17_.Net_CustomerImport;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace C17_.Net_CustomerImport
{
    public class LineReaderStub : TextReader
    {
        private String data ;
        private int lastLine = 0;

        public LineReaderStub(string data)
        {
            this.data = data;
        }

        public override String ReadLine()
        {
            var lineToRead = lastLine++;
            if (lineToRead >= data.Split('\n').Length)
            {
                return null;
            }
            var line = data.Split('\n')[lineToRead];

            return line;
        }

    }

    


    public class PersistentCustomerSystem : ICustomerSystem
    {
        public static ISession session;
        private static ITransaction transaction;

        public void AddCustomer(IDataObject objectToPersist)
        {
            session.Persist(objectToPersist);
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return session.Query<Supplier>().ToList();
        }
        public IEnumerable<Address> GetAllAddresses()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            session.Close();
            session.Dispose();
        }

        public IEnumerable<T> GetAll<T>() where T : IDataObject
        {
            return session.Query<T>().ToList();
        }




        public void Start()
        {
            OpenSession();
            BeginTransaction();
        }

        public void AddCustomer(Customer objectToPersist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        private static void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }

        private static void OpenSession()
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
        }
    }
}


public class TransientCustomerSystem : ICustomerSystem
{
    public List<Customer> persistedObjects = new List<Customer>();

    public void Start()
    {
    }

    
    public void AddCustomer(Customer objectToPersist)
    {
        persistedObjects.Add(objectToPersist);
    }

    public IEnumerable<Supplier> GetAllSuppliers()
    {
        return new List<Supplier>();
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return persistedObjects;
    }

    public IEnumerable<Address> GetAllAddresses()
    {
        List<Address> retAddr = new List<Address>();
        foreach (var cust in persistedObjects)
        {
            retAddr.AddRange(cust.Addresses);
        }
        return retAddr;
    }

    public void Close()
    {
    }

    public IEnumerable<T> GetAll<T>() where T : IDataObject
    {
        return persistedObjects.OfType<T>();
    }


}