using System;
using DisableHotkeysApp.Models;
using ReactiveUI;
using System.Reactive.Linq;

namespace DisableHotkeysApp.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        #region Fields

        private readonly HotkeysManager _hotkeysManager;
        private bool _isUpdating;

        #endregion

        #region Ctor

        public MainViewModel(HotkeysManager hotkeysManager)
        {
            _hotkeysManager = hotkeysManager;

            IsAltF4Enabled = hotkeysManager.IsAltF4Enabled;
            IsCtrlAltDelEnabled = hotkeysManager.IsAltKeyEnabled;
            IsAltSpaceEnabled = hotkeysManager.IsAltSpaceEnabled;
            IsAltTabEnabled = hotkeysManager.IsAltTabEnabled;
            IsCtrlAltEndEnabled = hotkeysManager.IsCtrlAltEndEnabled;
            IsCtrlShiftEscEnabled = hotkeysManager.IsCtrlShiftEscEnabled;
            IsWinEnabled = hotkeysManager.IsWinEnabled;
            IsCtrlEscEnabled = hotkeysManager.IsCtrlEscEnabled;

            SubscribeToIsAltF4EnabledChanged();
            SubscribeToIsAltSpaceEnabledChanged();
            SubscribeToIsCtrlAltDelEnabledChanged();
            SubscribeToIsAltTabEnabledChanged();
            SubscribeToIsCtrlAltEndEnabledChanged();
            SubscribeToIsCtrlShiftEscEnabledChanged();
            SubscribeToIsWinEnabledChanged();
            SubscribeToIsCtrlEscEnabledChanged();
        }

        #endregion

        #region IsAltF4Enabled

        private bool _IsAltF4Enabled;

        public bool IsAltF4Enabled
        {
            get { return _IsAltF4Enabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsAltF4Enabled, value); }
        }

        private IDisposable SubscribeToIsAltF4EnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsAltF4Enabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsAltF4EnabledChanged());
        }

        private void OnIsAltF4EnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsAltF4Enabled = IsAltF4Enabled;
            });
        }

        #endregion

        #region IsCtrlAltDelEnabled

        private bool _IsCtrlAltDelEnabled;

        public bool IsCtrlAltDelEnabled
        {
            get { return _IsCtrlAltDelEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsCtrlAltDelEnabled, value); }
        }

        private IDisposable SubscribeToIsCtrlAltDelEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsCtrlAltDelEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsCtrlAltDelEnabledChanged());
        }

        private void OnIsCtrlAltDelEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.SetAltKeyEnable(IsCtrlAltDelEnabled);
            });
        }

        #endregion

        #region IsAltSpaceEnabled

        private bool _IsAltSpaceEnabled;

        public bool IsAltSpaceEnabled
        {
            get { return _IsAltSpaceEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsAltSpaceEnabled, value); }
        }

        private IDisposable SubscribeToIsAltSpaceEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsAltSpaceEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsAltSpaceEnabledChanged());
        }

        private void OnIsAltSpaceEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsAltSpaceEnabled = IsAltSpaceEnabled;
            });
        }

        #endregion

        #region IsAltTabEnabled

        private bool _IsAltTabEnabled;

        public bool IsAltTabEnabled
        {
            get { return _IsAltTabEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsAltTabEnabled, value); }
        }

        private IDisposable SubscribeToIsAltTabEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsAltTabEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsAltTabEnabledChanged());
        }

        private void OnIsAltTabEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsAltTabEnabled = IsAltTabEnabled;
            });
        }

        #endregion

        #region IsCtrlAltEndEnabled

        private bool _IsCtrlAltEndEnabled;

        public bool IsCtrlAltEndEnabled
        {
            get { return _IsCtrlAltEndEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsCtrlAltEndEnabled, value); }
        }

        private IDisposable SubscribeToIsCtrlAltEndEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsCtrlAltEndEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsCtrlAltEndEnabledChanged());
        }

        private void OnIsCtrlAltEndEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsCtrlAltEndEnabled = IsCtrlAltEndEnabled;
            });
        }

        #endregion

        #region IsCtrlShiftEscEnabled

        private bool _IsCtrlShiftEscEnabled;

        public bool IsCtrlShiftEscEnabled
        {
            get { return _IsCtrlShiftEscEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsCtrlShiftEscEnabled, value); }
        }

        private IDisposable SubscribeToIsCtrlShiftEscEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsCtrlShiftEscEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsCtrlShiftEscEnabledChanged());
        }

        private void OnIsCtrlShiftEscEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.SetCtrlShiftEscEnabled(IsCtrlShiftEscEnabled);
                IsCtrlShiftEscEnabled = _hotkeysManager.IsCtrlShiftEscEnabled;
            });
        }

        #endregion

        #region IsWinEnabled

        private bool _IsWinEnabled;

        public bool IsWinEnabled
        {
            get { return _IsWinEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsWinEnabled, value); }
        }

        private IDisposable SubscribeToIsWinEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsWinEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsWinEnabledChanged());
        }

        private void OnIsWinEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsWinEnabled = IsWinEnabled;
            });
        }

        #endregion

        #region IsCtrlEscEnabled

        private bool _IsCtrlEscEnabled;

        public bool IsCtrlEscEnabled
        {
            get { return _IsCtrlEscEnabled; }
            set { this.RaiseAndSetIfChanged(x => x.IsCtrlEscEnabled, value); }
        }

        private IDisposable SubscribeToIsCtrlEscEnabledChanged()
        {
            return this.ObservableForProperty(x => x.IsCtrlEscEnabled)
                       .Where(_ => !_isUpdating)
                       .Subscribe(_ => OnIsCtrlEscEnabledChanged());
        }

        private void OnIsCtrlEscEnabledChanged()
        {
            Update(() =>
            {
                _hotkeysManager.IsCtrlEscEnabled = IsCtrlEscEnabled;
            });
        }

        #endregion

        private void Update(Action action)
        {
            _isUpdating = true;
            action();
            _isUpdating = false;
        }
    }
}