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

namespace MyItems.Views.TYPET
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypetPage : TabbedPage
    {
        private List<Task> generalList;        
        private bool editOption;
        private IOrderedEnumerable<Task> _sortedList;
        public ObservableCollection<Task> myWholeList;
        private Task currentTask;
        private Task mainTask;
        private DateTime selectedDate;

        public TypetPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                NavigationPage.SetHasBackButton(this, false);
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
                var marketList = await App.ItemController.GetTasks();
                myWholeList = new ObservableCollection<Task>(marketList);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing", e.Message, "OK");
            }
        }
        
        private async void AddToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var result = await UserDialogs.Instance.PromptAsync("Νέα Σελίδα", null, "Τίτλος Νέας Σελίδας", "Ακυρο", currentTask.Text, inputType: InputType.Default);
                UserDialogs.Instance.ShowLoading();
                Children.Add(new ContentPage());
                //if (string.IsNullOrEmpty(result.Text))
                //{
                //    await DisplayAlert(null, "Πληκτρολόγησε κάτι!", "OK");
                //    return;
                //}
                //this.Children.Add(new ContentPage());
                //await Navigation.PushAsync(this);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                await DisplayAlert("AddToolbarItem_Clicked", exception.Message, "OK");
            }
        }
    }
}