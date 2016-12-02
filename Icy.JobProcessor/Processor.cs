using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icy.JobProcessor
{
    internal class Processor : IJobProcesorInterface
    {
        private string _jobType;
        private string _currentJobId;
        private ProcessorStatus _status;

        public ProcessorSummary Ping()
        {
            return new ProcessorSummary
            {
                ProcessingJobId = _currentJobId,
                Status = _status,
                ProcessingJobType = _jobType,
            };
        }

        public string ExecuteJob(string jobId, string jobTypeName, object param)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _jobType = null;
                _currentJobId = null;
                _status = ProcessorStatus.Idle;
            }
        }
    }
}
