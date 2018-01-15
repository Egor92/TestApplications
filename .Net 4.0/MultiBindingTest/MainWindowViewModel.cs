using System.Windows.Data;
using ReactiveUI;

namespace MultiBindingTest
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Text

        private string _Text;

        public string Text
        {
            get { return _Text; }
            set { this.RaiseAndSetIfChanged(x => x.Text, value); }
        }

        #endregion

        #region Value

        private int _Value;

        public int Value
        {
            get { return _Value; }
            set { this.RaiseAndSetIfChanged(x => x.Value, value); }
        }

        #endregion
    }
}
