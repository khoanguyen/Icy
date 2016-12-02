using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Icy
{
    [ServiceContract]
    public interface IJobProcesorInterface
    {
        [OperationContract]
        ProcessorSummary Ping();

        [OperationContract]
        JobExecutionSummary ExecuteJob(string jobId, string jobTypeName, object param);
    }
}
