using Bakalarka.role;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace Bakalarka.logika
{
    static class Sklad
    {


        /*
         * nacteni skladu
         */
        public static String NacteniSkladu()
        {
            AktualizaceSkladu();

            //zobrazeni skladiste hezky do gridu
            VytvoreniGridu();
            //nacitani produktu
            NacteniProduktu();


            return null;
        }
        /*
         * vytvoreni gridu pro skladiste
         */
        public static void VytvoreniGridu()
        {
            for (int i = 0; i < Hra.produkty.Count() + 6; i++)
            {
                Hra.skladiste.RowDefinitions.Add(new RowDefinition());
            }
            Hra.skladiste.ColumnDefinitions.Add(new ColumnDefinition());
            Hra.skladiste.ColumnDefinitions.Add(new ColumnDefinition());
        }
        /*
         * nacte produkty do mrizky
         */
        public static String NacteniProduktu()
        {
            int radek = 0;
            var uskladnit = new Button { Text = "Uložit do skladu", BackgroundColor = Color.RoyalBlue, TextColor = Color.DarkGray, FontSize = 20, CornerRadius = 4, BorderColor = Color.DarkGray, BorderWidth = 2 };
            //ulozeni do skladu
            uskladnit.Clicked += async (sender, args) =>
            {
                Hra.skladiste.Children.Clear();
                if (Hrac.role == 3)// zobrazeni podle roli
                {
                    var nadpis = new Label() { Text = "Vložení produktu do domečku", TextColor = Color.Black,VerticalOptions=LayoutOptions.Center, HorizontalOptions=LayoutOptions.Center,FontSize=15 };
                    Grid.SetColumnSpan(nadpis, 3);
                    Grid.SetRow(nadpis, 1);
                    Grid.SetColumn(nadpis, 0);
                    Hra.skladiste.Children.Add(nadpis);
                    var vstup = new Entry() { Keyboard=Keyboard.Numeric };
                    vstup.TextChanged += async (s, a) =>
                    {
                        Validace.PouzeInt((Entry)s, a);
                    };
                    var popis = new Label { Text = "Kód:", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                    Hra.skladiste.Children.Add(popis, 0, 2);
                    Hra.skladiste.Children.Add(vstup, 1, 2);
                    var ulozit = new Button { Text = "Uložit do skladu", BackgroundColor = Color.RoyalBlue, TextColor = Color.DarkGray, FontSize = 20, CornerRadius = 4, BorderColor = Color.DarkGray, BorderWidth = 2 };
                    ulozit.Clicked += async (sender1, args1) => // ulozeni button
                    {
                        String prubeh = UlozeniDoSkladu(vstup.Text);
                        if (prubeh == null)//kdyz probehlo ok
                        {
                            Hra.skladiste.Children.Clear();
                            NacteniProduktu();
                        }
                        else //kdyz neprobehl ok
                        {
                            await Application.Current.MainPage.DisplayAlert("Chyba", "Zadaný kód je neplatný. Zkus to znova.", "Zavřít");
                        }
                    };
                    Hra.skladiste.Children.Add(ulozit, 1, 3);
                    var zpet = new Button { Text = "Zpět", BackgroundColor = Color.DarkGray, TextColor = Color.RoyalBlue, FontSize = 15, CornerRadius = 4, BorderColor = Color.RoyalBlue, BorderWidth = 2 };
                    Hra.skladiste.Children.Add(zpet, 1, 4);
                    zpet.Clicked += async (sender1, args2) =>
                    {
                        Hra.skladiste.Children.Clear();
                        NacteniProduktu();
                    };
                }
                else// pokud neni domecek
                {
                    var kod = new Label { Text ="Kód pro uložení: " +VygenerovaniKodu() };
                    var zpet = new Button { Text = "Zpět", BackgroundColor = Color.DarkGray, TextColor = Color.RoyalBlue, FontSize = 15, CornerRadius = 4, BorderColor = Color.RoyalBlue, BorderWidth = 2 };
                    Hra.skladiste.Children.Add(kod, 0, 1);
                    Hra.skladiste.Children.Add(zpet, 1, 2);
                    zpet.Clicked += async (sender1, args1) =>
                    {
                        Hrac.AktualizaceZivotInventar();
                        Hra.skladiste.Children.Clear();
                        NacteniProduktu();
                    };
                }

            };
            Grid.SetColumnSpan(uskladnit, 3);
            Grid.SetRow(uskladnit, radek);
            Grid.SetColumn(uskladnit, 0);
            Hra.skladiste.Children.Add(uskladnit);
            radek++;
            for (int i = 1; i <= 4; i++)//dokolecka dokola je to pekna prasarna
            {
                var uroven = new Label { Text = "Úroveň " + i.ToString(), TextColor=Color.Black,FontSize=15,HorizontalOptions=LayoutOptions.CenterAndExpand,VerticalOptions=LayoutOptions.CenterAndExpand,};
                Grid.SetColumnSpan(uroven, 3);
                Grid.SetRow(uroven, radek);
                Grid.SetColumn(uroven, 0);
                Hra.skladiste.Children.Add(uroven);
                radek++;
                foreach (var produkt in Hra.produkty)
                {
                    if (produkt.uroven == i)
                    {
                        var nazev = new Label { Text = produkt.nazev + ":" };
                        var pocet = new Label { Text = produkt.ulozene.ToString() };
                        var recept = new Button { Text = "Recept", BackgroundColor = Color.RoyalBlue, TextColor = Color.DarkGray, CornerRadius = 4, BorderColor = Color.DarkGray, BorderWidth = 2 };
                        //vytvoreni stranky na smenu podle receptu
                        recept.Clicked += async (sender, args) =>
                        {
                            
                            var receptik = new Recept(produkt.id);
                            if (receptik.vysledek == 0)//pokud nema recept
                            {
                                await Application.Current.MainPage.DisplayAlert("Info", "Pro ten to produkt neexistuje recept! je to základní produkt!", "Zavřít");
                            }
                            else // pokud ma recept
                            {
                                Hra.skladiste.Children.Clear();
                                var popis = new Label() { TextColor = Color.Black, Text = "Recept na " + receptik.popis, HorizontalOptions=LayoutOptions.CenterAndExpand, VerticalOptions=LayoutOptions.CenterAndExpand,FontSize=15 };
                                var prisada1 = new Label { Text = receptik.prisada1 + " " + receptik.mnozstvi1.ToString() + "x" };
                                var prisada2 = new Label { Text = receptik.prisada2 + " " + receptik.mnozstvi2.ToString() + "x" };
                                Grid.SetColumnSpan(popis, 3);
                                Grid.SetRow(popis, 0);
                                Grid.SetColumn(popis, 0);
                                Hra.skladiste.Children.Add(popis);
                                Grid.SetColumnSpan(prisada1, 3);
                                Grid.SetRow(prisada1, 1);
                                Grid.SetColumn(prisada1, 0);
                                Hra.skladiste.Children.Add(prisada1);
                                Grid.SetColumnSpan(prisada2, 3);
                                Grid.SetRow(prisada2, 2);
                                Grid.SetColumn(prisada2, 0);
                                Hra.skladiste.Children.Add(prisada2);
                                if (receptik.prisada3 != null)
                                {
                                    var prisada3 = new Label { Text = receptik.prisada3 + " " + receptik.mnozstvi3 + "x" };
                                    Grid.SetColumnSpan(prisada3, 3);
                                    Grid.SetRow(prisada3, 3);
                                    Grid.SetColumn(prisada3, 0);
                                    Hra.skladiste.Children.Add(prisada3);
                                }
                                if (Hrac.role == 3)// domecek muze smeniit
                                {
                                    var smenit = new Button { Text = "Směnit", BackgroundColor = Color.RoyalBlue, TextColor = Color.DarkGray, FontSize = 20, CornerRadius = 4, BorderColor = Color.DarkGray, BorderWidth = 2 };
                                    Hra.skladiste.Children.Add(smenit, 1, 4);
                                    smenit.Clicked += async (sender1, args2) =>
                                    {
                                        String prubeh = receptik.Smena();
                                        if (prubeh == null)
                                        {
                                            Hra.skladiste.Children.Clear();
                                            NacteniProduktu();
                                        }
                                        else
                                        {
                                            await Application.Current.MainPage.DisplayAlert("Chyba", "Tuto směnu nemůžeš provést!", "Zavřít");
                                        }
                                    };
                                }
                                var zpet = new Button { Text = "Zpět", BackgroundColor = Color.DarkGray, TextColor = Color.RoyalBlue, FontSize = 15, CornerRadius = 4, BorderColor = Color.RoyalBlue, BorderWidth = 2 };
                                Hra.skladiste.Children.Add(zpet, 1, 5);
                                
                                zpet.Clicked += async (sender1, args2) =>
                                {
                                    Hra.skladiste.Children.Clear();
                                    NacteniProduktu();
                                };

                            }

                           

                        };
                        Hra.skladiste.Children.Add(nazev, 0, radek);
                        Hra.skladiste.Children.Add(pocet, 1, radek);
                        Hra.skladiste.Children.Add(recept, 2, radek);
                        radek++;
                    }
                }
            }


            return null;
        }

        /*
         * aktualizace stavu skladu
         */
        public static String AktualizaceSkladu()
        {
            MySqlCommand prikaz = new MySqlCommand("Select count(*) as pocet from bakalarka.sklad where idtym=@idtym and idprodukt=@idprodukt");
            prikaz.Parameters.AddWithValue("@idtym", Hrac.tym);
            var idpro = new MySqlParameter("@idprodukt", MySqlDbType.Int32);
            prikaz.Parameters.Add(idpro);
            foreach (var produkt in Hra.produkty)
            {
                idpro.Value = produkt.id;
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                while (data.Read())
                {
                    if (Convert.IsDBNull(data["pocet"]))
                    {
                        return "neco neni v poho pri nacitani skladu";
                    }
                    produkt.ulozene = Convert.ToInt32(data["pocet"]);

                }
                data.Close();// netusim proc to jenom tady musi byt :O 
            }
            return null;
        }
        /*
         * ulozeni produktu od hrace do skladu pro domecek 
         */
        public static String UlozeniDoSkladu(String kod)
        {
            MySqlCommand prikazuskladneni = new MySqlCommand("Select uskladani from bakalarka.tym where idtym=@idtym;");
            prikazuskladneni.Parameters.AddWithValue("@idtym", Hrac.tym);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazuskladneni);
            String[] parser = new String[2];
            while (data.Read())
            {
                try
                {
                    parser = kod.Split(new string[] { Convert.ToString((int)data["uskladani"]) }, StringSplitOptions.None);
                }
                catch(Exception ex)
                {
                    return "neplatny kod";
                }
            }
            if (parser.Length<2)
            {
                return "neplatny kod";
            }
            MySqlCommand prikazTest = new MySqlCommand("Select inventar from bakalarka.uzivatel where iduzivatel=@id;");
            prikazTest.Parameters.AddWithValue("@id", parser[1]);
            MySqlDataReader dataTest = DBConnector.ProvedeniPrikazuSelect(prikazTest);
            int inv = 0;
            while (dataTest.Read())
            {
                inv = (int)dataTest["inventar"];
            }
            if (inv == Convert.ToInt32(parser[0]))
            {
                MySqlCommand prikazFinal = new MySqlCommand("Update bakalarka.uzivatel set inventar=null, pocetUlozeni=pocetUlozeni+1,zivot=1 where iduzivatel=@iduzivatel;" +
                    "Update bakalarka.tym set uskladani=@uskladani where idtym=@idtym;" +
                    "INSERT INTO `bakalarka`.`sklad` (`idprodukt`, `idtym`) VALUES (@idprodukt, @idtym);");
                prikazFinal.Parameters.AddWithValue("@iduzivatel", parser[1]);
                prikazFinal.Parameters.AddWithValue("@idtym", Hrac.tym);
                prikazFinal.Parameters.AddWithValue("@idprodukt", Convert.ToInt32(parser[0]));
                prikazFinal.Parameters.AddWithValue("@uskladani", new Random().Next(100000, 999999));
                DBConnector.ProvedeniPrikazuOstatni(prikazFinal);
                Hra.produkty.Find(item => item.id == Convert.ToInt32(parser[0])).ulozene++;
                Hrac.inventar = 0;
                Hrac.inventarNazev = "";
                return null;
            }
            else
            {
                return "zadany kod je spatny";
            }
            return null;
        }
        /*
         * vygenerovani kodu pro hrace, ktery predaji domeck
         */
        public static String VygenerovaniKodu()
        {
            if (Hrac.inventar == 0)
            {
                return "Inventar je prazdny";
            }
            else
            {
                MySqlCommand prikaz = new MySqlCommand("Select uskladani from bakalarka.tym where idtym=@idtym");
                prikaz.Parameters.AddWithValue("@idtym", Hrac.tym);
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                data.Read();
                return Hrac.inventar.ToString() + Convert.ToString((int)data["uskladani"]) + Hrac.iduzivatel.ToString();
            }

        }


    }
}
