using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Icy.JobProcessor
{
    class Program
    {

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ConsoleIOManager.CaptureConsole();
            try
            {
                var serviceName = args[0];
                var processor = new Processor(serviceName);
                var host = new ServiceHost(processor);
                host.AddServiceEndpoint(typeof(IJobProcesorInterface),
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                    "net.pipe://localhost/" + serviceName);

                host.Open();

                Console.WriteLine("Enter to exit...");
                Console.Read();
                host.Close();
                Console.WriteLine("Closed");
            }
            finally
            {
                try
                {
                    ConsoleIOManager.Finalize();
                }
                catch (Exception) { }
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
                Console.WriteLine("----- BEGIN ERROR -----");
                Console.WriteLine("Unhandled Exception - " + e.ExceptionObject.ToString());
                Console.WriteLine("-----  END ERROR  -----");
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private static void Run()
        {
            throw new DivideByZeroException();
        }
    }
}
