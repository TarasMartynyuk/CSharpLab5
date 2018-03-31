using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
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

        public ICommand ShowModulesForSelectedProcessCommand { get; } //, _ => ProcessSelected() )
        public ICommand ShowThreadsForSelectedProcessCommand { get; }
        // i feel like it is doing more work than others, let it be async
        public ICommand OpenSelectedProcessDirCommand { get; }
        public ICommand KillAndRemoveSelectedProcessCommand { get; } //, _ => ProcessSelected() )

        ProcessData selectedProcess;
        #endregion

        ObservableCollection<ProcessData> processes;
        // keeping ref here because i don't want to check what will happen with the threads obj started
        // and because later this guy will know how to stop
        PeriodicalProcessesUpdater processUpdater; 

        int prevSelectedId;

        public MainWindowViewModel()
        {
            // updater must have smth to update, he does not instantiate 
            Processes = new ObservableCollection<ProcessData>();

            processUpdater = new PeriodicalProcessesUpdater(this);
            processUpdater.StartRefreshing(CollectionUpdateInterval, ProcessesRefreshInterval, 
                OnBeforeProcessesUpdate, OnProcessesUpdate);

            ShowModulesForSelectedProcessCommand = new DelegateCommand(ShowModulesForSelectedProcess, _ => ProcessSelected() );
            ShowThreadsForSelectedProcessCommand = new DelegateCommand(ShowThreadsForSelectedProcess, _ => ProcessSelected() );
            OpenSelectedProcessDirCommand = new DelegateCommandAsync(OpenSelectedProcessDir, _ => ProcessSelected() );
            KillAndRemoveSelectedProcessCommand = new DelegateCommand(KillAndRemoveSelectedProcess, _ => ProcessSelected() );
        }

        #region ContextMenu commands
        void ShowModulesForSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

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

        void ShowThreadsForSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            var listView = new ThreadListView
            {
                DataContext = SelectedProcess,
            };

            var modulesWindow = new Window()
            {
                Width = 450,
                Height = 600,
                Content = listView
            };

            modulesWindow.ShowDialog();
        }

        async Task OpenSelectedProcessDir(object o)
        {
            Debug.Assert(ProcessSelected());

            var k = Path.GetDirectoryName(SelectedProcess.Filename);
            k = $"explorer.exe \"{Path.GetDirectoryName(SelectedProcess.Filename)}\"";
            Process.Start("explorer.exe", $"\"{Path.GetDirectoryName(SelectedProcess.Filename)}\"");
        }

        void KillAndRemoveSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            SelectedProcess.Kill();
            Processes.Remove(SelectedProcess);
            SelectedProcess = null;
            //TODO: select next/prev index
        }
        #endregion

        bool ProcessSelected()
        {
            return SelectedProcess != null;
        }

        // this two should have re-selected item on update
        void OnBeforeProcessesUpdate()
        {
            prevSelectedId = SelectedProcess.Id;
        }

        // and this one 100% sets the SelectedProcess value to a valid one
        // but the grid is not updated - 
        // mb after that the value gets overriden to null by some default behaviour of the datagrid
        // or it is some weird closure - thing, or non-ui thread changes problem
        // anyway, i tried
        // this is also invoked in Post to UI thread
        void OnProcessesUpdate()
        {
            SelectedProcess = Processes.First(p => p.Id == prevSelectedId);
        }

    }
}
