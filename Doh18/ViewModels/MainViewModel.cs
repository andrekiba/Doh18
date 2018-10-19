using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using Doh18.Base;
using Xamarin.Forms;

namespace Doh18.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Lifecycle
        public MainViewModel()
        {
            
        }

        #endregion

        #region Commands

        private ICommand sayCiaoCommand;
        public ICommand SayCiaoCommand => sayCiaoCommand ?? (sayCiaoCommand = new Command(async () =>
        {
            await UserDialogs.Instance.AlertAsync("Ciao!", null, "OK");
        }));

        private ICommand trackEventCommand;
        public ICommand TrackEventCommand => trackEventCommand ?? (trackEventCommand = new Command(() =>
        {
            "Button clicked!".TrackEvent();
        }));

        private ICommand handleCommand;
        public ICommand HandleCommand => handleCommand ?? (handleCommand = new Command(() =>
        {
            try
            {
                throw new Exception("Handled exception");
            }
            catch (Exception e)
            {
                e.TrackError();
            }
        }));

        private ICommand crashCommand;
        public ICommand CrashCommand => crashCommand ?? (crashCommand = new Command(() => throw new Exception("UnHandled exception")));

        #endregion
    }
}
