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
                OldDutyChoicesPicker.Items.Add("Διαγραφή");
                OldDutyChoicesPicker.Items.Add("Ενεργοποίηση");
                OldDutyChoicesPicker.Items.Add("Μετονομασία");
                DutyChoicesPicker.Items.Add("Διαγραφή");
                DutyChoicesPicker.Items.Add("Μετονομασία");
                DutyChoicesPicker.Items.Add("Απενεργοποίηση");
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
                myWholeList = new ObservableCollection<Task>(list);
                DutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(4));
                OldDutiesListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(6));
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
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                DutiesListView.ItemsSource = null;
                DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
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

        private async void OldDutyChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OldDutyChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Οριστική Διαγραφή Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        myWholeList.Remove(currentTask);                        
                        await App.ItemController.DeleteTask(currentTask);
                        OldDutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        await DisplayAlert(null, "Επιτυχής Οριστική Διαγραφή!", "OK");
                        }
                }
                else if (OldDutyChoicesPicker.SelectedIndex == 1) // enable task
                {
                    if (await DisplayAlert(null, "Ενεργοποίηση Υποχρέωσης?", "ΝΑΙ", "ΟΧΙ")) {
                        
                        currentTask.Type = 4;
                        await App.ItemController.UpdateTask(currentTask);
                        OldDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                        await DisplayAlert(null, "Επιτυχής Ενεργοποίηση Μεταφορά!", "OK");
                    }
                }
                OldDutiesListView.SelectedItem = null;
                OldDutyChoicesPicker.Unfocus();
                OldDutyChoicesPicker.IsVisible = false;
                //await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new MainPage(), true);
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
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
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
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    DutiesListView.ItemsSource = null;
                    DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                    //await Navigation.PopAsync();
                }
                else if (DutyChoicesPicker.SelectedIndex == 2) // disable task
                {
                    if (await DisplayAlert(null, "Έγινε η Υποχρέωση?", "ΝΑΙ", "ΟΧΙ"))
                    {
                        //myDutiesList.Remove(currentTask);
                        //myOldDutiesList.Add(currentTask);
                        currentTask.Type = 6;
                        await App.ItemController.UpdateTask(currentTask);
                        DutiesListView.ItemsSource = null;
                        OldDutiesListView.ItemsSource = null;
                        DutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(4));                        
                        OldDutiesListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(6));
                        await DisplayAlert(null, "Ολοκληρώθηκε η υποχρέωση!", "OK");
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
    }
}