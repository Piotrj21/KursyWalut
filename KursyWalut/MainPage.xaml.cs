using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
namespace KursyWalut
{
    public sealed partial class MainPage : Page
    {
        List<string> nazwy = new List<string>();
        List<string> kursy = new List<string>();
        List<string> przelicznik = new List<string>();
        const string daneNBP = "http://www.nbp.pl/kursy/xml/LastA.xml";
        
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            pobierzKursWaluty();

        }

        private async void pobierzKursWaluty()
        {
            var serwerNBP = new HttpClient();
            string dane = "";
            dane = await serwerNBP.GetStringAsync(new Uri(daneNBP));
            XmlDocument xmltest = new XmlDocument();
            xmltest.LoadXml(dane);
            XmlNodeList elemlist = xmltest.GetElementsByTagName("kod_waluty");
            XmlNodeList elemlist2 = xmltest.GetElementsByTagName("kurs_sredni");
            XmlNodeList elemlist3 = xmltest.GetElementsByTagName("przelicznik");

            nazwy.Add("PLN");
            kursy.Add("1,000");
            przelicznik.Add("1,0");
            lbxZWaluty.Items.Add("PLN  : 1,0000");
            lbxNaWalute.Items.Add("PLN  : 1,0000");
            int i = 0;
            while (elemlist[i] != null)
            {

                string result = elemlist[i].InnerXml;
                string result2 = elemlist2[i].InnerXml;
                string result3 = elemlist3[i].InnerXml;
                i++;
                nazwy.Add(result);
                kursy.Add(result2);
                przelicznik.Add(result3);
                lbxZWaluty.Items.Add(result + "  : " + result2);
                lbxNaWalute.Items.Add(result + "  : " + result2);
            }
        }

        private void Przelicz()
        {
            if (lbxZWaluty.SelectedItem != null && lbxNaWalute.SelectedItem != null && txtKwota != null)
            {
                string text1 = lbxZWaluty.SelectedItem.ToString();
                string text2 = lbxNaWalute.SelectedItem.ToString();
                int indeks1 = nazwy.FindIndex(text1.StartsWith);
                int indeks2 = nazwy.FindIndex(text2.StartsWith);
                float f;
                if (float.TryParse(txtKwota.Text, out f))
                {
                    tbPrzeliczona.Text = (float.Parse(txtKwota.Text) * float.Parse(kursy[indeks1]) * float.Parse(przelicznik[indeks2]) / float.Parse(przelicznik[indeks1]) / float.Parse(kursy[indeks2])).ToString(); // przeliczona = kwota wybrana * jej własny kurs / kurs docelowy
                }
            }
        }

        private void lbxZWaluty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Przelicz();
        }

        private void lbxNaWalute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Przelicz();
        }

        private void txtKwota_TextChanged(object sender, TextChangedEventArgs e)
        {
            Przelicz();
        }
    }
}
