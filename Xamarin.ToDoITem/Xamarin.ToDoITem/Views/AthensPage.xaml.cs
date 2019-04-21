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
        //public ObservableCollection<Task> myList;
        private List<Task> generalList;
        private bool editOption;
        private IOrderedEnumerable<Task> _sortedList;
        private Task currentTask;
        private Task mainTask;
        private DateTime selectedDate;

        public AthensPage ()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);                
                FillPickers();
            }
            catch (Exception ex)
            {
                DisplayAlert("AthensPage", ex.Message, "OK");
            }
        }

        protected async void AddNewItem(Task thisTask, Type thisType, DateTime thisDate)
        {
            try
            {

            }
            catch (Exception exception)
            {
                await DisplayAlert("AddNewItem: ", exception.Message, "OK");
            }
        }

        protected async void FillPickers()
        {
            try
            {
                EditGeneralCostPicker.Items.Clear();
                EditGeneralCostPicker.Items.Add("Αλλαγή Τιμής");
                EditGeneralCostPicker.Items.Add("Νέα Προθεσμία");
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
            catch (Exception e)
            {                
                await DisplayAlert("FillPickers Error: ", e.Message, "OK");
            }            
        }
        
        protected override async void OnAppearing()
        {
            try
            {
                generalList = await App.ItemController.GetTasks(); // var
                //myList = new ObservableCollection<Task>(generalList);
                _sortedList = from cTask in generalList orderby cTask.Date, cTask.Date select cTask; //IEnumerable
                AthensToDoListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(15)); //instead of myList
                AthensExodusListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(14)); //instead of myList
                AthensCostsListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(17)); //instead of myList                
                foreach (var t in generalList)
                {
                    if (t.Type == 22)
                    {
                        mainTask = t;
                        LastDayCostLabel.Text = mainTask.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture); ;
                        GeneralCostPriceLabel.Text = mainTask.Price + "€";
                    }
                }
                //foreach (Task t in generalList)
                //{
                //    if (t.Type == 22)
                //    {
                //        LastDayCostLabel.Text = t.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                //    }
                //}
                CountGeneralCosts();
                //TapGestureRecognizers();                
                UserDialogs.Instance.HideLoading();
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
                foreach (var item in _sortedList.Where(x=>x.Type == taskType)) // myList
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

        private void CountGeneralCosts()
        {
            try
            {
                double allCosts = 0.0;
                foreach (var expense in _sortedList)
                {
                    if (expense.Type == 17)
                    {
                        allCosts += double.Parse(expense.Price); // eg 700
                    }
                };
                var stringCosts = allCosts.ToString(CultureInfo.InvariantCulture); // "700"
                var stringCostsFinal = stringCosts.Remove(stringCosts.Length - 1); // "70"
                double almostFinalDouble = double.Parse(stringCostsFinal); // 70                
                double mainTaskPrice = double.Parse(mainTask.Price.Remove(mainTask.Price.Length-1)); // eg 40
                almostFinalDouble += mainTaskPrice;// 70 + 40 = 110
                AllCostsLabel.Text = "Σύνολo: " + almostFinalDouble + " €";
            }
            catch (Exception e)
            {
                DisplayAlert("CountGeneralCosts", e.Message, "OK");
            }
        }

        private async void AthensToDoChoicesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AthensToDoChoicesPicker.SelectedIndex == 0) // delete task
                {
                    UserDialogs.Instance.ShowLoading();
                    //myList.Remove(currentTask);
                    generalList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensToDoListView.ItemsSource = null;
                    //AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    _sortedList = from cTask in generalList orderby cTask.Date, cTask.Date select cTask;
                    AthensToDoListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(15));
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
                    AthensToDoListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(15));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία Υποχρέωσης!", "OK");
                }
                else if (AthensToDoChoicesPicker.SelectedIndex == 2)
                {
                    ToDoDatePicker.IsVisible = true;
                    ToDoDatePicker.Focus();
                    editOption = true;
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
                if (!CheckDuplicates(AthensToDoEntry.Text, 15))
                {
                    await DisplayAlert("ΣΦΑΛΜΑ", "Υπάρχει ήδη, δοκιμάστε κάτι άλλο!", "ΟΚ");
                    AthensToDoEntry.Text = "";
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
                    //AthensToDoListView.ItemsSource = myList.ToList().Where(x => x.Type.Equals(15));
                    _sortedList = from cTask in generalList orderby cTask.Date, cTask.Date select cTask;
                    AthensToDoListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(15));
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
                    generalList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensExodusListView.ItemsSource = null;
                    AthensExodusListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(14));
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
                    AthensExodusListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(14));
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
                if (!CheckDuplicates(AthensToDoEntry.Text, 14))
                {
                    await DisplayAlert("ΣΦΑΛΜΑ", "Υπάρχει ήδη, δοκιμάστε κάτι άλλο!", "ΟΚ");
                    AthensToDoEntry.Text = "";
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
                    AthensToDoListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(15));
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
                    generalList.Add(task);
                    await App.ItemController.InsertTask(task);
                    AthensToDoListView.ItemsSource = null;
                    AthensToDoListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(15));
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
                    generalList.Remove(currentTask);
                    await App.ItemController.DeleteTask(currentTask);
                    AthensCostsListView.ItemsSource = null;
                    AthensCostsListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(17));
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
                    AthensCostsListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(17));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Μετονομασία!", "OK");
                }
                else if (AthensCostChoicesPicker.SelectedIndex == 2) // edit price
                {
                    var result = await UserDialogs.Instance.PromptAsync("Τιμή", null, "Τιμή Εξόδου", "Ακυρο", currentTask.Price, inputType: InputType.Number);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading();
                    currentTask.Price = result.Text;
                    await App.ItemController.UpdateTask(currentTask);
                    CountGeneralCosts();
                    AthensCostsListView.ItemsSource = null;
                    AthensCostsListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(17));
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(null, "Επιτυχής Αλλαγή Τιμής!", "OK");
                }
                //NOT IMPLEMENTED YET
                //else if (AthensCostChoicesPicker.SelectedIndex == 3) // edit date
                //{
                //    AthensCostDatepicker.IsVisible = true;
                //    AthensCostDatepicker.Focus();
                //}
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
            generalList.Add(task);
            await App.ItemController.InsertTask(task);
            AthensExodusListView.ItemsSource = null;
            AthensExodusListView.ItemsSource = _sortedList.Where(x => x.Type.Equals(14)); //generalList.ToList()
            AthensExodusEntry.Text = "";
            AthensExodusListView.SelectedItem = null;
            UserDialogs.Instance.HideLoading();
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
                if (!CheckDuplicates(AthensToDoEntry.Text, 17))
                {
                    await DisplayAlert("ΣΦΑΛΜΑ", "Υπάρχει ήδη, δοκιμάστε κάτι άλλο!", "ΟΚ");
                    AthensToDoEntry.Text = "";
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
                generalList.Add(task);
                await App.ItemController.InsertTask(task);
                CountGeneralCosts();
                AthensCostsListView.ItemsSource = null;
                AthensCostsListView.ItemsSource = _sortedList.ToList().Where(x => x.Type.Equals(17));
                AthensCostEntry.Text = "";
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Προσθήκη", "Νέο Έξοδο προστέθηκε", "OK");

            }
            catch (Exception exception)
            {
                await DisplayAlert("CostButton_OnClicked", exception.Message, "OK");
            }
        }
        
        private async void ToDoDatePicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                ToDoDatePicker.IsVisible = false;
                ToDoDatePicker.Unfocus();
            }
            catch (Exception exception)
            {
                await DisplayAlert("ToDoDatePicker_OnUnfocused", exception.Message, "OK");
            }
        }

        private async void EditGeneralCost_OnClicked(object sender, EventArgs e)
        {
            try
            {
                EditGeneralCostPicker.IsVisible = true;
                EditGeneralCostPicker.Focus();                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void EditGeneralCostPicker_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                EditGeneralCostPicker.IsVisible = false;
                EditGeneralCostPicker.Unfocus();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void EditGeneralCostPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EditGeneralCostPicker.SelectedItem = null;
                if (EditGeneralCostPicker.SelectedIndex == 0) //edit price
                {
                    var result = await UserDialogs.Instance.PromptAsync("Τιμή γενικού εξόδου", null, "Αλλαγή", "Ακυρο", mainTask.Price, inputType: InputType.Number);
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                        return;
                    }
                    mainTask.Price = result.Text;
                    await App.ItemController.UpdateTask(mainTask);
                    GeneralCostPriceLabel.Text = mainTask.Price;
                    //CountGeneralCosts();
                    await DisplayAlert(null, "Επιτυχής Αλλαγή τιμής!", "OK");
                }
                else if (EditGeneralCostPicker.SelectedIndex == 1) // edit date
                {
                    AthensGeneralCostDatepicker.IsVisible = true;
                    AthensGeneralCostDatepicker.Focus();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void AthensGeneralCostDatepicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                mainTask.Date = AthensGeneralCostDatepicker.Date;
                await App.ItemController.UpdateTask(mainTask);
                LastDayCostLabel.Text = mainTask.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                AthensGeneralCostDatepicker.IsVisible = false;
                AthensGeneralCostDatepicker.Unfocus();                
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(null, "Επιτυχής Αλλαγή Ημερομηνίας!", "OK");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}