using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ReactiveUI;

namespace ObservableAsPropertyHelperMemoryLeak
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Iteration

        private int _Iteration;

        public int Iteration
        {
            get { return _Iteration; }
            private set { this.RaiseAndSetIfChanged(x => x.Iteration, value); }
        }

        #endregion

        #region TotalMemory

        private double _TotalMemory;

        public double TotalMemory
        {
            get { return _TotalMemory; }
            private set { this.RaiseAndSetIfChanged(x => x.TotalMemory, value); }
        }

        #endregion

        #region PrivateMemorySize64

        private double _PrivateMemorySize64;

        public double PrivateMemorySize64
        {
            get { return _PrivateMemorySize64; }
            private set { this.RaiseAndSetIfChanged(x => x.PrivateMemorySize64, value); }
        }

        #endregion

        #region IsPaused

        private bool _IsPaused;

        public bool IsPaused
        {
            get { return _IsPaused; }
            set { this.RaiseAndSetIfChanged(x => x.IsPaused, value); }
        }

        #endregion

        #region FakeProperty

        public int FakeProperty { get; set; }

        #endregion

        #region FreeMemoryCommand

        private ICommand _freeMemoryCommand;

        public ICommand FreeMemoryCommand
        {
            get { return _freeMemoryCommand ?? (_freeMemoryCommand = new DelegateCommand(FreeMemory)); }
        }

        private void FreeMemory()
        {
            GC.Collect();
            UpdateMemoryInfo();
        }

        #endregion

        public void Start()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(1), ThreadPoolScheduler.Instance).Subscribe(iteration =>
            {
                if (IsPaused)
                    return;

                Iteration = (int)iteration;

                for (int i = 0; i < 10000; i++)
                {
                    //var subscription = Observable.Empty<int>().ToProperty(this, x => x.FakeProperty, 0);
                    var subscription = Observable.Empty<int>().ToProperty(this, x => x.FakeProperty, 0, Scheduler.CurrentThread);
                    //var subscription = Observable.Empty<int>().Subscribe(_ => {});
                    subscription.Dispose();
                    //new MainWindowViewModel();
                }

                UpdateMemoryInfo();
            });
        }

        private void UpdateMemoryInfo()
        {
            TotalMemory = GC.GetTotalMemory(true);
            PrivateMemorySize64 = Process.GetCurrentProcess().PrivateMemorySize64;
        }
    }
}
