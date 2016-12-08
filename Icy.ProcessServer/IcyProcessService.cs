using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Icy.ProcessServer
{
    public partial class IcyProcessService : ServiceBase
    {
        private const string OldProcListFileName = "elder.lst";
        private const string CurrentProcListFileName = "chief.lst";
        private Dictionary<string, Process> _processors;

        public IcyProcessService()
        {
            InitializeComponent();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {            
            var chiefCount = Environment.ProcessorCount;
            _processors = new Dictionary<string, Process>();
            for (var i = 0; i < chiefCount; i++)
            {
                var guid = Guid.NewGuid();
                var procName = string.Format("proc-{0}", guid.ToString());
                var proc = StartProcessor(procName);
                _processors[procName] = proc;                
                Console.WriteLine(" - Process: " + procName + " started");
            }

            CreateCurrentProcList(_processors.Values.ToArray());
            KillOldProcessors();
        }

        protected override void OnStop()
        {
            KillOldProcessors().Join();
        }

        private static void CreateCurrentProcList(IEnumerable<Process> procList)
        {
            using (var writer = new StreamWriter(File.Open(CurrentProcListFileName, FileMode.Create, FileAccess.Write)))
            {
                foreach (var chief in procList) writer.WriteLine(chief.Id);
                writer.Flush();
                writer.Close();
            }
        }

        private static Process StartProcessor(string procName)
        {
            var pinfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @"Icy.JobProcessor.exe",
                Arguments = procName,
            };

            return Process.Start(pinfo);
        }
        
        private static Thread KillOldProcessors()
        {
            var thread = new Thread(new ThreadStart(() =>
            {
                var deadList = GetTheOldProcList();
                FindAnKillOldProcessors(deadList);
                if (File.Exists(OldProcListFileName)) File.Delete(OldProcListFileName);
                if (File.Exists(CurrentProcListFileName))
                {
                    File.Copy(CurrentProcListFileName, OldProcListFileName);
                    File.Delete(CurrentProcListFileName);
                }
            }))
            {
                IsBackground = true
            };

            thread.Start();

            return thread;
        }

        private static string[] GetTheOldProcList()
        {
            if (!File.Exists(OldProcListFileName)) return new string[0];

            using (var readers = new StreamReader(File.OpenRead(OldProcListFileName)))
            {
                var text = readers.ReadToEnd();
                readers.Close();
                return text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private static void FindAnKillOldProcessors(string[] deadList)
        {            
            foreach (var elderId in deadList.Select(s => int.Parse(s)))
            {
                try
                {
                    var elder = Process.GetProcessById(elderId);
                    elder.Kill();                    
                }
                catch (Exception) { }
            }
        }
    }
}
