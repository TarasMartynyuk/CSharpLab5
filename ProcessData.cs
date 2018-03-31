using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpLab5
{
    /// <summary>
    /// wraps process, adding the CurrentCpuUsage prop
    /// </summary>
    class ProcessData : ObservableObject
    {
        #region Bindable Props

        public string Name { get; set; } //=> process.ProcessName;
        //{
        //    get => name;
        //    set => SetValue(ref name, value);
        //}
        public int Id => process.Id;
        public bool Responding => process.Responding;
        public double CpuPercentage { get; set; }
        public long BytesCount => process.WorkingSet64;
        // should not require Thread collection re-instantiation, but who knows
        public int ThreadsCount => process.Threads.Count; 
        public string OwnerName { get; set; }
        public DateTime LaunchDateTime => process.StartTime;
        public ProcessModuleCollection Modules => process.Modules;
        #endregion

        readonly Process process;

        public ProcessData(Process process)
        {
            this.process = process ?? throw new ArgumentNullException(nameof(process));

            Debug.Assert(!process.HasExited);
            CpuPercentage = ProcessUtils.GetCpuPercentage(process);
            OwnerName = ProcessUtils.GetProcessOwner(process);

            //Name = "ctor";
            Name = new Random().Next(10).ToString();
        }

        public void Refresh()
        {
            if(process.HasExited)
                { return; }

            process.Refresh();

            CpuPercentage = ProcessUtils.GetCpuPercentage(process);
            //OwnerName = ProcessUtils.GetProcessOwner(process);

            NotifyProcessDependentPropertiesChanged();
        }

        #region INotifyPropertyChanged

        void NotifyProcessDependentPropertiesChanged()
        {
            var personDependentProperties = new string[]
            {
                nameof(Name), nameof(Id), nameof(Responding), nameof(CpuPercentage),
                nameof(BytesCount), nameof(ThreadsCount), nameof(OwnerName)
            };
            foreach(string propName in personDependentProperties)
            {
                OnPropertyChanged(propName);
            }
        }

        #endregion
    }
}
