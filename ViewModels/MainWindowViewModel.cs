using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CSharpLab5.LogicClasses;

namespace CSharpLab5.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        const int ProcessesUpdateInterval = 2000;
        const int ProcessesRefreshInterval = 2000;

        public ObservableCollection<ProcessData> Processes
        {
            get => processes;
            set
            {
                processes = value ?? throw new NullReferenceException(nameof(value));
                SetValue(ref processes, value);
            }
        }

        ObservableCollection<ProcessData> processes;
        ProcessUpdater processUpdater;

        public MainWindowViewModel()
        {
            Processes = new ObservableCollection<ProcessData>();
            //Processes = ProcessFetcher.FetchProcesses();

            //foreach(var p in Processes)
            //{
            //    p.Refresh();
            //}


            processUpdater = new ProcessUpdater(this);
            processUpdater.StartRefreshing(ProcessesUpdateInterval, ProcessesRefreshInterval);
        }

    }
}
