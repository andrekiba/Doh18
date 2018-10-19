using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Doh18.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        private IApp app;
        private readonly Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            //app.Query("WelcomeLabel");
            //app.Query(x => x.Marked("WelcomeLabel");
            var results = app.WaitForElement(x => x.Marked("WelcomeLabel").Text("Welcome to DOH18!"));
            app.Screenshot("Welcome screen");

            Assert.IsTrue(results.Any(), "Welcome label not found");
        }

        [Test]
        public void SayCiaoButtonTapped()
        {
            AppQuery ButtonQuery(AppQuery x) => x.Marked("SayCiaoButton");
            AppQuery LabelQuery(AppQuery x) => x.Marked("CiaoLabel").Text("Ciao!");

            var buttonResult = app.WaitForElement((Func<AppQuery, AppQuery>) ButtonQuery);

            Assert.IsTrue(buttonResult.Any());

            app.Tap((Func<AppQuery, AppQuery>) ButtonQuery);

            var labelResult = app.WaitForElement((Func<AppQuery, AppQuery>)LabelQuery);

            app.Screenshot("Welcome screen");

            Assert.IsTrue(labelResult.Any());
        }
    }
}
