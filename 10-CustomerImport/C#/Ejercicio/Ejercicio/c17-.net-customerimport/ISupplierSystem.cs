using System.Collections.Generic;
using System.Linq;
using com.tenpines.advancetdd;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace C17_.Net_CustomerImport
{
    public interface ISupplierSystem
    {
        void Start();
        void AddSupplier(Supplier supplierToPersist);
        void Close();
        IEnumerable<T> GetAll<T>() where T : IDataObject;
    }

    public class PersistentSupplierSystem : ISupplierSystem
    {
        private static ISession session;
        private static ITransaction transaction;

        public void Start()
        {
            OpenSession();
            BeginTransaction();
        }

        private static void OpenSession()
        {
            var storeConfiguration = new StoreConfiguration();
            var configuration = Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard.ShowSql().ConnectionString("Data Source=CustomerImport.sdf"))
                .Mappings(m => m.AutoMappings.Add(AutoMap
                    .AssemblyOf<Customer>(storeConfiguration)
                    .Override<Customer>(map => map.HasMany(x => x.Addresses).Cascade.All())
                    ));

            var sessionFactory = configuration.BuildSessionFactory();
            new SchemaExport(configuration.BuildConfiguration()).Execute(true, true, false);
            session = sessionFactory.OpenSession();
        }

        private static void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }
        public void AddSupplier(Supplier objectToPersist)
        {
            session.Persist(objectToPersist);
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>() where T : IDataObject
        {
            throw new System.NotImplementedException();
        }
    }

    public class TransientSupplierSystem : ISupplierSystem
    {
        private List<IDataObject> transientData = new List<IDataObject>();
        public void Start()
        {

        }

        public void AddSupplier(Supplier supplierToPersist)
        {
            transientData.Add(supplierToPersist);
            transientData.AddRange(supplierToPersist.Addresses);
            transientData.AddRange(supplierToPersist.Customers);
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>() where T : IDataObject
        {
            return transientData.OfType<T>();
        }
    }
}