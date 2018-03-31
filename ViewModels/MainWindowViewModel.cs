﻿using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CSharpLab5.LogicClasses;
using CSharpLab5.Views;

namespace CSharpLab5.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        const int CollectionUpdateInterval = 1;
        const int ProcessesRefreshInterval = 1;

        #region binding props
        public ObservableCollection<MyProcess> Processes
        {
            get => processes;
            set
            {
                 if(value == null)
                    { throw new NullReferenceException(nameof(value)); }
                SetValue(ref processes, value);
            }
        }

        public MyProcess SelectedProcess
        {
            get => selectedProcess;
            set => SetValue(ref selectedProcess, value);
        }

        public ICommand ShowModulesForSelectedProcessCommand { get; } //, _ => ProcessSelected() )
        public ICommand KillAndRemoveSelectedModuleCommand { get; } //, _ => ProcessSelected() )

        #endregion

        ObservableCollection<MyProcess> processes;
        MyProcess selectedProcess;

        PeriodicalProcessesUpdater processUpdater;

        public MainWindowViewModel()
        {
            // updater must have smth to update, he does not instantiate 
            Processes = new ObservableCollection<MyProcess>();

            processUpdater = new PeriodicalProcessesUpdater(this);
            processUpdater.StartRefreshing(CollectionUpdateInterval, ProcessesRefreshInterval, null, null);

            ShowModulesForSelectedProcessCommand = new DelegateCommandAsync(ShowModulesForSelectedProcess, _ => ProcessSelected() );
        }

        #region ContextMenu commands

        async Task ShowModulesForSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            //var p = new ProcessData(Process.GetProcesses()[10]);

            var listV = new ModulesInfoView
            {
                DataContext = SelectedProcess,
            };

            var modulesWindow = new Window()
            {
                Width = 450,
                Height = 600,
                Content = listV
            };

            modulesWindow.ShowDialog();
        }

        async Task KillSelectedProcess()
        {
            Debug.Assert(ProcessSelected());

            SelectedProcess.Kill();
            //Processes.Remove(SelectedProcess);
            SelectedProcess = null;
            //TODO: select next/prev index
        }

        #endregion

        bool ProcessSelected()
        {
            return SelectedProcess != null;
        }


    }
}
