using Bakalarka.logika;
using Bakalarka.role;
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
    public partial class MujProfil : ContentPage
    {
        Grid mujprofil;
        public MujProfil()
        {
            InitializeComponent();
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            var id = new Entry { Keyboard = Keyboard.Numeric };
            var heslo = new Entry { IsPassword= true, Keyboard = Keyboard.Numeric };
            var prihlasit = new Button { Text = "Prihlasit se" };
            var label = new Label { Text = "" };
            layout.Children.Add(id);
            layout.Children.Add(heslo);
            layout.Children.Add(prihlasit);
            layout.Children.Add(label);

            prihlasit.Clicked += async (sender, args) => {
                if (Hrac.Prihlaseni(Convert.ToInt32(id.Text), heslo.Text) == null)
                {
                    mujprofil = new Grid()
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
                },
                        ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
                    };
                    Content = mujprofil;
                    if (Hrac.role == 0)
                    {
                        MujProfilVedouci();
                       
                    }
                    else
                    {
                        MujProfilHrac();
                        
                    }

                }
                else
                {
                    label.Text="Spatne prihlasovaci udaje";
                }
               
            };
            this.Content = layout;
        }
        /*
         * Stranka na zalozeni hry
         */
        public void ZalozitHru()
        {
          
            mujprofil.Children.Clear();
            
            var nazev = new Label() { Text = "název" };
            var entryNazev = new Entry();
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
                Text = "SZ roh:",
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
                Text = "SV roh:",
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
                Text = "JV roh:",
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
                Text = "JZ roh:",
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
                Text = "Založit hru",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var zpet = new Button() { Text="zpět" };
            


            //umisteni prvku
            {

                Grid.SetRow(nazev, 0);
                Grid.SetColumnSpan(nazev, 3);
                mujprofil.Children.Add(nazev);

                Grid.SetColumnSpan(entryNazev, 3);
                Grid.SetRow(entryNazev, 0);
                Grid.SetColumn(entryNazev, 3);
                mujprofil.Children.Add(entryNazev);

                Grid.SetRow(pocetTymu, 1);
                Grid.SetColumn(pocetTymu, 0);
                Grid.SetColumnSpan(pocetTymu, 3);
                mujprofil.Children.Add(pocetTymu);

                Grid.SetRow(entryPocetTymu, 1);
                Grid.SetColumn(entryPocetTymu, 3);
                Grid.SetColumnSpan(entryPocetTymu, 3);
                mujprofil.Children.Add(entryPocetTymu);

                Grid.SetRow(pocetClenu, 2);
                Grid.SetColumn(pocetClenu, 0);
                Grid.SetColumnSpan(pocetClenu, 3);
                mujprofil.Children.Add(pocetClenu);

                Grid.SetRow(entryPocetClenu, 2);
                Grid.SetColumn(entryPocetClenu, 3);
                Grid.SetColumnSpan(entryPocetClenu, 3);
                mujprofil.Children.Add(entryPocetClenu);

                Grid.SetRow(roh1, 3);
                Grid.SetColumn(roh1, 0);
                Grid.SetColumnSpan(roh1, 2);
                mujprofil.Children.Add(roh1);
                Grid.SetRow(roh1X, 3);
                Grid.SetColumn(roh1X, 2);
                Grid.SetColumnSpan(roh1X, 3);
                mujprofil.Children.Add(roh1X);
                Grid.SetRow(roh1Lomitko, 3);
                Grid.SetColumn(roh1Lomitko, 5);
                mujprofil.Children.Add(roh1Lomitko);
                Grid.SetRow(roh1Y, 3);
                Grid.SetColumn(roh1Y, 6);
                Grid.SetColumnSpan(roh1Y, 3);
                mujprofil.Children.Add(roh1Y);

                Grid.SetRow(roh2, 4);
                Grid.SetColumn(roh2, 0);
                Grid.SetColumnSpan(roh2, 2);
                mujprofil.Children.Add(roh2);
                Grid.SetRow(roh2X, 4);
                Grid.SetColumn(roh2X, 2);
                Grid.SetColumnSpan(roh2X, 3);
                mujprofil.Children.Add(roh2X);
                Grid.SetRow(roh2Lomitko, 4);
                Grid.SetColumn(roh2Lomitko, 5);
                mujprofil.Children.Add(roh2Lomitko);
                Grid.SetRow(roh2Y, 4);
                Grid.SetColumn(roh2Y, 6);
                Grid.SetColumnSpan(roh2Y, 3);
                mujprofil.Children.Add(roh2Y);

                Grid.SetRow(roh3, 5);
                Grid.SetColumn(roh3, 0);
                Grid.SetColumnSpan(roh3, 2);
                mujprofil.Children.Add(roh3);
                Grid.SetRow(roh3X, 5);
                Grid.SetColumn(roh3X, 2);
                Grid.SetColumnSpan(roh3X, 3);
                mujprofil.Children.Add(roh3X);
                Grid.SetRow(roh3Lomitko, 5);
                Grid.SetColumn(roh3Lomitko, 5);
                mujprofil.Children.Add(roh3Lomitko);
                Grid.SetRow(roh3Y, 5);
                Grid.SetColumn(roh3Y, 6);
                Grid.SetColumnSpan(roh3Y, 3);
                mujprofil.Children.Add(roh3Y);

                Grid.SetRow(roh4, 6);
                Grid.SetColumn(roh4, 0);
                Grid.SetColumnSpan(roh4, 2);
                mujprofil.Children.Add(roh4);
                Grid.SetRow(roh4X, 6);
                Grid.SetColumn(roh4X, 2);
                Grid.SetColumnSpan(roh4X, 3);
                mujprofil.Children.Add(roh4X);
                Grid.SetRow(roh4Lomitko, 6);
                Grid.SetColumn(roh4Lomitko, 5);
                mujprofil.Children.Add(roh4Lomitko);
                Grid.SetRow(roh4Y, 6);
                Grid.SetColumn(roh4Y, 6);
                Grid.SetColumnSpan(roh4Y, 3);
                mujprofil.Children.Add(roh4Y);

                Grid.SetRow(zalozit, 7);
                Grid.SetColumn(zalozit, 0);
                Grid.SetColumnSpan(zalozit, 9);
                mujprofil.Children.Add(zalozit);

                Grid.SetColumnSpan(zpet, 9);
                Grid.SetRow(zpet, 8);
                Grid.SetColumn(zpet, 0);
                mujprofil.Children.Add(zpet);

            }
            zalozit.Clicked += async (sender, args) =>
            {
                String prubeh =  Hra.novaHra(roh1X.Text,roh1Y.Text, roh2X.Text, roh2Y.Text, roh3X.Text, roh3Y.Text, roh4X.Text, roh4Y.Text, Hrac.iduzivatel, Convert.ToInt32(entryPocetTymu.Text), Convert.ToInt32(entryPocetClenu.Text), entryNazev.Text);
                if (prubeh != null)
                {
                    var layout = new StackLayout { Padding = new Thickness(5, 10) };
                    Label chyba = new Label { Text = prubeh };
                    layout.Children.Add(chyba);
                    Content = layout;
                }
                else
                {
                    MujProfilVedouci();
                }          
            
            };
            zpet.Clicked += async (sender, args) =>
             {
                 MujProfilVedouci();
             };
        }
        /*
         * Muj profil hrac 
         */
        public void MujProfilHrac()
        {

            mujprofil.Children.Clear();
            var inv = new Label() { Text="Invenář:" };
            Grid.SetColumnSpan(inv, 2);
            Grid.SetRow(inv, 1);
            Grid.SetColumn(inv, 0);
            mujprofil.Children.Add(inv);
            Grid.SetRow(Hra.invObsah, 1);
            Grid.SetColumn(Hra.invObsah, 3);
            mujprofil.Children.Add(Hra.invObsah);
            var ziv = new Label() { Text = "Stav života:" };
            Grid.SetColumnSpan(ziv, 2);
            Grid.SetColumn(ziv, 0);
            Grid.SetRow(ziv, 2);
            mujprofil.Children.Add(ziv);           
            Grid.SetRow(Hra.stavZivotu, 2);
            Grid.SetColumn(Hra.stavZivotu, 3);
            mujprofil.Children.Add(Hra.stavZivotu);
            Content = mujprofil;
            var ozivit = new Button() { Text="Oživit se" };
            Grid.SetColumnSpan(ozivit, 4);
            Grid.SetColumn(ozivit, 0);
            Grid.SetRow(ozivit, 3);
            mujprofil.Children.Add(ozivit);
            var statistiky = new Button() { Text= "Statistiky" };
            Grid.SetColumnSpan(statistiky, 4);
            Grid.SetColumn(statistiky, 0);
            Grid.SetRow(statistiky, 4);
            mujprofil.Children.Add(statistiky);
         

            ozivit.Clicked += async (sender, args) =>
             {
                 if (Hrac.zivot == 2)
                 {
                     OziveniVzhled();
                 }
                 
             };
            statistiky.Clicked += async (sender, args) =>
             {
                 Statistiky();
             };
           
        }

        /*
         * Muj profil vedouci
         */
        public void MujProfilVedouci()
        {
            mujprofil.Children.Clear();
            var zalozit = new Button() { Text = "Založit novou hru" };
            Grid.SetColumnSpan(zalozit, 3);
            Grid.SetRow(zalozit, 1);
            Grid.SetColumn(zalozit, 0);
            mujprofil.Children.Add(zalozit);
            zalozit.Clicked += async (sender, args) =>
             {
                 ZalozitHru();
             };
            var nacistHru = new Button() { Text = "Načíst hru" };
            Grid.SetColumnSpan(nacistHru, 3);
            Grid.SetRow(nacistHru, 2);
            Grid.SetColumn(nacistHru, 0);
            mujprofil.Children.Add(nacistHru);
            nacistHru.Clicked += async (sender, args) =>
            {
                NacistHruVzhled();
            };
            var hraci = new Button() { Text = "Výpis hráču" };
            Grid.SetColumnSpan(hraci, 3);
            Grid.SetRow(hraci, 3);
            Grid.SetColumn(hraci, 0);
            mujprofil.Children.Add(hraci);
            hraci.Clicked+= async (sender, args)=>{
                Hraci();
            };
            Content = mujprofil;
        }

        /*
         * Stranka s ozivenim vzhled v mem profilu
         */
         void OziveniVzhled()
        {
            mujprofil.Children.Clear();
            var kodlbl = new Label() { Text = "kód:" };
            Grid.SetRow(kodlbl, 1);
            Grid.SetColumn(kodlbl, 0);
            mujprofil.Children.Add(kodlbl);
            var kod = new Entry() { Keyboard = Keyboard.Numeric };
            Grid.SetColumnSpan(kod, 2);
            Grid.SetRow(kod, 1);
            Grid.SetColumn(kod, 1);
            mujprofil.Children.Add(kod);
            var ozivit = new Button() { Text = "Oživit" };
            Grid.SetColumnSpan(ozivit, 3);
            Grid.SetRow(ozivit, 2);
            Grid.SetColumn(ozivit, 0);
            mujprofil.Children.Add(ozivit);
            ozivit.Clicked += async (sender, args) =>
             {
                 String prubeh=Hrac.Oziveni(Convert.ToInt32(kod.Text));
                 if (prubeh == null)
                 {
                     MujProfilHrac();
                 }
                 else
                 {
                     var lbl = new Label() { Text = prubeh };
                     Grid.SetColumnSpan(lbl, 3);
                     Grid.SetRow(lbl, 4);
                     Grid.SetColumn(lbl, 0);
                     mujprofil.Children.Add(lbl);
                 }

             };
            var zpet = new Button() { Text = "zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 3);
            Grid.SetColumn(zpet, 0);
            mujprofil.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
             {
                 MujProfilHrac();
             };
        }

        /*
         * Statistiky Vypsani a zobrazeni
         */
         void Statistiky()
        {
            mujprofil.Children.Clear();
            MySqlCommand prikazVyher = new MySqlCommand("Select count(*) as pocet from bakalarka.souboj where vyherce=@idhrac;");
            prikazVyher.Parameters.AddWithValue("@idhrac", Hrac.iduzivatel);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazVyher);
            data.Read();
            var lblvyher = new Label() { Text = "Počet vyhraných soubojů = " + Convert.ToString(data["pocet"]) };
            Grid.SetColumnSpan(lblvyher, 3);
            Grid.SetRow(lblvyher, 1);
            Grid.SetColumn(lblvyher, 0);
            mujprofil.Children.Add(lblvyher);
            data.Close();
            MySqlCommand prikazProduktu = new MySqlCommand("Select pocetUlozeni from bakalarka.uzivatel where iduzivatel=@iduzivatel;");
            prikazProduktu.Parameters.AddWithValue("@iduzivatel",Hrac.iduzivatel);
            data = DBConnector.ProvedeniPrikazuSelect(prikazProduktu);
            data.Read();
            var lblproduktu = new Label() { Text = "Počet uložených produktů do skladu =" + Convert.ToString(data["pocetUlozeni"]) };
            Grid.SetColumnSpan(lblproduktu, 3);
            Grid.SetRow(lblproduktu, 2);
            Grid.SetColumn(lblproduktu, 0);
            mujprofil.Children.Add(lblproduktu);
            var zpet = new Button() { Text = "zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 3);
            Grid.SetColumn(zpet, 0);
            mujprofil.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
            {
                MujProfilHrac();
            };

        }

        /*
         * Nacist hru pro vedouciho
         */
         void NacistHruVzhled()
        {
            mujprofil.Children.Clear();
            MySqlCommand prikaz = new MySqlCommand("Select nazev from bakalarka.hra;");
            prikaz.Parameters.AddWithValue("@id", Hrac.iduzivatel);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            Picker seznamBox = new Picker() { Title="Název",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            while (data.Read())
            {
                seznamBox.Items.Add(data["nazev"].ToString());
            }
            
            
            Grid.SetColumnSpan(seznamBox, 2);
            Grid.SetRow(seznamBox, 1);
            Grid.SetColumn(seznamBox, 1);
            mujprofil.Children.Add(seznamBox);
            var nazevlbl = new Label { Text = "název:" };
            mujprofil.Children.Add(nazevlbl, 0, 1);
            var nacist = new Button() { Text = "načíst hru" };
            Grid.SetColumnSpan(nacist, 3);
            Grid.SetRow(nacist, 2);
            Grid.SetColumn(nacist, 0);
            mujprofil.Children.Add(nacist);
            nacist.Clicked+= async (sender, args)=>{
                Hra.idHry(seznamBox.Items[seznamBox.SelectedIndex]);
                Hra.nacteniHry(Hra.idhry);
                MujProfilVedouci();
            };
            var zpet = new Button() { Text = "zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 3);
            Grid.SetColumn(zpet, 0);
            mujprofil.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
            {
                MujProfilVedouci();
            };

        }

        /*
         * Vypis hracu ve hre a jejich pinu
         */
        void Hraci()
        {
            mujprofil.Children.Clear();
            MySqlCommand prikaz = new MySqlCommand("Select bakalarka.uzivatel.iduzivatel as id, bakalarka.uzivatel.heslo as pin, bakalarka.uzivatel.tym as tym, bakalarka.uzivatel.role as role from bakalarka.uzivatel inner join bakalarka.tym on bakalarka.uzivatel.tym=bakalarka.tym.idtym where bakalarka.tym.hra=@idhra;");
            prikaz.Parameters.AddWithValue("@idhra", Hra.idhry);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            int radek = 1;
            var id = new Label() { Text = "ID uživatele" };
            mujprofil.Children.Add(id, 0, 0);
            var pin = new Label() { Text = "PIN" };
            mujprofil.Children.Add(pin,1,0);
            var role = new Label() { Text = "Role" };
            mujprofil.Children.Add(role, 2, 0);
            var tym = new Label() { Text = "Tým" };
            mujprofil.Children.Add(tym, 3, 0);
            while (data.Read())
            {
                var idh = new Label() { Text = Convert.ToString(data["id"]) };
                mujprofil.Children.Add(idh, 0, radek);
                var pinh = new Label() { Text = Convert.ToString(data["pin"]) };
                mujprofil.Children.Add(pinh, 1, radek);
                var roleh = new Label() ;
                if ((int)data["role"]==1)
                {
                    roleh.Text = "Těžař";
                }
                if ((int)data["role"] == 2)
                {
                    roleh.Text = "Lovec";
                }
                if ((int)data["role"] == 3)
                {
                    roleh.Text = "Domeček";
                }
                mujprofil.Children.Add(roleh, 2, radek);
                var tymh = new Label() { Text =Convert.ToString(data["tym"])};
                mujprofil.Children.Add(tymh, 3, radek);
                radek++;
            }
            var zpet = new Button() { Text = "zpět" };
            Grid.SetColumnSpan(zpet, 4);
            Grid.SetRow(zpet, radek);
            Grid.SetColumn(zpet, 0);
            mujprofil.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
            {
                MujProfilVedouci();
            };
            ScrollView scroll = new ScrollView();
            scroll.Content = mujprofil;
            Content = scroll;
        }
    }
    
  
}