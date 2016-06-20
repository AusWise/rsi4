using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILoadCallback))]
    public interface ILoad
    {
        [OperationContract(IsOneWay = true)]
        void Dodaj(System.IO.Stream stream);
    }

    public interface ILoadCallback
    {
        [OperationContract(IsOneWay = true)]
        void updateFileList();
    }
}
