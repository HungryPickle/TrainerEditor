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

            Data.Instance.SpeciesList = FileManager.ReadConstants(Constant.Species);
            Data.Instance.ItemsList = FileManager.ReadConstants(Constant.Items);
            Data.Instance.MovesList = FileManager.ReadConstants(Constant.Moves);

            ControlNav.LoadControlNavs();

            DataContext = Data.Instance;
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            List<Trainer> trainers = await Task.Run(() => FileManager.ReadTrainers());
            Debug.WriteLine("Trainers Read.");

            Dictionary<string, Party> parties = await Task.Run(() => FileManager.ReadParties());
            Debug.WriteLine("Parties Read.");

            trainers = await Task.Run(() => FileManager.StitchLists(trainers, parties));
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

            trainers = await Task.Run(() => FileManager.StitchLists(trainers, parties));
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


}
