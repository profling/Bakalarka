using Bakalarka.logika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bakalarka.UX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Skladiste : ContentPage
    {
        public Skladiste()
        {
            InitializeComponent();
            Hra.skladiste = new Grid();
            Content = Hra.skladiste;

        }
    }
}