using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Codeplex.Reactive;
using Intecom.Foundation.Wpf.Extensions;
using Microsoft.Practices.Prism.Commands;
using ReactiveUI;

namespace ReactiveObserveInnerProperties
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Ctor

        public MainWindowViewModel()
        {
            this.ObservableForProperty(x => x.ParentVM.ChildVM.Value)
                .Subscribe(OnValueChanged);

            InitReactivePropertyText();
            InitializeOunputTextProperty();
        }

        private void OnValueChanged(IObservedChange<MainWindowViewModel, string> observedChange)
        {
            SimpleText = observedChange.GetValue();
        }

        #endregion

        #region ParentVM

        private ParentViewModel _ParentVM = new ParentViewModel();

        public ParentViewModel ParentVM
        {
            get { return _ParentVM; }
            set { this.RaiseAndSetIfChanged(x => x.ParentVM, value); }
        }

        #endregion

        #region SimpleText

        private string _SimpleText;

        public string SimpleText
        {
            get { return _SimpleText; }
            set { this.RaiseAndSetIfChanged(x => x.SimpleText, value); }
        }

        #endregion

        #region ReactivePropertyText

        public ReactiveProperty<string> ReactivePropertyText { get; private set; }

        private IDisposable InitReactivePropertyText()
        {
            return ReactivePropertyText = this.ObservableForProperty(x => x.ParentVM.ChildVM.Value) // from UI to UI value routing
                                              .Select(s => s.Value) // rx query1
                                              .ToReactiveProperty(ParentVM.ChildVM.Value);
        }

        #endregion

        #region OunputText

        private ObservableAsPropertyHelper<string> _observableAsPropertyHalperText;

        public string ObservableAsPropertyHalperText
        {
            get { return _observableAsPropertyHalperText.Value; }
        }

        private IDisposable InitializeOunputTextProperty()
        {
            return _observableAsPropertyHalperText = this.ObservableForProperty(x => x.ParentVM.ChildVM.Value)
                                                         .Select(x => x.Value)
                                                         .ToProperty(this, x => x.ObservableAsPropertyHalperText, ParentVM.ChildVM.Value);
        }

        #endregion

        #region MyCommand

        private ICommand _myCommand;

        public ICommand MyCommand
        {
            get { return _myCommand ?? (_myCommand = new DelegateCommand(My)); }
        }

        private void My()
        {
            ParentVM.ChildVM.Value = "Hello ))";
        }

        #endregion
    }

    public class ParentViewModel : ReactiveObject
    {
        #region ChildVM

        private ChildViewModel _ChildVM = new ChildViewModel();

        public ChildViewModel ChildVM
        {
            get { return _ChildVM; }
            set { this.RaiseAndSetIfChanged(x => x.ChildVM, value); }
        }

        #endregion
    }

    public class ChildViewModel : ReactiveObject
    {
        #region Value

        private string _Value = "No value";

        public string Value
        {
            get { return _Value; }
            set { this.RaiseAndSetIfChanged(x => x.Value, value); }
        }

        #endregion
    }
}