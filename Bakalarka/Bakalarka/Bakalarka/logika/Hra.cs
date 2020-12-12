using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Threading.Tasks;

namespace Bakalarka.logika
{
    static class Hra
    {
        public static String nazev;
        public static int  idhry;
        public static double roh1X, roh1Y, roh2X, roh2Y, roh3X, roh3Y, roh4X, roh4Y;
        public static List<Produkt> produkty;
        public static int vybranyProdukt;
        public static Label produktPopis, invObsah, stavZivotu;

        public static Map mapa;
        public static Grid skladiste;
        public static Grid bojiste;

        /*
         * Metoda nova hra slouzi k vytvoreni nove hre. ukaze mapu a vygeneruje hrace a tymy
         */
        public static String novaHra(String roh1X,String roh1Y, String roh2X,String roh2Y, String roh3X,String roh3Y,String roh4X,String roh4Y, int zakladatel, int tymu, int hracu, String nazev)
        {
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO bakalarka.hra (`zakladatel`, `roh1X`, `roh1Y`, `roh2X`, `roh2Y`, `roh3X`, `roh3Y`, `roh4X`, `roh4Y`,`nazev` ) VALUES ( @zakladatel ,@roh1X ,@roh1Y,@roh2X,@roh2Y,@roh3X,@roh3Y,@roh4X,@roh4Y,@nazev);");
            prikaz.Parameters.AddWithValue("@zakladatel", zakladatel);
            prikaz.Parameters.AddWithValue("@roh1X", roh1X);
            prikaz.Parameters.AddWithValue("@roh1Y", roh1Y);
            prikaz.Parameters.AddWithValue("@roh2X", roh2X);
            prikaz.Parameters.AddWithValue("@roh2Y", roh2Y);
            prikaz.Parameters.AddWithValue("@roh3X", roh3X);
            prikaz.Parameters.AddWithValue("@roh3Y", roh3Y);
            prikaz.Parameters.AddWithValue("@roh4X", roh4X);
            prikaz.Parameters.AddWithValue("@roh4Y", roh4Y);
            prikaz.Parameters.AddWithValue("@nazev", nazev);
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikaz);
            
           


            if (prubeh == null)
            {
                idHry(nazev);
                //vytvoreni polohy produktu
                polohaProduktu();
                //vygenerovani tymu a hracu
                for (int y = 0; y < tymu; y++)
                {
                    var rand = new Random();
                    int idtym =0;
                    MySqlCommand prikaztym = new MySqlCommand("INSERT INTO `bakalarka`.`tym` (`nazev`, `hra`,uskladani,oziveni) VALUES (@nazev,@idhry,@ulozeni,@oziveni);");
                    prikaztym.Parameters.AddWithValue("@nazev",y);
                    prikaztym.Parameters.AddWithValue("@idhry",idhry);
                    prikaztym.Parameters.AddWithValue("@ulozeni", rand.Next(100000,999999));
                    prikaztym.Parameters.AddWithValue("@oziveni", rand.Next(100000, 999999));
                    prubeh = DBConnector.ProvedeniPrikazuOstatni(prikaztym);
                    if (prubeh == null)
                    {
                        MySqlCommand prikazidtym = new MySqlCommand("Select idtym from bakalarka.tym where nazev=@nazev and hra=@idhra");
                        prikazidtym.Parameters.AddWithValue("@nazev",Convert.ToString(y));
                        prikazidtym.Parameters.AddWithValue("@idhra", idhry);
                        MySqlDataReader dataidtym = DBConnector.ProvedeniPrikazuSelect(prikazidtym);
                        if (dataidtym.HasRows)
                        {
                            while (dataidtym.Read())
                            {
                                idtym = (int)dataidtym["idtym"];
                            }
                        }else
                        {
                            return "nenacte se tym";
                        }
                        //vytvoreni hracu
                        
                           
                            MySqlCommand prikazhrac = new MySqlCommand("INSERT INTO `bakalarka`.`uzivatel` (`jmeno`, `role`, `tym`, `heslo`,zivot, pocetUlozeni) VALUES (@jmeno,@role,@tym,@heslo,1, 0);");
                            var jmeno = new MySqlParameter("@jmeno", MySqlDbType.String);
                        prikazhrac.Parameters.Add(jmeno);
                        prikazhrac.Parameters.AddWithValue("@tym",idtym);
                            prikazhrac.Parameters.AddWithValue("@heslo",rand.Next(10000,20000));
                            var role = new MySqlParameter("@role", MySqlDbType.Int32);
                            prikazhrac.Parameters.Add(role);
                            double pocetlovcu = ((double)hracu - (double)1) / (double)3; //tretina hracu jsou lovci
                             //role lovec 1
                            for(int z = 0; z <Math.Round(pocetlovcu); z++)
                            {
                                role.Value = 1;
                                jmeno.Value = "hrac" + rand.Next(10000, 20000);
                                prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazhrac);
                                if (prubeh != null)
                                {
                                    return "vytvareni lovce" + prubeh;
                                }

                            }
                            //role tezer 2
                            for (int z = 0; z < hracu-1-Math.Round(pocetlovcu); z++)
                            {
                                role.Value = 2;
                            jmeno.Value = "hrac" + rand.Next(10000, 20000);
                            prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazhrac);
                                if (prubeh != null)
                                {
                                    return "vytvareni tezere" + prubeh;
                                }

                            }
                            //role domecek 3
                            role.Value = 3;
                        jmeno.Value = "hrac" + rand.Next(10000, 20000);
                        prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazhrac);
                            if (prubeh != null)
                            {
                                return "vytvareni domecku" + prubeh;
                            }

                            

                        }
                    
                    else
                    {
                        return "pokazil se insert tym" + prubeh;
                    }
                }










                return null;
            }
            else
            {
                return "pokazilo se to pri vytvareni hry:("+prubeh;
            }
        }
        /*
         * Slouzi k nacteni infu o hre
         */
        static public String nacteniHry(int idhry)
        {
          
            MySqlCommand prikaz = new MySqlCommand("Select * from bakalarka.hra where idhra=@idhra");
            prikaz.Parameters.AddWithValue("@idhra", idhry);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            
            if (data.HasRows)
            {
                while (data.Read())
                {
                    roh1X = (double)data["roh1X"];
                    roh1Y = (double)data["roh1Y"];
                    roh2X = (double)data["roh2X"];
                    roh2Y = (double)data["roh2Y"];
                    roh3X = (double)data["roh3X"];
                    roh3Y = (double)data["roh3Y"];
                    roh4X = (double)data["roh4X"];
                    roh4Y = (double)data["roh4Y"];
                    nazev = (String)data["nazev"];

                }

                //nacteni produktu
                MySqlCommand prikazpr = new MySqlCommand("select x1,y1,x2,y2,bakalarka.produkt.idprodukt as idprodukt, nazev,popis,uroven from bakalarka.polohaProduktu right join bakalarka.produkt on produkt.idprodukt=polohaProduktu.idprodukt where idhra=@idhra or idhra is null ; ");
                prikazpr.Parameters.AddWithValue("@idhra",idhry);
                MySqlDataReader datapr = DBConnector.ProvedeniPrikazuSelect(prikazpr);
                Hra.produkty = new List<Produkt>();
                while (datapr.Read())
                {
                    if ((int)datapr["uroven"]==1)
                    {
                        produkty.Add(new Produkt((int)datapr["idprodukt"], (String)datapr["nazev"], (String)datapr["popis"], (double)datapr["x1"], (double)datapr["y1"], (double)datapr["x2"], (double)datapr["y2"], (int)datapr["uroven"]));
                    }
                    else
                    {
                        produkty.Add(new Produkt((int)datapr["idprodukt"], (String)datapr["nazev"], (String)datapr["popis"],(int)datapr["uroven"]));
                    }
                  
                }

                //nacteni mapy 
                MapaKontroler.NacteniProduktu();
                MapaKontroler.HerniPole();
                MapaKontroler.PoziceHrace();
                //nacteni skladu
                Sklad.NacteniSkladu();
                //nacteni souboju 
                Bojiste.BojisteUvod();
               
                return null;
            }
            else
            {
                return "neco se pokazilo pri nacitani hry";
            }

            return null;
        }
        /*
         * ziskani idhry
         */
        static public String idHry(String nazev)
        {
            MySqlCommand prikazidhry = new MySqlCommand("SELECT idhra FROM `bakalarka`.`hra` WHERE `nazev`=@nazev; ");
            prikazidhry.Parameters.AddWithValue("@nazev", nazev);
            MySqlDataReader idhryData = DBConnector.ProvedeniPrikazuSelect(prikazidhry);
            if (idhryData.HasRows)
            {
                while (idhryData.Read())
                {
                    idhry = (int)idhryData["idhra"];
                    return null;
                }
            }
            else
            {
                return "nenacetlo se id hrzy";
            }
            
            return "neco se pokazilo pri nacitani id hry";
            
        }

        /*
         * Vytvoreni polohy produktu
         */
        static void polohaProduktu()
        {

            MySqlCommand prikazHra = new MySqlCommand("select * from bakalarka.hra;");
            MySqlDataReader dataHra = DBConnector.ProvedeniPrikazuSelect(prikazHra);
            double roh1x = 0, roh1y = 0, roh2x = 0, roh2y = 0, roh3x = 0, roh3y = 0, roh4x = 0, roh4y = 0;
            while (dataHra.Read())
            {
                roh1x = (double)dataHra["roh1X"];
                roh1y = (double)dataHra["roh1Y"];
                roh2x = (double)dataHra["roh2X"];
                roh2y = (double)dataHra["roh2Y"];
                roh3x = (double)dataHra["roh3X"];
                roh3y = (double)dataHra["roh3Y"];
                roh4x = (double)dataHra["roh4X"];
                roh4y = (double)dataHra["roh4Y"];
                Bod[] hraciPole = { new Bod(roh1x, roh1y), new Bod(roh2x, roh2y), new Bod(roh3x, roh3y), new Bod(roh4x, roh4y) };
                MySqlCommand prikazProdukty = new MySqlCommand("Select idprodukt from bakalarka.produkt where uroven=1;");
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazProdukty);

                double[] y = { roh1y, roh2y, roh3y, roh4y };
                double yMin = y.Min();
                double yMax = y.Max();
                double[] x = { roh1x, roh2x, roh3x, roh4x };
                double xMin = x.Min();
                double xMax = x.Max();
                while (data.Read())
                {
                    Boolean prvniSouradnice = true;
                    Boolean druhaSouradnice = true;
                    Bod prvni = new Bod();
                    Bod druha = new Bod();
                    while (prvniSouradnice) // dokud neni souradnice v polygonu
                    {
                        prvni.X = new Random().NextDouble() * (xMax - xMin) + xMin;
                        prvni.Y = new Random().NextDouble() * (yMax - yMin) + yMin;
                        if (BodVPolygonu(hraciPole, prvni)) prvniSouradnice = false;

                    }
                    while (druhaSouradnice) // dokud neni souradnice v polygonu
                    {
                        druha.X = new Random().NextDouble() * (xMax - xMin) + xMin;
                        druha.Y = new Random().NextDouble() * (yMax - yMin) + yMin;
                        if (BodVPolygonu(hraciPole, druha)) druhaSouradnice = false;

                    }
                    MySqlCommand prikazPoloha = new MySqlCommand("insert into bakalarka.polohaProduktu (x1,y1,x2,y2,idhra,idprodukt) values(@x1,@y1,@x2,@y2,@idhra,@idprodukt);");
                    prikazPoloha.Parameters.AddWithValue("@x1", prvni.X);
                    prikazPoloha.Parameters.AddWithValue("@y1", prvni.Y);
                    prikazPoloha.Parameters.AddWithValue("@x2", druha.X);
                    prikazPoloha.Parameters.AddWithValue("@y2", druha.Y);
                    prikazPoloha.Parameters.AddWithValue("@idhra", (int)dataHra["idhra"]);
                    prikazPoloha.Parameters.AddWithValue("@idprodukt", (int)data["idprodukt"]);
                     DBConnector.ProvedeniPrikazuOstatni(prikazPoloha);
                   
                }

            }
        }
        /*
         * Aktualizace polohy produktu
         */
         public static async void AktualizacePolohy()
        {
            while (true)
            {

           
                MySqlCommand prikaz = new MySqlCommand("Select *from bakalarka.polohaProduktu where idhra=@idhra;");
                prikaz.Parameters.AddWithValue("@idhra", idhry);
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                while (data.Read())
                {
                    Hra.produkty.Find(x => x.id == (int)data["idprodukt"]).jedna.Position = new Position((double)data["x1"], (double)data["y1"]);
                    Hra.produkty.Find(x => x.id == (int)data["idprodukt"]).dva.Position = new Position((double)data["x2"], (double)data["y2"]);
                }
                await Task.Delay(1000*10);// brzda na 10 vterin, takze aktualizace prohne jednou za 10 vterin
            }

        }
        /*
         * Overeni, ze se bod nachazi v polygonu
         */
        static bool BodVPolygonu(Bod[] polygon, Bod testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
