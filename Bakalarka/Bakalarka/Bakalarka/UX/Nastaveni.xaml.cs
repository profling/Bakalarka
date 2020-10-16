using Bakalarka.role;
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
    public partial class Nastaveni : ContentPage
    {
        public Nastaveni()
        {
            InitializeComponent();
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            var id = new Entry { Keyboard = Keyboard.Numeric };
            var heslo = new Entry { IsPassword= true };
            var prihlasit = new Button { Text = "Prihlasit se" };
            var label = new Label { Text = "Stav" };
            layout.Children.Add(id);
            layout.Children.Add(heslo);
            layout.Children.Add(prihlasit);
            layout.Children.Add(label);

            prihlasit.Clicked += async (sender, args) => {
                label.Text = Hrac.Prihlaseni(Convert.ToInt32(id.Text), heslo.Text);
                PrihlaseniVedouciho();
            };
            this.Content = layout;
        }

        public void PrihlaseniVedouciho()
        {
            Grid vedouci = new Grid
            {
                RowDefinitions =
                {

                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                },
                ColumnDefinitions =
               {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),


        }
            }; //definice gridu
            var novaHra = new Label
            {
                Text = "Nova hra",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var pocetTymu = new Label
            {
                Text = "Pocet tymu:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            var entryPocetTymu = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var pocetClenu = new Label
            {
                Text = "Pocet clenu:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var entryPocetClenu = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,

            };
            var roh1 = new Label
            {
                Text = "Roh 1:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh1X = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh1Y = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh1Lomitko = new Label
            {
                Text = "/",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh2 = new Label
            {
                Text = "Roh 2:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh2X = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh2Y = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh2Lomitko = new Label
            {
                Text = "/",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh3 = new Label
            {
                Text = "Roh 3:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh3X = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh3Y = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh3Lomitko = new Label
            {
                Text = "/",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh4 = new Label
            {
                Text = "Roh 4:",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var roh4X = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh4Y = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10,
            };
            var roh4Lomitko = new Label
            {
                Text = "/",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var zalozit = new Button
            {
                Text = "Zalozit hru",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //umisteni prvku
            Grid.SetRow(novaHra, 0);
            Grid.SetColumnSpan(novaHra, 9);
            vedouci.Children.Add(novaHra);
            
            Grid.SetRow(pocetTymu, 1);
            Grid.SetColumn(pocetTymu, 0);
            Grid.SetColumnSpan(pocetTymu, 3);
            vedouci.Children.Add(pocetTymu);
            
            Grid.SetRow(entryPocetTymu, 1);
            Grid.SetColumn(entryPocetTymu, 3);
            Grid.SetColumnSpan(entryPocetTymu, 3);
            vedouci.Children.Add(entryPocetTymu);
            
            Grid.SetRow(pocetClenu, 2);
            Grid.SetColumn(pocetClenu, 0);
            Grid.SetColumnSpan(pocetClenu, 3);
            vedouci.Children.Add(pocetClenu);
            
            Grid.SetRow(entryPocetClenu, 2);
            Grid.SetColumn(entryPocetClenu, 3);
            Grid.SetColumnSpan(entryPocetClenu, 3);
            vedouci.Children.Add(entryPocetClenu);

            Grid.SetRow(roh1, 3);
            Grid.SetColumn(roh1, 0);
            Grid.SetColumnSpan(roh1, 2);
            vedouci.Children.Add(roh1);
            Grid.SetRow(roh1X, 3);
            Grid.SetColumn(roh1X, 2);
            Grid.SetColumnSpan(roh1X, 3);
            vedouci.Children.Add(roh1X);
            Grid.SetRow(roh1Lomitko, 3);
            Grid.SetColumn(roh1Lomitko, 5);
            vedouci.Children.Add(roh1Lomitko);
            Grid.SetRow(roh1Y, 3);
            Grid.SetColumn(roh1Y, 6);
            Grid.SetColumnSpan(roh1Y, 3);
            vedouci.Children.Add(roh1Y);

            Grid.SetRow(roh2, 4);
            Grid.SetColumn(roh2, 0);
            Grid.SetColumnSpan(roh2, 2);
            vedouci.Children.Add(roh2);
            Grid.SetRow(roh2X, 4);
            Grid.SetColumn(roh2X, 2);
            Grid.SetColumnSpan(roh2X, 3);
            vedouci.Children.Add(roh2X);
            Grid.SetRow(roh2Lomitko, 4);
            Grid.SetColumn(roh2Lomitko, 5);
            vedouci.Children.Add(roh2Lomitko);
            Grid.SetRow(roh2Y, 4);
            Grid.SetColumn(roh2Y, 6);
            Grid.SetColumnSpan(roh2Y, 3);
            vedouci.Children.Add(roh2Y);

            Grid.SetRow(roh3, 5);
            Grid.SetColumn(roh3, 0);
            Grid.SetColumnSpan(roh3, 2);
            vedouci.Children.Add(roh3);
            Grid.SetRow(roh3X, 5);
            Grid.SetColumn(roh3X, 2);
            Grid.SetColumnSpan(roh3X, 3);
            vedouci.Children.Add(roh3X);
            Grid.SetRow(roh3Lomitko, 5);
            Grid.SetColumn(roh3Lomitko, 5);
            vedouci.Children.Add(roh3Lomitko);
            Grid.SetRow(roh3Y, 5);
            Grid.SetColumn(roh3Y, 6);
            Grid.SetColumnSpan(roh3Y, 3);
            vedouci.Children.Add(roh3Y);

            Grid.SetRow(roh4, 6);
            Grid.SetColumn(roh4, 0);
            Grid.SetColumnSpan(roh4, 2);
            vedouci.Children.Add(roh4);
            Grid.SetRow(roh4X, 6);
            Grid.SetColumn(roh4X, 2);
            Grid.SetColumnSpan(roh4X, 3);
            vedouci.Children.Add(roh4X);
            Grid.SetRow(roh4Lomitko, 6);
            Grid.SetColumn(roh4Lomitko, 5);
            vedouci.Children.Add(roh4Lomitko);
            Grid.SetRow(roh4Y, 6);
            Grid.SetColumn(roh4Y, 6);
            Grid.SetColumnSpan(roh4Y, 3);
            vedouci.Children.Add(roh4Y);

            Grid.SetRow(zalozit, 7);
            Grid.SetColumn(zalozit, 0);
            Grid.SetColumnSpan(zalozit, 9);
            vedouci.Children.Add(zalozit);
            Content = vedouci;


        }
    }
}