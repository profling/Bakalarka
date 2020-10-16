using System;
using System.Collections.Generic;
using System.Text;
using Bakalarka.logika;
using MySqlConnector;

namespace Bakalarka.role
{
    static class Hrac
    {
        static int iduzivatele;
        static String jmeno;
        static int role;
        static int tym;
        static int inventar;
        static Boolean prihlaseny = false;


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
                   // tym = (int)data["tym"];
                    prihlaseny = true;
                }
                return "prihlaseni probehlo v poradku";
            }
            else
            {
                return "spatne prihlasovaci udaje";

            }
            return null;
        }


    }
}
