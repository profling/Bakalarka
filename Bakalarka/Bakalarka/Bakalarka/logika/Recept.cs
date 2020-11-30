using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Bakalarka.role;
using MySqlConnector;

namespace Bakalarka.logika
{
    class Recept
    {
        public int vysledek;
        public String prisada1;
        public String prisada2;
        public String prisada3;
        public String popis;
        public int mnozstvi1, mnozstvi2,mnozstvi3;
        int id1, id2, id3;

        public Recept(int vysledek)
        {
            
            MySqlCommand prikaz = new MySqlCommand("Select * from bakalarka.recept where vysledek=@id ");//Borce, az to nebude zase fungovat tak musis zmenit nazev tabulky braseee
            prikaz.Parameters.AddWithValue("@id",vysledek);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            
            if (data.HasRows)
            {
                data.Read();    
                this.vysledek = vysledek;
                popis = Hra.produkty.Find(item => item.id == vysledek).popis;
                prisada1 = Hra.produkty.Find(item => item.id == (int)data["prisada1"]).nazev;
                mnozstvi1 = (int)data["mnozstvi1"];
                id1 = (int)data["prisada1"];
                prisada2 = Hra.produkty.Find(item => item.id == (int)data["prisada2"]).nazev;
                mnozstvi2 = (int)data["mnozstvi2"];
                id2 = (int)data["prisada2"];
                if (!Convert.IsDBNull(data["prisada3"]))
                  {
                      prisada3 = Hra.produkty.Find(item => item.id == (int)data["prisada3"]).nazev;
                      mnozstvi3 = (int)data["mnozstvi3"];
                      id3 = (int)data["prisada3"];
                  }
            }
            else
            {
                this.vysledek = 0; // pokud pro dany produkt neexistuje recept, tak se nastavi na 0
            }
           



        }
        /*
         * Smena produktu na jiny produkt dle receptu
         */
        public String Smena()
        {
            //overeni ze uzivatel na to ma suroviny
            if (mnozstvi1>Hra.produkty.Find(item => item.id ==id1).ulozene || mnozstvi2> Hra.produkty.Find(item => item.id == id2).ulozene)
            {

                return "Nedostatek surovin na smenu";
            }
            if (id3 != 0)
            {
                if (mnozstvi3 > Hra.produkty.Find(item => item.id == id3).ulozene) return "Nedostatek surovin na smenu";
            }
            //smazani prvni prisady
            MySqlCommand smazaniprisada1 = new MySqlCommand();
            for (int i = 0;i<mnozstvi1;i++)
            {
                smazaniprisada1.CommandText+="Delete from bakalarka.sklad where idprodukt=@idprodukt and idtym=@idtym limit 1;";
            }
            smazaniprisada1.Parameters.AddWithValue("@idprodukt", id1);
            smazaniprisada1.Parameters.AddWithValue("@idtym", Hrac.tym);
            String prubeh = DBConnector.ProvedeniPrikazuOstatni(smazaniprisada1);
            Hra.produkty.Find(item => item.id == id1).ulozene -= mnozstvi1;
            if (prubeh != null) return "neco se pokazilo pri mazani prisady1";

            //smazani druhe prisady
            MySqlCommand smazaniprisada2 = new MySqlCommand();
            for (int i = 0; i < mnozstvi2; i++)
            {
                smazaniprisada2.CommandText += "Delete from bakalarka.sklad where idprodukt=@idprodukt and idtym=@idtym limit 1;";
            }
            smazaniprisada2.Parameters.AddWithValue("@idprodukt", id2);
            smazaniprisada2.Parameters.AddWithValue("@idtym", Hrac.tym);
            prubeh = DBConnector.ProvedeniPrikazuOstatni(smazaniprisada2);
            Hra.produkty.Find(item => item.id == id2).ulozene -= mnozstvi2;
            if (prubeh != null) return "neco se pokazilo pri mazani prisady2";

            //smazani treti prisady pokud je potreba
            if (prisada3 != null)
            {
                MySqlCommand smazaniprisada3 = new MySqlCommand();
                for (int i = 0; i < mnozstvi3; i++)
                {
                    smazaniprisada3.CommandText += "Delete from bakalarka.sklad where idprodukt=@idprodukt and idtym=@idtym limit 1;";
                }
                smazaniprisada3.Parameters.AddWithValue("@idprodukt", id3);
                smazaniprisada3.Parameters.AddWithValue("@idtym", Hrac.tym);
                prubeh = DBConnector.ProvedeniPrikazuOstatni(smazaniprisada3);
                Hra.produkty.Find(item => item.id == id3).ulozene -= mnozstvi3;
                if (prubeh != null) return "neco se pokazilo pri mazani prisady3";
            }

            //pridani vysledku do skladu
            MySqlCommand pridani = new MySqlCommand("INSERT INTO `bakalarka`.`sklad` (`idprodukt`, `idtym`) VALUES (@idprodukt, @idtym);");
            pridani.Parameters.AddWithValue("@idprodukt", vysledek);
            pridani.Parameters.AddWithValue("@idtym", Hrac.tym);
            prubeh = DBConnector.ProvedeniPrikazuOstatni(pridani);
            Hra.produkty.Find(item => item.id == vysledek).ulozene++;
            if (prubeh != null) return "Neco se pokazilo pri vkladani vysledku";
            //stastny konec
            return null;
        }
    }

}
