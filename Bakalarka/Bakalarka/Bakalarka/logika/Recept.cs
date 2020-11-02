using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace Bakalarka.logika
{
    class Recept
    {
        public int vysledek;
        public String prisada1="blabla";
        public String prisada2 = "blabla";
        public String prisada3;
        public int mnozstvi1;
        public int mnozstvi2;
        public int mnozstvi3;

        public Recept(int vysledek)
        {
            
            MySqlCommand prikaz = new MySqlCommand("Select * from bakalarka.Recept where vysledek=@id ");//Borce, az to nebude zase fungovat tak musis zmenit nazev tabulky braseee
            prikaz.Parameters.AddWithValue("@id",vysledek);
            MySqlDataReader data = DBConnector.ProvedeniPrikazuSelect(prikaz);
            
            if (data.HasRows)
            {
                data.Read();    
                this.vysledek = vysledek;
                prisada1 = Hra.produkty.Find(item => item.id == (int)data["prisada1"]).nazev;
                mnozstvi1 = (int)data["mnozstvi1"];
                prisada2 = Hra.produkty.Find(item => item.id == (int)data["prisada2"]).nazev;
                mnozstvi2 = (int)data["mnozstvi2"];
                if (!Convert.IsDBNull(data["prisada3"]))
                  {
                      prisada3 = Hra.produkty.Find(item => item.id == (int)data["prisada3"]).nazev;
                      mnozstvi3 = (int)data["mnozstvi3"];
                  }
            }
            else
            {
                this.vysledek = 0; // pokud pro dany produkt neexistuje recept, tak se nastavi na 0
            }
           



        }
    }
}
