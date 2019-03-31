using System;
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
                NavigationPage.SetHasNavigationBar(this, false);
                TapRecognizers();
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
                    Navigation.PushAsync(new ExpensesPage(), true);
                };
                workoutsstap.Tapped += (object sender, EventArgs e) =>
                {
                    Navigation.PushAsync(new WodsPage(), true);
                };
                marketsTap.Tapped += (object sender, EventArgs e) =>
                {
                    Navigation.PushAsync(new MarketPage(), true); //PushModalAsync
                };
                dutiessTap.Tapped += (object sender, EventArgs e) =>
                {
                    Navigation.PushAsync(new ToDoPage(), true); //DutiesPage
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
            //((MainViewModel)BindingContext).DatabaseService.NetworkConnect();
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