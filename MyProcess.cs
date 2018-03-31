using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpLab5
{
    /// <summary>
    /// wraps process, adding some properties, and delegating needed Process methods
    /// </summary>
    /// <remarks>
    /// stupid name
    /// </remarks>
    class MyProcess : ObservableObject
    {
        #region Bindable Props
        // caching process props, to have a snapshot of the time he was last successfull refreshed
        // if we we
        public string Name
        {
            get => name;
            set => SetValue(ref name, value);
        }
        public int Id
        {
            get => id;
            set => SetValue(ref id, value);
        }
        public bool Responding 
        {
            get => responding;
            set => SetValue(ref responding, value);
        }
        public double CpuPercentage
        {
            get => cpuPercentage;
            set => SetValue(ref cpuPercentage, value);
        }
        public long BytesCount
        {
            get => bytesCount;
            set => SetValue(ref bytesCount, value);
        }
        public int ThreadsCount
        {
            get => threadsCount;
            set => SetValue(ref threadsCount, value);
        }
        public string OwnerName 
        {
            get => ownerName;
            set => SetValue(ref ownerName, value);
        }
        public DateTime LaunchDateTime
        {
            get => launchDateTime;
            set => SetValue(ref launchDateTime, value);
        }
        public ProcessModuleCollection Modules 
        {
            get => modules;
            set => SetValue(ref modules, value);
        }
        #endregion

        // TODO: optimise by reusing wrapper class instances, and re-setting process 
        readonly Process process;

        #region binding props backing fields
        string name;
        int id;
        bool responding;
        double cpuPercentage;
        long bytesCount;
        int threadsCount;
        string ownerName;
        DateTime launchDateTime;
        ProcessModuleCollection modules;
        #endregion

        public MyProcess(Process process)
        {
            if(process == null)
                { throw new ArgumentNullException(nameof(process)); }

            if(process.HasExited)
                { throw new ArgumentException("cannot wrap the process that has exited!"); }

            this.process = process;

            RefreshProperties(process);
            //Name = "ctor";
            //Name = new Random().Next(10).ToString();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
            // just keep the cashed data, the process will be removed from list on next update
            if(process.HasExited)
                { return; }

            process.Refresh();


            //NotifyProcessDependentPropertiesChanged();
        }

        public void Kill()
        {
            process.Kill();
        }

        /// <summary>
        /// refreshes the wrapper cached props
        /// to be up to date to those of <paramref name="process"/>
        /// </summary>
        /// <param name="process"></param>
        void RefreshProperties(Process upToDateProcess)
        {
            Debug.Assert(! upToDateProcess.HasExited);

            Name = process.ProcessName;
            Id = process.Id;
            Responding = process.Responding;
            CpuPercentage = ProcessUtils.GetCpuPercentage(process);
            BytesCount = process.WorkingSet64;
            ThreadsCount = process.Threads.Count;
            OwnerName = ProcessUtils.GetProcessOwner(process);
            LaunchDateTime = process.StartTime;
            Modules = process.Modules;
        }

        //void NotifyProcessDependentPropertiesChanged()
        //{
        //    var personDependentProperties = new string[]
        //    {
        //        nameof(Name), nameof(Id), nameof(Responding), nameof(CpuPercentage),
        //        nameof(BytesCount), nameof(ThreadsCount), nameof(OwnerName)
        //    };
        //    foreach(string propName in personDependentProperties)
        //    {
        //        OnPropertyChanged(propName);
        //    }
        //}
    }
}
