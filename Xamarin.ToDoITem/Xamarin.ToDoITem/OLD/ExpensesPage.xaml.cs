using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.OLD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpensesPage : ContentPage
	{
        public ObservableCollection<Task> myExpensesList;
        private Task currentTask;

        public ExpensesPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);
                ExpenseChoicesPicker.Items.Add("Διαγραφή");
                ExpenseChoicesPicker.Items.Add("Μετονομασία");
                ExpenseChoicesPicker.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            catch (Exception e)
            {
                DisplayAlert("ExpensesView", e.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                var list = await App.ItemController.GetTasks();
                myExpensesList = new ObservableCollection<Task>(list);
                ExpensesListView.ItemsSource = myExpensesList.Where(x => x.Type.Equals(5)); //.Where(x=>x.Type.Equals(1))
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private async void AddExpenseButton_OnClicked(object sender, EventArgs e)
        {
                try
                {
                    if (string.IsNullOrEmpty(ExpenseEntry.Text))
                    {
                        await DisplayAlert(null, "Γράψτε κάτι για προσθήκη!", "OK");
                        return;
                    }

                    var task = new Task
                    {
                        Text = ExpenseEntry.Text,
                        Type = 5
                    };
                    myExpensesList.Add(task);
                    await App.ItemController.InsertTask(task);
                    ExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    ExpenseEntry.Text = "";
                    await DisplayAlert("Προσθήκη", "Νέο Έξοδο προστέθηκε", "OK");
                    //ItemsListView.ItemsSource = null;
                    //ItemsListView.ItemsSource = myList;
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
                var task = (Task)ExpensesListView.SelectedItem;
                currentTask = task;
                ExpensesListView.SelectedItem = null;
                ExpenseChoicesPicker.IsVisible = true;
                ExpenseChoicesPicker.Focus();
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

        private async void ExpenseChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExpenseChoicesPicker.SelectedIndex == 0) // delete task
                {
                    myExpensesList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    ExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (ExpenseChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Εξόδου", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    ExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    await DisplayAlert(null, "Επιτυχής Προσθήκη!", "OK");
                }
                ExpenseChoicesPicker.IsVisible = false;
                ExpenseChoicesPicker.Unfocus();
                //await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new MainPage(), true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("OnBackButtonPressed", ex.Message, "OK");
            }
        }
    }
}