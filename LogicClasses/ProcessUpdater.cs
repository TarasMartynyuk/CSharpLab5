using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpLab5.ViewModels;

namespace CSharpLab5.LogicClasses
{
    class ProcessUpdater
    {
        readonly MainWindowViewModel mainWindowViewModel;
        SynchronizationContext synchronizationContext;

        public ProcessUpdater(MainWindowViewModel mainWindowViewModel)
        {
            synchronizationContext = SynchronizationContext.Current;
            this.mainWindowViewModel =
                mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        }

        /// <summary>
        /// every <paramref name="collectionRefreshInterval"/>, assigns the list of All Processes in the system
        /// to the mainWindowVM.Processes,
        /// every <paramref name="processDataRefreshInterval"/>, refreshes each process in the mainWindowVM.Processes
        /// </summary>
        public void StartRefreshing(int collectionRefreshInterval, int processDataRefreshInterval)
        {
            if(collectionRefreshInterval < 0 || processDataRefreshInterval < 0)
                { throw new ArgumentException("interval cannot be less then zero"); }

            //mainWindowViewModel.Processes = ProcessFetcher.FetchProcesses();

            Task.Run(() =>
            {
                UpdateProcessesCollectionPeriodicallyAsync(collectionRefreshInterval).Wait();
            });

            Task.Run(() =>
            {
                RefreshProcessesPeriodicallyAsync(processDataRefreshInterval).Wait();
            });

        }

        async Task UpdateProcessesCollectionPeriodicallyAsync(int interval)
        {
            while (true)
            {
                Debug.Assert(mainWindowViewModel != null);
                Debug.Assert(mainWindowViewModel.Processes != null);

                // change not reflected on UI, even with sync!
                IEnumerable<ProcessData> processes = ProcessFetcher.FetchProcesses();

                synchronizationContext.Post(_ =>
                {
                    UpdateVmProcessCollection(processes);
                    Debug.WriteLine("updated collection");
                }, 1);

                await Task.Delay(interval);
            }
        }

        async Task RefreshProcessesPeriodicallyAsync(int interval)
        {
            while(true)
            {
                await Task.Delay(interval);

                Debug.Assert(mainWindowViewModel != null);
                if(mainWindowViewModel.Processes == null)
                    { continue; }

                //synchronizationContext.Post(_ =>
                //{
                //    foreach (ProcessData data in mainWindowViewModel.Processes)
                //    {
                //        data.Refresh();
                //        Debug.WriteLine("refreshed process data objs");
                //    }
                //}, 1);
            }
        }

        void UpdateVmProcessCollection(IEnumerable<ProcessData> data)
        {
            //var collection = new ObservableCollection<ProcessData>();
            mainWindowViewModel.Processes.Clear();

            foreach (ProcessData p in data)
            {
                mainWindowViewModel.Processes.Add(p);
                //collection.Add(p);
            }

            //mainWindowViewModel.Processes = collection;
        }

    }
}
