using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpLab5.LogicClasses
{
    static class ProcessFetcher
    {
        static readonly HashSet<string> InaccessibleProcesses;

        static ProcessFetcher()
        {
            InaccessibleProcesses = new HashSet<string>
            {
                "bash", // yup, on windows
                "services",
                "svchost",
                "smss",
                "SecurityHealthService",
                "wininit",
                "csrss",
                "init",
                "Memory Compression",
                "System",
                "Idle",
                "git",
                "git-remote-http"
            };
        }

        public static IEnumerable<ProcessData> FetchProcesses()
        {
            //return new ObservableCollection<ProcessData>(
            //    Process.GetProcesses().Select(p => new ProcessData(p)));
            var res =  new List<ProcessData>();
           
            Process[] processes = Process.GetProcesses();

            // ugly, ugly hack but i do not understand why it throws access denied
            // when i've launched VS as admin, and added request for admin privileges in manifest file!
            // i also run as 64 process, 
            foreach (Process p in processes)
            {
                try
                {
                    //var p = processes[i];
                    if (InaccessibleProcesses.Contains(p.ProcessName) || p.HasExited)
                        { continue; }

                    res.Add(new ProcessData(p));
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
