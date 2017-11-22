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
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace C17_.Net_CustomerImport
{
    public class ValidLineReader : TextReader
    {
        private static String data = "C,Pepe,Sanchez,D,22333444\n" +
                                     "A,San Martin,3322,Olivos,1636,BsAs\n" +
                                     "A,Maipu,888,Florida,1122,Buenos Aires\n" +
                                     "C,Juan,Perez,C,23-25666777-9\n" +
                                     "A,Alem,1122,CABA,1001,CABA";

        private int lastLine = 0;

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
        private static ISession session;
        private static ITransaction transaction;
        private static TextReader lineReader;

        public void Persist()
        {

        }

        public void BeginImport()
        {
            new CustomerImporter(session, lineReader).ImportCustomers();
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
            var storeConfiguration = new StoreConfiguration();
            var configuration = Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard.ShowSql().ConnectionString("Data Source=CustomerImport.sdf"))
                .Mappings(m => m.AutoMappings.Add(AutoMap
                    .AssemblyOf<Customer>(storeConfiguration)
                    .Override<Customer>(map => map.HasMany(x => x.Addresses).Cascade.All())));

            var sessionFactory = configuration.BuildSessionFactory();
            new SchemaExport(configuration.BuildConfiguration()).Execute(true, true, false);
            session = sessionFactory.OpenSession();
            lineReader = new ValidLineReader();

            transaction = session.BeginTransaction();
        }

    }
}
