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
        public Souboj souboj;
        public MujProfil mujprofil;
        public Skladiste skladiste;
        public MainPage()
        {
            mapa = new Mapa();
            souboj = new Souboj();
            mujprofil = new MujProfil();
            skladiste = new Skladiste();
            Children.Add(mujprofil);
            Children.Add(mapa);
            Children.Add(souboj);
            Children.Add(skladiste);
            InitializeComponent();
            
        }
    }
}
