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
        public static String nacteniSkladu()
        {
            aktualizaceSkladu();

            //zobrazeni skladiste hezky do gridu
            vytvoreniGridu();
            //nacitani produktu
            nacteniProduktu();
            

            return null;
        }
        /*
         * vytvoreni gridu pro skladiste
         */
        public static void vytvoreniGridu()
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
        public static String nacteniProduktu()
        {
            int radek = 0;

            for (int i = 1; i <= 4; i++)//dokolecka dokola je to pekna prasarna
            {
                var uroven = new Label { Text = "Úroveň " + i.ToString() };
                Hra.skladiste.Children.Add(uroven, 1, radek);
                radek++;
                foreach (var produkt in Hra.produkty)
                {
                    if (produkt.uroven == i)
                    {
                        var nazev = new Label { Text = produkt.nazev + ":" };
                        var pocet = new Label { Text = produkt.ulozene.ToString() };
                        var recept = new Button { Text = "Recept" };
                        //vytvoreni stranky na smenu podle receptu
                        recept.Clicked += async (sender, args) => {
                            Hra.skladiste.Children.Clear();
                            var receptik = new Recept(produkt.id);
                           if (receptik.vysledek == 0)//pokud nema recept
                            {
                                var napis = new Label { Text="Dany recept je zakladni surovina, takze nejde z niceho slozit." };
                                Grid.SetColumnSpan(napis, 3);
                                Grid.SetRow(napis,0);
                                Grid.SetColumn(napis,0);
                                Hra.skladiste.Children.Add(napis);
                            }
                            else // pokud ma recept
                            {
                                var prisada1 = new Label { Text = receptik.prisada1 + " " + receptik.mnozstvi1.ToString() + "x" };
                                var prisada2 = new Label { Text = receptik.prisada2 + " " + receptik.mnozstvi2.ToString() + "x" };
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
                                var smenit = new Button { Text = "Směnit" };
                                Hra.skladiste.Children.Add(smenit, 1, 4);
                                smenit.Clicked += async (sender1, args2) =>
                                {
                                    //zavolat smenu
                                };
                            }
                           
                            var zpet = new Button { Text = "Zpet" };
                            Hra.skladiste.Children.Add(zpet, 1, 5);
                            zpet.Clicked += async (sender1, args2) =>
                                {
                                Hra.skladiste.Children.Clear();
                                nacteniProduktu();
                                };

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
        public static String aktualizaceSkladu()
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
    }
}
