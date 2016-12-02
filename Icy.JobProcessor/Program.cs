using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.Read();
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
