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
    public partial class WodsPage : TabbedPage
    {
        public ObservableCollection<Task> myList;
        private Task cTask;

        public WodsPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasBackButton(this, false);
            }
            catch (Exception e)
            {
                DisplayAlert("WodsPage Constructor Error", e.Message, "Yes");
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                var list = await App.ItemController.GetTasks();
                myList = new ObservableCollection<Task>(list);
                WoDChoicesPicker.Items.Clear();
                WoDChoicesPicker.Items.Add("Διαγραφή WoD");
                WoDChoicesPicker.Items.Add("Μετονομασία WoD");
                WoDChoicesPicker.Items.Add("Επεξεργασία Ημερομηνίας");
                OtherWodsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2));
                ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1));
                InfosListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0));
                RecordsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(16));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("OnAppearing Error", exception.Message, "Yes");
            }
        }
        
        private async void ItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                cTask = (Task)ItemsListView.SelectedItem;
                WoDChoicesPicker.IsVisible = true;
                WoDChoicesPicker.Focus();
            }
            catch (Exception exception)
            {
                await DisplayAlert("WodsListView_OnItemTapped Error", exception.Message, "Yes");
            }
        }

        private async void InfoListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)InfosListView.SelectedItem;
                InfosListView.SelectedItem = null;
                if (await DisplayAlert(null, "Διαγραφή info?", "OK", "Όχι"))
                {
                    myList.Remove(task);
                    await App.ItemController.DeleteTask(task);
                    await DisplayAlert(null, "Διαγράφηκε το info", "ΟΚ");
                    InfosListView.ItemsSource = null;
                    InfosListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0));
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("InfoListView_OnItemTapped Error", exception.Message, "Yes");
            }
        }

        private async void RecordsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                cTask = (Task)RecordsListView.SelectedItem;
                RecordsListView.SelectedItem = null;
                if (await DisplayAlert(null, "Διαγραφή Ρεκορ?", "ΟΚ", "Όχι"))
                {
                    myList.Remove(cTask);
                    await App.ItemController.DeleteTask(cTask);
                    RecordsListView.ItemsSource = null;
                    RecordsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(16));
                    await DisplayAlert(null, "Διαγράφηκε το Ρεκορ", "ΟΚ");
                    RecordsListView.SelectedItem = null;
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("RecordsListView_OnItemTapped Error", exception.Message, "Yes");
            }
        }
        
        private async void NewRecord_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(RecordTaskEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για Record!", "OK");
                    return;
                }
                if (await DisplayAlert(null, "Προσθήκη και ημερομηνίας?", "OK", "Όχι"))
                {
                    RecordDatePicker.IsVisible = true;
                    RecordDatePicker.Focus();
                }
                else
                {
                    var task = new Task
                    {
                        Text = RecordTaskEntry.Text,
                        Type = 16
                    };
                    myList.Add(task);
                    await App.ItemController.InsertTask(task);
                    ItemsListView.ItemsSource = null;
                    ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(16));
                    TaskEntry.Text = "";
                    await DisplayAlert("DONE", "Νέο Record προστέθηκε", "OK");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        private async void NewWoD_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TaskEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για WoD!", "OK");
                    return;
                }
                WoDDatePicker.IsVisible = true;
                WoDDatePicker.Focus();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void NewInfo_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(InfoTaskEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για Info!", "OK");
                    return;
                }
                var task = new Task
                {
                    Text = InfoTaskEntry.Text,
                    Type = 0
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                InfosListView.ItemsSource = null;
                InfosListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0));
                InfoTaskEntry.Text = "";
                await DisplayAlert("DONE", "Νέο info προστέθηκε", "OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void NewOtherWoD_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(OtherWodEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι για άλλο WoD!", "OK");
                    return;
                }

                var task = new Task
                {
                    Text = OtherWodEntry.Text + " (" +DateTime.Now.ToString("dd/MM/yyyy")+ ")",
                    Type = 2
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                OtherWodsListView.ItemsSource = null;
                OtherWodsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2));
                OtherWodEntry.Text = "";
                await DisplayAlert("DONE", "Νέο άλλο WoD προστέθηκε", "OK");
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
                await Navigation.PushAsync(new MainPage(), true); //Environment.Exit(1);
            }
            catch (Exception exception)
            {
                await DisplayAlert("MenuItem_OnClicked", exception.Message, "OK");
            }            
        }

        private async void WoDChoicesPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (WoDChoicesPicker.SelectedIndex == 0) // delete task
                {
                    UserDialogs.Instance.ShowLoading();
                    myList.Remove(cTask);
                    await App.ItemController.DeleteTask(cTask);
                    ItemsListView.ItemsSource = null;
                    ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(5));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                } else if (WoDChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Εξόδου", "Ακυρο", cTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    cTask.Text = result.Text;
                    await App.ItemController.UpdateTask(cTask);
                    ItemsListView.ItemsSource = null;
                    ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (WoDChoicesPicker.SelectedIndex == 2) // rename task
                {
                    WoDDatePicker.IsVisible = true;
                    WoDDatePicker.Focus();
                }
                WoDChoicesPicker.SelectedItem = null;
                //WoDChoicesPicker.Unfocus();
                ItemsListView.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("WoDChoicesPicker_OnSelectedIndexChanged: ", exception.Message, "OK");
            }
        }

        private async void WoDChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                ItemsListView.SelectedItem = null;
                WoDChoicesPicker.IsVisible = false;
                WoDChoicesPicker.SelectedItem = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("WoDChoicesPicker_OnUnfocused: ", exception.Message, "OK");
            }
        }

        private async void WoDDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();                
                cTask.Date = WoDDatePicker.Date;
                await App.ItemController.UpdateTask(cTask);
                ItemsListView.ItemsSource = null;
                ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1));
                ItemsListView.SelectedItem = null;
                WoDDatePicker.Unfocus();
                WoDDatePicker.IsVisible = false;
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέα υποχρέωση προστέθηκε", "OK");
            }
            catch (Exception exception)
            {
                await DisplayAlert("WoDChoicesPicker_OnUnfocused: ", exception.Message, "OK");
            }
        }
        
        private async void RecordDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = RecordTaskEntry.Text, Type = 0, Date = RecordDatePicker.Date
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                RecordsListView.ItemsSource = null;
                RecordTaskEntry.Text = "";
                RecordsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(16));
                RecordsListView.SelectedItem = null;
                RecordDatePicker.Unfocus();
                RecordDatePicker.IsVisible = false;
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέο ρεκόρ προστέθηκε", "OK");
            }
            catch (Exception exception)
            {
                await DisplayAlert("RecordDatePicker_OnDateSelected: ", exception.Message, "OK");
            }
        }
    }
}