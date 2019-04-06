using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TESTPage : ContentPage
    {
        private bool _pbIndicator;
        public bool PBIndicator
        {
            get { return _pbIndicator; }
            set
            {
                _pbIndicator = value;
                OnPropertyChanged();
            }
        }

        public TESTPage()
        {
            InitializeComponent();

            var parentLayout = new AbsoluteLayout();

            var stackContent = new StackLayout();
            AbsoluteLayout.SetLayoutFlags(stackContent, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(stackContent, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.Black
            };

            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding(nameof(PBIndicator)));
            activityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding(nameof(PBIndicator)));
            activityIndicator.BindingContext = this;

            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            var button = new Button
            {
                Text = "Click",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            button.Clicked += OnClicked;

            stackContent.Children.Add(button);

            parentLayout.Children.Add(stackContent);
            parentLayout.Children.Add(activityIndicator);

            Content = parentLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PBIndicator = true;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            PBIndicator = !PBIndicator;
        }

        //protected ActivityIndicator CreateLoadingIndicator()
        //{
        //    var loadingIndicator = new ActivityIndicator
        //    {
        //        HorizontalOptions = LayoutOptions.CenterAndExpand,
        //        VerticalOptions = LayoutOptions.Start,
        //        Scale = 2,
        //        Color = Color.Silver
        //    };
        //    loadingIndicator.SetBinding(IsVisibleProperty, "IsLoading");
        //    loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
        //    return loadingIndicator;
        //}
    }
}