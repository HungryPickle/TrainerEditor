using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Trainer_Editor.Windows;
using System.IO;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum Input {
        Moves, HeldItem
    };


    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            
            Data.Instance.SelectedTrainer = Trainer.CreateDummy();
            Data.Instance.SelectedMon = Data.Instance.SelectedTrainer.Party[0];

            //Data.Instance.FilePaths = FileManager.DeserializeFilePaths();
            Data.Instance.RegexConfig = FileManager.DeserializeRegexConfig();
            //FileManager.DeserializeAllConstantLists(Data.Instance);
            ControlNav.LoadControlNavs();

            DataContext = Data.Instance;

        }

        private async void ReadTrainersAndParties_Click(object sender, RoutedEventArgs e) {

            await Task.Run(() => FileManager.ParseTrainersAndParties());
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
            await Task.Run(() => FileManager.ParseAllConstantHeadersToSerializedLists());
            await Task.Run(() => FileManager.DeserializeAllConstantLists());

            Debug.WriteLine("Read and Wrote Constants.");
        }

        private async void MenuOpen_Click(object sender, RoutedEventArgs e) {

            VistaFolderBrowserDialog folderBrowser = new VistaFolderBrowserDialog();
            folderBrowser.ShowDialog();

            statusBar.Text = "Parsing for Trainers...";
            
            Data.Instance.FilePaths.PokeEmeraldDirectory = folderBrowser.SelectedPath;
            FileManager.SerializeFilePaths();

            await Task.Run(() => FileManager.ParseTrainersAndParties());

            await Task.Run(() => FileManager.ParseAllConstantHeadersToSerializedLists());
            await Task.Run(() => FileManager.DeserializeAllConstantLists());

            statusBar.Text = "Parse Complete.";

            //await Task.Run(() => FileManager.WriteTrainers(trainers));
            //Debug.WriteLine("Trainers Saved.");

            //await Task.Run(() => FileManager.WriteParties(trainers));
            //Debug.WriteLine("Parties Saved.");

        }

        private void MenuRegex_Click(object sender, RoutedEventArgs e) {
            RegexSettings regexSettings = new RegexSettings();
            regexSettings.ShowDialog();
        }

    }

    public class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


}
