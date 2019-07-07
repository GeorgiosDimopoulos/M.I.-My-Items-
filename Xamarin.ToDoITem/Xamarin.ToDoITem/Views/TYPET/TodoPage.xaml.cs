using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views.TYPET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoPage : ContentPage
	{
        private List<Task> generalList;
        private bool editOption;
        private IOrderedEnumerable<Task> _sortedList;
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;
        private Task mainTask;

        public TodoPage ()
		{
            try
            {
                InitializeComponent();
                TodoPicker.Items.Clear();
                TodoPicker.Items.Add("Διαγραφή");
                TodoPicker.Items.Add("Αλλαγή");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                TodoDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(33));
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

        private async void EditTitleToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new PopupPage());
                //var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Τίτλου", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                //UserDialogs.Instance.ShowLoading();
                //if (string.IsNullOrEmpty(result.Text))
                //{
                //    await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                //    return;
                //}
                //Title = result.Text;
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("EditTitleToolbarItem_Clicked", exception.Message, "OK");
            }
        }
    }
}