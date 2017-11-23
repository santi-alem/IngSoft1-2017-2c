using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.tenpines.advancetdd;
using NHibernate;

namespace C17_.Net_CustomerImport
{
    public interface ICustomerSystem
    {
        void Start();
        void AddCustomer(Customer objectToPersist);

        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Address> GetAllAddresses();
        IEnumerable<Supplier> GetAllSuppliers();

        void Close();
        IEnumerable<T> GetAll<T>() where T : IDataObject;
    }
}
