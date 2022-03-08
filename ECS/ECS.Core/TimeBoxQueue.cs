using ECS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ECS.Core
{
    public class TimeBoxQueue : IDisposable
    {
        #region Event
        public event EventHandler<TimeBox> EnqueueTimeBoxEvent;
        public event EventHandler<TimeBox> DequeueTimeBoxEvent;
        public event EventHandler<TimeBox> DequeueTimerTimeBoxEvent;

        public event EventHandler<TimeBox> EnqueueDeniedTickEvent;
        #endregion

        #region Field
        private Queue<TimeBox> BoxQueue = new Queue<TimeBox>();
        private Timer DequeueTimer;
        private bool disposed = false;
        #endregion

        #region Prop
        public double DequeueInterval { get; set; }
        #endregion

        #region Ctor
        public TimeBoxQueue(double dequeueInterval)
        {
            this.DequeueInterval = dequeueInterval;
            this.DequeueTimer = new Timer(this.DequeueInterval);
            this.DequeueTimer.Elapsed += DequeueTimer_Elapsed;
        }
        #endregion

        #region Method
        public void BoxEnqueue(TimeBox box)
        {
            lock (this.BoxQueue)
            {
                if (this.BoxQueue.Count > 0)
                {
                    TimeBox[] array = this.BoxQueue.ToArray();
                    var lastInput = array[array.Length - 1];

                    var during = (box.BcrReadTime - lastInput.BcrReadTime).TotalMilliseconds;
                    if (during < 400)
                    {
                        this.EnqueueDeniedTickEvent?.Invoke(this, box);
                        return;
                    }
                }

                this.BoxQueue.Enqueue(box);
                this.DequeueTimerStart();

                this.EnqueueTimeBoxEvent?.Invoke(this, box);
            }
        }

        public TimeBox BoxDequeue()
        {
            TimeBox box = null;
            lock (this.BoxQueue)
            {
                if (this.BoxQueue.Count > 0)
                {
                    box = this.BoxQueue.Dequeue();

                    if (this.BoxQueue.Count == 0)
                        this.DequeueTimerStop();

                    this.DequeueTimeBoxEvent?.Invoke(this, box);
                }
            }

            return box;
        }

        public void DequeueTimerStart() => this.DequeueTimer?.Start();

        public void DequeueTimerStop() => this.DequeueTimer?.Stop();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.DequeueTimer != null)
                    {
                        this.DequeueTimer.Stop();
                        this.DequeueTimer.Elapsed -= DequeueTimer_Elapsed;
                        this.DequeueTimer?.Dispose();
                    }
                }

                this.disposed = true;
            }
        }
        #endregion

        #region Event Handler
        private void DequeueTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.BoxQueue)
            {
                if (this.BoxQueue.Count > 0)
                {
                    TimeBox[] array = this.BoxQueue.ToArray();

                    var span = DateTime.Now - array[0].BcrReadTime;
                    if (span.TotalMilliseconds > this.DequeueInterval)
                    {
                        var box = this.BoxQueue.Dequeue();
                        this.DequeueTimerTimeBoxEvent?.Invoke(this, box);
                    }
                }
            }
        }
        #endregion
    }
}
