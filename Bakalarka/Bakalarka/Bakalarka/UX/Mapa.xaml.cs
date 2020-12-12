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

  
            Hra.produktPopis = new Label {FontSize=25,
                TextColor=Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            sebrat = new Button { Text="Sebrat", BackgroundColor = Color.RoyalBlue, TextColor = Color.DarkGray, FontSize = 20, CornerRadius = 4, BorderColor = Color.DarkGray, BorderWidth = 2 };
            Grid.SetRow(Hra.produktPopis, 4);
            Grid.SetRow(sebrat, 5);
            grid.Children.Add(Hra.produktPopis);
            grid.Children.Add(sebrat);
            
            MapaKontroler.PoziceHrace();

           

            sebrat.Clicked += async (sender, arg) =>
             {
                 if (Hrac.prihlaseny)
                 {
                    String prubeh= Hrac.VlozeniDoInventare(Hra.vybranyProdukt);
                     if (prubeh != null)
                     {
                         await DisplayAlert("Info", "Při sebrání se něco nepovedlo. Nejspíše už máš plný inventář.", "Zavřít");
                     }

                 }
                 else
                 {
                     await DisplayAlert("Chyba", "Musíš se nejdřív přihlásit nebo načíst hru!", "Zavřít");
                 }
                     
             };

            Content = grid;

        }
        
    }
}