using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypetPage : TabbedPage
    {
        private List<Task> generalList;        
        private bool editOption;
        private IOrderedEnumerable<Task> _sortedList;
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;
        private Task mainTask;
        private DateTime selectedDate;

        public TypetPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);                
            }
            catch (Exception ex)
            {
                DisplayAlert("AthensPage", ex.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                //App.Indicator.Start();
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                KidsDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(31));
                StaffDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(32));
                TodoDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(33));
                //App.Indicator.Stop();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private void TodoDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                DisplayAlert("TodoDutiesListView_OnItemTapped", ex.Message, "OK");
            }
        }

        private void TodoDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DisplayAlert("TodoDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }

        private void KidsDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DisplayAlert("KidsDutiesListView_OnItemTapped", ex.Message, "OK");
            }
        }

        private void KidsDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DisplayAlert("KidsDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }

        private void StaffDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DisplayAlert("StaffDutiesListView_OnItemTapped", ex.Message, "OK");
            }
        }

        private void StaffDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DisplayAlert("StaffDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }

        private async void ToDoItemButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ToDoItemEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }
                var task = new Task
                {
                    Text = StaffItemEntry.Text,
                    Type = 33
                };
                UserDialogs.Instance.ShowLoading();
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                TodoDutiesListView.ItemsSource = null;
                TodoDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(33));
                ToDoItemEntry.Text = "";
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
               await DisplayAlert("ToDoItemButton_OnClicked", ex.Message, "OK");
            }
        }

        private async void StaffItemEntryButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(StaffItemEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }
                var task = new Task
                {
                    Text = StaffItemEntry.Text,
                    Type = 32
                };
                UserDialogs.Instance.ShowLoading();
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                StaffDutiesListView.ItemsSource = null;
                StaffDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(32));
                StaffItemEntry.Text = "";
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await DisplayAlert("StaffDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }

        private async void KidItemButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(KidItemEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }
                var task = new Task
                {
                    Text = StaffItemEntry.Text,
                    Type = 31
                };
                UserDialogs.Instance.ShowLoading();
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                KidsDutiesListView.ItemsSource = null;
                KidsDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(31));
                KidItemEntry.Text = "";
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
               await DisplayAlert("KidItemButton_OnClicked", ex.Message, "OK");
            }
        }
    }
}