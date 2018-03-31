using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Diagnostics;
using CSharpLab5.LogicClasses;

namespace CSharpLab5.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        const int CollectionUpdateInterval = 1;
        const int ProcessesRefreshInterval = 1;

        public ObservableCollection<ProcessData> Processes
        {
            get => processes;
            set
            {
                 if(value == null)
                    { throw new NullReferenceException(nameof(value)); }
                SetValue(ref processes, value);
            }
        }

        public ProcessData SelectedProcess
        {
            get => selectedProcess;
            set => SetValue(ref selectedProcess, value);
        }

        ObservableCollection<ProcessData> processes;
        ProcessData selectedProcess;

        PeriodicalProcessesUpdater processUpdater;

        public MainWindowViewModel()
        {
            // updater must have smth to update, he does not instantiate 
            Processes = new ObservableCollection<ProcessData>();

            processUpdater = new PeriodicalProcessesUpdater(this);
            processUpdater.StartRefreshing(CollectionUpdateInterval, ProcessesRefreshInterval, null, null);
        }

        #region ContextMenu commands

        void ShowModulesForSelectedProcess()
        {
            Debug.Assert(SelectedProcess != null);
        }

        #endregion




    }
}
