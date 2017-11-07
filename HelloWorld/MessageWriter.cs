using System;
using System.Configuration;

namespace MsgWriter
{
    using System;

    public interface InfoWriter
    {
        string Message { get; set; }

        bool Write();

    }

    public class MessageWriter : InfoWriter
    {

        private object _target;
        public object Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private Func<string, object, bool> _write;

        public delegate void WriterFunc(string msg);
        public void SetWriterFuncNoTarget(WriterFunc w)
        {
            _write = (m,t) => { w(m); return true; };
        }
        public delegate void WriterFuncWithTarget(string msg, object target);
        public void SetWriterFunc(WriterFuncWithTarget w)
        {
            _write = (m, t) => { w(m, t); return true; };
        }

        public MessageWriter()
        {
            try
            {
                Target = (ConfigurationManager.AppSettings["target"] ?? "null").ToLower();
                Message = (ConfigurationManager.AppSettings["message"] ?? "Hello World");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught accessing configuration file.", e);
                Console.WriteLine("Attempting to continue with default values.");
                Target = "null";
                Message = "Hello World";
            }

            switch (Target.ToString()) //future predefined target writers go in here
            {
                case "console":
                default:
                    _write = (m, t) => { Console.Write(m); return true; };
                    break;
            }

        }

        public MessageWriter(object target, string message, WriterFuncWithTarget writer)
        {
            Target = target;
            Message = message;
            SetWriterFunc(writer);
        }

        public bool Write()
        {
            try
            {
                return _write(_message, _target);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }
    }
}
