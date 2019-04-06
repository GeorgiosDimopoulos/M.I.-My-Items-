using System;
using System.Threading;
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
                //UserDialogs.Instance.ShowLoading();
                //App.Indicator.Start();
                NavigationPage.SetHasNavigationBar(this, false);
                //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                TapRecognizers();
                //Thread.Sleep(2000);
                //UserDialogs.Instance.HideLoading();
                //App.Indicator.Stop();
                //UserDialogs.Instance.HideLoading();
                //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Dispose();
            }
            catch (Exception e)
            {
                DisplayAlert("MainPage Constructor", e.Message, "OK");
            }
        }

        //void MyMethod()
        //{
        //    UserDialogs.Instance.ShowLoading("TEST", MaskType.Black);
        //    ViewModel.LoadData().ContinueWith((task) =>
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    });
        //}

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
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ");
                    UserDialogs.Instance.ShowLoading();
                    Thread.Sleep(1000);
                    Navigation.PushAsync(new ExpensesPage(), true);
                    //UserDialogs.Instance.Alert("BYE","CLOSE").Dispose();
                    //UserDialogs.Instance.Progress("ΠΕΡΙΜΕΝΕΤΕ").Hide();
                    //UserDialogs.Instance.HideLoading();
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

        private ActivityIndicator IndicatorRunning()
        {
            var loadingIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 2,
                Color = Color.Silver
            };
            loadingIndicator.SetBinding(IsVisibleProperty, "IsLoading");
            loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            loadingIndicator.IsRunning = true;
            Thread.Sleep(2000);
            loadingIndicator.IsRunning = false;
            return loadingIndicator;
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