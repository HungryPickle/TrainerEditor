using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Globalization;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            ////for(int i = 0; i < parties.Count(); i ++) {
            ////    trainers[i+1].party = parties.Find(p => p.TrainerPartyName.Equals(trainers[i+1].partySize));
            ////}

            DataContext = Data.Instance;
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {

            List<Trainer> trainers = await Task.Run(() => FileManager.ReadTrainers());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ReadParties());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => Data.StitchLists(trainers, parties));
            Debug.WriteLine($"Stitch Complete.");

            //await Task.Run(() => FileManager.WriteTrainers(trainers));
            //Debug.WriteLine("Trainers Saved.");
            //
            //await Task.Run(() => FileManager.WriteParties(trainers));
            //Debug.WriteLine("Parties Saved.");

            await Task.Run(() => Data.Instance.Trainers = trainers);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {

            TextBox textbox = sender as TextBox;
            if (textbox != null && Data.Instance.Trainers.Count > 0) {
                ListBox1.ItemsSource = Data.Instance.Trainers.Where(t => t.TrainerIndexName.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase));
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Trainer tr = ListBox1.SelectedItem as Trainer;
            if(tr?.Party != null) {
                Data.Instance.SelectedMon = tr.Party.MonList[0];
            }

        }

        private async void MenuOpen_Click(object sender, RoutedEventArgs e) {

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //    Debug.WriteLine(openFileDialog.FileName);

            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            if (d.ShowDialog() == true)
                Debug.WriteLine(d.SelectedPath);

            List<Trainer> trainers = await Task.Run(() => FileManager.ReadTrainers());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ReadParties());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => Data.StitchLists(trainers, parties));
            Debug.WriteLine($"Stitch Complete.");

            //await Task.Run(() => FileManager.WriteTrainers(trainers));
            //Debug.WriteLine("Trainers Saved.");

            //await Task.Run(() => FileManager.WriteParties(trainers));
            //Debug.WriteLine("Parties Saved.");

            await Task.Run(() => Data.Instance.Trainers = trainers);
        }



    }
    public class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class Data : ObservableObject {

        private Data() { }

        private static Data _instance = new Data();
        public static Data Instance {
            get { return _instance; }
        }

        private List<Trainer> _trainers = new List<Trainer>();
        public List<Trainer> Trainers {
            get { return _trainers; }
            set { _trainers = value; OnPropertyChanged("Trainers"); }
        }

        private Trainer selectedTrainer;
        public  Trainer SelectedTrainer {
            get { return selectedTrainer; }
            set { selectedTrainer = value;
                //SelectedMon = value?.Party?.MonList[0];
                OnPropertyChanged("SelectedTrainer"); }
        }

        private Mon selectedMon;
        public Mon SelectedMon {
            get { return selectedMon; }
            set { selectedMon = value; OnPropertyChanged("SelectedMon"); }
        }


        private List<string> _species = SpeciesDefines.List;
        public List<string> Species {
            get { return _species = SpeciesDefines.List; }
            set { _species = SpeciesDefines.List = value; }
        }


        public static List<Trainer> StitchLists(List<Trainer> trainers, Dictionary<string, Party> parties) {

            for (int i = 0; i < parties.Count(); i++) {
                trainers[i].Party = parties.GetValueOrDefault(trainers[i].PartySize);
            }
            return trainers;

        }

    }

    public class ComparisonConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
    public class MovesEnabledCoverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch (value) {
                case TYPE.TrainerMonNoItemDefaultMoves:
                    return false;
                case TYPE.TrainerMonItemDefaultMoves:
                    return false;
                default:
                    return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }
}
