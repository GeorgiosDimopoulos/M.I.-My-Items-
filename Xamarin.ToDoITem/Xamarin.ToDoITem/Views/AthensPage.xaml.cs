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
    public partial class AthensPage : TabbedPage
    {

        public ObservableCollection<Task> myList;
        private Task currentTask;
        private DatePicker datePicker;
        private DateTime selectedDate;

        public AthensPage ()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);
                AthensToDoChoicesPicker.Items.Clear();
                AthensExodusChoicesPicker.Items.Clear();
                AthensToDoChoicesPicker.Items.Add("Διαγραφή");
                AthensToDoChoicesPicker.Items.Add("Μετονομασία");
                AthensToDoChoicesPicker.Items.Add("Αλλαγή Ημερομηνίας");
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
                    await DisplayAlert(null, "NOT YET!", "OK");
                }
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
                AthensToDoChoicesPicker.IsVisible = true;
                AthensToDoChoicesPicker.Focus();
                AthensToDoListView.SelectedItem = null;
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

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
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
                    datePicker.IsVisible = true;
                    datePicker.Focus();
                    datePicker.DateSelected += DatePicker_DateSelected;
                }
                AthensExodusChoicesPicker.IsVisible = false;
                AthensExodusChoicesPicker.IsVisible = false;
            }
            catch (Exception exception)
            {
                await DisplayAlert(null, exception.Message, "OK");
            }
        }

        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                selectedDate = datePicker.Date; // TimeSpan timeSpan =datePicker.Date - startDatePicker.Date + (includeSwitch.IsToggled ? TimeSpan.FromDays(1) : TimeSpan.Zero);
                await DisplayAlert(null, selectedDate.ToString(CultureInfo.InvariantCulture), "OK");
                //resultLabel.Text = String.Format("{0} day{1} between dates",timeSpan.Days, timeSpan.Days == 1 ? "" : "s");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void AthensExodusChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                AthensExodusChoicesPicker.IsVisible = false;
                AthensExodusChoicesPicker.Unfocus();
                AthensExodusListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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
                var currentDate = "-";
                var resultTime = await UserDialogs.Instance.PromptAsync("Ημερομηνία Εξόδου", null, "Προσθήκη Ημερομηνίας", "Ακυρο", "Προσθέστε Ημερομηνία", inputType: InputType.Default);
                if (!string.IsNullOrEmpty(resultTime.Text))
                {
                    currentDate = resultTime.Text;
                }
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = AthensExodusEntry.Text,
                    Date = DateTime.Parse(currentDate),
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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void ToDoDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                selectedDate = ToDoDatePicker.Date;
                UserDialogs.Instance.ShowLoading();
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
                //await DisplayAlert(null, ToDoDatePicker.Date.ToString(CultureInfo.InvariantCulture), "OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}