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
                TodoPicker.Items.Clear();
                TodoPicker.Items.Add("Διαγραφή");
                TodoPicker.Items.Add("Αλλαγή");
                KidsPicker.Items.Clear();
                KidsPicker.Items.Add("Διαγραφή");
                KidsPicker.Items.Add("Αλλαγή");
                StaffPicker.Items.Clear();
                StaffPicker.Items.Add("Διαγραφή");
                StaffPicker.Items.Add("Αλλαγή");
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

        private void TodoDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                TodoDutiesListView.IsVisible = false;
                TodoDutiesListView.Unfocus();
                TodoDutiesListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("TodoDutiesListView_OnUnfocused", ex.Message, "OK");
            }
        }
        private void KidsDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                KidsDutiesListView.IsVisible = false;
                KidsDutiesListView.Unfocus();
                KidsDutiesListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("KidsDutiesListView_OnUnfocused", ex.Message, "OK");
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

        private void TodoDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)TodoDutiesListView.SelectedItem;
                currentTask = task;
                TodoPicker.IsVisible = true;
                TodoPicker.Focus();
            }
            catch (Exception ex)
            {
                DisplayAlert("TodoDutiesListView_OnItemTapped", ex.Message, "OK");
            }
        }
        private void KidsDutiesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)KidsDutiesListView.SelectedItem;
                currentTask = task;
                KidsPicker.IsVisible = true;
                KidsPicker.Focus();
            }
            catch (Exception ex)
            {
                DisplayAlert("KidsDutiesListView_OnItemTapped", ex.Message, "OK");
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
                    Text = ToDoItemEntry.Text,
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
                    Text = KidItemEntry.Text,
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
        
        private async void TodoPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TodoPicker.SelectedIndex == 0)
                {
                    UserDialogs.Instance.ShowLoading();
                    myWholeList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    TodoDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(33)); ;
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (TodoPicker.SelectedIndex == 1)
                {

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("TodoPicker_OnSelectedIndexChanged", ex.Message, "OK");
            }
        }

        private async void KidsPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (KidsPicker.SelectedIndex == 0)
                {
                    UserDialogs.Instance.ShowLoading();
                    myWholeList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    KidsDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(31)); ;
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (KidsPicker.SelectedIndex == 1)
                {
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("KidsPicker_OnSelectedIndexChanged", ex.Message, "OK");
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

        private async void TodoPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                TodoDutiesListView.SelectedItem = null;
                TodoPicker.SelectedItem = null;
                TodoPicker.IsVisible = false;
                TodoPicker.Unfocus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("TodoPicker_OnUnfocused", ex.Message, "OK");
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
        private async void KidsPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                KidsPicker.SelectedItem = null;
                KidsDutiesListView.SelectedItem = null;
                KidsPicker.IsVisible = false;
                KidsPicker.Unfocus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("KidsPicker_OnUnfocused", ex.Message, "OK");
            }
        }
    }
}