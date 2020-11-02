using Bakalarka.UX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bakalarka
{
    public partial class MainPage :TabbedPage
    {
        public Mapa mapa;
        public Hierarchie hie;
        public Souboj souboj;
        public Nastaveni nastaveni;
        public Skladiste skladiste;
        public MainPage()
        {
            mapa = new Mapa();
            hie = new Hierarchie();
            souboj = new Souboj();
            nastaveni = new Nastaveni();
            skladiste = new Skladiste();
            
            Children.Add(mapa);
            Children.Add(hie);
            Children.Add(souboj);
            Children.Add(nastaveni);
            Children.Add(skladiste);
            InitializeComponent();
            
        }
    }
}
