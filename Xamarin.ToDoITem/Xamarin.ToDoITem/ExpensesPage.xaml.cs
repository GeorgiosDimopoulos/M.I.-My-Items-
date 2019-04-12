using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesPage : TabbedPage
    {
        public ObservableCollection<Task> myExpensesList;
        private Task currentTask;

        public ExpensesPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                //BarBackgroundColor = Color.FromHex("#ff3030"); 
                NavigationPage.SetHasBackButton(this, false);
                OldExpenseChoicesPicker.Items.Clear();
                OldExpenseChoicesPicker.Items.Add("Διαγραφή");
                OldExpenseChoicesPicker.Items.Add("Μετονομασία");
                OldExpenseChoicesPicker.Items.Add("Ενεργοποίηση");
                ExpenseChoicesPicker.Items.Clear();
                ExpenseChoicesPicker.Items.Add("Διαγραφή");
                ExpenseChoicesPicker.Items.Add("Μετονομασία");
                ExpenseChoicesPicker.Items.Add("Απενεργοποίηση");
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
                ExpensesListView.ItemsSource = myExpensesList.Where(x => x.Type.Equals(5));
                OldExpensesListView.ItemsSource = myExpensesList.Where(x => x.Type.Equals(8));
                UserDialogs.Instance.HideLoading();
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
                UserDialogs.Instance.ShowLoading();
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
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέο Έξοδο προστέθηκε", "OK");
                //ItemsListView.ItemsSource = null;
                //ItemsListView.ItemsSource = myList;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
        
        private async void OldItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)ExpensesListView.SelectedItem;
                currentTask = task;
                OldExpensesListView.SelectedItem = null;
                OldExpenseChoicesPicker.IsVisible = true;
                OldExpenseChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("OldItemsListView_OnItemTapped", ex.Message, "OK");
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

        private async void OldExpenseChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OldExpenseChoicesPicker.SelectedIndex == 0) // delete task
                {
                    UserDialogs.Instance.ShowLoading();
                    myExpensesList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    OldExpensesListView.ItemsSource = null;
                    OldExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (OldExpenseChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Εξόδου", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    OldExpensesListView.ItemsSource = null;
                    OldExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (OldExpenseChoicesPicker.SelectedIndex == 2) // enable task
                {
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Type = 5;
                    await App.ItemController.UpdateTask(currentTask);
                    OldExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = null;
                    OldExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(8));
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    //await DisplayAlert(null, "Επιτυχής Ενεργοποίηση!", "OK");
                }
                OldExpenseChoicesPicker.IsVisible = false;
                OldExpenseChoicesPicker.IsVisible = false;
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
                    UserDialogs.Instance.ShowLoading();
                    myExpensesList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    ExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
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
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    ExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (ExpenseChoicesPicker.SelectedIndex == 2) // disable task
                {
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Type = 8;
                    await App.ItemController.UpdateTask(currentTask);
                    OldExpensesListView.ItemsSource = null;
                    ExpensesListView.ItemsSource = null;
                    OldExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(8));
                    ExpensesListView.ItemsSource = myExpensesList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Απενεργοποίηση!", "OK");
                }
                ExpenseChoicesPicker.IsVisible = false;
                ExpenseChoicesPicker.Unfocus();
                ExpenseChoicesPicker.SelectedItem = null;
                //await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new MainPage(), true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("OnBackButtonPressed", ex.Message, "OK");
            }
        }

        private void OldExpenseChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                OldExpensesListView.SelectedItem = null;
                OldExpenseChoicesPicker.Unfocus();
                OldExpenseChoicesPicker.IsVisible = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void ExpenseChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                ExpenseChoicesPicker.IsVisible = false;
                ExpenseChoicesPicker.Unfocus();
                ExpensesListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}