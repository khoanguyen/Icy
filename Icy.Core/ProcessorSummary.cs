using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Icy
{
    [DataContract]
    public class ProcessorSummary
    {
        [DataMember]
        public ProcessorStatus Status { get; set; }
        [DataMember]
        public string ProcessingJobId { get; set; }
        [DataMember]
        public string ProcessingJobType { get; set; }
        [DataMember]
        public string ProcessorID { get; set; }
    }
}
