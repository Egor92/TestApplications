using ReactiveUI;

namespace TabControlTest
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Names

        private ReactiveCollection<string> _names;

        public ReactiveCollection<string> Names
        {
            get { return _names ?? (_names = CreateNames()); }
        }

        private ReactiveCollection<string> CreateNames()
        {
            return new ReactiveCollection<string>()
            {
                "Dan",
                "Alex",
                "Marty",
                "Gloria",
            };
        }

        #endregion

        #region SelectedName

        private string _SelectedName;

        public string SelectedName
        {
            get { return _SelectedName; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedName, value); }
        }

        #endregion
    }
}