using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Doh18.Base;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Doh18.Helpers
{
    public class Settings : INotifyPropertyChanged
    {
        private static ISettings AppSettings => CrossSettings.Current;
        private static Settings settings;

        #region Properties

        public static Settings Current => settings ?? (settings = new Settings());

        public string Email
        {
            get => AppSettings.GetValueOrDefault(nameof(Email), null);
            set
            {
                if (AppSettings.AddOrUpdateValue(nameof(Email), value))
                    OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), null);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public bool IsLoggedIn => !Email.IsNullOrWhiteSpace();

        #endregion

        #region Methods

        public static void Clear()
        {
            AppSettings.Clear();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}
