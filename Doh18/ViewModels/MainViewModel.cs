using System;
using System.Windows.Input;
using Doh18.Base;
using Xamarin.Forms;

namespace Doh18.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int ciaoCounter = 0;

        #region Properties

        public string CiaoText { get; set; }

        #endregion 

        #region Lifecycle

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await App.Instance.CheckUpdates();
        }

        #endregion

        #region Commands

        private ICommand sayCiaoCommand;
        public ICommand SayCiaoCommand => sayCiaoCommand ?? (sayCiaoCommand = new Command(() =>
        {
            //await UserDialogs.Instance.AlertAsync("Ciao!", null, "OK");
            CiaoText = $"Ciao {ciaoCounter++}!";
        }));

        private ICommand trackEventCommand;
        public ICommand TrackEventCommand => trackEventCommand ?? (trackEventCommand = new Command(() =>
        {
            "Track event button clicked!".TrackEvent();
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
