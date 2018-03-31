using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CSharpLab5.LogicClasses
{
    static class ProcessesUpdater
    {
        // tried to create object in the helper thread and just change refs in the main one - 
        // not working - UI not updated
        // some closure problem?
        public static void UpdateProcessCollection(IEnumerable<ProcessData> newProcesses, ObservableCollection<ProcessData> processes)
        {
            // so yeah, this is the minimal ammount of work that needs to be done in the main thread
            //var collection = new ObservableCollection<ProcessData>();
            // please write me what i did wrong in setting up binding to view, and updating vm async-y
            processes.Clear();

            foreach (ProcessData p in newProcesses)
            {
                processes.Add(p);
                //collection.Add(p);
            }

            //mainWindowViewModel.Processes = collection;
        }

        public static void RefreshData(ObservableCollection<ProcessData> processes)
        {
            foreach (ProcessData data in processes)
            {
                data.Refresh();
            }
            Debug.WriteLine("refreshed process data objs");
        }
    }
}
