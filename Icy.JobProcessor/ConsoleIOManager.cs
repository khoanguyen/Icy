using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icy.JobProcessor
{
    public static class ConsoleIOManager
    {
        private static MemoryStream _output;
        private static StreamReader _reader;
        private static StreamWriter _writer;
        private static TextWriter _defaultWriter;

        static ConsoleIOManager()
        {
            _defaultWriter = Console.Out;
        }

        public static void CaptureConsole()
        {
            _output = new MemoryStream();
            _reader = new StreamReader(_output);
            _writer = new StreamWriter(_output);
            _writer.AutoFlush = true;

            Console.SetOut(_writer);
        }

        public static string ReadAll()
        {
            _output.Seek(0, SeekOrigin.Begin);
            return _reader.ReadToEnd();
        }

        public static long GetCurrentOutputLength()
        {
            return _output.Length;
        }

        public static void RestoreDefault()
        {
            Finalize();
            Console.SetOut(_defaultWriter);
        }           

        public static void ResetIO()
        {
            Finalize();
            CaptureConsole();        
        }

        public static void Finalize()
        {
            if (_reader != null) _reader.Close();
            if (_writer != null) _writer.Close();
            if (_output != null) _output.Dispose();            
        }
    }
}
