using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Globalization;
using Trainer_Editor.Pages;
using System.Windows.Controls;
using Trainer_Editor.UserControls;
using Trainer_Editor.Windows;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum Input {
        Moves, HeldItem
    };


    public partial class MainWindow : Window {

        private RegexSettings regexSettings;
        public RegexSettings RegexSettings { get { return regexSettings; } set => regexSettings = value; }

        public MainWindow() {
            InitializeComponent();

            Data.Instance.SelectedTrainer = Trainer.CreateDummy();
            Data.Instance.SelectedMon = Data.Instance.SelectedTrainer.Party[0];

            //FileManager.DeserializeAllConstantLists(Data.Instance);

            Data.Instance.RegexConfig = FileManager.DeserializeRegexConfig();

            ControlNav.LoadControlNavs();

            DataContext = Data.Instance;

        }

        private async void ReadTrainersAndParties_Click(object sender, RoutedEventArgs e) {
            List<Trainer> trainers = await Task.Run(() => FileManager.ParseTrainersHeader());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ParsePartiesHeader());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => FileManager.AttachPartiesToTrainers(trainers, parties));
            Debug.WriteLine($"Stitch Complete.");

            await Task.Run(() => Data.Instance.Trainers = trainers);
        }
        private async void SaveTrainersAndParties_Click(object sender, RoutedEventArgs e) {

            if (true) {
                await Task.Run(() => FileManager.WriteTrainersHeader(Data.Instance.Trainers));
                Debug.WriteLine("Trainers Saved.");

                await Task.Run(() => FileManager.WritePartiesHeader(Data.Instance.Trainers));
                Debug.WriteLine("Parties Saved.");
            }
        }
        private async void ReadWriteConstants_Click(object sender, RoutedEventArgs e) {
            await Task.Run(() => FileManager.ParseAllConstantHeadersToJson());
            await Task.Run(() => FileManager.DeserializeAllConstantLists(Data.Instance));

            Debug.WriteLine("Read and Wrote Constants.");
        }

        private async void MenuOpen_Click(object sender, RoutedEventArgs e) {

            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            if (d.ShowDialog() == true)
                Debug.WriteLine(d.SelectedPath);

            List<Trainer> trainers = await Task.Run(() => FileManager.ParseTrainersHeader());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ParsePartiesHeader());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => FileManager.AttachPartiesToTrainers(trainers, parties));
            Debug.WriteLine($"Stitch Complete.");

            //await Task.Run(() => FileManager.WriteTrainers(trainers));
            //Debug.WriteLine("Trainers Saved.");

            //await Task.Run(() => FileManager.WriteParties(trainers));
            //Debug.WriteLine("Parties Saved.");

            await Task.Run(() => Data.Instance.Trainers = trainers);
        }

        private void MenuRegex_Click(object sender, RoutedEventArgs e) {
            RegexSettings = new RegexSettings();
            RegexSettings.ShowDialog();
        }

    }

    public class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


}
