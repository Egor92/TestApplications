using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace EventLoopSchedulerApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("MainThread: {0}", threadId);


            //IScheduler scheduler = Scheduler.Immediate;
            //IScheduler scheduler = new EventLoopScheduler();
            IScheduler scheduler = ThreadPoolScheduler.Instance;
            //IScheduler scheduler = NewThreadScheduler.Default;
            //IScheduler scheduler = Scheduler.CurrentThread;
            //IScheduler scheduler = TaskPoolScheduler.Default;
            /*scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);
            scheduler.Schedule(WriteThreadId);

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();*/
            
            var subscription = Observable.Interval(TimeSpan.FromMilliseconds(100))
                                         //.ObserveOn(Scheduler.Immediate)
                                         //.ObserveOn(new EventLoopScheduler())
                                         //.ObserveOn(ThreadPoolScheduler.Instance)
                                         //.ObserveOn(Scheduler.NewThread)
                                         //.ObserveOn(new EventLoopScheduler())
                                         .Subscribe(OnNext);
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
            subscription.Dispose();
        }

        private static void WriteThreadId()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Thread: {0}", threadId);
        }

        private static void OnNext(long value)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("{0,5}. Thread: {1}", value, threadId);
        }
    }
}