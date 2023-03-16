using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fajlNev = "naplo.txt";
        //Így minden metódus fogja tudni használni.
        ObservableCollection<Osztalyzat> jegyek = new ObservableCollection<Osztalyzat>();

        int jeygekAtlaga = 0;

        public MainWindow()
        {
            InitializeComponent();
            // todo Fájlok kitallózásával tegye lehetővé a naplófájl kiválasztását!
            // Ha nem választ ki semmit, akkor "naplo.csv" legyen az állomány neve. A későbbiekben ebbe fog rögzíteni a program.

            // todo A kiválasztott naplót egyből töltse be és a tartalmát jelenítse meg a datagrid-ben!


            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                fajlNev = openFileDialog.FileName;

                lblEditor.Content = fajlNev;
                OsztalyzatokBetoltese(fajlNev);
                lblJegyekSzama.Content = dgJegyek.Items.Count;
            }
            else
            {
                OsztalyzatokBetoltese(fajlNev);
                lblEditor.Content = fajlNev;
                lblJegyekSzama.Content = dgJegyek.Items.Count;
            }




        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            //todo Ne lehessen rögzíteni, ha a következők valamelyike nem teljesül!
            // a) - A név legalább két szóból álljon és szavanként minimum 3 karakterből!
            //      Szó = A szöközökkel határolt karaktersorozat.
            // b) - A beírt dátum újabb, mint a mai dátum

            //todo A rögzítés mindig az aktuálisan megnyitott naplófájlba történjen!

            string[] Nev = txtNev.Text.Split(' ');
            string nevMegforditas = Nev[1] +" "+ Nev[0];



            //A CSV szerkezetű fájlba kerülő sor előállítása
            string csvSor = $"{txtNev.Text};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value}";
            string csvSorNevmegforditva = $"{nevMegforditas};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value}";
            //Megnyitás hozzáfűzéses írása (APPEND)


            if (Nev.Length < 2)
            {
                MessageBox.Show("Teljes nevet adj meg!");
            }
            else if(Nev[0].Length < 3 || Nev[1].Length < 3)
            {
                MessageBox.Show("Legalább 3 betűből álljanak a nevek!");
            }
            else if (DateTime.Compare((DateTime)datDatum.SelectedDate, DateTime.Now)>0)
            {
                MessageBox.Show("Nem lehet a dátum a mainál későbbi!");
            }
            else if (rbKeresztnevbolVezetekNev.IsChecked == true)
            {
                StreamWriter sw = new StreamWriter(fajlNev, append: true);
                sw.WriteLine(csvSorNevmegforditva);
                sw.Close();
                OsztalyzatokBetoltese(fajlNev);
                lblJegyekSzama.Content = dgJegyek.Items.Count;
            }
            else if (rbVezetekbolKeresztnev.IsChecked == true)
            {
                StreamWriter sw = new StreamWriter(fajlNev, append: true);
                sw.WriteLine(csvSor);
                sw.Close();
                OsztalyzatokBetoltese(fajlNev);
                lblJegyekSzama.Content = dgJegyek.Items.Count;
            }
            //todo Az újonnan felvitt jegy is jelenjen meg a datagrid-ben!
        }

        
        private void OsztalyzatokBetoltese(string fajlNev)
        {
            jegyek.Clear();
            StreamReader sr = new StreamReader(fajlNev);
            while (!sr.EndOfStream)
            {
                string[] mezok = sr.ReadLine().Split(";");
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]));
                jegyek.Add(ujJegy);
            }
            sr.Close();
            dgJegyek.ItemsSource = jegyek;
            MessageBox.Show("Az állomány befejezése befejeződött!");
        }




        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            OsztalyzatokBetoltese(fajlNev);
            lblJegyekSzama.Content = dgJegyek.Items.Count;
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

        //todo Felület bővítése: Az XAML átszerkesztésével biztosítsa, hogy láthatóak legyenek a következők!
        // - A naplófájl neve
        // - A naplóban lévő jegyek száma
        // - Az átlag

        //todo Új elemek frissítése: Figyeljen rá, ha új jegyet rögzít, akkor frissítse a jegyek számát és az átlagot is!

        //todo Helyezzen el alkalmas helyre 2 rádiónyomógombot!
        //Feliratok: [■] Vezetéknév->Keresztnév [O] Keresztnév->Vezetéknév
        //A táblázatban a név aszerint szerepeljen, amit a rádiónyomógomb mutat!
        //A feladat megoldásához használja fel a ForditottNev metódust!
        //Módosíthatja az osztályban a Nev property hozzáférhetőségét!
        //Megjegyzés: Felételezzük, hogy csak 2 tagú nevek vannak
    }
}

