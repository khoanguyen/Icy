using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Icy
{
    [DataContract]
    public class JobExecutionSummary
    {       
        public string JobId { get; set; }
        public string JobType { get; set; }
        public JobExecutionResult Result { get; set; }
        public string Output { get; set; }
    }
}
