using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views
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
                OldProductChoicesPicker.Items.Add("Αλλαγή τιμής");
                ProductChoicesPicker.Items.Clear();                
                ProductChoicesPicker.Items.Add("Διαγραφή");
                ProductChoicesPicker.Items.Add("Μετονομασία");
                ProductChoicesPicker.Items.Add("Αγορά");
                ProductChoicesPicker.Items.Add("Αλλαγή τιμής");
                OtherProductChoicesPicker.Items.Clear();
                OtherProductChoicesPicker.Items.Add("Διαγραφή");
                OtherProductChoicesPicker.Items.Add("Μετονομασία");
                OtherProductChoicesPicker.Items.Add("Αλλαγή τιμής");
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
                //App.Indicator.Start();
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                OtherMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(10));
                //App.Indicator.Stop();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }
        
        private async void AddOtherProductButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(OtherProductEntry.Text))
                {
                    await DisplayAlert(null, "Γράψτε κάτι!", "OK");
                    return;
                }
                var currentPrice = "-";
                var resultTime = await UserDialogs.Instance.PromptAsync("Τιμή Προϊόντος", null, "Προσθήκη", "Ακυρο", "Προσθέστε Τιμή", inputType: InputType.DecimalNumber);
                if (!string.IsNullOrEmpty(resultTime.Text))
                {
                    currentPrice = resultTime.Text;
                }
                UserDialogs.Instance.ShowLoading();
                var task = new Task
                {
                    Text = OtherProductEntry.Text,
                    Price = currentPrice,
                    Type = 10
                };
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                OtherMarketListView.ItemsSource = null;
                OtherMarketListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(10));
                OtherProductEntry.Text = "";
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddOtherProductButton_OnClicked", ex.Message, "OK");
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
                var currentPrice = "-";
                var resultTime = await UserDialogs.Instance.PromptAsync("Τιμή Προϊόντος", null, "Προσθήκη", "Ακυρο", "Προσθέστε Τιμής", inputType: InputType.Default);
                if (!string.IsNullOrEmpty(resultTime.Text))
                {
                    currentPrice = resultTime.Text;
                }
                var task = new Task
                {
                    Text = ProductEntry.Text,
                    Price = currentPrice,
                    Type = 3
                };
                UserDialogs.Instance.ShowLoading();
                myWholeList.Add(task);
                await App.ItemController.InsertTask(task);
                MarketListView.ItemsSource = null;
                MarketListView.ItemsSource = myWholeList.ToList().Where(x => x.Type.Equals(3));
                ProductEntry.Text = "";
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddDutyButton_OnClicked", ex.Message, "OK");
            }
        }

        private async void OtherItemsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var task = (Task)OtherMarketListView.SelectedItem;
                currentTask = task;
                OtherMarketListView.SelectedItem = null;
                OtherMarketListView.IsVisible = true;
                OtherProductChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("OtherItemsListView_OnItemTapped", ex.Message, "OK");
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
                UserDialogs.Instance.ShowLoading();
                var task = (Task)MarketListView.SelectedItem;
                currentTask = task;
                currentTask.Type = 7;
                await App.ItemController.UpdateTask(currentTask);
                MarketListView.ItemsSource = null;
                OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(null, "Επιτυχής Αγορά!", "OK");
                //var task = (Task)MarketListView.SelectedItem;
                //currentTask = task;
                //MarketListView.SelectedItem = null;
                //ProductChoicesPicker.IsVisible = true;
                //ProductChoicesPicker.Focus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("ItemsListView_OnItemTapped error", ex.Message, "OK");
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
            OldMarketListView.SelectedItem = null;
        }

        private async void OtherProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OtherProductChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Οριστική Διαγραφή προϊόντος;", "NAI", "OXI"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        OtherMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(10));
                        await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else if (OtherProductChoicesPicker.SelectedIndex == 1) // rename task
                {
                    var result = await UserDialogs.Instance.PromptAsync("Μετονομασία", null, "Μετονομασία Προϊόντος", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    OtherMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(10));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (OtherProductChoicesPicker.SelectedIndex == 2) // change price
                {
                    var result = await UserDialogs.Instance.PromptAsync("Αλλαγή", null, "Αλλαγή Τιμής", "Ακυρο", currentTask.Price, inputType: InputType.Default);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Price= result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    OtherMarketListView.ItemsSource = null;                    
                    OtherMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(10));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Αλλαγή Τιμής!", "OK");                    
                }
                OtherProductChoicesPicker.IsVisible = false;
                OtherProductChoicesPicker.Unfocus();
                OtherMarketListView.SelectedItem = null;
                currentTask = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("OtherProductChoicesPicker_SelectedIndexChanged error", ex.Message, "OK");
            }
        }
        
        private async void OldProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (OldProductChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Οριστική Διαγραφή προϊόντος;", "NAI", "OXI"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));                        
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Διαγραφή προϊόντος!", "OK");
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
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);                    
                    OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (OldProductChoicesPicker.SelectedIndex == 2) // enable task
                {
                    if (await DisplayAlert(null, "Ενεργοποίηση προϊόντος;", "NAI", "OXI"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        currentTask.Type = 3;
                        await App.ItemController.UpdateTask(currentTask);                        
                        OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                        MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));                        
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Ενεργοποίηση!", "OK");
                    }
                }
                OldProductChoicesPicker.IsVisible = false;
                OldProductChoicesPicker.Unfocus();
                OldMarketListView.SelectedItem = null;
                currentTask = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("OldProductChoicesPicker_SelectedIndexChanged error", exception.Message, "OK");
            }
        }

        private void ChangeItemIndex(object sender, EventArgs e)
        {
            try
            {
                //var currentIndex = listView1.SelectedItems[0].Index;
                //var item = listView1.Items[index];
                //if (currentIndex > 0)
                //{
                //    listView1.Items.RemoveAt(currentIndex);
                //    listView1.Items.Insert(currentIndex - 1, item);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void ProductChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ProductChoicesPicker.SelectedIndex == 0) // delete task
                {
                    if (await DisplayAlert(null, "Διαγραφή προϊόντος;", "NAI", "OXI"))
                    {
                        UserDialogs.Instance.ShowLoading();
                        myWholeList.Remove(currentTask);
                        await App.ItemController.DeleteTask(currentTask);
                        MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3)); ;
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(null, "Επιτυχής Διαγραφή!", "OK");
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
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Text = result.Text;
                    await App.ItemController.UpdateTask(currentTask);                    
                    await App.ItemController.DeleteTask(currentTask);
                    MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (ProductChoicesPicker.SelectedIndex == 2) // disable task
                {
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Type = 7;
                    await App.ItemController.UpdateTask(currentTask);
                    OldMarketListView.ItemsSource = null;
                    MarketListView.ItemsSource = null;
                    OldMarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(7));
                    MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));                    
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Απενεργοποίηση!", "OK");
                }
                else if (ProductChoicesPicker.SelectedIndex == 3) // change task's price
                {
                    var isNumeric = int.TryParse(currentTask.Price, out int n);
                    if (!isNumeric)
                    {
                        currentTask.Price = "0";
                    }
                    UserDialogs.Instance.ShowLoading();
                    var result = await UserDialogs.Instance.PromptAsync("Τιμή Προϊόντος", null, "Τιμή Προϊόντος", "Ακυρο", currentTask.Price, inputType: InputType.Default);
                    currentTask.Price = string.IsNullOrEmpty(result.Text) ? "-" : result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    MarketListView.ItemsSource = null;
                    MarketListView.ItemsSource = myWholeList.Where(x => x.Type.Equals(3));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Προσθήκη Τιμής!", "OK");
                }
                ProductChoicesPicker.IsVisible = false;
                ProductChoicesPicker.Unfocus();
                ProductChoicesPicker.SelectedItem = null;
                MarketListView.SelectedItem = null;
                currentTask = null;
            }
            catch (Exception exception)
            {
                await DisplayAlert("ProductChoicesPicker_SelectedIndexChanged error", exception.Message, "OK");
            }
        }

        private void OtherProductChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            OtherProductChoicesPicker.IsVisible = false;
            OtherMarketListView.SelectedItem = null;
            OtherProductChoicesPicker.Unfocus();
        }
        
        private void ProductChoicesPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            ProductChoicesPicker.IsVisible = false;
            ProductChoicesPicker.Unfocus();
            MarketListView.SelectedItem = null;
        }

        private async void MarketProduct_OnClicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                ImageButton cbutton = (ImageButton)sender; //var task = (Task)MarketListView.SelectedItem;
                MarketListView.SelectedItem = null;
                Grid listViewItem = (Grid)cbutton.Parent;
                Label clabel = (Label)listViewItem.Children[0];
                foreach (Task product in MarketListView.ItemsSource)
                {
                    if (product.Text.Equals(clabel.Text))
                    {
                        currentTask = product;
                    }
                }
                ProductChoicesPicker.IsVisible = true;
                ProductChoicesPicker.Focus();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("MarketProduct_OnClicked error", exception.Message, "OK");
            }
        }
    }
}