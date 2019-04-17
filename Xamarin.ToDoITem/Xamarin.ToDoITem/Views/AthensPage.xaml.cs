﻿using System;
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
    public partial class AthensPage : TabbedPage
    {

        public ObservableCollection<Task> myList;
        private bool editOption;
        private Task currentTask;
        private DateTime selectedDate;

        public AthensPage ()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);                
                AthensExodusChoicesPicker.Items.Clear();
                AthensToDoChoicesPicker.Items.Clear();
                AthensToDoChoicesPicker.Items.Add("Διαγραφή");
                AthensToDoChoicesPicker.Items.Add("Μετονομασία");
                AthensToDoChoicesPicker.Items.Add("Αλλαγή Ημερομηνίας");
                AthensCostChoicesPicker.Items.Clear();
                AthensCostChoicesPicker.Items.Add("Διαγραφή");
                AthensCostChoicesPicker.Items.Add("Μετονομασία");
                AthensCostChoicesPicker.Items.Add("Αλλαγή Τιμής");
                AthensExodusChoicesPicker.Items.Add("Διαγραφή");
                AthensExodusChoicesPicker.Items.Add("Μετονομασία");
                AthensExodusChoicesPicker.Items.Add("Αλλαγή Ημερομηνίας");

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
                var list = await App.ItemController.GetTasks();
                myList = new ObservableCollection<Task>(list);
                AthensToDoListView.ItemsSource = myList.Where(x => x.Type.Equals(15));
                AthensExodusListView.ItemsSource = myList.Where(x => x.Type.Equals(14));
                AthensCostsListView.ItemsSource = myList.Where(x => x.Type.Equals(17));                
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }

        private async void AthensToDoChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AthensToDoChoicesPicker.SelectedIndex == 0) // delete task
                {
                    UserDialogs.Instance.ShowLoading();
                    myList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (AthensToDoChoicesPicker.SelectedIndex == 1) // rename task
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
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία Υποχρέωσης!", "OK");
                }
                else if (AthensToDoChoicesPicker.SelectedIndex == 2)
                {
                    ToDoDatePicker.IsVisible = true;
                    ToDoDatePicker.Focus();
                    editOption = true;
                }
                //AthensToDoChoicesPicker.Unfocus();
                //AthensToDoChoicesPicker.IsVisible = false;
                //AthensToDoListView.SelectedItem = null;
                //AthensToDoChoicesPicker.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("AthensToDoChoicesPicker_SelectedIndexChanged error", ex.Message, "OK");
            }
        }

        private void AthensToDoChoicesPicker_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                AthensToDoChoicesPicker.IsVisible = false;
                AthensToDoChoicesPicker.Unfocus();
                AthensToDoListView.SelectedItem = null;
                AthensToDoChoicesPicker.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("AthensToDoChoicesPicker_SelectedIndexChanged error", ex.Message, "OK");
            }
        }

        private async void ToDoImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AthensToDoEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για προσθήκη!", "OK");
                    return;
                }
                if (await DisplayAlert("Προσθήκη", "Προσθήκη Ημερομηνίας", "ΟΚ","OXI"))
                {
                    ToDoDatePicker.IsVisible = true;
                    ToDoDatePicker.Focus();
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    var task = new Task
                    {
                        Text = AthensToDoEntry.Text, Type = 15, Date = DateTime.Today
                    };
                    await App.ItemController.InsertTask(task);
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(14));
                    AthensToDoEntry.Text = "";
                    AthensToDoListView.SelectedItem = null;
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Προσθήκη!", "OK");
                }                
            }
            catch (Exception ex)
            {
                await DisplayAlert("ImageButton_Clicked error", ex.Message, "OK");
            }
        }

        private void AthensToDoListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)AthensToDoListView.SelectedItem;
                currentTask = task;
                AthensToDoChoicesPicker.IsVisible = true;
                AthensToDoChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                DisplayAlert("AthensToDoListView_ItemTapped error", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Environment.Exit(0);
                //await Navigation.PopAsync();
                //await Navigation.PushAsync(new MainPage(), true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void AthensExodusListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)AthensExodusListView.SelectedItem;
                currentTask = task;
                AthensExodusChoicesPicker.IsVisible = true;
                AthensExodusChoicesPicker.Focus();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void AthensExodusChoicesPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AthensExodusChoicesPicker.SelectedIndex == 0) // delete task
                {
                    UserDialogs.Instance.ShowLoading();
                    myList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensExodusListView.ItemsSource = null;
                    AthensExodusListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(14));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if (AthensExodusChoicesPicker.SelectedIndex == 1) // rename task
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
                    AthensExodusListView.ItemsSource = null;
                    AthensExodusListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(14));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (AthensExodusChoicesPicker.SelectedIndex == 2) // date of task
                {
                    ExodusDatePicker.IsVisible = true;
                    ExodusDatePicker.Focus();
                    ExodusDatePicker.DateSelected += ExodusDatePicker_OnDateSelected;
                }
                AthensExodusChoicesPicker.IsVisible = false;
                AthensExodusChoicesPicker.IsVisible = false;
            }
            catch (Exception exception)
            {
                await DisplayAlert(null, exception.Message, "OK");
            }
        }
        
        private async void AthensExodusChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                AthensExodusChoicesPicker.IsVisible = false;
                AthensExodusChoicesPicker.Unfocus();
                AthensExodusChoicesPicker.SelectedItem = null;
                AthensExodusListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("AthensExodusChoicesPicker_OnUnfocused", exception.Message, "OK");
            }
        }

        private async void ExodusButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AthensExodusEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για προσθήκη!", "OK");
                    return;
                }
                ExodusDatePicker.IsVisible = true;
                ExodusDatePicker.Focus();
            }
            catch (Exception exception)
            {
                await DisplayAlert("ExodusButton_OnClicked", exception.Message, "OK");
            }
        }

        private async void ToDoDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {                
                if (editOption)
                {
                    UserDialogs.Instance.ShowLoading();
                    selectedDate = ToDoDatePicker.Date;
                    currentTask.Date = selectedDate;
                    await App.ItemController.UpdateTask(currentTask);
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    AthensToDoListView.SelectedItem = null;
                    ToDoDatePicker.Unfocus();
                    ToDoDatePicker.IsVisible = false;
                    //ToDoDatePicker.Date = null;
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Αλλαγη Ημερομηνίας!", "OK");
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    selectedDate = ToDoDatePicker.Date;
                    var task = new Task
                    {
                        Text = AthensToDoEntry.Text,
                        Date = selectedDate.Date,
                        Type = 15
                    };
                    myList.Add(task);
                    await App.ItemController.InsertTask(task);
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    AthensToDoEntry.Text = "";
                    UserDialogs.Instance.HideLoading();
                    AthensToDoListView.SelectedItem = null;
                    await DisplayAlert("Προσθήκη", "Νέα υποχρέωση προστέθηκε", "OK");
                    ToDoDatePicker.Unfocus();
                    ToDoDatePicker.IsVisible = false;
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert(null, exception.Message, "OK");
            }
        }

        private async void AthensCostChoicesPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //AthensCostChoicesPicker.Unfocus();                
                if (AthensCostChoicesPicker.SelectedIndex == 0) // delete
                {
                    UserDialogs.Instance.ShowLoading();
                    myList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensCostsListView.ItemsSource = null;
                    AthensCostsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(17));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                }
                else if(AthensCostChoicesPicker.SelectedIndex == 1) // rename
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
                    AthensCostsListView.ItemsSource = null;
                    AthensCostsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(17));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (AthensCostChoicesPicker.SelectedIndex == 2) // edit price
                {
                    var result = await UserDialogs.Instance.PromptAsync("Τιμή", null, "Τιμή Εξόδου", "Ακυρο", currentTask.Text, inputType: InputType.Number);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Price = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    AthensCostsListView.ItemsSource = null;
                    AthensCostsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(17));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Αλλαγή Τιμής!", "OK");
                }
                else if (AthensCostChoicesPicker.SelectedIndex == 3) // edit date
                {
                    AthensCostDatepicker.IsVisible = true;
                    AthensCostDatepicker.Focus();
                }
                AthensCostChoicesPicker.IsVisible = false;
                AthensCostChoicesPicker.Unfocus();
                AthensCostsListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("AthensCostChoicesPicker_OnSelectedIndexChanged", exception.Message, "OK");
            }
        }

        private async void AthensCostChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                AthensCostChoicesPicker.Unfocus();
                AthensCostChoicesPicker.IsVisible = false;
                AthensCostChoicesPicker.SelectedItem = null;
                AthensExodusListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("AthensCostChoicesPicker_OnUnfocused", exception.Message, "OK");
            }
        }

        private async void AthensCostsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)AthensCostsListView.SelectedItem;
                currentTask = task;
                AthensCostChoicesPicker.IsVisible = true;
                AthensCostChoicesPicker.Focus();
                AthensCostsListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("AthensCostsListView_OnItemTapped", exception.Message, "OK");
            }
        }

        private async void ExodusDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            selectedDate = ExodusDatePicker.Date;
            var task = new Task
            {
                Text = AthensExodusEntry.Text,
                Date = selectedDate.Date,
                Type = 14
            };
            myList.Add(task);
            await App.ItemController.InsertTask(task);
            AthensExodusListView.ItemsSource = null;
            AthensExodusListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(14));
            AthensExodusEntry.Text = "";
            AthensExodusListView.SelectedItem = null;
            UserDialogs.Instance.HideLoading();
        }

        private async void AthensCostDatepicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                selectedDate = AthensCostDatepicker.Date;
                var task = new Task
                {
                    Text = AthensCostEntry.Text,
                    Date = selectedDate.Date,
                    Type = 14
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                AthensCostsListView.ItemsSource = null;
                AthensCostsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(17));
                AthensCostEntry.Text = "";
                AthensCostsListView.SelectedItem = null;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("AthensCostDatepicker_OnDateSelected", exception.Message, "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                Environment.Exit(0);                
                return true;
            }
            catch (Exception e)
            {
                DisplayAlert("OnBackButtonPressed", e.Message, "OK");
                return false;
            }
        }

        private void ExodusDatePicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            ExodusDatePicker.Unfocus();
            ExodusDatePicker.IsVisible = false;
        }

        private async void CostButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AthensCostEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για προσθήκη!", "OK");
                    return;
                }
                var result = await UserDialogs.Instance.PromptAsync("Τιμή", null, "Προσθήκη", "Ακυρο", "Προσθήκη Τιμής", inputType: InputType.Number);
                if (string.IsNullOrEmpty(result.Text))
                {
                    await DisplayAlert(null, "Πληκτρολόγησε κάτι για τη τιμή!", "OK");
                    return;
                }
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = AthensCostEntry.Text,
                    Type = 17,
                    Price = result.Text
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                AthensCostsListView.ItemsSource = null;
                AthensCostsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(17));
                AthensCostEntry.Text = "";
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέο Έξοδο προστέθηκε", "OK");

            }
            catch (Exception exception)
            {
                await DisplayAlert("CostButton_OnClicked", exception.Message, "OK");
            }
        }
    }
}