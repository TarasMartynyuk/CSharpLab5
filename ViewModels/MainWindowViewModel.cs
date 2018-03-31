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

        ObservableCollection<ProcessData> processes;
        PeriodicalProcessesUpdater processUpdater;

        public MainWindowViewModel()
        {
            processUpdater = new PeriodicalProcessesUpdater(this);
            processUpdater.StartRefreshing(CollectionUpdateInterval, ProcessesRefreshInterval, OnBeforeProcessesUpdate, OnProcessesUpdate);
        }

        #region ContextMenu commands
        void 

        #endregion




        void OnBeforeProcessesUpdate()
        {
            Debug.WriteLine("OnBeforeUpdate");
            //prevSelectedProcId = SelectedProcess?.Id ?? -1;
        }

        void OnProcessesUpdate()
        {
            Debug.WriteLine("OnUpdate");
        }


    }
}
