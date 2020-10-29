using Bakalarka.role;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bakalarka.logika
{
   static class Sklad
    {
         public static List<Produkt> ulozene;

        /*
         * nacteni skladu
         */
        public static String nacteniSkladu()
        {
            ulozene = Hra.produkty;
            MySqlCommand prikaz = new MySqlCommand("Select count(*) as pocet from bakalarka.sklad where idtym=@idtym and idprodukt=@idprodukt");
            prikaz.Parameters.AddWithValue("@idtym", Hrac.tym);
            var idpro = new MySqlParameter("@idprodukt", MySqlDbType.Int32);
            prikaz.Parameters.Add(idpro);
            foreach (var produkt in ulozene)
            {
                idpro.Value = produkt.id;
                MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
                data.Read();
                if (Convert.IsDBNull(data["pocet"]))
                    {
                    return "neco neni v poho pri nacitani skladu";
                }
                produkt.ulozene =(int) data["pocet"];

            }

            return null;
        }
    }
}
