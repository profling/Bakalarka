using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Bakalarka.logika;

namespace Bakalarka.UX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hierarchie : ContentPage
    {
        public Hierarchie()
        {
            InitializeComponent();
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            Label label1 = new Label();

           
            var prikaz= new MySqlCommand("select;");
            String test = DBConnector.ProvedeniPrikazuOstatni(prikaz);

            label1.Text = test;
            label1.IsVisible = true;
            layout.Children.Add(label1);
            this.Content = layout;
        }
    }
}