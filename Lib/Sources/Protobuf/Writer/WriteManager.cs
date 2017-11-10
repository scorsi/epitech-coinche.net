using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Coinche.Protobuf.Writer
{
    public class WriteManager
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
        public bool Run(NetworkStream stream, Wrapper.Type type, string input = null)
        {
            try
            {
                return ((IWriter) Table[type]).Run(stream, input);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /**
         * Initialize
         */
        public void Initialize(Dictionary<Wrapper.Type, IWriter> handlers)
        {
            if (IsInitialized)
                return;
            
            AddEntries(handlers);
            IsInitialized = true;
        }
        
        /**
         * Add an entry to Table
         */
        private void AddEntry(Wrapper.Type type, IWriter reader)
        {
            Table.Add(type, reader);
        }
        
        /**
         * Add entries to Table
         */
        private void AddEntries(Dictionary<Wrapper.Type, IWriter> tableToAdd)
        {
            foreach (var entry in tableToAdd)
            {
                AddEntry(entry.Key, entry.Value);
            }
        }
    }
}