using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.tenpines.advancetdd;

namespace C17_.Net_CustomerImport
{
    public interface ICustomerSystem
    {
        void Start();
        void Persist();
        void Close();
        IEnumerable<T> GetAll<T>() where T : IDataObject;
        void BeginImport();



    }
}
