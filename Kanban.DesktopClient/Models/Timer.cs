using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Kanban.DesktopClient.Models
{
    public class Timer
    {
        public delegate void OnTimerTick(object sender, EventArgs e);

        private OnTimerTick onTimer;
        private DispatcherTimer timer = new DispatcherTimer();

        public Timer(OnTimerTick onTimerTick)
        {
            onTimer = onTimerTick;

            Set();
        }

        public async void Start()
        {
            timer.Start();
        }

        public async void Stop()
        {
            timer.Start();
        }

        private void Set()
        {
            timer.Tick += new EventHandler(onTimer);
            timer.Interval = new TimeSpan(0, 0, 10);
        }
    }
}
