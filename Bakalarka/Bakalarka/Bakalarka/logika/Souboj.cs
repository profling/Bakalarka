using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using MySqlConnector;
using System.Linq;
using Xamarin.Forms;
using Bakalarka.role;

namespace Bakalarka.logika
{
    class Souboj
    {
        int typ;
        int id;
        DateTime start;
        DateTime stop;
        int hrac;

        /*
         * Musim ten popis jeste vymyslet
         */
        public Souboj(int typ, int id, int hrac)
        {
            this.hrac = hrac;
            this.id = id;//vyuzito jako seed do Random
            start = DateTime.Now;
            Hra.bojiste.Children.Clear();
            if (typ == 1)//Lusteni
            {
                Lusteni();
            }
            if (typ == 2)
            {
                Vedomostni();
            }
            if (typ == 3)
            {
                Behaci();
            }
        }

        /*
         * Vytvoreni lusticiho souboje
         */
        private void Lusteni()
        {
            int typSifry = new Random(id).Next(1, 4);//nahodne vzbrani sifry
            String zprava = "";
            String reseni= "";
            String[,] sifra = new String[,] { { "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z" },
                { ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....","..", ".---", "-.-", ".-..", "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-","...-", ".--", "-..-", "-.--", "--.." },
             { "-.",".---",".-.-",".--","-","--.-","..-","----","--","-...",".-.","-.--","..",".-","...","-..-","..-.","-.-","---",".","--.","---.","-..",".--.",".-..","..--" },
            { "01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26" },
            { "2","22","222","3","33","333","4","44","444","5","55","555","6","66","666","7","77","777","7777","8","88","888","9","99","999","9999" }};
            MySqlCommand prikaz = new MySqlCommand("Select slovo from bakalarka.lusteni where idlusteni=@id");
            prikaz.Parameters.AddWithValue("@id", new Random(id).Next(1, 4));// Databaze obsahuje 100 slov, ale zatim jenom 4 snif
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            List<char> znaky = new List<char>();
            while (data.Read())
            {
                reseni = data["slovo"].ToString();
                znaky.AddRange(data["slovo"].ToString().ToCharArray());
            }

            foreach (char znak in znaky)//vyhleda sifru pro kazdy znak a vlozeno do zpravy
            {
                int i = 0;
                Boolean stop = true;
                while (stop)
                {
                    if (znak.ToString().Equals(sifra[0, i]))
                    {
                        zprava += sifra[typSifry, i];
                        if (typSifry == 1 || typSifry == 2)//morseovka se oddeluje lomitkem
                        {
                            zprava += "/";
                        }
                        else
                        {
                            zprava += " ";
                        }
                        stop = false;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            var sifraLBL = new Label() { Text = zprava };
            var vysledek = new Entry();
            var hotovo = new Button() { Text = "Hotovo" };
            Grid.SetColumnSpan(sifraLBL, 3);
            Grid.SetColumn(sifraLBL, 0);
            Grid.SetRow(sifraLBL, 1);
            Hra.bojiste.Children.Add(sifraLBL);
            Grid.SetColumnSpan(vysledek, 3);
            Grid.SetColumn(vysledek, 0);
            Grid.SetRow(vysledek, 2);
            Hra.bojiste.Children.Add(vysledek);
            Grid.SetColumnSpan(hotovo, 3);
            Grid.SetColumn(hotovo, 0);
            Grid.SetRow(hotovo, 3);
            Hra.bojiste.Children.Add(hotovo);
            hotovo.Clicked += async (sender, args) =>
            {
                if (vysledek.Text == reseni)
                {
                    hotovo.IsEnabled = false;
                    await Vyhodnoceni();
                    
                }
               
            };
        }



        /*
         * Vytvoreni sportovniho souboje
         */
        private void Behaci()
        {

        }
        /*
         * Vytvoreni vedomostni otazky
         */
        private void Vedomostni()
        {
            MySqlCommand prikaz = new MySqlCommand("Select * from bakalarka.vedomostni where idvedomostni=@id");
            prikaz.Parameters.AddWithValue("@id", new Random(id).Next(1, 2));//zatim 2 ale az bude vic otazek tak viiiic
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            data.Read();
            var otazka = new Label() { Text = data["otazka"].ToString() };
            Grid.SetColumnSpan(otazka, 3);
            Grid.SetRow(otazka, 1);
            Grid.SetColumn(otazka, 0);
            Hra.bojiste.Children.Add(otazka);
            String spravna = data["spravna"].ToString();
            List<String> odpovedi= new List<String>  { data["spravna"].ToString(), data["spatna1"].ToString(), data["spatna2"].ToString(), data["spatna3"].ToString() };
            for(int i = 0; i < 4; i++)
            {
                int p = new Random().Next(0, odpovedi.Count);
                var odpo = new Button() { Text = odpovedi[p] };
                odpovedi.RemoveAt(p);
                Grid.SetColumnSpan(odpo, 3);
                Grid.SetRow(odpo, 2 + i);
                Grid.SetColumn(odpo, 0);
                Hra.bojiste.Children.Add(odpo);
                odpo.Clicked += async (sender, args) =>
                 {
                     if (odpo.Text.Equals(spravna)){
                         
                         //proste kdyz odpovi spravne tak se zobrazi dalsi cekaci stranka, kde cekas na vysledek
                         Hra.bojiste.Children.Clear();
                         var spravne = new Label() {Text="Spravně! Čeká se na výsledek protihráče..." };
                         Grid.SetColumnSpan(spravne, 3);
                         Grid.SetRow(spravne, 1);
                         Grid.SetColumn(spravne, 0);
                         Hra.bojiste.Children.Add(spravne);
                         await Vyhodnoceni(); //Vyhodnoceniiiiii
                     }
                     else
                     {
                         odpo.BackgroundColor= Color.Red;
                         odpo.IsEnabled = false;
                         start.AddSeconds(5);
                     }
                };
            }
           
           
        }
        /*
         * Vyhodnoceni souboje
         */
        private async Task Vyhodnoceni()
        {
            stop = DateTime.Now;
            TimeSpan t = stop - start;
            int vysledek = Convert.ToInt32(t.TotalMilliseconds);
            int vysledekProtihrac = 0;
            MySqlCommand prikazVlozitVysledek = new MySqlCommand("Update bakalarka.souboj set cas" + hrac.ToString() + "=@vysledek where idsouboj=@idsouboj;"); //je to trosku au, ale pry to jinak udelat nejde :(((
            prikazVlozitVysledek.Parameters.AddWithValue("@idsouboj", id);
            prikazVlozitVysledek.Parameters.AddWithValue("@vysledek", vysledek);// jedem s presnosti na mili brasko
            DBConnector.ProvedeniPrikazuOstatni(prikazVlozitVysledek);
           
            MySqlCommand prikaz = new MySqlCommand("Select cas1,cas2 from bakalarka.souboj where idsouboj=@idsouboj;");
            prikaz.Parameters.AddWithValue("@idsouboj", id);
            while (vysledekProtihrac == 0)
            {

                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                data.Read();
                if (!(Convert.IsDBNull(data["cas1"]) || Convert.IsDBNull(data["cas2"])))
                {
                    if (hrac == 1)
                    {
                        vysledekProtihrac = (int)data["cas2"];
                    }
                    else
                    {
                        vysledekProtihrac = (int)data["cas1"];
                    }
                }
                data.Close();
                await Task.Delay(1000);//brzda na vterinu aby volani nebylo jak kulomet ale spise pomala pistolka
            }
             
             if (vysledek < vysledekProtihrac)
            {
              await  Vyhra();
            }
            else
            {
                if (vysledek == vysledekProtihrac)
                {
                  await   Remiza();
                }
                else
                {
                  await  Prohra();
                }
                
            }
            

        
        }
        /*
         * Zobrazeni vyhry a pridani nesmrtelnosti uzivateli a nacteni inv;
         */
        private async Task Vyhra() {
            Boolean uz = true;
            while (uz)// ceka az prohrany uzivatel upravi databazy a nacetl inventar
            {
                MySqlCommand prikaz = new MySqlCommand("Select inventar from bakalarka.uzivatel where iduzivatel=@id");
                prikaz.Parameters.AddWithValue("@id", Hrac.iduzivatel);
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                data.Read();
                if (Convert.IsDBNull(data["inventar"]))
                {
                    data.Close();
                    await Task.Delay(1000);//brzda na vterinu aby volani nebylo jak kulomet ale spise pomala pistolka
                }
                else
                {
                    uz = false;
                    Hrac.inventar = (int)data["inventar"];
                }

            }
            MySqlCommand prikazUpdate = new MySqlCommand("update bakalarka.uzivatel set zivot=3 where iduzivatel=@id;update bakalarka.souboj set vyherce=@id where idsouboj=@idsouboj;");
            prikazUpdate.Parameters.AddWithValue("@id",Hrac.iduzivatel);
            prikazUpdate.Parameters.AddWithValue("@idsouboj", id);
             String prubeh=DBConnector.ProvedeniPrikazuOstatni(prikazUpdate);
            Hra.bojiste.Children.Clear();
            var vyhra = new Label() { Text="Výhra!!! Získal si nesmrtelnost na dobu než odneseš produkt do domečku."+prubeh };
            Grid.SetColumnSpan(vyhra, 3);
            Grid.SetRow(vyhra, 1);
            Grid.SetColumn(vyhra, 0);
            Hra.bojiste.Children.Add(vyhra);
            var zpet = new Button() { Text = "Zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 2);
            Grid.SetColumn(zpet, 0);
            Hra.bojiste.Children.Add(zpet);
            Hrac.AktualizaceZivotInventar();
            zpet.Clicked += async (sender, args) =>
             {
                 Bojiste.BojisteUvod();
             };
           



        }
        /*
         * Zobrazeni prohry, usmrceni uzivatele a vymazani suroviny se kterou bezi a predeni vyhercovi
         */
        private async Task Prohra()
        {
            MySqlCommand prikaz = new MySqlCommand("update bakalarka.uzivatel set inventar=@inventar where iduzivatel=(select uzivatel1 from bakalarka.souboj where idsouboj=@idsouboj)or iduzivatel=(select uzivatel2 from bakalarka.souboj where idsouboj=@idsouboj);update bakalarka.uzivatel set inventar=null where iduzivatel=@idhrac;update bakalarka.uzivatel set zivot=2 where iduzivatel=@idhrac;");
            if (Hrac.inventar == 0)//Mysql nezpapa 0 misto null a int zas nepapa null, tak to osetrime ifem
            {
                prikaz.Parameters.AddWithValue("@inventar", null);
            }
            else
            {
                prikaz.Parameters.AddWithValue("@inventar", Hrac.inventar);
            }           
            prikaz.Parameters.AddWithValue("@idsouboj", id);
            prikaz.Parameters.AddWithValue("@idhrac", Hrac.iduzivatel);
            string prubeh =DBConnector.ProvedeniPrikazuOstatni(prikaz);
            Hrac.inventar = 0;
            Hra.bojiste.Children.Clear();
            var prohra = new Label() { Text = "Prohra!!! Jseš mrtvý. Utíkej se oživit do domečku." +prubeh};
            Grid.SetColumnSpan(prohra, 3);
            Grid.SetRow(prohra, 1);
            Grid.SetColumn(prohra, 0);
            Hra.bojiste.Children.Add(prohra);
            var zpet = new Button() { Text = "Zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 2);
            Grid.SetColumn(zpet, 0);
            Hra.bojiste.Children.Add(zpet);
            Hrac.AktualizaceZivotInventar();
            zpet.Clicked += async (sender, args) =>
            {

                Bojiste.BojisteUvod();
            };
        }
        /*
         * Zobrazeni remizy
         */
        private async Task Remiza()
        {
            Hra.bojiste.Children.Clear();
            var remiza = new Label() { Text = "Remíza! Proveďte nový souboj!!" };
            Grid.SetColumnSpan(remiza, 3);
            Grid.SetRow(remiza, 1);
            Grid.SetColumn(remiza, 0);
            Hra.bojiste.Children.Add(remiza);
            var zpet = new Button() { Text = "Zpět" };
            Grid.SetColumnSpan(zpet, 3);
            Grid.SetRow(zpet, 2);
            Grid.SetColumn(zpet, 0);
            Hra.bojiste.Children.Add(zpet);
            Hrac.AktualizaceZivotInventar();
            zpet.Clicked += async (sender, args) =>
            {
                Bojiste.BojisteUvod();
            };
        }




    }
}
