using System;
using System.Collections.Generic;
using System.Text;
using Bakalarka.logika;
using MySqlConnector;
using Xamarin.Forms;

namespace Bakalarka.role
{
    static class Hrac
    {
        static public int iduzivatel;
        public static String jmeno;
        public static int role;
        public static int tym;
        public static String inventarNazev="";
        public static int _inventar = 0;
        public static int inventar 
        {
            get { return _inventar; }
            set { // kdyz se zmani inventar tak se provede toto
                _inventar = value;
                Hra.invObsah.Text =inventarNazev ;
            }
        }
        public static int _zivot=1;
        public static int zivot {
            get { return _zivot; }
            set { _zivot = value;
                if (value == 1)
                {
                    Hra.stavZivotu.Text = "Živý";
                }
                if (value == 2)
                {
                    Hra.stavZivotu.Text = "Mrtvý";
                }
                if (value == 3)
                {
                    Hra.stavZivotu.Text = "Nesmrtelný";
                }
            }
        }
        
        public static Boolean prihlaseny = false;
       

        /*
         * prihlaseni hrace do aplikace
         */
        static public String Prihlaseni(int id, String heslo)
        {
            Hra.stavZivotu = new Label() { };
            Hra.invObsah = new Label() { };
            MySqlCommand prikaz = new MySqlCommand("SELECT * FROM bakalarka.uzivatel WHERE heslo=@heslo and iduzivatel=@id; ");
            prikaz.Parameters.AddWithValue("@heslo", heslo);
            prikaz.Parameters.AddWithValue("@id", id);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            if (data.HasRows)
            {

                while (data.Read())
                {
                    iduzivatel = (int)data["iduzivatel"];
                    jmeno = data["iduzivatel"].ToString();
                    role = (int)data["role"];
                    zivot = (int)data["zivot"];
                    if (Hrac.role == 0)//vedouci nema tym
                    {
                        Hrac.tym = 0;
                    }
                    else {
                        tym = (int)data["tym"];

                    }
                    if (Hrac.role != 0)// nacteni hry a inventare
                    {
                        MySqlCommand prikazNacteniHry = new MySqlCommand("Select hra from tym where idtym=@idtym;");
                        prikazNacteniHry.Parameters.AddWithValue("@idtym", tym);
                        MySqlDataReader dataNacteni = DBConnector.ProvedeniPrikazuSelect(prikazNacteniHry);
                        dataNacteni.Read();
                        Hra.nacteniHry((int)dataNacteni["hra"]); // mozna tady bude potraba idhry

                        if (!Convert.IsDBNull(data["inventar"]))
                        {
                            inventarNazev = Hra.produkty.Find(x => x.id == (int)data["inventar"]).nazev;
                            inventar = (int)data["inventar"];
                        }
                    }
                  
                        
                    prihlaseny = true;
                }
                
                return null;
            }
            else
            {
                return "Neco se nepovedlo pri prihlaseni";

            }
            
        }

       /* vlozeni hrace do databaze
        */
        static public String Vytvoreni(int id, int rolep, int tymp, String heslo)
        {
            MySqlCommand prikaz = new MySqlCommand("INSERT INTO `bakalarka`.`uzivatel` (`iduzivatel`, `role`, `tym`, `heslo`) VALUES (@id,@role,@tym,@heslo);");
            prikaz.Parameters.AddWithValue("@id", id);
            prikaz.Parameters.AddWithValue("@role",rolep);
            prikaz.Parameters.AddWithValue("@tym", tymp);
            prikaz.Parameters.AddWithValue("@heslo", heslo);
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikaz);
            if (prubeh == null)
            {
                return null;
            }
            else
            {
                return prubeh;
            }        }

        /*
         * vlozeni produktu do inventare
         */
        static public String VlozeniDoInventare(int idProduktu)
        {
            if (inventar!=0)
            {
                return "Inventar uz je plny";
            }
            MySqlCommand prikazvlozit = new MySqlCommand("Update bakalarka.uzivatel set inventar=@idprodukt where iduzivatel=@iduzivatel");
            prikazvlozit.Parameters.AddWithValue("@idprodukt", idProduktu);
            prikazvlozit.Parameters.AddWithValue("@iduzivatel", iduzivatel);
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(prikazvlozit);
            if (prubeh != null)
            {
                return "neco se nepovedlo pri vkladani produktu" + prubeh;
            }

            inventarNazev = Hra.produkty.Find(x => x.id == idProduktu).nazev;
            inventar = idProduktu;
            return null;
        }

        /*
         * Oziveni hrace 
         */
        static public String Oziveni(int kod)
        {
            MySqlCommand prikaz = new MySqlCommand("Select oziveni from bakalarka.tym where idtym=@idtym;");
            prikaz.Parameters.AddWithValue("@idtym", Hrac.tym);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            data.Read();
            if (kod==(int)data["oziveni"])
            {
                MySqlCommand prikazOziveni = new MySqlCommand("update bakalarka.uzivatel set zivot=1 where iduzivatel=@iduz;update bakalarka.tym set oziveni=@ran where idtym=@idtym;");
                prikazOziveni.Parameters.AddWithValue("@iduz", Hrac.iduzivatel);
                prikazOziveni.Parameters.AddWithValue("@idtym", Hrac.tym);
                prikazOziveni.Parameters.AddWithValue("@ran", new Random().Next(100000, 999999));
                DBConnector.ProvedeniPrikazuOstatni(prikazOziveni);
                Hrac.zivot = 1;
                return null;
            }
            else
            {
                return "Zadaný kód je špatný";
            }
            
        }
    }

    

}
