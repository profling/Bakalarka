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
            if (Hrac.role == 1 ||Hrac.role ==0)

            {
                Button novySouboj = new Button {
                Text="Nový souboj"};
                Grid.SetColumnSpan(novySouboj, 3);
                Grid.SetColumn(novySouboj, 0);
                Grid.SetRow(novySouboj, 1);
                Hra.bojiste.Children.Add(novySouboj);
                novySouboj.Clicked += async (sender, args) =>
                 {
                     if (Hrac.zivot == 1)
                     {
                         VyvolatSouboj();
                     }
                     else
                     {
                         var info = new Label() { Text = "Jsi nesmrtelny nebo mrtvy. Za takoveho stavu nemuzes bojovat." };
                         Grid.SetColumnSpan(info, 3);
                         Grid.SetColumn(info, 0);
                         Grid.SetRow(info, 2);
                         Hra.bojiste.Children.Add(info);
                     }
                 };
            }// vyvolani souboje

            if (Hrac.role != 3)// prijmout soubouj
            {


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
                    if (Hrac.zivot == 1)
                    {
                        PrijmoutSouboj();
                    }
                    else
                    {
                        var info = new Label() { Text = "Jsi nesmrtelny nebo mrtvy. Za takoveho stavu nemuzes bojovat." };
                        Grid.SetColumnSpan(info, 3);
                        Grid.SetColumn(info, 0);
                        Grid.SetRow(info, 2);
                        Hra.bojiste.Children.Add(info);
                    }

                };
            }// prijmout souboj

            if (Hrac.role == 3) // oziveni
            {
                var oziveni = new Button() { Text = "Kód pro oživení" };
                Grid.SetColumnSpan(oziveni, 3);
                Grid.SetRow(oziveni, 2);
                Grid.SetColumn(oziveni, 0);
                Hra.bojiste.Children.Add(oziveni);
                oziveni.Clicked += async (sender, args) =>
                 {

                     KodOziveni();
                 };
                

              
            }
            return null;
        } 
        
        /*
         * Metado pomoci ktere se vyvola souboj
         */
        static String VyvolatSouboj()
        {
            Hra.bojiste.Children.Clear();
            int idSouboje = ((Hrac.iduzivatel * 10) + (boju++)) * 100000 + new Random().Next(100, 999);
            int typSouboje = new Random().Next(1, 3);
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO `bakalarka`.`souboj` (`uzivatel1`, `idsouboj`,`idtyp_souboje`) VALUES (@idhrac, @idsouboj, @idtyp);");
            prikaz.Parameters.AddWithValue("@idhrac", Hrac.iduzivatel);
            prikaz.Parameters.AddWithValue("@idsouboj", idSouboje );//vic jak tisic jich nezvladne
            prikaz.Parameters.AddWithValue("@idtyp", typSouboje);
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
                start.Clicked += async (sender, args) =>// zacatek biiiiiitvyyy
                {
                    MySqlCommand prikazSouper = new MySqlCommand("Select uzivatel2 from bakalarka.souboj where idsouboj=@id; ");
                    prikazSouper.Parameters.AddWithValue("@id", idSouboje);
                    MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazSouper);
                    data.Read();
                    if (!Convert.IsDBNull(data["uzivatel2"]))//zjisteni jestli je protivnik pripraven do boje
                    {
                        Souboj boj = new Souboj(1 ,idSouboje,1);//pro TEST posilame jednicku pak zmenit na typSouoboje
                    }
                    else
                    {
                        var info = new Label() { Text = "Soupeř neni ještě připraven" };
                        Grid.SetColumnSpan(info, 3);
                        Grid.SetColumn(info, 0);
                        Grid.SetRow(info, 4);
                        Hra.bojiste.Children.Add(info);

                    }
                   
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
                 data.Read();
                     if (data.HasRows)
                     {
                         MySqlCommand prikazUpdate = new MySqlCommand("update bakalarka.souboj set uzivatel2=@iduzivatel where idsouboj=@idsouboj;");
                         prikazUpdate.Parameters.AddWithValue("@iduzivatel", Hrac.iduzivatel);
                         prikazUpdate.Parameters.AddWithValue("@idsouboj", kod.Text);
                         DBConnector.ProvedeniPrikazuOstatni(prikazUpdate);
                         Hra.bojiste.Children.Clear();//stranka pripraven do bojeeee a vlozeni uzivatele do tabulky
                         var lbl = new Label() { Text = "Souboj " + kod.Text };
                         Grid.SetColumnSpan(lbl, 3);
                         Grid.SetColumn(lbl, 0);
                         Grid.SetRow(lbl, 1);
                         Hra.bojiste.Children.Add(lbl);
                         var start = new Button() { Text = "Start" };
                         Grid.SetColumnSpan(start, 3);
                         Grid.SetColumn(start, 0);
                         Grid.SetRow(start, 2);
                         Hra.bojiste.Children.Add(start);
                         start.Clicked += async (sender1, args1) =>
                          {
                              Souboj boj = new Souboj(2, (int)data["idsouboj"], 2);// do booojeeee pro test typ souboje 2
                      };


                     }
                     else
                     {
                         var info = new Label { Text = "Zadaný kód je špatný!" };
                         Grid.SetColumnSpan(info, 3);
                         Grid.SetColumn(info, 0);
                         Grid.SetRow(info, 4);
                         Hra.bojiste.Children.Add(info);

                     }
                 
             };

            return null;
        }

        /*
         * Zobrazeni ozivovaciho kodu u domecku
         */
        static void KodOziveni()
        {
            Hra.bojiste.Children.Clear();
            MySqlCommand prikaz = new MySqlCommand("Select oziveni from bakalarka.tym where idtym=@idtym;");
            prikaz.Parameters.AddWithValue("@idtym", Hrac.tym);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            data.Read();
            var kodlbl = new Label() { Text = "Kód pro oživení tvých hráču je " + Convert.ToString((int)data["oziveni"]) };
            Grid.SetColumnSpan(kodlbl, 3);
            Grid.SetRow(kodlbl, 1);
            Grid.SetColumn(kodlbl, 0);
            Hra.bojiste.Children.Add(kodlbl);
            var zpet = new Button() {Text="Zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetColumn(zpet, 0);
            Grid.SetRow(zpet, 2);
            Hra.bojiste.Children.Add(zpet);
            zpet.Clicked += async (sender, args) =>
             {
                 BojisteUvod();
             };
        }
    }
}
