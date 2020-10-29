using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Bakalarka.logika
{
    static class Hra
    {
        public static String nazev;
        public static int tymu, hracu, idhry;
        public static double roh1X, roh1Y, roh2X, roh2Y, roh3X, roh3Y, roh4X, roh4Y;
        public static List<Produkt> produkty;
        public static int vybranyProdukt;
        public static Label produktPopis;
        public static Map mapa;


        /*
         * Metoda nova hra slouzi k vytvoreni nove hre. ukaze mapu a vygeneruje hrace a tymy
         */
        public static String novaHra(String roh1Xp,String roh1Yp, String roh2Xp,String roh2Yp, String roh3Xp,String roh3Yp,String roh4Xp,String roh4Yp, int zakladatel, int tymup, int hracup, String nazev)
        {
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO bakalarka.hra (`zakladatel`, `roh1X`, `roh1Y`, `roh2X`, `roh2Y`, `roh3X`, `roh3Y`, `roh4X`, `roh4Y`,`nazev` ) VALUES ( @zakladatel ,@roh1X ,@roh1Y,@roh2X,@roh2Y,@roh3X,@roh3Y,@roh4X,@roh4Y,@nazev);");
            prikaz.Parameters.AddWithValue("@zakladatel", zakladatel);
            prikaz.Parameters.AddWithValue("@roh1X", roh1Xp);
            prikaz.Parameters.AddWithValue("@roh1Y", roh1Yp);
            prikaz.Parameters.AddWithValue("@roh2X", roh2Xp);
            prikaz.Parameters.AddWithValue("@roh2Y", roh2Yp);
            prikaz.Parameters.AddWithValue("@roh3X", roh3Xp);
            prikaz.Parameters.AddWithValue("@roh3Y", roh3Yp);
            prikaz.Parameters.AddWithValue("@roh4X", roh4Xp);
            prikaz.Parameters.AddWithValue("@roh4Y", roh4Yp);
            prikaz.Parameters.AddWithValue("@nazev", nazev);
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikaz);
            if (prubeh == null)
            {
                idHry(nazev);                
                //vygenerovani tymu a hracu
                for (int y = 0; y < tymup; y++)
                {
                    int idtym =0;
                    MySqlCommand prikaztym = new MySqlCommand("INSERT INTO `bakalarka`.`tym` (`nazev`, `hra`) VALUES (@nazev,@idhry);");
                    prikaztym.Parameters.AddWithValue("@nazev",y);
                    prikaztym.Parameters.AddWithValue("@idhry",idhry);
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
                        
                            var rand = new Random();
                            MySqlCommand prikazhrac = new MySqlCommand("INSERT INTO `bakalarka`.`uzivatel` (`jmeno`, `role`, `tym`, `heslo`) VALUES (@jmeno,@role,@tym,@heslo);");
                            var jmeno = new MySqlParameter("@jmeno", MySqlDbType.String);
                        prikazhrac.Parameters.Add(jmeno);
                        prikazhrac.Parameters.AddWithValue("@tym",idtym);
                            prikazhrac.Parameters.AddWithValue("@heslo",rand.Next(10000,20000));
                            var role = new MySqlParameter("@role", MySqlDbType.Int32);
                            prikazhrac.Parameters.Add(role);
                            double pocetlovcu = ((double)hracup - (double)1) / (double)3; //tretina hracu jsou lovci
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
                            for (int z = 0; z < hracup-1-Math.Round(pocetlovcu); z++)
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
                    roh1X = Double.Parse(data["roh1X"].ToString(),System.Globalization.CultureInfo.InvariantCulture);
                    roh1Y = Double.Parse(data["roh1Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh2X = Double.Parse(data["roh2X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh2Y = Double.Parse(data["roh2Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh3X = Double.Parse(data["roh3X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh3Y = Double.Parse(data["roh3Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh4X = Double.Parse(data["roh4X"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    roh4Y = Double.Parse(data["roh4Y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    nazev = (String)data["nazev"];

                }

                //nacteni produktu
                MySqlCommand prikazpr = new MySqlCommand("Select * from bakalarka.produkt");
                MySqlDataReader datapr = DBConnector.ProvedeniPrikazuSelect(prikazpr);
                Hra.produkty = new List<Produkt>();
                while (datapr.Read())
                {
                    produkty.Add(new Produkt((int)datapr["idprodukt"], (String)datapr["nazev"], (String)datapr["popis"], (double)datapr["X"], (double)datapr["Y"], (double)datapr["X2"], (double)datapr["Y2"]));
                }

                //nacteni mapy 
                MapaKontroler.nacteniProduktu();
                MapaKontroler.HerniPole();
               
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
    }
}
