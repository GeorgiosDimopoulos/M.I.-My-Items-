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
    public partial class MarketPage : TabbedPage
    {
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;

        public MarketPage()
        {
            try
            {
                InitializeComponent();
                ProductChoicesPicker.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);
                OldProductChoicesPicker.Items.Clear();
                OldProductChoicesPicker.Items.Add("Διαγραφή");
                OldProductChoicesPicker.Items.Add("Μετονομασία");
                OldProductChoicesPicker.Items.Add("Ενεργοποίηση");
                ProductChoicesPicker.Items.Clear();
                ProductChoicesPicker.Items.Add("Διαγραφή");
                ProductChoicesPicker.Items.Add("Μετονομασία");
                ProductChoicesPicker.Items.Add("Αγορά");
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
                myWholeList = new ObservableCollection<Task>(marketList);
                MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
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
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                MarketListView.ItemsSource = null;
                MarketListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(3));
                ProductEntry.Text = "";
                //await DisplayAlert("DONE", "Νέο Προϊόν προστέθηκε", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddDutyButton_OnClicked", ex.Message, "OK");
            }
        }

        private async void OldItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)OldMarketListView.SelectedItem;
                currentTask = task;
                OldMarketListView.SelectedItem = null;
                OldProductChoicesPicker.IsVisible = true;
                OldProductChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("OldItemsListView_OnItemTapped error: ", ex.Message, "OK");
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
                await DisplayAlert("ItemsListView_OnItemTapped error", ex.Message, "OK");
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
        
        private void OldProductChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            OldProductChoicesPicker.IsVisible = false;
            OldProductChoicesPicker.Unfocus();
        }

        private async void OldProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OldProductChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Οριστική Διαγραφή προϊόντος;", "NAI", "OXI"))
                    {
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7)); ;
                    }
                }
                else if (OldProductChoicesPicker.SelectedIndex == 1) // rename task
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
                    OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                }
                else if (OldProductChoicesPicker.SelectedIndex == 2) // enable task
                {
                    if (await DisplayAlert(null, "Ενεργοποίηση προϊόντος;", "NAI", "OXI"))
                    {
                        currentTask.Type = 3;
                        await App.ItemController.UpdateTask(currentTask);
                        await DisplayAlert(null, "Επιτυχής Ενεργοποίηση!", "OK");
                        OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                        MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                    }
                }
                OldProductChoicesPicker.IsVisible = false;
                OldProductChoicesPicker.Unfocus();
                OldMarketListView.SelectedItem = null;
                //await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new MainPage(), true);
            }
            catch (Exception exception)
            {
                await DisplayAlert("OldProductChoicesPicker_SelectedIndexChanged error", exception.Message, "OK");
            }
        }

        private async void ProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductChoicesPicker.SelectedIndex == 0) // delete task
            {
                if (await DisplayAlert(null, "Διαγραφή προϊόντος;", "NAI", "OXI"))
                {
                    myWholeList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3)); ;
                }
            }
            else if (ProductChoicesPicker.SelectedIndex == 1) // rename task
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
                MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
            }
            else if (ProductChoicesPicker.SelectedIndex == 2) // disable task
            {
                currentTask.Type = 7;
                await App.ItemController.UpdateTask(currentTask);
                OldMarketListView.ItemsSource = null;
                MarketListView.ItemsSource = null;
                OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                await DisplayAlert(null, "Επιτυχής Απενεργοποίηση!", "OK");
            }
            ProductChoicesPicker.IsVisible = false;
            ProductChoicesPicker.Unfocus();
            ProductChoicesPicker.SelectedItem = null;
            MarketListView.SelectedItem = null;
        }

        private void ProductChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            ProductChoicesPicker.IsVisible = false;
            ProductChoicesPicker.Unfocus();
        }
    }
}