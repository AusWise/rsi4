using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string[] getFilenames();
        
        [OperationContract]
        System.IO.Stream GetStream(string filepath);

        [OperationContract]
        long getFileLength(string filepath);
    }
}
