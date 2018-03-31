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
    class ProcessData : ObservableObject
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
        public string Filename
        {
            get => filename;
            set => SetValue(ref filename, value);
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
        public ProcessThreadCollection Threads
        {
            get => threads;
            set => SetValue(ref threads, value);
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
        string filename;
        DateTime launchDateTime;
        ProcessModuleCollection modules;
        ProcessThreadCollection threads;
        #endregion

        public ProcessData(Process process)
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
        /// to be up to date to those of <paramref name="upToDateProcess"/>
        /// </summary>
        void RefreshProperties(Process upToDateProcess)
        {
            Debug.Assert(!upToDateProcess.HasExited);

            Name = upToDateProcess.ProcessName;
            Id = upToDateProcess.Id;
            Responding = upToDateProcess.Responding;
            CpuPercentage = ProcessUtils.GetCpuPercentage(upToDateProcess);
            BytesCount = upToDateProcess.WorkingSet64;
            ThreadsCount = upToDateProcess.Threads.Count;
            OwnerName = ProcessUtils.GetProcessOwner(upToDateProcess);
            Filename = upToDateProcess.MainModule.FileName;
            LaunchDateTime = upToDateProcess.StartTime;
            Modules = upToDateProcess.Modules;
            Threads = upToDateProcess.Threads;
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
