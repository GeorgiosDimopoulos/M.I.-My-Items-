using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MyCrossFitApp;
using MyCrossFitApp.SQLite;

namespace Xamarin.ToDoITem.Droid
{
    [Activity(Label = "MyCrossfFitApp", Icon = "@drawable/icon",  MainLauncher = true, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Forms.Platform.Android.FormsAppCompatActivity, ISqliteManage
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {                
                base.OnCreate(bundle);
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                Forms.Forms.Init(this, bundle);
                SQLitePCL.Batteries_V2.Init();

                App.Init(this);
                LoadApplication(new App());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);                
            }
        }

        public string DatabaseFolder()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ToDoItemSQLite.db3");
        }
    }
}