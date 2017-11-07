using System;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Client.Inputs
{
    public class InputManager
    {
        /**
         * Hashtable which contains : Wrapper.Type & IProtobufReader 
         */
        private Hashtable Table { get; } = new Hashtable();

        /**
         * Status
         */
        private bool IsInitialized { get; set; } = false;
        
        /**
         * Run
         */
        public bool Run(NetworkStream stream, WriteManager writeManager, string input)
        {
            try
            {
                if (!input.StartsWith("/")) 
                    return writeManager.Run(stream, Wrapper.Type.Message, input);
                
                input = input.ToLower();
                foreach (DictionaryEntry entry in Table)
                {
                    if (!((string[]) entry.Key).Any(alias => Regex.Split(input, @"\s+")[0].Equals(alias)))
                        continue;
                    
                    if (writeManager.Run(stream, (Wrapper.Type) entry.Value, input))
                        return true;
                    
                    Console.Out.WriteLineAsync("Invalid Arguments");
                    return false;
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