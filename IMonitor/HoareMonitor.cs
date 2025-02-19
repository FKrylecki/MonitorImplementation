﻿namespace MonitorImplementation.HoareMonitor
{
    public abstract class HoareMonitor
    {
        protected interface ISignal
        {
            void Send();

            void Wait();

            bool Await();
        }

        /// <summary>
        /// Creates <see cref="ISignal"/> to be instantiated and used inside the monitor. If <see cref="ISignal"/> is not used in the context of the monitor it was created an exception is thrown.
        /// </summary>
        /// <returns>a new instance of <see cref="ISignal"/> attached to this monitor.</returns>
        protected abstract ISignal CreateSignal();

    }
}