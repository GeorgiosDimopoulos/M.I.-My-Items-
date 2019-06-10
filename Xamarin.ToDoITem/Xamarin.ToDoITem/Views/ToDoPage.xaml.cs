using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyItems.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoPage : TabbedPage
    {
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;

        public ToDoPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);
                DutyChoicesPicker.Items.Clear();
                OldDutyChoicesPicker.Items.Clear();
                TodayDutyChoicesPicker.Items.Clear();
                TodayDutyChoicesPicker.Items.Add("Διαγραφή");
                TodayDutyChoicesPicker.Items.Add("Μετονομασία");
                TodayDutyChoicesPicker.Items.Add("Γενική Ενεργοποίηση");
                TodayDutyChoicesPicker.Items.Add("Απενεργοποίηση");
                OldDutyChoicesPicker.Items.Add("Διαγραφή");
                OldDutyChoicesPicker.Items.Add("Γενική Ενεργοποίηση");                
                OldDutyChoicesPicker.Items.Add("Άμεση Ενεργοποίηση");
                OldDutyChoicesPicker.Items.Add("Μετονομασία");
                DutyChoicesPicker.Items.Add("Διαγραφή");
                DutyChoicesPicker.Items.Add("Μετονομασία");
                DutyChoicesPicker.Items.Add("Απενεργοποίηση");
                DutyChoicesPicker.Items.Add("Άμεση Ενεργοποίηση");
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
                //App.Indicator.Start();
                //EnableIndicator_Clicked();
                var list = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(list);
                DutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(4));
                OldDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(6));
                TodayDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(9));
                //DisableIndicator_Clicked();
                UserDialogs.Instance.HideLoading();
                //App.Indicator.Stop();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private bool CheckDuplicates(string possibleText, int taskType)
        {
            try
            {
                foreach (var item in myWholeList.Where(x => x.Type == taskType)) // myList
                {
                    if (possibleText == item.Text)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                DisplayAlert("CheckDuplicates", e.Message, "OK");
                return false;
            }
        }

        private async void AddTodayDutyButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TodayExpenseEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = TodayExpenseEntry.Text,
                    Type = 9
                };
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                TodayDutiesListView.ItemsSource = null;
                TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                TodayExpenseEntry.Text = "";
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέα άμεση υποχρέωση προστέθηκε", "OK");

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = ExpenseEntry.Text,
                    Type = 4
                };
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                DutiesListView.ItemsSource = null;
                DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                ExpenseEntry.Text = "";
                UserDialogs.Instance.HideLoading();
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
                var task = (Task)OldDutiesListView.SelectedItem;
                currentTask = task;
                OldDutyChoicesPicker.IsVisible = true;
                OldDutyChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("ItemsListView_OnItemTapped", ex.Message, "OK");
            }
        }
        
        private async void TodayItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)TodayDutiesListView.SelectedItem;
                currentTask = task;
                TodayDutyChoicesPicker.IsVisible = true;
                TodayDutyChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("TodayItemsListView_OnItemTapped", ex.Message, "OK");
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

        private async void TodayDutyChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TodayDutyChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Διαγραφή Οριστική Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        TodayDutiesListView.ItemsSource = null;
                        TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Οριστική Διαγραφή!", "OK");
                    }
                }
                else if (TodayDutyChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Υποχρέωσης", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    TodayDutiesListView.ItemsSource = null;
                    TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (TodayDutyChoicesPicker.SelectedIndex == 2)
                {
                    if (await DisplayAlert(null, "Γενική Ενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 4;
                        await App.ItemController.UpdateTask(currentTask);
                        TodayDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                        TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Γενική Ενεργοποίηση!", "OK");
                    }
                }
                else if (TodayDutyChoicesPicker.SelectedIndex == 3)
                {
                    if (await DisplayAlert(null, "Απενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 6;
                        await App.ItemController.UpdateTask(currentTask);
                        TodayDutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Απενεργοποίηση!", "OK");
                    }
                }
                TodayDutyChoicesPicker.Unfocus();
                TodayDutyChoicesPicker.IsVisible = false;
                TodayDutyChoicesPicker.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("TodayDutyChoicesPicker_SelectedIndexChanged Error:", exception.Message, "OK");
            }
        }
        
        private async void OldDutyChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OldDutyChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Οριστική Διαγραφή Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);                        
                        await App.ItemController.DeleteTask(currentTask);
                        OldDutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Οριστική Διαγραφή!", "OK");
                        }
                }
                else if (OldDutyChoicesPicker.SelectedIndex == 1) // enable task
                {
                    if (await DisplayAlert(null, "Γενική Ενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ")) {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 4;
                        await App.ItemController.UpdateTask(currentTask);
                        OldDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Γενική Ενεργοποίηση Μεταφορά!", "OK");
                    }
                }
                else if (OldDutyChoicesPicker.SelectedIndex == 2) // immediate enable task
                {
                    if (await DisplayAlert(null, "Άμεση Ενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 9;
                        await App.ItemController.UpdateTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        TodayDutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Άμεση  Ενεργοποίηση!", "OK");
                    }
                }
                else if (OldDutyChoicesPicker.SelectedIndex == 3) // rename task
                {
                    if (await DisplayAlert(null, "Μετονομασία Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Υποχρέωσης", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                        if (string.IsNullOrEmpty(result.Text))
                        {
                            await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                            return;
                        }
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Text = result.Text;
                        await App.ItemController.UpdateTask(currentTask);
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                    }
                }
                OldDutiesListView.SelectedItem = null;
                OldDutyChoicesPicker.Unfocus();
                OldDutyChoicesPicker.IsVisible = false;
            }
            catch (Exception exception)
            {
                await DisplayAlert("OldDutyChoicesPicker_SelectedIndexChanged", exception.Message, "OK");
            }
        }

        private async void DutyChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DutyChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Διαγραφή Οριστική Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Οριστική Διαγραφή!", "OK");
                    }
                }
                else if (DutyChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Υποχρέωσης", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    DutiesListView.ItemsSource = null;
                    DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                    //await Navigation.PopAsync();
                }
                else if (DutyChoicesPicker.SelectedIndex == 2) // disable task
                {
                    if (await DisplayAlert(null, "Έγινε η Υποχρέωση?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 6;
                        await App.ItemController.UpdateTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));                        
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Ολοκληρώθηκε η υποχρέωση!", "OK");
                    }
                }
                else if (DutyChoicesPicker.SelectedIndex == 3) // immediate enable task
                {
                    if (await DisplayAlert(null, "Άμεση Ενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 9;
                        await App.ItemController.UpdateTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        TodayDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                        TodayDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(9));
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Ολοκληρώθηκε η Άμεση Ενεργοποίηση!", "OK");
                    }
                }
                DutyChoicesPicker.Unfocus();
                DutyChoicesPicker.IsVisible = false;
                DutyChoicesPicker.SelectedItem = null;
                //await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new MainPage(), true);
            }
            catch (Exception exception)
            {
                await DisplayAlert("DutyChoicesPicker_SelectedIndexChanged", exception.Message, "OK");
            }
        }

        private async void TodayDutiesListView_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                TodayDutiesListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("TodayDutiesListView_OnUnfocused", exception.Message, "OK");
            }
        }

        private void TodayDutyChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                TodayDutiesListView.SelectedItem = null;
                TodayDutyChoicesPicker.IsVisible = false;
                TodayDutyChoicesPicker.Unfocus();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void DutyChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            DutiesListView.SelectedItem = null;
            DutyChoicesPicker.IsVisible = false;
            DutyChoicesPicker.Unfocus();
        }

        private void OldDutyChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            OldDutiesListView.SelectedItem = null;
            OldDutyChoicesPicker.IsVisible = false;
            OldDutyChoicesPicker.Unfocus();
        }
    }
}