using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using Bakalarka.logika;
using Bakalarka.role;

namespace Bakalarka.UX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        public Label produktlbl;
        Button sebrat;
         Grid grid;
        public Mapa()
        {
            InitializeComponent();
             grid = new Grid
            {
                RowDefinitions={
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                },
            };
            Hra.mapa = new Xamarin.Forms.Maps.Map
            {
                IsShowingUser=true,
            };           
            Grid.SetRow(Hra.mapa, 0);
            Grid.SetRowSpan(Hra.mapa, 4);
            Grid.SetColumn(Hra.mapa, 0);
            grid.Children.Add(Hra.mapa);

  
            Hra.produktPopis = new Label { Text="blup"};
            sebrat = new Button { Text="sebrat" };
            Grid.SetRow(Hra.produktPopis, 4);
            Grid.SetRow(sebrat, 5);
            grid.Children.Add(Hra.produktPopis);
            grid.Children.Add(sebrat);
            Task.Delay(2000);
            MapaKontroler.PoziceHrace();

           

            sebrat.Clicked += async (sender, arg) =>
             {
                 if (Hrac.prihlaseny)
                 {
                    Hra.produktPopis.Text= Hrac.VlozeniDoInventare(Hra.vybranyProdukt);

                 }
                     
             };

            Content = grid;

        }
        
    }
}