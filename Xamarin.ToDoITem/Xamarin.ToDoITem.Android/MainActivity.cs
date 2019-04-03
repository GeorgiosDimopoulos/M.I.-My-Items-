﻿using System;
using System.IO;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MyItems;
using MyItems.SQLite;

namespace Xamarin.ToDoITem.Droid
{
    [Activity(Label = "Mi", Icon = "@drawable/todo",  MainLauncher = false, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
                UserDialogs.Init(this);
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