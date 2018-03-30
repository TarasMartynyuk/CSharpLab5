using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpLab5.LogicClasses
{
    static class ProcessFetcher
    {
        public static IEnumerable<ProcessData> FetchProcesses()
        {
                //return new ObservableCollection<ProcessData>(
                //    Process.GetProcesses().Select(p => new ProcessData(p)));
            var res =  new List<ProcessData>();
            try
            {
                Process[] processes = Process.GetProcesses();

                // ugly, ugly hack but i do not understand why it throws access denied
                // when i've launched VS as admin, and added request for admin priveleges in manifest file!
                foreach (Process p in processes)
                //for(int i = 0; i < 2; i++)
                {
                
                        //var p = processes[i];
                        if(p.HasExited)
                            { continue; }

                        res.Add(new ProcessData(p));
                        res.Add(new ProcessData(p));
                
                }
            }
            catch(System.ComponentModel.Win32Exception e )
            {
                //Console.WriteLine($"\n\n\ncrashed on owner name: {ProcessUtils.GetProcessOwner(p)}\n\n\n");
            }
            return res;
        }
    }
}
