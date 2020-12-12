using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bakalarka
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage() {BarBackgroundColor=Color.RoyalBlue,SelectedTabColor=Color.SkyBlue, };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
