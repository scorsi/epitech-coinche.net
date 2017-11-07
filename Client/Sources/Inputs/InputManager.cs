using System;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Client.Inputs
{
    public class InputManager
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
        public bool Run(NetworkStream ns, WriteManager wm, string input)
        {
            try
            {
                if (!input.StartsWith("/"))
                    return wm.Run(ns, Wrapper.Type.Message, input);
                
                input = input.ToLower();
                foreach (DictionaryEntry entry in Table)
                {
                    if (((string[]) entry.Key).Any(alias => input.StartsWith(alias)))
                    {
                        return wm.Run(ns, (Wrapper.Type) entry.Value, input);
                    }
                }
                Console.Out.WriteLineAsync("Invalid Command");
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /**
         * Initialize
         */
        public void Initialize(Hashtable infos)
        {
            if (IsInitialized)
                return;
            
            AddEntries(infos);
            IsInitialized = true;
        }
        
        /**
         * Add an entry to Table
         */
        private void AddEntry(string[] inputs, Wrapper.Type type)
        {
            Table.Add(inputs, type);
        }
        
        /**
         * Add entries to Table
         */
        private void AddEntries(Hashtable tableToAdd)
        {
            foreach (DictionaryEntry entry in tableToAdd)
            {
                AddEntry((string[]) entry.Key, (Wrapper.Type) entry.Value);
            }
        }
    }        
}