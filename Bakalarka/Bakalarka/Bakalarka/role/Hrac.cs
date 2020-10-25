using System;
using System.Collections.Generic;
using System.Text;
using Bakalarka.logika;
using MySqlConnector;

namespace Bakalarka.role
{
    static class Hrac
    {
        static public int iduzivatele;
        public static String jmeno;
        public static int role;
        public static int tym;
        public static int inventar;
        public static Boolean prihlaseny = false;

        /*
         * prihlaseni hrace do aplikace
         */
        static public String Prihlaseni(int id, String heslo)
        {
            MySqlCommand prikaz = new MySqlCommand("SELECT * FROM bakalarka.uzivatel WHERE heslo=@heslo and iduzivatel=@id; ");
            prikaz.Parameters.AddWithValue("@heslo", heslo);
            prikaz.Parameters.AddWithValue("@id", id);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            if (data.HasRows)
            {

                while (data.Read())
                {
                    iduzivatele = (int)data["iduzivatel"];
                    jmeno = data["iduzivatel"].ToString();
                    role = (int)data["role"];
                    tym = (int)data["tym"];
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

    }
}
