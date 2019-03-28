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
	public partial class MarketPage : ContentPage
	{
        public ObservableCollection<Task> myMarketList;
        private Task currentTask;

        public MarketPage()
        {
            try
            {
                InitializeComponent();
                ProductChoicesPicker.IsVisible = false;
                ProductChoicesPicker.Items.Clear();
                ProductChoicesPicker.Items.Add("Διαγραφή");
                ProductChoicesPicker.Items.Add("Μετονομασία");
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
                var marketList = await App.ItemController.GetTasks();
                myMarketList = new ObservableCollection<Task>(marketList);
                MarketListView.ItemsSource = myMarketList.Where(x => x.Type.Equals(3));
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
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

        private async void AddProductButton_OnClicked(object sender, EventArgs e)
        {
                try
                {
                    if (string.IsNullOrEmpty(ProductEntry.Text))
                    {
                        await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                        return;
                    }

                    var task = new Task
                    {
                        Text = ProductEntry.Text,
                        Type = 3
                    };
                    myMarketList.Add(task);
                    await App.ItemController.InsertTask(task);
                    MarketListView.ItemsSource = null;
                    MarketListView.ItemsSource = myMarketList.ToList().Where(x => x.Type.Equals(3));
                    ProductEntry.Text = "";
                    //await DisplayAlert("DONE", "Νέο Προϊόν προστέθηκε", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddDutyButton_OnClicked", ex.Message, "OK");
            }
        }

        private async void ItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)MarketListView.SelectedItem;
                currentTask = task;
                MarketListView.SelectedItem = null;
                ProductChoicesPicker.IsVisible = true;
                ProductChoicesPicker.Focus();
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

        private async void ProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductChoicesPicker.SelectedIndex == 0) // delete task
            {
                if (await DisplayAlert(null, "Διαγραφή προϊόντος;", "NAI", "OXI"))
                {
                    myMarketList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    MarketListView.ItemsSource = myMarketList.Where(x => x.Type.Equals(3)); ;
                }
            }
            if (ProductChoicesPicker.SelectedIndex == 1) // rename task
            {
                var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Προϊόντος", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                if (string.IsNullOrEmpty(result.Text))
                {
                    await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                    return;
                }
                currentTask.Text = result.Text;
                await App.ItemController.UpdateTask(currentTask);
                await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                await App.ItemController.DeleteTask(currentTask);
                MarketListView.ItemsSource = myMarketList.Where(x => x.Type.Equals(3));
            }
            ProductChoicesPicker.IsVisible = false;
            ProductChoicesPicker.Unfocus();
            MarketListView.SelectedItem = null;
            await Navigation.PopAsync();
            await Navigation.PushModalAsync(new MainPage(), true);
        }

        private void ProductChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            ProductChoicesPicker.IsVisible = false;
            ProductChoicesPicker.Unfocus();
        }
    }
}