using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems.Views.TYPET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopUpPage : ContentPage
	{
		public PopUpPage ()
		{
			InitializeComponent ();
		}

        private void SubmitButton_OnClicked(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}