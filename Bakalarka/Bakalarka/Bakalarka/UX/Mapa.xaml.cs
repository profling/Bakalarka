using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace Bakalarka.UX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        public Mapa()
        {
            InitializeComponent();
            var grid = new Grid
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
            Map mapa = new Map();
            Grid.SetRow(mapa, 0);
            Grid.SetRowSpan(mapa, 4);
            Grid.SetColumn(mapa, 0);
            grid.Children.Add(mapa);
            Content = grid;

        }
    }
}