using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.ToDoITem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public ObservableCollection<Task> myList;

        public MainTabbedPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);                
            }
            
        }

        protected override async void OnAppearing()
        {
            try
            {
                var list = await App.ItemController.GetTasks();
                myList = new ObservableCollection<Task>(list);

                //ItemsListView.ItemsSource = myList;
                RecordsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2)); //.Where(x => x.Type == 1)
                ItemsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(1)); //.Where(x => x.Type == 0)
                InfosListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(0)); //.Where(x => x.Type == 2)                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        private async void ItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var task = (Task)ItemsListView.SelectedItem;
            ItemsListView.SelectedItem = null;

            if (await DisplayAlert(null, "Delete selected task?", "Yes", "No"))
            {
                myList.Remove(task);
                await App.ItemController.DeleteTask(task);
                ItemsListView.ItemsSource = myList;
            }
        }

        private async void InfoListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var task = (Task)InfosListView.SelectedItem;
            InfosListView.SelectedItem = null;

            if (await DisplayAlert(null, "Delete selected task?", "Yes", "No"))
            {
                myList.Remove(task);
                await App.ItemController.DeleteTask(task);
                InfosListView.ItemsSource = myList;
            }
        }

        private async void RecordsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var task = (Task)RecordsListView.SelectedItem;
            RecordsListView.SelectedItem = null;

            if (await DisplayAlert(null, "Delete selected task?", "Yes", "No"))
            {
                myList.Remove(task);
                await App.ItemController.DeleteTask(task);
                RecordsListView.ItemsSource = myList;
            }
        }

        private async void NewWoD_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TaskEntry.Text))
                {
                    await DisplayAlert(null, "Cannot add empty task!", "OK");
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
                    await DisplayAlert(null, "Cannot add empty task!", "OK");
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

        private async void NewRecord_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(RecordEntry.Text))
                {
                    await DisplayAlert(null, "Cannot add empty task!", "OK");
                    return;
                }

                var task = new Task
                {
                    Text = RecordEntry.Text,
                    Type = 2
                };

                myList.Add(task);
                await App.ItemController.InsertTask(task);
                RecordsListView.ItemsSource = null;
                RecordsListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(2));
                RecordEntry.Text = "";
                await DisplayAlert("DONE", "Νέο ρεκόρ προστέθηκε", "OK");
                //await Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);                
            }
        }
    }
}