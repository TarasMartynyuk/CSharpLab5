using System;
using System.Threading.Tasks;

namespace CSharpLab5.LogicClasses
{
    /// <summary>
    /// repeats the action asynchronously every given interval of time
    /// </summary>
    //TODO: add ability to stop - won't be static then
    static class ActionRepeater
    {
        /// <summary>
        /// runs the action every <paramref name="interval"/> milliseconds in the other thread
        /// </summary>
        public static void RepeatAction(Action action, int interval)
        {
            Task.Run(async () => {
                while (true)
                {
                    await Task.Delay(interval);
                    action();
                }
            });
        }
    }
}
