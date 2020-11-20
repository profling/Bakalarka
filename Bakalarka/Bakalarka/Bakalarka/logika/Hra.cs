using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;

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
                {/*
                    roh1X = Double.Parse(data["roh1X"].ToString(),System.Globalization.CultureInfo.InvariantCulture);
                    roh1Y = Double.Parse(data["roh1Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh2X = Double.Parse(data["roh2X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh2Y = Double.Parse(data["roh2Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh3X = Double.Parse(data["roh3X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh3Y = Double.Parse(data["roh3Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh4X = Double.Parse(data["roh4X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh4Y = Double.Parse(data["roh4Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);*/
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
        static String polohaProduktu()
        {

            MySqlCommand prikazPole = new MySqlCommand("Select * from bakalarka.hra where idhra=@idhra;");
            prikazPole.Parameters.AddWithValue("@idhra", idhry);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikazPole);
            double roh1x=0, roh1y=0, roh2x=0, roh2y=0, roh3x=0, roh3y=0, roh4x=0, roh4y=0;
            while (data.Read())
            {
                roh1x = (double)data["roh1X"];
                roh1y = (double)data["roh1Y"];
                roh2x = (double)data["roh2X"];
                roh2y = (double)data["roh2Y"];
                roh3x = (double)data["roh3X"];
                roh3y = (double)data["roh3Y"];
                roh4x = (double)data["roh4X"];
                roh4y = (double)data["roh4Y"];
            }

            
            MySqlCommand prikazProdukty = new MySqlCommand("Select idprodukt from bakalarka.produkt where uroven=1;");
             data = DBConnector.ProvedeniPrikazuSelect(prikazProdukty);

            double[] y = {roh1y,roh2y,roh3y,roh4y };
            double yMin = y.Min();
            double yMax = y.Max();
            while (data.Read())
            {
                int idprodukt = (int)data["idprodukt"];
                double y1 = new Random().NextDouble() * (yMax - yMin) + yMin;
                double y2 = new Random().NextDouble() * (yMax - yMin) + yMin;

                double xMaxX1 = roh1x + (roh1x-roh2x)*((y1-roh1y)/(roh1y-roh2y)); // Ode dneska muzu tvrdit, ze jsem v zivote vyuzil znalosti parametricke rovnice krivky
                double xMinX1 = roh4x + (roh4x - roh3x) * ((y1 - roh4y) / (roh4y - roh3y));
                double x1 = new Random().NextDouble() * (xMaxX1 - xMinX1) + xMinX1;
                double xMaxX2 = roh1x + (roh1x - roh2x) * ((y1 - roh1y) / (roh1y - roh2y)); // Ode dneska muzu tvrdit, ze jsem v zivote vyuzil znalosti parametricke rovnice krivky
                double xMinX2 = roh4x + (roh4x - roh3x) * ((y1 - roh4y) / (roh4y - roh3y));
                double x2 = new Random().NextDouble() * (xMaxX2 - xMinX2) + xMinX2;

                MySqlCommand prikazPoloha = new MySqlCommand("INSERT INTO `bakalarka`.`polohaProduktu` ( `x1`, `y1`, `x2`, `y2`, `idhra`, `idprodukt`) VALUES(@x1,@y1,@x2,@y2,@idhra,@idprodukt );");
                prikazPoloha.Parameters.AddWithValue("@x1", x1);
                prikazPoloha.Parameters.AddWithValue("@y1", y1);
                prikazPoloha.Parameters.AddWithValue("@x2", x2);
                prikazPoloha.Parameters.AddWithValue("@y2", y2);
                prikazPoloha.Parameters.AddWithValue("@idhra", idhry);
                prikazPoloha.Parameters.AddWithValue("@idprodukt", idprodukt);
                String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazPoloha);
                Console.WriteLine("AAAA" + prubeh);
            }
             
            return null;
        }
    }
}
