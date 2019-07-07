using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views.TYPET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KidsPage : ContentPage
	{
        private List<Task> generalList;
        private bool editOption;
        private IOrderedEnumerable<Task> _sortedList;
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;
        private Task mainTask;

        public KidsPage ()
		{
            try
            {
                InitializeComponent();
                KidsPicker.Items.Clear();
                KidsPicker.Items.Add("Διαγραφή");
                KidsPicker.Items.Add("Αλλαγή");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void EditTitleToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Τίτλου", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                UserDialogs.Instance.ShowLoading();
                if (string.IsNullOrEmpty(result.Text))
                {
                    await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                    return;
                }
                Title = result.Text;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("EditTitleToolbarItem_Clicked", exception.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                KidsDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(31));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
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