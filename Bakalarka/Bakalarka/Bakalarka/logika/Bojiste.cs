using Bakalarka.role;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Bakalarka.logika
{
     static class Bojiste
    {
       static int boju = 0;
        /*
         * Metoda ktera vytvori grid s tlacitkama pro uvod souboje
         */
        public static String BojisteUvod()
        {
            Hra.bojiste.Children.Clear();
            if (Hrac.role == 1)
            {
                Button novySouboj = new Button {
                Text="Nový souboj"};
                Grid.SetColumnSpan(novySouboj, 3);
                Grid.SetColumn(novySouboj, 0);
                Grid.SetRow(novySouboj, 1);
                Hra.bojiste.Children.Add(novySouboj);
                novySouboj.Clicked += async (sender, args) =>
                 {
                     VyvolatSouboj();
                 };
            }
            Button prijmoutSouboj = new Button
            {
                Text = "Přijmout souboj"
            };
            Grid.SetColumnSpan(prijmoutSouboj, 3);
            Grid.SetColumn(prijmoutSouboj, 0);
            Grid.SetRow(prijmoutSouboj, 2);
            Hra.bojiste.Children.Add(prijmoutSouboj);
            prijmoutSouboj.Clicked += async (sender, args) =>
            {
                PrijmoutSouboj();
            };
            return null;
        } 
        
        /*
         * Metado pomoci ktere se vyvola souboj
         */
        static String VyvolatSouboj()
        {
            Hra.bojiste.Children.Clear();
            int idSouboje = ((Hrac.iduzivatele * 10) + (boju++)) * 100000 + new Random().Next(100, 999);
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO `bakalarka`.`souboj` (`uzivatel1`, `idsouboj`,`idtyp_souboje`) VALUES (@idhrac, @idsouboj, @idtyp);");
            prikaz.Parameters.AddWithValue("@idhrac", Hrac.iduzivatele);
            prikaz.Parameters.AddWithValue("@idsouboj", idSouboje );//vic jak tisic jich nezvladne
            prikaz.Parameters.AddWithValue("@idtyp", new Random().Next(1, 3));
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikaz);
            if (prubeh == null)
            {
                Label idHry = new Label { Text = "Souboj: " + idSouboje.ToString() };
                Grid.SetColumnSpan(idHry, 3);
                Grid.SetColumn(idHry, 0);
                Grid.SetRow(idHry, 1);
                Hra.bojiste.Children.Add(idHry);
                Button start = new Button { Text = "Start" };
                Grid.SetColumnSpan(start, 3);
                Grid.SetColumn(start, 0);
                Grid.SetRow(start, 2);
                Hra.bojiste.Children.Add(start);
                Button zpet = new Button { Text = "Zpět" };
                Grid.SetColumnSpan(zpet, 3);
                Grid.SetColumn(zpet, 0);
                Grid.SetRow(zpet, 3);
                Hra.bojiste.Children.Add(zpet);
                start.Clicked += async (sender, args) =>
                {
                    //TODO DO bojeeeeee
                };
                zpet.Clicked += async (sender, args) =>
                 {
                     BojisteUvod();
                 };

            }
            else
            {
                BojisteUvod();
                return "neco se se pokazilo pri vyvolani souboje";
            }
            return null;
        }
        /*
         * Metoda pomoci ktere prijmeme souboj
         */
        static String PrijmoutSouboj()
        {
            Hra.bojiste.Children.Clear();
            var kodlbl = new Label { Text = "Kód souboje:" };
            var kod = new Entry { Keyboard = Keyboard.Numeric };
            var potvrdit = new Button { Text = "Potvrdit" };
            var zpet = new Button { Text = "Zpět" };
            Hra.bojiste.Children.Add(kodlbl, 0, 1);
            Hra.bojiste.Children.Add(kod, 1, 1);
            Grid.SetColumnSpan(potvrdit, 3);
            Grid.SetColumn(potvrdit, 0);
            Grid.SetRow(potvrdit, 2);
            Hra.bojiste.Children.Add(potvrdit);
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetColumn(zpet, 0);
            Grid.SetRow(zpet, 3);
            Hra.bojiste.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
            {
                BojisteUvod();
            };
            potvrdit.Clicked += async (sender, args) =>
             {
                 MySqlCommand prikaz = new MySqlCommand("Select * from bakalarka.souboj where idsouboj=@id;");
                 prikaz.Parameters.AddWithValue("@id", kod.Text);
                 MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                 if (data.HasRows)
                 {
                     //TODO DO bojeeeeeeee
                 }
                 else
                 {
                     var info = new Label { Text = "Zadaný kód je špatný" };
                     Grid.SetColumnSpan(info, 3);
                     Grid.SetColumn(info, 0);
                     Grid.SetRow(info, 4);
                     Hra.bojiste.Children.Add(info);
                 }
             };

            return null;
        }
    }
}
