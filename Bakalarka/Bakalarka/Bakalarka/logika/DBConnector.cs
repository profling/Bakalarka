using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlConnector;

namespace Bakalarka.logika
{
    class DBConnector
    {
      
        MySqlConnectionStringBuilder builder;
        
       

        public DBConnector()
        {
            builder = new MySqlConnectionStringBuilder();
            builder.Server = "34.107.13.57";
            builder.Database = "bakalarka";
            builder.UserID = "app";
            builder.Password = "lov3MaxiKing";
            /*builder.SslCert = @"Bakalarka.client-cert.pem";
            builder.SslKey = @"Bakalarka.client-key.pem";
            builder.SslCa = @"Bakalarka.server-ca.pem";
            builder.SslMode = MySqlSslMode.VerifyFull;*/
        }
   
            
        /**
         * Provedeni prikazu update, delete, insert. Pokud probehne v poradku vrati data jinak vrati null
         */
        
        public MySqlDataReader ProvedeniPrikazuSelect(MySqlCommand prikaz)
        {
            try
            {
                
                MySqlConnection pripojeni = new MySqlConnection(builder.ConnectionString);
                pripojeni.Open();
                prikaz.Connection = pripojeni;
                try
                {
                    MySqlDataReader data = prikaz.ExecuteReader();
                    return data;
                }
                catch(MySqlException e)
                {
                    return null;
                }

            }
            catch (MySqlException e)
            {
                return null;
            }
        }
        /**
         * Provedeni prikazu update, delete, insert. Pokud probehne v poradku vrati null jinak vrati chybovou hlasku
         */
        public String ProvedeniPrikazuOstatni( MySqlCommand prikaz)
        {
            try
            {
                MySqlConnection pripojeni = new MySqlConnection(builder.ConnectionString);
                pripojeni.Open();
                prikaz.Connection=pripojeni;
                try
                {
                    prikaz.ExecuteNonQuery();
                    pripojeni.Close();
                    return null;
                }
                catch (MySqlException e)
                {
                    return e.ToString();
                }

            }
            catch (MySqlException e)
            {
                return e.ToString();
            }

        }
        
    }
}
