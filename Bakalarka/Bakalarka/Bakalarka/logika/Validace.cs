using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Bakalarka.logika
{
    static class Validace
    {
        public static void PouzeDouble(Entry entry, TextChangedEventArgs args )
        {
            char[] text = args.NewTextValue.ToCharArray();
            bool tecka = false;
            foreach (char znak in text)
            {
                if (!char.IsDigit(znak)&&!znak.Equals('.'))
                {
                    entry.Text= entry.Text.Remove(entry.Text.Length-1);
                    return;
                }
                if (znak == '.')
                {
                    if (tecka)
                    {
                        entry.Text = entry.Text.Remove(entry.Text.Length - 1);
                        return;
                    }
                    tecka = true;
                }
            }
        }

        public static void PouzeInt(Entry entry,TextChangedEventArgs args)
        {
            char[] text = args.NewTextValue.ToCharArray();

            foreach(char znak in text)
            {
                if (!char.IsDigit(znak))
                {
                    entry.Text = entry.Text.Remove(text.Length-1);
                }
            }
        }
    }
}
