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
                OtherWodsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2));
                ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1));
                InfosListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0));
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
                var task = (Task)ItemsListView.SelectedItem;
                ItemsListView.SelectedItem = null;

                if (await DisplayAlert(null, "Διαγραφή WoD?", "ΟΚ", "Όχι"))
                {
                    myList.Remove(task);
                    await App.ItemController.DeleteTask(task);                    
                    await DisplayAlert(null, "Διαγράφηκε το WoD", "ΟΚ");
                    ItemsListView.ItemsSource = null;
                    ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0));
                }
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
                var task = (Task)OtherWodsListView.SelectedItem;
                OtherWodsListView.SelectedItem = null;
                if (await DisplayAlert(null, "Διαγραφή WoD?", "ΟΚ", "Όχι"))
                {
                    myList.Remove(task);
                    await App.ItemController.DeleteTask(task); //RecordsListView.ItemsSource = myList;
                    await DisplayAlert(null, "Διαγράφηκε το WoD", "ΟΚ");
                    OtherWodsListView.ItemsSource = null;
                    OtherWodsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2));
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("RecordsListView_OnItemTapped Error", exception.Message, "Yes");
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

                var task = new Task
                {
                    Text = TaskEntry.Text,Type = 1
                };
                myList.Add(task);
                await App.ItemController.InsertTask(task);
                ItemsListView.ItemsSource = null;
                ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1));
                TaskEntry.Text = "";
                await DisplayAlert("DONE", "Νέο WoD προστέθηκε", "OK");
                //ItemsListView.ItemsSource = null;
                //ItemsListView.ItemsSource = myList;
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
                Console.WriteLine(exception);
            }            
        }
    }
}