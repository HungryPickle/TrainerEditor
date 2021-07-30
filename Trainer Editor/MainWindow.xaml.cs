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
    /// 
    public enum Input {
        Moves, HeldItem
    };
    public enum Constant {
        Species, Moves, Items
    };

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            
            ////for(int i = 0; i < parties.Count(); i ++) {
            ////    trainers[i+1].party = parties.Find(p => p.TrainerPartyName.Equals(trainers[i+1].partySize));
            ////}
            
            Data.Instance.SpeciesList = FileManager.ReadConstants(Constant.Species);
            Data.Instance.ItemsList = FileManager.ReadConstants(Constant.Items);
            Data.Instance.MovesList = FileManager.ReadConstants(Constant.Moves);


            DataContext = Data.Instance;
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            List<Trainer> trainers = await Task.Run(() => FileManager.ReadTrainers());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ReadParties());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => Data.StitchLists(trainers, parties));
            Debug.WriteLine($"Stitch Complete.");

            await Task.Run(() => Data.Instance.Trainers = trainers);
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e) {

            if (true) {
                await Task.Run(() => FileManager.WriteTrainers(Data.Instance.Trainers));
                Debug.WriteLine("Trainers Saved.");

                await Task.Run(() => FileManager.WriteParties(Data.Instance.Trainers));
                Debug.WriteLine("Parties Saved.");
            }
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e) {
            Data.Instance.SpeciesList = await Task.Run(() => FileManager.ParseHeaderWriteConstants(Constant.Species));
            Data.Instance.ItemsList = await Task.Run(() => FileManager.ParseHeaderWriteConstants(Constant.Items));
            Data.Instance.MovesList = await Task.Run(() => FileManager.ParseHeaderWriteConstants(Constant.Moves));


            Debug.WriteLine("Read and Wrote Constants.");
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

        private void LabeledTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up) {
                var request = new TraversalRequest(FocusNavigationDirection.Up);
                //Window.GetWindow(this).
                
                Grid1.MoveFocus(request);
                
                
                e.Handled = true;
            }
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

        private static Data instance = new Data();
        public static Data Instance {
            get { return instance; }
        }

        private List<Trainer> trainers = new List<Trainer>();
        public List<Trainer> Trainers {
            get { return trainers; }
            set { trainers = value; OnPropertyChanged("Trainers"); FilteredTrainers = value; }
        }

        private List<Trainer> filteredTrainers = new List<Trainer>();
        public List<Trainer> FilteredTrainers {
            get { return filteredTrainers; }
            set { filteredTrainers = value; OnPropertyChanged("FilteredTrainers"); }
        }


        private Trainer selectedTrainer;
        public Trainer SelectedTrainer {
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

        private List<string> speciesList;

        public List<string> SpeciesList {
            get { return speciesList; }
            set { speciesList = value;
                OnPropertyChanged("SpeciesList");
            }
        }

        private List<string> itemsList;

        public List<string> ItemsList {
            get { return itemsList; }
            set { itemsList = value;
                OnPropertyChanged("ItemsList");
            }
        }

        private List<string> movesList;

        public List<string> MovesList {
            get { return movesList; }
            set { movesList = value;
                OnPropertyChanged("MovesList");
            }
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
            //return value?.Equals(true) == true ? parameter : Binding.DoNothing;
            return (bool)value ? parameter : Binding.DoNothing;
        }
    }
    public class InputEnabledConverter : IMultiValueConverter {

        public static Dictionary<Input, bool> TrainerMonNoItemDefaultMoves = new Dictionary<Input, bool> { { Input.HeldItem, false }, { Input.Moves, false } };
        public static Dictionary<Input, bool> TrainerMonItemDefaultMoves = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, false } };
        public static Dictionary<Input, bool> TrainerMonNoItemCustomMoves = new Dictionary<Input, bool> { { Input.HeldItem, false }, { Input.Moves, true } };
        public static Dictionary<Input, bool> TrainerMonItemCustomMoves = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, true } };

        public static Dictionary<PartyType, Dictionary<Input, bool>> IsInputEnabled = new Dictionary<PartyType, Dictionary<Input, bool>> {
            { PartyType.TrainerMonNoItemDefaultMoves, TrainerMonNoItemDefaultMoves },
            { PartyType.TrainerMonItemDefaultMoves, TrainerMonItemDefaultMoves },
            { PartyType.TrainerMonNoItemCustomMoves, TrainerMonNoItemCustomMoves },
            { PartyType.TrainerMonItemCustomMoves, TrainerMonItemCustomMoves }
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {

            if (!(values[0] is PartyType && values[1] is Input))
                return false;

            PartyType partyType = (PartyType)values[0];
            Input input = (Input)values[1];

            return IsInputEnabled[partyType][input];
            
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class EnabledBackgroundConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? 1.0 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
