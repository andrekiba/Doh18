using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Doh18.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .EnableLocalScreenshots()
                    .ApkFile(@"C:\Users\andrea.ceroni\Desktop\net.elfo.doh18.apk")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                //.InstalledApp("bundleId")
                .StartApp();
        }
    }
}