using System;
using System.Globalization;
using System.Threading;
using ILvYou.Zero.API;
using ILvYou.Zero.Logs;

namespace ILvYou.Zero.Threads
{
    public abstract class ZeroThread
    {
        #region Field
        private bool paused;
        private bool halted;
        private readonly Thread thread;
        public readonly object sigLock = new object();

        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
            }
        }

        public bool Halted
        {
            get
            {
                return halted;
            }
            set
            {
                halted = value;
            }
        }
        #endregion

        #region APIs
        public virtual void TogglePause(bool pause)
        {
            lock (this.sigLock)
            {
                this.paused = pause;
                if (this.paused)
                {
                    this.SignalSchedulingChange(null);
                }
                else
                {
                    Monitor.PulseAll(this.sigLock);
                }
            }
        }

        public virtual void Halt()
        {
            lock (this.sigLock)
            {
                this.halted = true;
                if (this.paused)
                {
                    Monitor.PulseAll(this.sigLock);
                }
                else
                {
                    this.SignalSchedulingChange(null);
                }
            }
        }

        public void SignalSchedulingChange(DateTimeOffset? candidateNewNextFireTimeUtc)
        {
        }

        public string Name
        {
            get
            {
                return this.thread.Name;
            }
            protected set
            {
                this.thread.Name = value;
            }
        }

        protected ThreadPriority Priority
        {
            get
            {
                return this.thread.Priority;
            }
            set
            {
                this.thread.Priority = value;
            }
        }

        protected bool IsBackground
        {
            set
            {
                this.thread.IsBackground = value;
            }
        }

        protected ZeroThread()
        {
            this.Paused = false;
            this.Halted = false;
            this.thread = new Thread(new ThreadStart(this.Run));
        }

        protected ZeroThread(string name)
        {
            this.thread = new Thread(new ThreadStart(this.Run));
            this.Name = name;
        }

        public virtual void Run()
        {
        }

        public void Start()
        {
            ZeroLog.LogInfo("thread start....");

            if (this.thread.ThreadState != ThreadState.Running)
            {
                this.thread.Start();
            }
        }

        public void Abort()
        {
            try
            {
                ZeroLog.LogInfo("thread abort....");

                this.thread.Abort();
            }
            finally
            {

            }
        }

        public void Interrupt()
        {
            ZeroLog.LogInfo("thread interrupt....");

            this.thread.Interrupt();
        }

        public void Join()
        {
            ZeroLog.LogInfo("thread join....");

            this.thread.Join();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Thread[{0},{1},]", new object[]
			{
				this.Name,
				this.Priority
			});
        }
        #endregion
    }
}
