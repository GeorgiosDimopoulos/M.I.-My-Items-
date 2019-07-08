using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views.TYPET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopUpView //ContentView
    {
        public static string Result;
        public static int KindOfPage; //1 for Kids,2 for Stuff , 3 for duties

		public PopUpView ()
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

        private async void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                Result = NewItemEntry.Text;
                if (KindOfPage == 1)
                {
                    KidsPage.PageTitle = NewItemEntry.Text;
                    await Navigation.PushAsync(new KidsPage(), true);
                    await Navigation.PopAsync();
                }
                else if (KindOfPage == 2)
                {
                    StaffPage.PageTitle = NewItemEntry.Text;
                    await Navigation.PushAsync(new StaffPage(), true);
                    await Navigation.PopAsync();
                }
                else if (KindOfPage == 3)
                {
                    TodoPage.PageTitle = NewItemEntry.Text;
                    await Navigation.PushAsync(new TodoPage(), true);
                    await Navigation.PopAsync();
                }
                UserDialogs.Instance.HideLoading();
                //IsVisible = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}