using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Bakalarka;
using Bakalarka.UX;
using Xamarin.Forms;


namespace Bakalarka.logika
{
    class Produkt
    {
       public String nazev;
       public String popis;
        public int id;
         public Pin jedna;
         public Pin dva;
        public int ulozene;
        public int uroven;
        
        

        public Produkt(int id, String nazev, String popis, double x, double y, double x2, double y2, int uroven)
        {
            
            this.nazev = nazev;
            this.popis = popis;
            this.uroven = uroven;
            this.id = id;

            jedna = new Pin
            {
                Position = new Position(x, y),
                Label = nazev,
                Address = nazev,
                Type = PinType.Place
            };

            jedna.MarkerClicked += async (s, args) =>
            {
                Hra.vybranyProdukt = id;
                Hra.produktPopis.Text = nazev;
            };
            dva = new Pin
            {
                Position = new Position(x2, y2),
                Label = nazev,
                Address = nazev,
                Type = PinType.Place
            };
            dva.MarkerClicked += async (s, args) =>
            {
                Hra.vybranyProdukt = id;
                Hra.produktPopis.Text = nazev;

            };
        }

    }
}
