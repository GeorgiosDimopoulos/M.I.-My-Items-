using System;
using System.Threading;
using Acr.UserDialogs;
using MyItems.Views;
using Xamarin.Forms;

namespace MyItems
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            try
            {
                InitializeComponent();
                //App.Indicator.Start();
                NavigationPage.SetHasNavigationBar(this, false);
                TapRecognizers();
                //App.Indicator.Stop();
            }
            catch (Exception e)
            {
                DisplayAlert("MainPage Constructor", e.Message, "OK");
            }
        } 

        private void TapRecognizers()
        {
            try
            {
                TapGestureRecognizer dutiessTap = new TapGestureRecognizer();
                TapGestureRecognizer marketsTap = new TapGestureRecognizer();
                TapGestureRecognizer workoutsstap = new TapGestureRecognizer();
                TapGestureRecognizer expensessTap = new TapGestureRecognizer();

                expensessTap.Tapped += (object sender, EventArgs e) =>
                {
                    UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new ExpensesPage(), true); //AthensPage
                };
                workoutsstap.Tapped += (object sender, EventArgs e) =>
                {
                    UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new WodsPage(), true);
                };
                marketsTap.Tapped += (object sender, EventArgs e) =>
                {
                    UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new MarketPage(), true); //PushModalAsync
                };
                dutiessTap.Tapped += (object sender, EventArgs e) =>
                {
                    UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new ToDoPage(), true);
                };

                DutiessGrid.GestureRecognizers.Add(dutiessTap);
                MarketsGrid.GestureRecognizers.Add(marketsTap);
                ExpensessGrid.GestureRecognizers.Add(expensessTap);
                WorkOutsGrid.GestureRecognizers.Add(workoutsstap);
            }
            catch (Exception e)
            {
                DisplayAlert("TapRecognizers", e.Message, "OK");
            }
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //await((IDataContext)BindingContext).Refresh(null);
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //
            //});
        }

        protected override bool OnBackButtonPressed()
        {
            Environment.Exit(0);
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    if (await DisplayAlert(null, "Σίγουρη Έξοδος", "ΝΑΙ", "ΟΧΙ"))
            //    {
            //        Environment.Exit(0);
            //        //await Navigation.PushModalAsync(new LogPage(), true);
            //    }
            //});
            return true;
        }
    }
}