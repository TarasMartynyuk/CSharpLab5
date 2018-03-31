using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpLab5.LogicClasses
{
    static class ProcessFetcher
    {
        static readonly HashSet<string> UnreachableProcesses;

        static ProcessFetcher()
        {
            UnreachableProcesses = new HashSet<string>
            {
                "services",
                "svchost",
                "smss",
                "SecurityHealthService",
                "bash", // yup, on windows
                "wininit",
                "csrss",
                "init",
                "Memory Compression",
                "System",
                "Idle"
            };
        }

        public static IEnumerable<MyProcess> FetchProcesses()
        {
            //return new ObservableCollection<ProcessData>(
            //    Process.GetProcesses().Select(p => new ProcessData(p)));
            var res =  new List<MyProcess>();
           
            Process[] processes = Process.GetProcesses();

            // ugly, ugly hack but i do not understand why it throws access denied
            // when i've launched VS as admin, and added request for admin privileges in manifest file!
            // i also run as 64 process, 
            foreach (Process p in processes)
            {
                try
                {
                    //var p = processes[i];
                    if (UnreachableProcesses.Contains(p.ProcessName) || p.HasExited)
                        { continue; }

                    res.Add(new MyProcess(p));
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    Console.WriteLine($"crashed on name: {p.ProcessName} owner name: {ProcessUtils.GetProcessOwner(p)}");
                }
        }
            
            return res;
        }
    }
}
