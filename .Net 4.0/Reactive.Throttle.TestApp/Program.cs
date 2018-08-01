using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Reactive.Throttle.TestApp
{
    public static class Program
    {
        private static void Main()
        {
            var whenChenged = new Subject<Unit>();
            var algorithm = new Algorithm(whenChenged);
            algorithm.Start();

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(300);
                    whenChenged.OnNext(Unit.Default);
                }
            });

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }

    public class Algorithm
    {
        private readonly IObservable<Unit> _whenEventRaised;
        private readonly AutoResetEvent _eventHandlingLock = new AutoResetEvent(true);
        private readonly IScheduler _scheduler;

        public Algorithm(IObservable<Unit> whenEventRaised)
        {
            _scheduler = CurrentThreadScheduler.Instance;
            _whenEventRaised = whenEventRaised;
        }

        public IDisposable Start()
        {
            return _whenEventRaised.Do(_ => Logger.Log("Event has come"))
                                   .ObserveOn(ThreadPoolScheduler.Instance)
                                   .Throttle(TimeSpan.FromMilliseconds(250), ThreadPoolScheduler.Instance)
                                   .Subscribe(_ => HandleEvent());
        }

        private void HandleEvent()
        {
            _eventHandlingLock.WaitOne();
            Logger.Log("Locked", ConsoleColor.Yellow);
            Logger.Log("Event is handling...", ConsoleColor.DarkYellow);

            Thread.Sleep(TimeSpan.FromSeconds(3));

            _scheduler.Schedule(() =>
            {
                Logger.Log("Event handled", ConsoleColor.DarkGreen);
                _eventHandlingLock.Set();
                Logger.Log("Unlocked", ConsoleColor.Green);
            });
        }
    }

    public static class Logger
    {
        private static readonly object SyncRoot = new object();

        public static void Log(string message, ConsoleColor? consoleColor = null)
        {
            lock (SyncRoot)
            {
                var format = string.Format("\tThread {0:d2} {1}: {2}", Thread.CurrentThread.ManagedThreadId, DateTime.Now, message);
                Debug.WriteLine(format);
                consoleColor = consoleColor ?? ConsoleColor.Gray;
                Console.ForegroundColor = (ConsoleColor)consoleColor;
                Console.WriteLine(format);
            }
        }
    }
}