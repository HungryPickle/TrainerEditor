﻿using Ookii.Dialogs.Wpf;
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
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            Data.Instance.StatusBar = statusBar;
            
            Data.Instance.SelectedTrainer = Trainer.CreateDummy();
            Data.Instance.SelectedMon = Data.Instance.SelectedTrainer.Party[0];

            FileManager.Instance.DeserializeFilePaths();
            
            FileManager.Instance.DeserializeAllConstants();

            //Debug @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            FileManager.Instance.FilePaths.PokeEmeraldDirectory = @"C:\Users\Scott\Decomps\pokeemerald";
            FileManager.Instance.ParseTrainersAndParties();

            ControlNav.LoadControlNavs();

            DataContext = Data.Instance;

        }

        private async void ReadTrainersAndParties_Click(object sender, RoutedEventArgs e) {

            await Task.Run(() => FileManager.Instance.ParseTrainersAndParties());
        }
        private async void SaveTrainersAndParties_Click(object sender, RoutedEventArgs e) {

            await Task.Run(() => FileManager.Instance.WriteTrainersAndPartiesHeaders());
            statusBar.Text = "Trainers Saved.";
        }
        private async void ReadWriteConstants_Click(object sender, RoutedEventArgs e) {
            await Task.Run(() => FileManager.Instance.ParseAllConstants());
            await Task.Run(() => FileManager.Instance.SerializeAllConstants());
            await Task.Run(() => FileManager.Instance.DeserializeAllConstants());

            statusBar.Text = "Parsed Constants.";
        }

        private async void MenuOpen_Click(object sender, RoutedEventArgs e) {

            VistaFolderBrowserDialog folderBrowser = new VistaFolderBrowserDialog();
            folderBrowser.ShowDialog();

            statusBar.Text = "Parsing for Trainers...";
            FileManager.Instance.FilePaths.PokeEmeraldDirectory = folderBrowser.SelectedPath;
            FileManager.Instance.SerializeFilePaths();
            await Task.Run(() => FileManager.Instance.ParseTrainersAndParties());
            statusBar.Text = "Parsed Trainers.";

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
