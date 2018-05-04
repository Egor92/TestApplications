using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Win32;
using System.Linq;

namespace DisableHotkeysApp.Models
{
    internal static class Keys
    {
        internal const int F4 = 115;
        internal const int Space = 32;
        internal const int Tab = 9;
        internal const int Escape = 27;
        internal const int End = 35;
        internal const int LWin = 91;
        internal const int RWin = 92;
    }

    internal static class Flags
    {
        internal const int Alt = 32;
        internal const int CtrlAlt = 129;
        internal const int Ctrl = 0;
    }

    public sealed class HotkeysManager : IDisposable
    {
        #region Fields

        private readonly object _syncLock = new object();
        private readonly GlobalKeyboardHook _globalKeyboardHook;

        private readonly CompositeDisposable _subscriptions;
        private Process _taskManagerProcess;

        #endregion

        #region Ctor

        public HotkeysManager()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();

            InitializeAltKeyEnabled();
            InitializeCtrlShiftEscEnabled();

            IsAltF4Enabled = true;
            IsAltSpaceEnabled = true;
            IsAltTabEnabled = true;
            IsCtrlAltEndEnabled = true;
            IsWinEnabled = true;
            IsCtrlEscEnabled = true;

            _subscriptions = new CompositeDisposable
            {
                SubscribeToAltF4Pressed(),
                SubscribeToAltSpacePressed(),
                SubscribeToAltTabPressed(),
                SubscribeToCtrlAltEndPressed(),
                SubscribeToWinPressed(),
                SubscribeToCtrlEscPressed(),
            };
        }

        #endregion

        #region Alt+F4

        public bool IsAltF4Enabled { get; set; }

        private IDisposable SubscribeToAltF4Pressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.Flags == Flags.Alt && x.KeyboardData.VirtualCode == Keys.F4)
                                      .Where(_ => !IsAltF4Enabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Alt key

        private static readonly byte[] DisableAltRegistryValue = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x38, 0xE0, 0x00, 0x00, 0x00, 0x00,
        };
        private const string DisableAltRegistrySubkey = @"SYSTEM\CurrentControlSet\Control\Keyboard Layout";
        private const string DisableAltRegistryKey = "Scancode Map";

        public bool IsAltKeyEnabled { get; set; }

        private void InitializeAltKeyEnabled()
        {
            UpdateIsCtrlShiftEscEnabled();
        }

        private void UpdateIsAltKeyEnabled()
        {
            IsCtrlShiftEscEnabled = GetIsAltKeyEnabled();
        }

        public void SetAltKeyEnable(bool isAltKeyEnabled)
        {
            lock (_syncLock)
            {
                try
                {
                    if (isAltKeyEnabled)
                    {
                        EnableAltKey();
                    }
                    else
                    {
                        DisableAltKey();
                    }
                }
                finally
                {
                    UpdateIsAltKeyEnabled();
                }
            }
        }

        private static void EnableAltKey()
        {
            RemoveValueFromRegistry(() => Registry.LocalMachine.CreateSubKey(DisableAltRegistrySubkey), DisableAltRegistryKey);
        }

        private static void DisableAltKey()
        {
            ChangeValueInRegistry(() => Registry.LocalMachine.CreateSubKey(DisableAltRegistrySubkey), DisableAltRegistryKey, DisableAltRegistryValue, RegistryValueKind.Binary);
        }

        private static bool GetIsAltKeyEnabled()
        {
            return !HasValueInRegistry(() => Registry.LocalMachine.CreateSubKey(DisableAltRegistrySubkey), DisableAltRegistryKey, DisableAltRegistryValue);
        }

        #endregion

        #region Alt+Space

        public bool IsAltSpaceEnabled { get; set; }

        private IDisposable SubscribeToAltSpacePressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.Flags == Flags.Alt && x.KeyboardData.VirtualCode == Keys.Space)
                                      .Where(_ => !IsAltSpaceEnabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Alt+Tab

        public bool IsAltTabEnabled { get; set; }

        private IDisposable SubscribeToAltTabPressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.Flags == Flags.Alt && x.KeyboardData.VirtualCode == Keys.Tab)
                                      .Where(_ => !IsAltTabEnabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Ctrl+Alt+End

        public bool IsCtrlAltEndEnabled { get; set; }

        private IDisposable SubscribeToCtrlAltEndPressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.Flags == Flags.CtrlAlt && x.KeyboardData.VirtualCode == Keys.End)
                                      .Where(_ => !IsCtrlAltEndEnabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Ctrl+Shift+Esc

        private const string DisableTaskManagerRegistrySubkey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string DisableTaskManagerRegistryKey = "DisableTaskMgr";
        private const string DisableTaskManagerRegistryValue = "1";

        public bool IsCtrlShiftEscEnabled { get; private set; }

        private void InitializeCtrlShiftEscEnabled()
        {
            _taskManagerProcess = Process.GetProcessesByName("taskmgr").FirstOrDefault();
            UpdateIsCtrlShiftEscEnabled();
        }

        private void UpdateIsCtrlShiftEscEnabled()
        {
            IsCtrlShiftEscEnabled = _taskManagerProcess == null && IsTaskManagerEnabled();
        }

        public void SetCtrlShiftEscEnabled(bool isCtrlShiftEscEnabled)
        {
            lock (_syncLock)
            {
                try
                {
                    if (isCtrlShiftEscEnabled)
                    {
                        //KillTaskManagerProcess();
                        EnableTaskManager();
                    }
                    else
                    {
                        //StartTaskManagerInBackground();
                        DisableTaskManager();
                    }
                }
                finally
                {
                    UpdateIsCtrlShiftEscEnabled();
                }
            }
        }

        private void StartTaskManagerInBackground()
        {
            if (_taskManagerProcess != null)
                return;

            string taskmngPath = Path.Combine(Environment.SystemDirectory, "taskmgr.exe");
            var processStartInfo = new ProcessStartInfo(taskmngPath)
            {
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
            };

            _taskManagerProcess = Process.Start(processStartInfo);
        }

        private void KillTaskManagerProcess()
        {
            if (_taskManagerProcess == null)
                return;

            try
            {
                _taskManagerProcess.Kill();
            }
            catch
            {
            }
            _taskManagerProcess = null;
        }

        private static void EnableTaskManager()
        {
            RemoveValueFromRegistry(() => Registry.CurrentUser.CreateSubKey(DisableTaskManagerRegistrySubkey), DisableTaskManagerRegistryKey);
        }

        private static void DisableTaskManager()
        {
            ChangeValueInRegistry(() => Registry.CurrentUser.CreateSubKey(DisableTaskManagerRegistrySubkey), DisableTaskManagerRegistryKey, DisableTaskManagerRegistryValue);
        }

        private static bool IsTaskManagerEnabled()
        {
            return !HasValueInRegistry(() => Registry.CurrentUser.CreateSubKey(DisableTaskManagerRegistrySubkey), DisableTaskManagerRegistryKey, DisableTaskManagerRegistryValue);
        }

        #endregion

        #region Win

        public bool IsWinEnabled { get; set; }

        private IDisposable SubscribeToWinPressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.VirtualCode == Keys.LWin || x.KeyboardData.VirtualCode == Keys.RWin)
                                      .Where(_ => !IsWinEnabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Ctrl+Esc

        public bool IsCtrlEscEnabled { get; set; }

        private IDisposable SubscribeToCtrlEscPressed()
        {
            return _globalKeyboardHook.KeyboardPressed
                                      .Where(x => x.KeyboardData.Flags == Flags.Ctrl && x.KeyboardData.VirtualCode == Keys.Escape)
                                      .Where(_ => !IsCtrlEscEnabled)
                                      .Subscribe(x => x.Handled = true);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        #endregion

        private static void ChangeValueInRegistry(Func<RegistryKey> getSubkey, string key, object value, RegistryValueKind registryValueKind = RegistryValueKind.String)
        {
            try
            {
                using (RegistryKey objRegistryKey = getSubkey())
                {
                    if (objRegistryKey == null)
                        return;

                    objRegistryKey.SetValue(key, value, registryValueKind);
                }
            }
            catch (Exception e)
            {
            }
        }

        private static void RemoveValueFromRegistry(Func<RegistryKey> getSubkey, string key)
        {
            try
            {
                using (RegistryKey objRegistryKey = getSubkey())
                {
                    if (objRegistryKey == null)
                        return;

                    if (objRegistryKey.GetValue(key) != null)
                        objRegistryKey.DeleteValue(key);
                }
            }
            catch (Exception e)
            {
            }
        }

        private static bool HasValueInRegistry(Func<RegistryKey> getSubkey, string key, object value)
        {
            try
            {
                using (RegistryKey objRegistryKey = getSubkey())
                {
                    if (objRegistryKey == null)
                        return true;

                    var ragistryValue = objRegistryKey.GetValue(key, null);
                    return Equals(ragistryValue, value);
                }
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
}