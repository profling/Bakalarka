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
        public MainPage()
        {
            mapa = new Mapa();
            hie = new Hierarchie();
            souboj = new Souboj();
            nastaveni = new Nastaveni();
            Children.Add(mapa);
            Children.Add(hie);
            Children.Add(souboj);
            Children.Add(nastaveni);
            InitializeComponent();
            
        }
    }
}
