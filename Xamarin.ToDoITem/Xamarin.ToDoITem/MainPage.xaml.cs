using System;
using Acr.UserDialogs;
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

        public void kindsOfIndicators()
        {
            //App.Indicator.Start();
            //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
            //UserDialogs.Instance.ShowLoading();

            //UserDialogs.Instance.Alert("BYE","CLOSE").Dispose(); //dlg.Dispose();
            //App.Indicator.Stop();
            //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
            //UserDialogs.Instance.HideLoading();
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
                    //App.Indicator.Start();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                    //UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new ExpensesPage(), true);
                    //UserDialogs.Instance.Alert("BYE","CLOSE").Dispose(); //dlg.Dispose();
                    //App.Indicator.Stop();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
                    //UserDialogs.Instance.HideLoading();
                };
                workoutsstap.Tapped += (object sender, EventArgs e) =>
                {
                    //App.Indicator.Start();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                    //UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new WodsPage(), true);
                    //App.Indicator.Stop();
                    //UserDialogs.Instance.HideLoading();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
                };
                marketsTap.Tapped += (object sender, EventArgs e) =>
                {
                    //App.Indicator.Start();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                    //UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new MarketPage(), true); //PushModalAsync
                    //App.Indicator.Stop();
                    //UserDialogs.Instance.HideLoading();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
                };
                dutiessTap.Tapped += (object sender, EventArgs e) =>
                {
                    //App.Indicator.Start();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                    //UserDialogs.Instance.ShowLoading();
                    Navigation.PushAsync(new ToDoPage(), true);
                    //App.Indicator.Stop();
                    //UserDialogs.Instance.HideLoading();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
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