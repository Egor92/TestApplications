using System.ComponentModel;
using ReactiveUI;
using System;

namespace ValidationError
{
    public class MainWindowViewModel : ReactiveObject, IDataErrorInfo
    {
        #region Ctor

        public MainWindowViewModel()
        {
            this.ObservableForProperty(x => x.A.Property1)
                .Subscribe(_ => this.RaisePropertyChanged("A"));
        }

        #endregion

        #region Property1

        private string _Property1 = "ggr";

        public string Property1
        {
            get { return _Property1; }
            set { this.RaiseAndSetIfChanged(x => x.Property1, value); }
        }

        #endregion

        #region Property2

        private string _Property2 = "agsg";

        public string Property2
        {
            get { return _Property2; }
            set { this.RaiseAndSetIfChanged(x => x.Property2, value); }
        }

        #endregion

        #region A

        private A _A = new A();

        public A A
        {
            get { return _A; }
            set { this.RaiseAndSetIfChanged(x => x.A, value); }
        }

        #endregion

        #region Implementation of IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Property1")
                {
                    if (Property1 != null && !Property1.EndsWith("1"))
                        return "Property1 fail";
                }
                if (columnName == "Property2")
                {
                    if (Property2 != null && !Property2.EndsWith("2"))
                        return "Property2 fail";
                }
                if (columnName == "A")
                {
                    if (A.Property1 != null && !A.Property1.EndsWith("1"))
                        return "A fail";
                }
                return null;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion
    }

    public class A : ReactiveObject, IDataErrorInfo
    {
        #region Property1

        private string _Property1;

        public string Property1
        {
            get { return _Property1; }
            set { this.RaiseAndSetIfChanged(x => x.Property1, value); }
        }

        #endregion

        #region Implementation of IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                /*if (columnName == "Property1")
                {
                    if (Property1 != null && !Property1.EndsWith("1"))
                        return "Property1 fail";
                }*/
                return null;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion
    }
}