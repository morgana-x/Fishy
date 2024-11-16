namespace Fishy.Models
{
    class ScheduledTask
    {
        private readonly System.Timers.Timer _timer;

        public ScheduledTask(Action task, int interval)
        {
            _timer = new(interval)
            {
                AutoReset = true
            };
            _timer.Elapsed += (_, _) => task();
        }

        public void Start()
            => _timer.Start();

        public void Stop()
            => _timer.Stop();
    }
}
