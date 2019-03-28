using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DutiesPage : ContentPage
	{
        public ObservableCollection<Task> myDutiesList;
        private Task currentTask;

        public DutiesPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                DutyChoicesPicker.Items.Clear();
                DutyChoicesPicker.Items.Add("Διαγραφή");
                DutyChoicesPicker.Items.Add("Μετονομασία");
            }
            catch (Exception e)
            {
                DisplayAlert("DutiesView", e.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                var list = await App.ItemController.GetTasks();
                myDutiesList = new ObservableCollection<Task>(list); //.Where
                DutiesListView.ItemsSource = myDutiesList.Where(x => x.Type.Equals(4)); //.Where(x=>x.Type.Equals(1))
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private async void AddDutyButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ExpenseEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }

                var task = new Task
                {
                    Text = ExpenseEntry.Text,
                    Type = 4
                };
                myDutiesList.Add(task);
                await App.ItemController.InsertTask(task);
                DutiesListView.ItemsSource = null;
                DutiesListView.ItemsSource = myDutiesList.ToList().Where(x => x.Type.Equals(4));
                ExpenseEntry.Text = "";
                await DisplayAlert("Προσθήκη", "Η νέα υποχρέωση προστέθηκε", "OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
                await Navigation.PushAsync(new MainPage(), true);
                //Environment.Exit(1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void ItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)DutiesListView.SelectedItem;
                currentTask = task;
                DutyChoicesPicker.IsVisible = true;
                DutyChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("ItemsListView_OnItemTapped", ex.Message, "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await Navigation.PushModalAsync(new MainPage(), true);
                });
                return true;
            }
            catch (Exception e)
            {
                DisplayAlert("OnBackButtonPressed", e.Message, "OK");
                return false;
            }
        }

        private async void DutyChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DutyChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Διαγραφή Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        myDutiesList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myDutiesList.ToList().Where(x => x.Type.Equals(4));
                        await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                        //await Navigation.PopAsync();
                    }
                }
                if (DutyChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Προϊόντος", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);                    
                    DutiesListView.ItemsSource = null;
                    DutiesListView.ItemsSource = myDutiesList.ToList().Where(x => x.Type.Equals(4));
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                    //await Navigation.PopAsync();
                }
                DutiesListView.SelectedItem = null;
                DutyChoicesPicker.Unfocus();
                DutyChoicesPicker.IsVisible = false;
                await Navigation.PopAsync();
                await Navigation.PushModalAsync(new MainPage(), true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}