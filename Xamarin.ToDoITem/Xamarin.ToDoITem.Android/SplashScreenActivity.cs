using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.Content.PM;
using Android.Util;

namespace Xamarin.ToDoITem.Droid
{
    [Activity(Icon = "@drawable/todo", Label = "MI", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreenActivity: Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            //UserDialogs.Init(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            await Task.Delay(2000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}