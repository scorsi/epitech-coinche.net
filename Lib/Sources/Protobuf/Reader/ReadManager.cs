using System;
using System.Collections;
using System.Net.Sockets;

namespace Coinche.Protobuf.Reader
{
    public class ReadManager
    {
        /**
         * Hashtable which contains : Wrapper.Type & IProtobufReader 
         */
        private Hashtable Table { get; set; } = new Hashtable();

        /**
         * Status
         */
        private bool IsInitialized { get; set; } = false;
        
        /**
         * Run
         */
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var header = new byte[2];
            if (stream.Read(header, 0, 2) != 2) return false;
            var type = (Wrapper.Type) BitConverter.ToInt16(header, 0);

            try
            {
                return ((IReader) Table[type]).Run(stream, clientId);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /**
         * Initialize
         */
        public void Initialize(Hashtable handlers)
        {
            if (IsInitialized)
                return;
            
            AddEntries(handlers);
            IsInitialized = true;
        }
        
        /**
         * Add an entry to Table
         */
        private void AddEntry(Wrapper.Type type, IReader reader)
        {
            Table.Add(type, reader);
        }
        
        /**
         * Add entries to Table
         */
        private void AddEntries(Hashtable tableToAdd)
        {
            foreach (DictionaryEntry entry in tableToAdd)
            {
                AddEntry((Wrapper.Type) entry.Key, (IReader) entry.Value);
            }
        }
    }
}