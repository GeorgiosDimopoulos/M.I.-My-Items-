using System;
using System.IO;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MyItems;
using MyItems.SQLite;
using System.Threading;
using System.Threading.Tasks;
using ProgressDialog = Android.App.ProgressDialog;
namespace Xamarin.ToDoITem.Droid
{
    [Activity(Label = "Mi", Icon = "@drawable/todo",  MainLauncher = false, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Forms.Platform.Android.FormsAppCompatActivity, ISqliteManage, IIndicate
    {
        private ProgressDialog _dialog;

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
                App.Init(this,this);
                _dialog = ProgressDialog.Show(this, "", "Παρακαλώ περιμένετε...", true); //_dialog = new ProgressDialog(this);
                LoadApplication(new App());
                _dialog.Hide();
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

        public void Start()
        {
            try
            {
                if (!_dialog.IsShowing)
                    _dialog = ProgressDialog.Show(this, "", "Παρακαλώ περιμένετε...", true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Start(int type)
        {
            if (!_dialog.IsShowing)
                _dialog = ProgressDialog.Show(this, "", "Παρακαλώ περιμένετε...", true);
        }

        public void Stop()
        {
            try
            {
                if (_dialog.IsShowing)
                    _dialog.Dismiss();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}