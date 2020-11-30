using Bakalarka.role;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MySqlConnector;

namespace Bakalarka.logika
{
    static class MapaKontroler
    {
        /*
         * zobrazeni mapu tam kde se hrac nachazi
         */
        static public async void PoziceHrace()
        {
            try
            {
                var lokace = await Geolocation.GetLastKnownLocationAsync();

                if (lokace != null)
                {
                    Position pozice = new Position(lokace.Latitude, lokace.Longitude);
                    MapSpan mapSapn = MapSpan.FromCenterAndRadius(pozice, Distance.FromKilometers(.444));
                    Hra.mapa.MoveToRegion(mapSapn);
                    Console.WriteLine($"Latitude: {lokace.Latitude}, Longitude: {lokace.Longitude}, Altitude: {lokace.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        /*
         * Zobrazeni herniho pole
         */
        static public void HerniPole()
        {
            Hra.mapa.MapElements.Clear();
            Polygon pole = new Polygon
            {
                StrokeWidth = 8,
                StrokeColor = Xamarin.Forms.Color.FromHex("#eb0000"),
               // FillColor = Xamarin.Forms.Color.FromHex("##881BA1E2"),
                Geopath =
                {
                    new Position(Hra.roh1X,Hra.roh1Y),
                    new Position(Hra.roh2X,Hra.roh2Y),
                    new Position(Hra.roh3X,Hra.roh3Y),
                    new Position(Hra.roh4X,Hra.roh4Y)
                }
            };
            Hra.mapa.MapElements.Add(pole);
           
        }
        /*
         * Nacteni produktu
         */
        static public void NacteniProduktu()
        {
            Hra.mapa.Pins.Clear();
            foreach (var produkt in Hra.produkty)
            {
                if (produkt.uroven == 1)
                {
                
                Hra.mapa.Pins.Add(produkt.jedna);
                Hra.mapa.Pins.Add(produkt.dva);
                }
            }
        }
    }
       
}
