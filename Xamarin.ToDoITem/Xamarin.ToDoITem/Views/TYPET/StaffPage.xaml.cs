using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views.TYPET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StaffPage : ContentPage
	{
        private List<Task> generalList;
        private bool editOption;
        public static string PageTitle;
        private IOrderedEnumerable<Task> _sortedList;
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;
        private Task mainTask;
        //private DateTime selectedDate;

        public StaffPage ()
		{
            try
            {
                InitializeComponent();
                StaffPicker.Items.Clear();
                StaffPicker.Items.Add("Διαγραφή");
                StaffPicker.Items.Add("Αλλαγή");
            }
            catch (Exception e)
            {
                
            }
		}

        protected override async void OnAppearing()
        {
            try
            {
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                StaffDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(32));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private void StaffDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                StaffDutiesListView.IsVisible = false;
                StaffDutiesListView.Unfocus();
                StaffDutiesListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("StaffDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }

        private async void EditTitleToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopUpView.KindOfPage = 2;
                await PopupNavigation.Instance.PushAsync(page: new PopUpView());
                //PopUpLayout.IsVisible = true;
                //MainLayout.IsVisible = false;
            }
            catch (Exception exception)
            {
                await DisplayAlert("EditTitleToolbarItem_Clicked", exception.Message, "OK");
            }
        }

        private async void StuffPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                StaffPicker.SelectedItem = null;
                StaffDutiesListView.SelectedItem = null;
                StaffPicker.IsVisible = false;
                StaffPicker.Unfocus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("StuffPicker_OnUnfocused", ex.Message, "OK");
            }
        }


        private async void StuffPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (StaffPicker.SelectedIndex == 0)
                {
                    UserDialogs.Instance.ShowLoading();
                    myWholeList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    StaffDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(32)); ;
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (StaffPicker.SelectedIndex == 1)
                {
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("StuffPicker_OnSelectedIndexChanged", ex.Message, "OK");
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
        
        private void StaffDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)StaffDutiesListView.SelectedItem;
                currentTask = task;
                StaffPicker.IsVisible = true;
                StaffPicker.Focus();
            }
            catch (Exception ex)
            {
                DisplayAlert("StaffDutiesListView_OnItemTapped", ex.Message, "OK");
            }
        }
    }
}