using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpLab5.ViewModels;

namespace CSharpLab5.LogicClasses
{
    class PeriodicalProcessesUpdater
    {
        readonly MainWindowViewModel mainWindowViewModel;
        readonly SynchronizationContext synchronizationContext;

        public PeriodicalProcessesUpdater(MainWindowViewModel mainWindowViewModel)
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
        public void StartRefreshing(int collectionRefreshInterval, int processDataRefreshInterval, 
            Action OnBeforeUpdate, Action OnCollectionUpdate)
        {
            if(collectionRefreshInterval < 0 || processDataRefreshInterval < 0)
                { throw new ArgumentException("interval cannot be less then zero"); }

            if(mainWindowViewModel.Processes == null)
                { throw new ArgumentNullException(nameof(mainWindowViewModel.Processes)); }

            ProcessesUpdater.UpdateProcessCollection(ProcessFetcher.FetchProcesses(), mainWindowViewModel.Processes);

            Task.Run(() =>
            {
                UpdateProcessesCollectionPeriodicallyAsync(collectionRefreshInterval, 
                    OnBeforeUpdate, OnCollectionUpdate).Wait();
            });

            //Task.Run(() =>
            //{
            //    RefreshProcessesPeriodicallyAsync(processDataRefreshInterval).Wait();
            //});
        }

        async Task UpdateProcessesCollectionPeriodicallyAsync(int interval, Action OnBeforeUpdate, Action OnUpdate)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(interval));

                Debug.Assert(mainWindowViewModel != null);
                Debug.Assert(mainWindowViewModel.Processes != null);

                // change not reflected on UI, even with sync!
                IEnumerable<MyProcess> processes = ProcessFetcher.FetchProcesses();

                synchronizationContext.Post(_ =>
                {
                    OnBeforeUpdate?.Invoke();
                    ProcessesUpdater.UpdateProcessCollection(processes, mainWindowViewModel.Processes);
                    Debug.WriteLine("updated collection");
                    OnUpdate?.Invoke();
                }, 1);
                //return;
            }
        }

        //async Task RefreshProcessesPeriodicallyAsync(int interval)
        //{
        //    while (true)
        //    {
        //        Debug.Assert(mainWindowViewModel != null);
        //        if (mainWindowViewModel.Processes == null)
        //            { continue; }

        //        await Task.Delay(TimeSpan.FromSeconds(interval));

        //        synchronizationContext.Post(_ =>
        //        {
        //            RefreshData();
        //        }, 1);
        //    }
        //}




    }
}
