using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Icy.JobProcessor
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, 
        InstanceContextMode = InstanceContextMode.Single, 
        IncludeExceptionDetailInFaults = true)]    
    internal class Processor : IJobProcesorInterface
    {
        private string _jobType;
        private string _currentJobId;
        private ProcessorStatus _status;

        public string ProcessorID { get; private set; }

        internal Processor(string id)
        {
            ProcessorID = id;
        }

        public ProcessorSummary Ping()
        {
            return new ProcessorSummary
            {
                ProcessingJobId = _currentJobId,
                Status = _status,
                ProcessingJobType = _jobType,
                ProcessorID = ProcessorID
            };
        }

        public JobExecutionSummary ExecuteJob(string jobId, string jobTypeName, object param)
        {
            var currentDir = Environment.CurrentDirectory;
            var output = string.Empty;
            var executionResult = JobExecutionResult.Failed;

            _jobType = jobTypeName;
            _currentJobId = jobId;
            _status = ProcessorStatus.Processing;
            
            try
            {
                Environment.CurrentDirectory = GetJobExecutionFolder(jobTypeName);
                var jobType = Type.GetType(jobTypeName);
                var job = (IJob)Activator.CreateInstance(jobType);
                job.Execute(param);
                executionResult = JobExecutionResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("----- BEGIN ERROR -----");
                Console.WriteLine("Unhandled Exception - " + ex.ToString());
                Console.WriteLine("-----  END ERROR  -----");
            }
            finally
            {
                _jobType = null;
                _currentJobId = null;
                _status = ProcessorStatus.Idle;
                Environment.CurrentDirectory = currentDir;
                output = ConsoleIOManager.ReadAll();
                ConsoleIOManager.ResetIO();
            }

            return new JobExecutionSummary
            {
                JobId = jobId,
                JobType = jobTypeName,
                Output = output,
                ProcessorID = this.ProcessorID,
                Result = executionResult
            };
        }

        private string GetJobExecutionFolder(string jobTypeName)
        {
            return Path.Combine(Environment.CurrentDirectory, jobTypeName);
        }
    }
}
