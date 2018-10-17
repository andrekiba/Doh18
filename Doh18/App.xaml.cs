using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Doh18.Base;
using Doh18.Pages;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Doh18
{
    public partial class App : Application
    {
        #region Lifecycle

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            #region App Center

            Distribute.ReleaseAvailable = OnReleaseAvailable;
            AppCenter.LogLevel = LogLevel.Verbose;

            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    // Add the notification message and title to the message
                    var summary = $"Push notification received:\n\tNotification title: {e.Title}\n\tMessage: {e.Message}";

                    // If there is custom data associated with the notification,
                    // print the entries
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        summary += e.CustomData.Keys.Aggregate(summary, (current, key) => current + $"\t\t{key} : {e.CustomData[key]}\n");
                    }

                    // Send the notification summary to debug output
                    Debug.WriteLine(summary);
                };
            }

            AppCenter.Start($"ios={Constants.AppCenterIos};android={Constants.AppCenterAndroid}",
                typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));

            #endregion

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion 

        #region Methods

        #region App Center

        public async Task CheckUpdates()
        {
            await Distribute.SetEnabledAsync(false);
            await Distribute.SetEnabledAsync(true);
        }

        private static bool OnReleaseAvailable(ReleaseDetails release)
        {
            AppCenterLog.Info(Constants.AppCenterLogTag, $"{nameof(OnReleaseAvailable)} id={release.Id} version={release.Version} mandatory={release.MandatoryUpdate}");

            var message = $"New release {release.ShortVersion}({release.Version}) available";

            // On mandatory update, user cannot postpone
            var answer = release.MandatoryUpdate ? UserDialogs.Instance.AlertAsync(message, "Warning!", "Download and install") :
                UserDialogs.Instance.ConfirmAsync(message, "Warning!", "Download and install", "Not now");
            answer.ContinueWith(task =>
            {
                // If mandatory or if answer was positive
                if (release.MandatoryUpdate || ((Task<bool>)task).Result)
                {
                    // Notify SDK that user selected update
                    AppCenterLog.Info(Constants.AppCenterLogTag, "Notify Update");
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // Note that this method call is ignored by the SDK if the update is mandatory
                    AppCenterLog.Info(Constants.AppCenterLogTag, "Notify Postpone");
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you are using your own dialog, false otherwise
            return true;
        }

        #endregion 

        #endregion
    }
}
